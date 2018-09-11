using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Inectio.Lite
{
    public interface ICommandBinder
    {
        ICommandBinding Map<TKey, TValue>();
        ICommandBinding Map<TKey>();
        ICommandBinding Map(Type key, object value);
        ICommandBinding Map(Type type);
        ICommandBinding GetBinding(Type key);
        ICommandBinding GetBinding(Type key, string name);
        //object GetInstance(Type key);
        //object GetInstance(Type key, string name);
        //T GetInstance<T>(string name);
        //T GetInstance<T>();
        void ResolveBinding(IBinding binding);
        void OnRemove();
    }

    public class CommandBinder : CoreBinder, ICommandBinder
    {
        [Inject] private IInjectionBinder injectionBinder { get; set; }

        private readonly Dictionary<Type, ICommand> poolDict;

        public CommandBinder()
        {
            poolDict = new Dictionary<Type, ICommand>();
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
            if(bindings.ContainsKey(key))
            {
                throw new InectioException(key + " type has already command mapping");
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

        new public ICommandBinding GetBinding(Type key, string name)
		{
            return base.GetBinding(key, name) as ICommandBinding;
		}

		public override void ResolveBinding(IBinding binding)
        {
            base.ResolveBinding(binding);
            if(bindings.ContainsKey(binding.Key))
            {
                var type = binding.Key.BaseType;
                var method = getGenericMethod(binding.Key);
                var b = injectionBinder.GetBinding(binding.Key);
                var signal = injectionBinder.GetInstance(binding.Key) as IBaseSignal;
                RemoveDelegate(this, signal, method);
                AddDelegate(this, signal, method);
            }
        }

		virtual public object GetInstance(Type key)
        {
            return GetInstance(key, "");
        }

        virtual public T GetInstance<T>()
        {
            return (T)GetInstance(typeof(T), null);
        }

        virtual public T GetInstance<T>(string name)
        {
            return (T)GetInstance(typeof(T), name);
        }

        virtual public object GetInstance(Type key, string name)
        {
            //var binding = GetBinding(key, name);
            //return injector.GetInstance(binding as IInjectionBinding);
            return null;
        }

        virtual public void OnRemove()
        {
            //UnityEngine.Debug.Log("OnRemove from command binder");
            foreach(var binding in bindings)
            {
                var method = getGenericMethod(binding.Key);
                var b = injectionBinder.GetBinding(binding.Key);
                RemoveDelegate(this, b.Value as IBaseSignal, method);
            }
        }

        protected override void resolver(IBinding binding)
        {
            base.resolver(binding);
        }

        protected override IBinding GetRawBinding()
        {
            return new CommandBinding(resolver);
        }

        private void GenericCommandZero(IBaseSignal signal)
        {
            var command = getCommand(signal.GetType());
            (command as Command).Execute();
        }

        private void GenericCommandOne<T>(IBaseSignal signal, T type1)
        {
            var command = getCommand(signal.GetType());
            (command as Command<T>).Execute(type1);
        }

        private void GenericCommandTwo<T, U>(IBaseSignal signal, T type1, U type2)
        {
            var command = getCommand(signal.GetType());
            (command as Command<T, U>).Execute(type1, type2);
        }

        private void GenericCommandThree<T, U, V>(IBaseSignal signal, T type1, U type2, V type3)
        {
            var command = getCommand(signal.GetType());
            (command as Command<T, U, V>).Execute(type1, type2, type3);
        }

        private void GenericCommandFour<T, U, V, W>(IBaseSignal signal, T type1, U type2, V type3, W type4)
        {
            var command = getCommand(signal.GetType());
            (command as Command<T, U, V, W>).Execute(type1, type2, type3, type4);
        }

        protected ICommand getCommand(Type key)
        {
            var binding = GetBinding(key);
            if(binding.IsPooled)
            {
                if(!poolDict.ContainsKey(key))
                {
                    //UnityEngine.Debug.Log("Creating instance First time");
                    var o = binding.Value;
                    Type value = (o is Type) ? o as Type : o.GetType();
                    //Console.WriteLine("Creating instacne of " + value);
                    var c = Activator.CreateInstance(value, null) as ICommand;
                    poolDict[key] = c;
                }

                //UnityEngine.Debug.Log("Getting command from pool");
                return poolDict[key];
            }

            //make sure if you have command usage prequently, better to make it as pooled() to stay away from GC
            injectionBinder.Map(typeof(ICommand), binding.Value);
            var command = injectionBinder.GetInstance(typeof(ICommand));
            injectionBinder.UnBind(key);
            return command as ICommand;
        }

        //both add and remove delegate methods are taken from strange since it is common function...
        private void AddDelegate(object target, IBaseSignal signal, MethodInfo method)
        {
            //UnityEngine.Debug.Log("Adding delegate to " + target);
            if (signal.GetType().BaseType.IsGenericType)
            {
                //UnityEngine.Debug.Log("adding delegate from command " + signal);
                var toAdd = Delegate.CreateDelegate(signal.Listener.GetType(), target, method);
                signal.Listener = Delegate.Combine(signal.Listener, toAdd);
            }
            else
            {
                //UnityEngine.Debug.Log("adding delegate to " + signal);
                var s = signal as BaseSignal;
                s.AddListener((Action<IBaseSignal>)Delegate.CreateDelegate(typeof(Action<IBaseSignal>), target, method));
            }
        }

        private void RemoveDelegate(object target, IBaseSignal signal, MethodInfo method)
        {
            if (signal.GetType().BaseType.IsGenericType)
            {
                //UnityEngine.Debug.Log("removing delegate to " + signal);
                Delegate toRemove = Delegate.CreateDelegate(signal.Listener.GetType(), target, method);
                signal.Listener = Delegate.Remove(signal.Listener, toRemove);
            }

            else
            {
                //UnityEngine.Debug.Log("removing delegate to " + signal);
                var s = signal as BaseSignal;
                s.RemoveListener((Action<IBaseSignal>)Delegate.CreateDelegate(typeof(Action<IBaseSignal>), target, method));
            }
        }

        private MethodInfo getGenericMethod(Type key)
        {
            var binding = GetBinding(key);
            var type = binding.Key.BaseType;
            var gtype = this.GetType();
            var flags =  BindingFlags.FlattenHierarchy |
                         BindingFlags.SetProperty |
                         BindingFlags.Public |
                         BindingFlags.NonPublic |
                         BindingFlags.Instance;
            if (type.IsGenericType)
            {
                var gParams = type.GetGenericArguments();
                var signal = injectionBinder.GetInstance(binding.Key);
                switch (gParams.Length)
                {
                    case 1:
                        return gtype.GetMethod("GenericCommandOne", flags).MakeGenericMethod(gParams);
                    case 2:
                        return gtype.GetMethod("GenericCommandTwo", flags).MakeGenericMethod(gParams);
                    case 3:
                        return gtype.GetMethod("GenericCommandThree", flags).MakeGenericMethod(gParams);
                    case 4:
                        return gtype.GetMethod("GenericCommandFour", flags).MakeGenericMethod(gParams);
                }
            }

            return gtype.GetMethod("GenericCommandZero", flags);
        }
    }
}
