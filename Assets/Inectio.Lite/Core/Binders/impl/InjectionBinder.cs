using System;
using System.Linq;
 
namespace Inectio.Lite
{
    public class InectioAutoEnableDisableSignal : Signal<bool, IView> { }

    public interface IInjectionBinder
    {
        IInjectionBinding Map<TKey, TValue>();
        IInjectionBinding Map<TKey>();
        IInjectionBinding Map(Type key, object value);
        IInjectionBinding Map(Type type);
        IInjectionBinding GetBinding(Type key);
        IInjectionBinding GetBinding(Type key, string name);
        void UnBind<T>();
        void UnBind(Type key);
        object GetInstance(Type key);
        object GetInstance(Type key, string name);
        object GetInstance(IInjectionBinding binding);
        T GetInstance<T>(string name);
        T GetInstance<T>();
        void TryToInject(object type);
        void ResolveBinding(IBinding binding);
        void OnRemove(IView view);
        void OnAutoSignalHandler(bool enable, IView view);
    }

    public class InjectionBinder : CoreBinder, IInjectionBinder
    {
        private readonly Injector injector;
        [Inject] private InectioAutoEnableDisableSignal autoEnableDisableSignal { get; set; }

        public InjectionBinder()
        {
            injector = new Injector();
            injector.injectionBinder = this;
            Map(typeof(IInjectionBinder), this); // self inject...
            Map<InectioAutoEnableDisableSignal>();
            //autoEnableDisableSignal = new InectioAutoEnableDisableSignal();
            GetInstance<InectioAutoEnableDisableSignal>().AddListener(OnAutoSignalHandler);
        }

        public Injector GetInjector()
        {
            return injector;
        }

        new public IInjectionBinding Map<TKey, TValue>()
        {
            return base.Map<TKey, TValue>() as IInjectionBinding;
        }

        new public IInjectionBinding Map(Type key, object value)
        {
            return base.Map(key, value) as IInjectionBinding;
        }

        new public IInjectionBinding Map(Type type)
        {
            return base.Map(type) as IInjectionBinding;
        }

        new public IInjectionBinding Map<TKey>()
        {
            return base.Map<TKey>() as IInjectionBinding;
        }

        new public IInjectionBinding GetBinding(Type key)
        {
            return base.GetBinding(key) as IInjectionBinding;
        }

        new public IInjectionBinding GetBinding(Type key, string name)
		{
            return base.GetBinding(key, name) as IInjectionBinding;
		}

        virtual public void UnBind<T>()
        {
            UnBind(typeof(T));
        }

        virtual public void UnBind(Type key)
        {
            if (bindings.ContainsKey(key))
            {
                bindings.Remove(key);
            }
        }

		public override void ResolveBinding(IBinding binding)
        {
            base.ResolveBinding(binding);
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

        virtual public void TryToInject(object type)
        {
            injector.Inject(type);
        }

        virtual public void OnRemove(IView view)
        {
            injector.OnRemove(view);
        }

        virtual public object GetInstance(Type key, string name)
        {
            var binding = GetBinding(key, name);
            return injector.GetInstance(binding as IInjectionBinding);
        }

        virtual public object GetInstance(IInjectionBinding binding)
        {
            return injector.GetInstance(binding);
        }

        protected override void resolver(IBinding binding)
        {
            base.resolver(binding);
        }

        protected override IBinding GetRawBinding()
        {
            return new InjectionBinding(resolver);
        }

        public void OnAutoSignalHandler(bool enable, IView view)
        {
            injector.RemoveAutoSignals(view, enable);
        }
    }
}
