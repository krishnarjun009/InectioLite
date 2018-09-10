using System;

namespace Inectio.Lite
{
    public interface IInjectionBinding
    {
        IInjectionBinding Map(Type key, object value);
        void SetValue(object value);
        IInjectionBinding ToName(string name);
        IInjectionBinding ToValue(object value);
        IInjectionBinding ToSingle();
        IInjectionBinding ToMultiple();
        Type Key { get; }
        object Value { get; }
        string Name { get; }
        InstanceType Instance { get; }
    }

    public class InjectionBinding : Binding, IInjectionBinding
    {
        public InjectionBinding(CoreBinder.Resolver resolver) : base(resolver)
        {
        }

        public void SetValue(object value)
        {
            this.value = value;
        }

        new public IInjectionBinding Map(Type key, object value)
		{
            return base.Map(key, value) as IInjectionBinding;
		}

        new public IInjectionBinding ToName(string name)
        {
            return base.ToName(name) as IInjectionBinding;
        }

        new public IInjectionBinding ToValue(object value)
        {
            return base.ToValue(value) as IInjectionBinding;
        }

        new public IInjectionBinding ToSingle()
		{
            return base.ToSingle() as IInjectionBinding;
		}

        new public IInjectionBinding ToMultiple()
		{
            return base.ToMultiple() as IInjectionBinding;
		}
	}
}
