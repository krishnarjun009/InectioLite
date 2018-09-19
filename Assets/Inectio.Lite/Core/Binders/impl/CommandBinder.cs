using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Iniectio.Lite
{
    public interface ICommandBinder
    {
        ICommandBinding Map<TKey, TValue>();
        //ICommandBinding Map<TKey>();
        ICommandBinding Map(Type key, object value);
        //ICommandBinding Map(Type type);
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

        private readonly Dictionary<Type, Queue<ICommand>> pool = new Dictionary<Type, Queue<ICommand>>();

        public CommandBinder()
        {
            
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
                //UnityEngine.Debug.Log("Mapping from command " + key);
            }
            if(bindings.ContainsKey(key)) // SUPPORT ME FOR MULTIPLE COMMAND BINDINGS FOR SAME SIGNAL...
            {
                throw new InectioException(key + " type already has command mapping");
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
                RemoveDelegate(this, signal, method); // remove previous delegate
                AddDelegate(this, signal, method);

                if((binding as ICommandBinding).IsPooled)
                {
                    var t = binding.Value as Type;
                    if (!pool.ContainsKey(t))
                    {
                        pool[t] = new Queue<ICommand>();
                    }
                }
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
            foreach(var binding in bindings)
            {
                var method = getGenericMethod(binding.Key);
                var b = injectionBinder.GetBinding(binding.Key);
                RemoveDelegate(this, b.Value as IBaseSignal, method);
            }

            bindings.Clear();
            pool.Clear();
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
            releaseCommand(command);
        }

        private void GenericCommandOne<T>(IBaseSignal signal, T type1)
        {
            var command = getCommand(signal.GetType());
            (command as Command<T>).Execute(type1);
            releaseCommand(command);
        }

        private void GenericCommandTwo<T, U>(IBaseSignal signal, T type1, U type2)
        {
            var command = getCommand(signal.GetType());
            (command as Command<T, U>).Execute(type1, type2);
            releaseCommand(command);
        }

        private void GenericCommandThree<T, U, V>(IBaseSignal signal, T type1, U type2, V type3)
        {
            var command = getCommand(signal.GetType());
            (command as Command<T, U, V>).Execute(type1, type2, type3);
            releaseCommand(command);
        }

        private void GenericCommandFour<T, U, V, W>(IBaseSignal signal)
        {
            var command = getCommand(signal.GetType());
            var bs = signal as BaseSignal<T, U, V, W>; // exception case
            (command as Command<T, U, V, W>).Execute(bs.Type1, bs.Type2, bs.Type3, bs.Type4);
            releaseCommand(command);
        }

        protected ICommand createCommandForPool(Type type)
        {
            injectionBinder.Map(typeof(ICommand), type);
            var cmd = injectionBinder.GetInstance(typeof(ICommand)) as ICommand;
            injectionBinder.GetInjector().Inject(cmd, true);
            injectionBinder.UnBind(typeof(ICommand));
            return cmd;
        }

        protected ICommand getCommand(Type t)
        {
            var binding = GetBinding(t);
            var type = binding.Value as Type;
            if(pool.ContainsKey(type))
            {
                if(pool[type].Count == 0)
                {
                    //create one and give...
                    return createCommandForPool(type);
                }
                return pool[type].Dequeue();
            }

            //make sure if you have command usage prequently, better to make it as pooled() to stay away from GC
            var cmdb = injectionBinder.GetBinding(typeof(ICommand));
            if(cmdb == null)
            {
                cmdb = injectionBinder.Map(typeof(ICommand), type).ToMultiple();
            }

            var command = injectionBinder.GetInstance(cmdb);
            injectionBinder.TryToInject(command);
            return command as ICommand;
            //return createCommandForPool(type); // will cause 10 times more GC...
        }

        protected void releaseCommand(ICommand command)
        {
            var t = command.GetType();
            if(pool.ContainsKey(t))
            {
                pool[t].Enqueue(command);
            }
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
            var flags = BindingFlags.FlattenHierarchy |
                         BindingFlags.SetProperty |
                         BindingFlags.Public |
                         BindingFlags.NonPublic |
                         BindingFlags.Instance;

            var gParams = type.GetGenericArguments();
            switch (gParams.Length)
            {
                case 0:
                    return gtype.GetMethod("GenericCommandZero", flags);
                case 1:
                    return gtype.GetMethod("GenericCommandOne", flags).MakeGenericMethod(gParams);
                case 2:
                    return gtype.GetMethod("GenericCommandTwo", flags).MakeGenericMethod(gParams);
                case 3:
                    return gtype.GetMethod("GenericCommandThree", flags).MakeGenericMethod(gParams);
                default:
                    return gtype.GetMethod("GenericCommandFour", flags).MakeGenericMethod(gParams);
            }
        }
    }
}
