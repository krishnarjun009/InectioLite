using System;

namespace Iniectio.Lite
{
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.Constructor | AttributeTargets.Field | AttributeTargets.Method,
        AllowMultiple = false,
        Inherited = true)]
    public class Inject : Attribute
    {
        public string name { get; set; }

        public Inject() { }

        public Inject(string name)
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
        public enum ListenType
        {
            AUTO,
            MANUAL
        };

        public Type type { get; set; }
        public ListenType listenType { get; set; }

        public Listen(Type type) 
        {
            this.type = type;
            listenType = ListenType.MANUAL; // by default we setting it to manual.
        }

        public Listen(Type type, ListenType listenType)
        {
            this.type = type;
            this.listenType = listenType;
        }
    }
}
