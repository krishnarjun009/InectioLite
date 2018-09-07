using System;
using System.Linq;

namespace Inectio.Lite
{
    public interface IInjectionBinder
    {
        IInjectionBinding Map<TKey, TValue>();
        IInjectionBinding Map<TKey>();
        IInjectionBinding Map(Type key, Type value);
        IInjectionBinding Map(Type type);
        IInjectionBinding GetBinding(Type key);
        IInjectionBinding GetBinding(Type key, object name);
        object GetInstance(Type key);
        object GetInstance(Type key, object name);
        T GetInstance<T>(object name);
        T GetInstance<T>();
        void TryToInject(object type);
        void ResolveBinding(IBinding binding);
    }

    public class InjectionBinder : CoreBinder, IInjectionBinder
    {
        private readonly Injector injector;

        public InjectionBinder()
        {
            injector = new Injector();
            injector.injectionBinder = this;
        }

        new public IInjectionBinding Map<TKey, TValue>()
        {
            return base.Map<TKey, TValue>() as IInjectionBinding;
        }

        new public IInjectionBinding Map(Type key, Type value)
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

        new public IInjectionBinding GetBinding(Type key, object name)
		{
            return base.GetBinding(key, name) as IInjectionBinding;
		}

		public override void ResolveBinding(IBinding binding)
        {
            base.ResolveBinding(binding);
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

        virtual public void TryToInject(object type)
        {
            injector.Inject(type);
        }

        virtual public object GetInstance(Type key, object name)
        {
            var binding = GetBinding(key, name);
            return injector.GetInstance(binding as IInjectionBinding);
        }

        protected override void resolver(IBinding binding)
        {
            base.resolver(binding);
        }

        protected override IBinding GetRawBinding()
        {
            return new InjectionBinding(resolver);
        }
    }
}
