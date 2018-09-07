using System;

namespace Inectio.Lite
{
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.Constructor | AttributeTargets.Field | AttributeTargets.Method,
        AllowMultiple = false,
        Inherited = true)]
    public class Inject : Attribute
    {
        public object name { get; set; }

        public Inject() { }

        public Inject(object name)
        {
            this.name = name;
        }
    }

    [AttributeUsage(AttributeTargets.Method,
        AllowMultiple = false,
        Inherited = true)]
    public class PostConstruct : Attribute
    {
        public int index { get; set; }

        public PostConstruct() { }

        public PostConstruct(int index)
        {
            this.index = index;
        }
    }

    [AttributeUsage(AttributeTargets.Method,
        AllowMultiple = false,
        Inherited = true)]
    public class Listen : Attribute
    {
        public Signal signal { get; set; }

        public Listen(Signal signal) 
        {
            this.signal = signal;
        }

    }
}
