using System;
using System.Linq;

namespace Inectio.Lite
{
    public interface ISignal
    {
        Delegate Listener { get; set; }
        void RemoveAllListeners();
    }

    public class BaseSignal
    {
        private event Action listener;

        public virtual void AddListener(Action callback)
        {
            listener = Add(listener, callback);
        }

        public virtual void Dispatch()
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

    public class Signal : ISignal
    {
        public Delegate Listener
        {
            get
            {
                return listener ?? (listener = delegate { });
            }
            set
            {
                listener = (Action)value;
            }
        }

        private event Action listener;

        public void AddListener(Action callback)
        {
            listener = AddUnique(listener, callback);
        }

        public void RemoveListener(Action callback)
        {
            if (callback != null && listener.GetInvocationList().Contains(callback))
            {
                //UnityEngine.Debug.Log("Removing listener");
                listener -= callback;
            }
        }

        public void RemoveAllListeners()
        {
            listener = null;
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

    public class Signal<T> : ISignal
    {
        public Delegate Listener
        {
            get
            {
                return listener ?? (listener = delegate { });
            }
            set
            {
                listener = (Action<T>)value;
            }
        }

        private event Action<T> listener;

        public void AddListener(Action<T> callback)
        {
            listener = AddUnique(listener, callback);
        }

        public void RemoveListener(Action<T> callback)
        {
            if (callback != null && listener.GetInvocationList().Contains(callback))
            {
                //UnityEngine.Debug.Log("Removing listener");
                listener -= callback;
            }
        }

        public void RemoveAllListeners()
        {
            listener = null;
        }

        public void Dispatch(T type)
        {
            if (listener != null)
                listener(type);
        }

        private Action<T> AddUnique(Action<T> listeners, Action<T> callback)
        {
            if (listeners == null || !listeners.GetInvocationList().Contains(callback))
            {
                listeners += callback;
            }
            return listeners;
        }
    }

    public class Signal<T, U> : ISignal
    {
        public Delegate Listener
        {
            get
            {
                return listener ?? (listener = delegate { });
            }
            set
            {
                listener = (Action<T, U>)value;
            }
        }

        private event Action<T, U> listener;

        public void AddListener(Action<T, U> callback)
        {
            listener = AddUnique(listener, callback);
        }

        public void RemoveListener(Action<T, U> callback)
        {
            if (callback != null && listener.GetInvocationList().Contains(callback))
            {
                //UnityEngine.Debug.Log("Removing listener");
                listener -= callback;
            }
        }

        public void RemoveAllListeners()
        {
            listener = null;
        }

        public void Dispatch(T type, U type1)
        {
            if (listener != null)
                listener(type, type1);
        }

        private Action<T, U> AddUnique(Action<T, U> listeners, Action<T, U> callback)
        {
            if (listeners == null || !listeners.GetInvocationList().Contains(callback))
            {
                listeners += callback;
            }
            return listeners;
        }
    }

    public class Signal<T, U, Q> : ISignal
    {
        public Delegate Listener
        {
            get
            {
                return listener ?? (listener = delegate { });
            }
            set
            {
                listener = (Action<T, U, Q>)value;
            }
        }

        private event Action<T, U, Q> listener;

        public void AddListener(Action<T, U, Q> callback)
        {
            listener = AddUnique(listener, callback);
        }

        public void RemoveListener(Action<T, U, Q> callback)
        {
            if (callback != null && listener.GetInvocationList().Contains(callback))
            {
                //UnityEngine.Debug.Log("Removing listener");
                listener -= callback;
            }
        }

        public void RemoveAllListeners()
        {
            listener = null;
        }

        public void Dispatch(T type, U type1, Q type3)
        {
            if (listener != null)
                listener(type, type1, type3);
        }

        private Action<T, U, Q> AddUnique(Action<T, U, Q> listeners, Action<T, U, Q> callback)
        {
            if (listeners == null || !listeners.GetInvocationList().Contains(callback))
            {
                listeners += callback;
            }
            return listeners;
        }
    }
}
