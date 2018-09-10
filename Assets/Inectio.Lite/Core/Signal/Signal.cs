using System;
using System.Linq;

namespace Inectio.Lite
{
    public interface ISignal
    {
        Delegate Listener { get; set; }
        void RemoveAllListeners();
    }

    public interface IBaseSignal
    {
        Delegate Listener { get; set; }
        void RemoveAllListeners();
    }

    public interface IGenericCommand
    {
        int GenericValue { get; }
    }

    public class BaseSignal : IGenericCommand, IBaseSignal
    {
        private event Action commandListener;

        public int GenericValue { get { return 0; }}

        public Delegate Listener
        {
            get
            {
                return commandListener ?? (commandListener = delegate { });
            }
            set
            {
                commandListener = (Action)value;
            }
        }

        public virtual void AddListener(Action callback)
        {
            commandListener = Add(commandListener, callback);
        }

        public virtual void RemoveListener(Action callback)
        {
            if (commandListener != null || commandListener.GetInvocationList().Contains(callback))
            {
                commandListener -= callback;
            }
        }

        public virtual void Dispatch()
        {
            if (commandListener != null)
                commandListener();
        }

        private Action Add(Action _listener, Action action)
        {
            if(_listener == null || !_listener.GetInvocationList().Contains(action))
            {
                _listener += action;
            }

            return _listener;
        }

        public void RemoveAllListeners()
        {
            throw new NotImplementedException();
        }
    }

    public class BaseSignal<T> : IGenericCommand
    {
        private event Action<T> commandListener;

        public int GenericValue { get { return 1; } }

        public virtual void AddListener(Action<T> callback)
        {
            commandListener = Add(commandListener, callback);
        }

        public virtual void RemoveListener(Action<T> callback)
        {
            if (commandListener != null || commandListener.GetInvocationList().Contains(callback))
            {
                commandListener -= callback;
            }
        }

        public virtual void Dispatch(T type)
        {
            if (commandListener != null)
                commandListener(type);
        }

        private Action<T> Add(Action<T> _listener, Action<T> action)
        {
            if (_listener == null || !_listener.GetInvocationList().Contains(action))
            {
                _listener += action;
            }

            return _listener;
        }
    }

    public class Signal : BaseSignal, ISignal
    {
        new public Delegate Listener
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

        public override void AddListener(Action callback)
        {
            listener = AddUnique(listener, callback);
        }

        public override void RemoveListener(Action callback)
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

        public override void Dispatch()
        {
            if (listener != null)
                listener();
        }

        public void DispatchToAll()
        {
            if (listener != null)
                listener();
            base.Dispatch();
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
