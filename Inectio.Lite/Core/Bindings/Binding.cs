using System;

namespace Inectio.Lite
{
    public enum InstanceType
    {
        ToSignle,
        ToMultiple
    }

    public interface IBinding
    {
        IBinding Map(Type key, object value);
        IBinding ToName(object name);
        IBinding ToValue(object value);
        IBinding ToSingle();
        IBinding ToMultiple();

        Type Key { get; }
        object Value { get; }
        object Name { get; }
        InstanceType Instance { get; }
    }

    public class Binding : IBinding
    {
        protected CoreBinder.Resolver resolver;
        protected Type key;
        protected object value;
        protected object name;
        protected InstanceType instanceType;

        public Type Key { get { return key; } }
        public object Value { get { return value; } }
        public object Name { get { return name; } }
        public InstanceType Instance { get { return instanceType; }}

        public Binding(CoreBinder.Resolver resolver)
        {
            this.resolver = resolver;
        }

        virtual public IBinding Map(Type key, object value)
        {
            instanceType = InstanceType.ToSignle; // by default...
            this.key = key;
            this.value = value;
            Resolve();
            return this;
        }

        virtual public IBinding ToName(object name)
        {
            this.name = name;
            Resolve();
            return this;
        }

        virtual public IBinding ToValue(object value)
        {
            this.value = value;
            Resolve();
            return this;
        }

        virtual public IBinding ToSingle()
        {
            instanceType = InstanceType.ToSignle;
            Resolve();
            return this;
        }

        virtual public IBinding ToMultiple()
        {
            instanceType = InstanceType.ToMultiple;
            Resolve();
            return this;
        }

        protected void Resolve()
        {
            if (resolver != null)
                resolver(this);
        }
    }
}
