using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Inectio.Lite
{
    public class ReflectionBinder : CoreBinder, IReflectionBinder
    {
        public ReflectionBinder()
        {
        }

        public ReflectedItems Get(Type type)
        {
            var binding = GetBinding(type);
            if(binding == null)
            {
                //Console.WriteLine("Reflected once");
                binding = GetRawBinding();
                var reflected = new ReflectedItems();
                mapProperties(reflected, type);
                mapFields(reflected, type);
                mapMethods(reflected, type);
                binding.Map(type, reflected);//.Bind();
            }

            return binding.Value as ReflectedItems;
        }

		private void mapProperties(ReflectedItems reflected, Type type)
        {
            //var properties = new List<PropertyAttributes>();
            var members = type.FindMembers(MemberTypes.Property,
                                                             BindingFlags.FlattenHierarchy |
                                                             BindingFlags.SetProperty |
                                                             BindingFlags.Public |
                                                             BindingFlags.NonPublic |
                                                             BindingFlags.Instance,
                                                             null, null);

            var properties = new Dictionary<object, PropertyAttributes>();

            foreach (var member in members)
            {
                object[] injections = member.GetCustomAttributes(typeof(Inject), true);
                if (injections.Length > 0)
                {
                    var attr = injections[0] as Inject;
                    var point = member as PropertyInfo;
                    var baseType = member.DeclaringType.BaseType;
                    bool hasInheritedProperty = baseType != null ? baseType.GetProperties().Any(p => p.Name == point.Name) : false;
                    bool toAddOrOverride = true;
                    //if we have an overriding value, we need to know whether to override or leave it out.
                    //We leave out the base if it's hidden
                    if (properties.ContainsKey(point.Name))
                        toAddOrOverride = hasInheritedProperty; //if this attribute has been 'hidden' by a new or override keyword, we should not add this.

                    if (toAddOrOverride)
                        properties[point.Name] = new PropertyAttributes(point, attr.name, point.PropertyType);
                }
            }

            reflected.properties = properties.Values.ToArray();
        }

        private void mapFields(ReflectedItems reflected, Type type)
        {
            //var fields = new List<FieldAttributes>();
            var members = type.FindMembers(MemberTypes.Field,
                                                             BindingFlags.FlattenHierarchy |
                                                             BindingFlags.SetProperty |
                                                             BindingFlags.Public |
                                                             BindingFlags.NonPublic |
                                                             BindingFlags.Instance,
                                                             null, null);

            var fields = new Dictionary<object, FieldAttributes>();

            foreach (var member in members)
            {
                object[] injections = member.GetCustomAttributes(typeof(Inject), true);
                if (injections.Length > 0)
                {
                    var attr = injections[0] as Inject;
                    var point = member as FieldInfo;
                    var baseType = member.DeclaringType.BaseType;
                    bool hasInheritedProperty = baseType != null ? baseType.GetFields().Any(p => p.Name == point.Name) : false;
                    bool toAddOrOverride = true;
                    //if we have an overriding value, we need to know whether to override or leave it out.
                    //We leave out the base if it's hidden
                    if (fields.ContainsKey(point.Name))
                        toAddOrOverride = hasInheritedProperty; //if this attribute has been 'hidden' by a new or override keyword, we should not add this.

                    if (toAddOrOverride)
                        fields[point.Name] = new FieldAttributes(point, attr.name, point.FieldType);
                }
            }

            reflected.fields = fields.Values.ToArray();
        }

        private void mapMethods(ReflectedItems reflected, Type type)
        {
            var methods = new List<MethodAttributes>();
            var members = type.FindMembers(MemberTypes.Method,
                                                             BindingFlags.FlattenHierarchy |
                                                             BindingFlags.SetProperty |
                                                             BindingFlags.Public |
                                                             BindingFlags.NonPublic |
                                                             BindingFlags.Instance,
                                                             null, null);

            foreach (var member in members)
            {
                object[] injections = member.GetCustomAttributes(typeof(Inject), true);
                if (injections.Length > 0)
                {
                    var attr = injections[0] as Inject;
                    var point = member as MethodInfo;
                    var parms = point.GetParameters();
                    var length = parms.Length;
                    var parmAttributes = new MethodParamAttributes[length];
                    for (int i = 0; i < length; i++)
                    {
                        object[] pInections = parms[i].GetCustomAttributes(typeof(Inject), true);
                        if (pInections.Length > 0)
                        {
                            var pAttr = pInections[0] as Inject;
                            parmAttributes[i] = new MethodParamAttributes(parms[i], pAttr.name, parms[i].ParameterType);
                            //Console.WriteLine("Inject attribute found inside method param");
                        }
                        else
                        {
                            parmAttributes[i] = new MethodParamAttributes(parms[i], null, parms[i].ParameterType);
                        }
                    }
                    methods.Add(new MethodAttributes(point, attr.name, point.DeclaringType, parmAttributes));
                }
            }

            reflected.methods = methods.ToArray();
        }
    }
}
