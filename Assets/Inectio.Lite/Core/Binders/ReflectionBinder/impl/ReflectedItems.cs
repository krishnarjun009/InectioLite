using System;
using System.Reflection;

namespace Inectio.Lite
{
    public class ReflectedItems
    {
        //all possible injections
        public PropertyAttributes[] properties { get; set; }
        public FieldAttributes[] fields { get; set; }
        //public ConstructorInfo[] constructors { get; set; }
        public MethodAttributes[] methods { get; set; }
        public AutoListenMethodAttributes[] listenMethods { get; set; }
        public bool preReflected { get; set; }
    }

    public class AutoListenMethodAttributes
    {
        public MethodInfo info { get; private set; }
        public Type type { get; private set; }
        public Listen.ListenType listenType { get; private set; }

        public AutoListenMethodAttributes(MethodInfo info, Type type, Listen.ListenType listenType)
        {
            this.type = type;
            this.listenType = listenType;
            this.info = info;
        }
    }

    public class PropertyAttributes
    {
        public PropertyInfo info { get; private set; }
        public string name { get; private set; }
        public Type type { get; private set; }

        public PropertyAttributes(PropertyInfo info, string name, Type type)
        {
            this.info = info;
            this.name = name;
            this.type = type;
        }
    }

    public class MethodAttributes
    {
        public MethodInfo info { get; private set; }
        public string name { get; private set; }
        public Type type { get; private set; }
        public MethodParamAttributes[] methodParams { get; private set; }
        public ListenMethodAttributes listenMethodAttributes { get; set; }

        public MethodAttributes(MethodInfo info, string name, Type type, MethodParamAttributes[] methodParams)
        {
            this.info = info;
            this.name = name;
            this.type = type;
            this.methodParams = methodParams;
        }
    }

    public class ListenMethodAttributes
    {
        public Type type { get; private set; }
        public Listen.ListenType listenType { get; private set; }

        public ListenMethodAttributes(Type type, Listen.ListenType listenType)
        {
            this.type = type;
            this.listenType = listenType;
        }
    }

    public class MethodParamAttributes
    {
        public ParameterInfo info { get; private set; }
        public string name { get; private set; }
        public Type type { get; private set; }

        public MethodParamAttributes(ParameterInfo info, string name, Type type)
        {
            this.info = info;
            this.name = name;
            this.type = type;
        }
    }

    public class FieldAttributes
    {
        public FieldInfo info { get; private set; }
        public string name { get; private set; }
        public Type type { get; private set; }

        public FieldAttributes(FieldInfo info, string name, Type type)
        {
            this.info = info;
            this.name = name;
            this.type = type;
        }
    }
}
