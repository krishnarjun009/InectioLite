using System;
using System.Linq;

namespace Inectio.Lite
{
    public class BaseSignal
    {
        private event Action listener;

        public void AddListener(Action callback)
        {
            listener = Add(listener, callback);
        }

        public void Dispatch()
        {
            if (listener != null)
                listener();
        }

        private Action Add(Action listener, Action action)
        {
            if(listener == null || !listener.GetInvocationList().Contains(action))
            {
                listener += action;
            }

            return listener;
        }
    }

    public class Signal : BaseSignal
    {
        private event Action listener;

        public void AddListener(Action callback)
        {
            listener = AddUnique(listener, callback);
        }

        public void RemoveListener(Action callback)
        {

        }

        public void Dispatch()
        {
            if (listener != null)
                listener();
        }

        private Action AddUnique(Action listeners, Action callback)
        {
            if (listeners == null || !listeners.GetInvocationList().Contains(callback))
            {
                listeners += callback;
            }
            return listeners;
        }
    }
}
