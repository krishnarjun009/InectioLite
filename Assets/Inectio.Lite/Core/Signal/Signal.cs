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

    public class BaseSignal : IBaseSignal
    {
        private event Action<IBaseSignal> commandListener;
        public Delegate Listener
        {
            get
            {
                return commandListener ?? (commandListener = delegate { });
            }
            set
            {
                commandListener = (Action<IBaseSignal>)value;
            }
        }

        public virtual void AddListener(Action<IBaseSignal> callback)
        {
            commandListener = Add(commandListener, callback);
        }

        public virtual void RemoveListener(Action<IBaseSignal> callback)
        {
            if (commandListener != null && commandListener.GetInvocationList().Contains(callback))
            {
                commandListener -= callback;
            }
        }

        public virtual void Dispatch()
        {
            if (commandListener != null)
                commandListener(this);
        }

        private Action<IBaseSignal> Add(Action<IBaseSignal> _listener, Action<IBaseSignal> action)
        {
            if(_listener == null || !_listener.GetInvocationList().Contains(action))
            {
                _listener += action;
            }

            return _listener;
        }

        public void RemoveAllListeners()
        {
            commandListener = null;
        }
    }

    public class BaseSignal<T> : IBaseSignal
    {
        private event Action<IBaseSignal, T> commandListener;
        public Delegate Listener
        {
            get
            {
                return commandListener ?? (commandListener = delegate { });
            }
            set
            {
                commandListener = (Action<IBaseSignal, T>)value;
            }
        }

        public virtual void AddListener(Action<IBaseSignal, T> callback)
        {
            commandListener = Add(commandListener, callback);
        }

        public virtual void RemoveListener(Action<IBaseSignal, T> callback)
        {
            if (commandListener != null && commandListener.GetInvocationList().Contains(callback))
            {
                commandListener -= callback;
            }
        }

        public virtual void Dispatch(T type)
        {
            if (commandListener != null)
                commandListener(this, type);
        }

        private Action<IBaseSignal, T> Add(Action<IBaseSignal, T> _listener, Action<IBaseSignal, T> action)
        {
            if (_listener == null || !_listener.GetInvocationList().Contains(action))
            {
                _listener += action;
            }

            return _listener;
        }

        public void RemoveAllListeners()
        {
            commandListener = null;
        }
    }

    public class BaseSignal<T, U> : IBaseSignal
    {
        private event Action<IBaseSignal, T, U> commandListener;

        public Delegate Listener
        {
            get
            {
                return commandListener ?? (commandListener = delegate { });
            }
            set
            {
                commandListener = (Action<IBaseSignal, T, U>)value;
            }
        }

        public virtual void AddListener(Action<IBaseSignal, T, U> callback)
        {
            commandListener = Add(commandListener, callback);
        }

        public virtual void RemoveListener(Action<IBaseSignal, T, U> callback)
        {
            if (commandListener != null && commandListener.GetInvocationList().Contains(callback))
            {
                commandListener -= callback;
            }
        }

        public virtual void Dispatch(T type, U type2)
        {
            if (commandListener != null)
                commandListener(this, type, type2);
        }

        private Action<IBaseSignal, T, U> Add(Action<IBaseSignal, T, U> _listener, Action<IBaseSignal, T, U> action)
        {
            if (_listener == null || !_listener.GetInvocationList().Contains(action))
            {
                _listener += action;
            }

            return _listener;
        }

        public void RemoveAllListeners()
        {
            commandListener = null;
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

        new public void RemoveAllListeners()
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

    public class Signal<T> : BaseSignal<T>, ISignal
    {
        new public Delegate Listener
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

        new public void RemoveAllListeners()
        {
            listener = null;
        }

        public void Dispatch(T type)
        {
            if (listener != null)
                listener(type);
        }

        public void DispatchToAll(T type)
        {
            if (listener != null)
                listener(type);
            base.Dispatch(type);
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

    public class Signal<T, U> : BaseSignal<T, U>, ISignal
    {
        new public Delegate Listener
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

        new public void RemoveAllListeners()
        {
            listener = null;
        }

        public override void Dispatch(T type, U type2)
        {
            if (listener != null)
                listener(type, type2);
            base.Dispatch(type, type2);
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
