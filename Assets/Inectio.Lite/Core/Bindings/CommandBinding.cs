using System;
using System.Collections.Generic;

namespace Inectio.Lite
{
    public interface ICommandBinding
    {
        ICommandBinding Map(Type key, object value);
        ICommandBinding ToName(string name);
        ICommandBinding Pooled();
        Type Key { get; }
        object Value { get; }
        string Name { get; }
        bool IsPooled { get; }
    }

    public class CommandBinding : Binding, ICommandBinding
    {
        private bool pooled = false;
        public bool IsPooled { get { return pooled; }}

        public CommandBinding(CoreBinder.Resolver resolver) : base(resolver)
        {
        }

        new public ICommandBinding Map(Type key, object value)
		{
            return base.Map(key, value) as ICommandBinding;
		}

        new public ICommandBinding ToName(string name)
        {
            return base.ToName(name) as ICommandBinding;
        }

        virtual public ICommandBinding Pooled()
        {
            pooled = true;
            Resolve();
            return this;
        }
	}
}
