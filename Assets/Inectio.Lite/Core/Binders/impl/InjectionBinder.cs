using System;
using System.Linq;

namespace Inectio.Lite
{
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
        void RemoveView(IView view);
        void OnRemove();
    }

    public class InjectionBinder : CoreBinder, IInjectionBinder
    {
        private readonly Injector injector;

        public InjectionBinder()
        {
            injector = new Injector();
            injector.injectionBinder = this;
            Map(typeof(IInjectionBinder), this); // self inject...
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

        virtual public void RemoveView(IView view)
        {
            injector.RemoveInjections(view);
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

        virtual public void OnRemove()
        {

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
