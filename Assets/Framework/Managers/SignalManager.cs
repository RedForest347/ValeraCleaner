using UnityEngine;

namespace RangerV
{
    public class SignalManager<T> where T : ISignal, new()
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

        public void AddReceiver(IReceive<T> receiver)
        {
            signalHandler += receiver.SignalHandler;
        }

        public void RemoveReceiver(IReceive<T> receiver)
        {
            signalHandler -= receiver.SignalHandler;
        }

        public static void SendSignal(T arg)
        {
            Instance.signalHandler?.Invoke(arg);
        }

    }
}
