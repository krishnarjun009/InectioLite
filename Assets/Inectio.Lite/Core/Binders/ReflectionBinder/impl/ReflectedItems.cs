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
    }

    public class PropertyAttributes
    {
        public PropertyInfo info { get; private set; }
        public object name { get; private set; }
        public Type type { get; private set; }

        public PropertyAttributes(PropertyInfo info, object name, Type type)
        {
            this.info = info;
            this.name = name;
            this.type = type;
        }
    }

    public class MethodAttributes
    {
        public MethodInfo info { get; private set; }
        public object name { get; private set; }
        public Type type { get; private set; }
        public MethodParamAttributes[] methodParams { get; private set; }
        public Type methodListenType { get; set; }

        public MethodAttributes(MethodInfo info, object name, Type type, MethodParamAttributes[] methodParams)
        {
            this.info = info;
            this.name = name;
            this.type = type;
            this.methodParams = methodParams;
        }
    }

    public class MethodParamAttributes
    {
        public ParameterInfo info { get; private set; }
        public object name { get; private set; }
        public Type type { get; private set; }

        public MethodParamAttributes(ParameterInfo info, object name, Type type)
        {
            this.info = info;
            this.name = name;
            this.type = type;
        }
    }

    public class FieldAttributes
    {
        public FieldInfo info { get; private set; }
        public object name { get; private set; }
        public Type type { get; private set; }

        public FieldAttributes(FieldInfo info, object name, Type type)
        {
            this.info = info;
            this.name = name;
            this.type = type;
        }
    }
}
