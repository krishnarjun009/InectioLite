using System;
using System.Collections.Generic;
using System.Linq;

namespace Inectio.Lite
{
    public interface ICommandBinder
    {
        ICommandBinding Map<TKey, TValue>();
        ICommandBinding Map<TKey>();
        ICommandBinding Map(Type key, object value);
        ICommandBinding Map(Type type);
        ICommandBinding GetBinding(Type key);
        ICommandBinding GetBinding(Type key, object name);
        object GetInstance(Type key);
        object GetInstance(Type key, object name);
        T GetInstance<T>(object name);
        T GetInstance<T>();
        void ResolveBinding(IBinding binding);
    }

    public class CommandBinder : CoreBinder, ICommandBinder
    {
        [Inject] private IInjectionBinder injectionBinder { get; set; }

        private readonly Dictionary<Type, Command> poolDict;

        public CommandBinder()
        {
            poolDict = new Dictionary<Type, Command>();
        }

        new public ICommandBinding Map<TKey, TValue>()
        {
            return Map(typeof(TKey), typeof(TValue)) as ICommandBinding;
        }

        new public ICommandBinding Map(Type key, object value)
        {
            var binding = injectionBinder.GetBinding(key);
            if (binding == null)
            {
                //lets create one...
                injectionBinder.Map(key);
            }

            return base.Map(key, value) as ICommandBinding;
        }

        new public ICommandBinding Map(Type type)
        {
            return base.Map(type) as ICommandBinding;
        }

        new public ICommandBinding Map<TKey>()
        {
            return base.Map<TKey>() as ICommandBinding;
        }

        new public ICommandBinding GetBinding(Type key)
        {
            return base.GetBinding(key) as ICommandBinding;
        }

        new public ICommandBinding GetBinding(Type key, object name)
		{
            return base.GetBinding(key, name) as ICommandBinding;
		}

		public override void ResolveBinding(IBinding binding)
        {
            //if (bindings.ContainsKey(binding.Key))
            //{
            //    //UnityEngine.Debug.Log("Removing command listener to " + binding.Key);
            //    var signal = binding.Value as Signal;
            //    signal.RemoveListener(CommandHanlder);
            //}
            base.ResolveBinding(binding);
            if(bindings.ContainsKey(binding.Key))
            {
                UnityEngine.Debug.Log("Adding command listener to " + binding.Key);
                var type = binding.Key.BaseType;

                if(type.IsGenericType)
                {
                    UnityEngine.Debug.Log("Generic command " + type);
                    foreach(var t in type.GetGenericArguments())
                    {
                        UnityEngine.Debug.Log("Generic parm " + t);
                    }
                    var signal = injectionBinder.GetInstance(binding.Key) as ISignal;
                    var b = signal as IBaseSignal;
                    //b.AddListener(CommandHanlder);
                    AddDelegate(b, b, "GenericCommandHanlder");
                }
                else
                {
                    UnityEngine.Debug.Log(" Non Generic command " + type);
                    var signal = injectionBinder.GetInstance(binding.Key);
                    var b = signal as BaseSignal;
                    b.AddListener(CommandHanlder);
                    //UnityEngine.Debug.Log(" Non Generic command " + type);
                }
            }
        }

		virtual public object GetInstance(Type key)
        {
            return GetInstance(key, BindingNameType.NULL);
        }

        virtual public T GetInstance<T>()
        {
            return (T)GetInstance(typeof(T), null);
        }

        virtual public T GetInstance<T>(object name)
        {
            return (T)GetInstance(typeof(T), name);
        }

        virtual public object GetInstance(Type key, object name)
        {
            //var binding = GetBinding(key, name);
            //return injector.GetInstance(binding as IInjectionBinding);
            return null;
        }

        protected override void resolver(IBinding binding)
        {
            base.resolver(binding);
        }

        protected override IBinding GetRawBinding()
        {
            return new CommandBinding(resolver);
        }

        private void CommandHanlder()
        {
            
        }

        private void GenericCommandHanlder<T, U>(T type1, U type2)
        {
            
        }

        //both add and remove delegate methods are taken from strange since it is common function...
        private void AddDelegate(object target, IBaseSignal signal, string method)
        {
            UnityEngine.Debug.Log("Adding delegate to " + target);
            var toAdd = Delegate.CreateDelegate(signal.Listener.GetType(), target, method);
            signal.Listener = Delegate.Combine(signal.Listener, toAdd);
        }

        private void RemoveDelegate(object target, IBaseSignal signal, string method)
        {
            if (signal.GetType().BaseType.IsGenericType)
            {
                Delegate toRemove = Delegate.CreateDelegate(signal.Listener.GetType(), target, method);
                signal.Listener = Delegate.Remove(signal.Listener, toRemove);
            }
            else
            {
                //UnityEngine.Debug.Log("Removing Listener " + method.Name);
                var s = signal as BaseSignal;
                s.RemoveListener((Action)Delegate.CreateDelegate(typeof(Action), target, method));
            }
        }
    }
}
