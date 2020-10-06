using System.Threading;
using System.Collections.Generic;
using Debug = UnityEngine.Debug;
using System;
using UnityEngine;

//https://docs.microsoft.com/en-us/dotnet/api/system.threading.manualresetevent?redirectedfrom=MSDN&view=netframework-4.8

namespace RangerV
{
    public delegate void ThreadSStart();
    public delegate void ParameterizedThreadSStart(object o);

    /// <summary>
    /// как пользоваться:
    /// есть несколько способов:
    ///     1) добавить некоторую функцию на выполнение с помощью функции StartThread, StartThreadNow
    ///         ThreadManager.StartThread() - функция запустится в одном из следующих кадров
    ///         ThreadManager.StartThreadNow() - функция запустится моментально
    ///         
    ///     2) с помощью TaskInfo, при данном способе появляется возможность отслеживать состояние выполнения функции с помощью публичных полей TaskInfo (см. класс TaskInfo)
    ///         ThreadManager.TaskInfo taskInfo = ThreadManager.TaskInfo.StartNew() или ThreadManager.TaskInfo.StartNewNow()
    ///             StartNew() - функция запустится в одном из следующих кадров
    ///             StartNewNow() - функция запустится моментально
    ///         вызов потока занимает примерно 0,03 мс
    /// </summary>
    sealed class ThreadManager : ProcessingBase, ICustomUpdate, ICustomAwake
    {
        #region Options

        const int start_thread_pool_size = 40;
        const int max_thread_pool_size = 40;
        const int max_start_threads_per_update = 40;
        const int num_of_standby_thread = 10; // количество потоков, которых смогут сипользоваться только в функции *Now (StartThreadNow, например)

        #endregion Options

        public static int Num_of_free_thread { get => FreeThreadStack.Count; }

        static int current_thread_pool_size;
        static Stack<WorkThread> FreeThreadStack;
        static Queue<TaskContainer> Tasks;
        static bool init;

        static ThreadManager()
        {
            if (!init)
            {
                Init();
                if (!Application.isPlaying)
                    Debug.LogWarning("обращение к ThreadManager в EditorMode. это делать не рекомендуется");
                //else
                //    Debug.LogWarning("обращение к ThreadManager до его инициализации. возможно, ее вызов не прописан в Starter");
            }
        }

        public void OnAwake()
        {
            Init();
            //Debug.Log("ThreadManager init");

        }

        public void CustomUpdate()
        {
            StartTasks();
        }

        public static void Init()
        {
            current_thread_pool_size = 0;
            FreeThreadStack = new Stack<WorkThread>();
            Tasks = new Queue<TaskContainer>();

            for (int id = 0; id < start_thread_pool_size; id++)
                PushToThreadStack(new WorkThread(id));

            current_thread_pool_size = start_thread_pool_size;
            init = true;
        }

        static void PushToThreadStack(WorkThread workThread)
        {
            lock (FreeThreadStack)
            {
                FreeThreadStack.Push(workThread);
            }
        }

        static WorkThread PopFromThreadStack()
        {
            lock (FreeThreadStack)
            {
                return FreeThreadStack.Pop();
            }
        }

        #region Start Thread

        public static void StartThread(ThreadSStart threadSStart)
        {
            Tasks.Enqueue(new TaskContainer(threadSStart));
        }

        public static void StartThread(ParameterizedThreadSStart parameterizedThreadSStart, object func_param)
        {
            Tasks.Enqueue(new TaskContainer(parameterizedThreadSStart, func_param));
        }

        public static void StartThread(TaskContainer taskContainer)
        {
            Tasks.Enqueue(taskContainer);
        }

        public static void StartThreadNow(ThreadSStart threadSStart)
        {
            if (!(FreeThreadStack.Count > 0))
            {
                Debug.LogWarning("нет совободных потоков для быстрого запуска потока. возможно, следует увеличить max_thread_pool_size");
                AddThread();
            }

            PopFromThreadStack().Start(new TaskContainer(threadSStart));
        }

        public static void StartThreadNow(ParameterizedThreadSStart parameterizedThreadSStart, object func_param)
        {
            if (!(FreeThreadStack.Count > 0))
            {
                Debug.LogWarning("нет совободных потоков для быстрого запуска потока. возможно, следует увеличить max_thread_pool_size");
                AddThread();
            }

            PopFromThreadStack().Start(new TaskContainer(parameterizedThreadSStart, func_param));
        }

        public static void StartThreadNow(TaskContainer taskContainer)
        {
            if (!(FreeThreadStack.Count > 0))
            {
                Debug.LogWarning("нет совободных потоков для быстрого запуска потока. возможно, следует увеличить max_thread_pool_size");
                AddThread();
            }

            PopFromThreadStack().Start(taskContainer);
        }

        #endregion Start Thread

        static void StartTasks()
        {
            int num_of_start_thread = 0;
            int count = Tasks.Count;

            for (int i = 0; i < count; i++)
            {
                if (num_of_start_thread > max_start_threads_per_update)
                    return;

                if (FreeThreadStack.Count > num_of_standby_thread)
                {
                    PopFromThreadStack().Start(Tasks.Dequeue());
                    num_of_start_thread++;
                }
            }
            TryAddThread();
        }

        static bool TryAddThread()
        {
            if (current_thread_pool_size < max_thread_pool_size)
            {
                FreeThreadStack.Push(new WorkThread(current_thread_pool_size++));
                return true;
            }
            return false;
        }

        static void AddThread()
        {
            FreeThreadStack.Push(new WorkThread(current_thread_pool_size++));
        }

        class WorkThread : IDisposable //переименовать
        {
            public int id { get; private set; }

            private ManualResetEventSlim manualReset;
            private TaskContainer taskContainer;
            private ThreadSStart StartFunc;
            private Thread thread;
            private bool need_stop_thread;

            public WorkThread(int id)
            {
                manualReset = new ManualResetEventSlim(false);
                StartFunc = new ThreadSStart(ThreadStart);
                need_stop_thread = false;
                this.id = id;

                thread = new Thread(new ThreadStart(ThreadStart));
                thread.IsBackground = true;
                thread.Start();
            }

            public void Start(TaskContainer taskContainer)
            {
                this.taskContainer = taskContainer;
                manualReset.Set();
            }

            void ThreadStart()
            {
                while (!need_stop_thread)
                {
                    manualReset.Reset();
                    manualReset.Wait();
                    taskContainer.WorkFunk();
                    PushToThreadStack(this);
                }
            }

            public void StopThread()
            {
                need_stop_thread = true;
                manualReset.Set();
            }

            public void Abort()
            {
                thread.Abort();
            }

            #region Dispose

            private bool disposed = false;

            // реализация интерфейса IDisposable.
            public void Dispose()
            {
                Dispose(true);
                // подавляем финализацию
                GC.SuppressFinalize(this);
            }

            protected virtual void Dispose(bool disposing)
            {
                if (!disposed)
                {
                    if (disposing)
                    {
                        // Освобождаем управляемые ресурсы
                    }
                    // освобождаем неуправляемые объекты
                    manualReset.Dispose();
                    disposed = true;
                }
            }

            // Деструктор
            ~WorkThread()
            {
                Dispose(false);
            }

            #endregion Dispose
        }

        public class TaskContainer
        {
            bool is_void_func;
            TaskInfo taskInfo;
            ThreadSStart threadSStart;
            ParameterizedThreadSStart parameterizedThreadSStart;
            object o;

            public TaskContainer(ThreadSStart threadSStart, TaskInfo threadInfo = null)
            {
                this.threadSStart = threadSStart;
                this.taskInfo = threadInfo;
                parameterizedThreadSStart = null;
                o = null;
                is_void_func = true;
            }

            public TaskContainer(ParameterizedThreadSStart parameterizedThreadSStart, object o, TaskInfo threadInfo = null)
            {
                this.parameterizedThreadSStart = parameterizedThreadSStart;
                this.o = o;
                this.taskInfo = threadInfo;
                this.threadSStart = null;
                this.is_void_func = false;
            }

            public void WorkFunk()
            {
                if (taskInfo != null)
                {
                    lock (taskInfo.to_lock)
                    {
                        taskInfo.is_start = true;

                        if (is_void_func)
                            threadSStart();
                        else
                            parameterizedThreadSStart(o);

                        taskInfo.is_complete = true;
                    }
                }
                else
                {
                    if (is_void_func)
                        threadSStart();
                    else
                        parameterizedThreadSStart(o);
                }
            }
        }

        public sealed class TaskInfo
        {
            TaskContainer taskContainer;
            public bool is_start;
            public bool is_complete;
            public object to_lock; // блокируется до конца выполнения заданной функции
            public object _out; // пока не используется

            public TaskInfo(ThreadSStart Funk)
            {
                _out = null;
                is_start = false;
                is_complete = false;
                to_lock = new object();
                taskContainer = new TaskContainer(Funk, this);
            }

            public TaskInfo(ParameterizedThreadSStart Funk, object o)
            {
                _out = null;
                is_start = false;
                is_complete = false;
                to_lock = new object();
                taskContainer = new TaskContainer(Funk, o, this);
            }

            // стартует поток на выполнение, но не сразу, а как появяться сободные ресурсы для этого
            public void Start()
            {
                Tasks.Enqueue(taskContainer);
            }

            public void StartNow()
            {
                StartThreadNow(taskContainer);
            }

            public static TaskInfo StartNew(ThreadSStart Funk)
            {
                TaskInfo threadInfo = new TaskInfo(Funk);
                threadInfo.Start();
                return threadInfo;
            }

            public static TaskInfo StartNew(ParameterizedThreadSStart Funk, object o)
            {
                TaskInfo threadInfo = new TaskInfo(Funk, o);
                threadInfo.Start();
                return threadInfo;
            }

            public static TaskInfo StartNewNow(ThreadSStart Funk)
            {
                TaskInfo threadInfo = new TaskInfo(Funk);
                threadInfo.StartNow();
                return threadInfo;
            }

            public static TaskInfo StartNewNow(ParameterizedThreadSStart Funk, object o)
            {
                TaskInfo threadInfo = new TaskInfo(Funk, o);
                threadInfo.StartNow();
                return threadInfo;
            }
        }
    }
}
