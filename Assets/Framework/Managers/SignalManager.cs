using UnityEngine;

/// <summary>
/// как пользоваться:
///     чтобы послать сигнал:
///         SignalManager<SomeSignal>.SendSignal(new SomeSignal());,
///             где SomeSignal - класс, наследованный от интерфейса ISignal.
///     чтобы подписаться на сигнал:
///         класс, который хочет подписаться на сигнал должен быть унаследован от IReceive<SomeSignal>,
///             где SomeSignal - класс, наследованный от интерфейса ISignal, в котором будет передоваться вся дата сигнала
///         должен быть реализован интерфейс IReceive:
///             public void SignalHandler(SomeSignal arg) { }
///         в Start, Awaka (любом другом месте) должна быть вызвана запись в SignalManager<SomeSignal>:
///             SignalManager<SomeSignal>.AddReceiver(this);
/// </summary>
namespace RangerV
{
    public class SignalManager<T> where T : ISignal
    {
        static SignalManager<T> instance;
        public static SignalManager<T> Instance 
        {
            get
            {
                if (instance == null)
                    instance = new SignalManager<T>();
                return instance;
            }
        }

        delegate void SignalHandler(T arg);
        event SignalHandler signalHandler;

        public static void AddReceiver(IReceive<T> receiver)
        {
            Instance.signalHandler += receiver.SignalHandler;
        }

        public static void RemoveReceiver(IReceive<T> receiver)
        {
            Instance.signalHandler -= receiver.SignalHandler;
        }

        public static void SendSignal(T arg)
        {
            Instance.signalHandler?.Invoke(arg);
        }

    }
}
