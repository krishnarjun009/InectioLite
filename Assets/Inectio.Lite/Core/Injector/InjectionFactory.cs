using System;

namespace Inectio.Lite
{
    public class InjectionFactory
    {
        public object GetInstance(IInjectionBinding binding)
        {
            return InstanceOf(binding);
        }

        private object GetImplicitly(object key, object[] args)
        {
            Type type = key as Type;
            if (!type.IsInterface && !type.IsAbstract && !type.IsPrimitive)
            {
                return CreateFromValue(key, args);
            }

            throw new InectioException("InjectorFactory can't instantiate an Interface or Abstract Class or Primitive. Class: " + key.ToString());
        }

        public object CreateInstance(object value)
        {
            if (value != null)
            {
                if (value.GetType().IsInstanceOfType(typeof(Type)))
                {
                    object o = CreateFromValue(value, null);
                    if (o == null)
                        return null;
                    return o;
                }
            }

            return GetImplicitly(value, null);
        }

        private object InstanceOf(IInjectionBinding binding)
        {
            if (binding == null)
                throw new InectioException("Attempt to instantiate null binding");

            if (binding.Value != null)
            {
                if (CheckForAssignee(binding.Key, binding.Value))
                {
                    //its fine...

                    if (binding.Value.GetType().IsInstanceOfType(typeof(Type)))
                    {
                        //UnityEngine.Debug.Log("Instance ");
                        object o = CreateFromValue(binding.Value, null);
                        if (o == null)
                            return null;

                        if (binding.Instance != InstanceType.ToSignle)
                            return o;
                        binding.SetValue(o);
                    }
                }
                else
                    throw new InectioException(binding.Value + " is not Implementing of Type " + binding.Key);
            }
            else
            {
                var o = GetImplicitly(binding.Key, null);
                if (binding.Instance != InstanceType.ToSignle)
                    return o;
                binding.SetValue(o);
            }

            return binding.Value;
        }

        private bool CheckForAssignee(Type key, object value)
        {
            //check for value subclass
            Type t = (value is Type) ? value as Type : value.GetType();
            if (key.IsInterface)
            {
                var interfaces = t.GetInterfaces();
                foreach (var i in interfaces)
                {
                    if (i == key)
                    {
                        return true;
                    }
                }
            }
            else if (t.IsAssignableFrom(key) || t.IsSubclassOf(key))
            {
                return true;
            }

            return false;
        }

        private object CreateFromValue(object o, object[] args)
        {
            Type value = (o is Type) ? o as Type : o.GetType();
            //Console.WriteLine("Creating instacne of " + value);
            return Activator.CreateInstance(value, args);
        }
    }
}
