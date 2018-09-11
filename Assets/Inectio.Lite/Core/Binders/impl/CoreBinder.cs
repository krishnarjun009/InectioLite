using System;
using System.Collections.Generic;

namespace Inectio.Lite
{
    public class CoreBinder : ICoreBinder, IDisposable
    {        
        public enum BindingNameType
        {
            NULL
        }
        
        protected readonly Dictionary<Type, Dictionary<string, IBinding>> bindings;
        private readonly Dictionary<IBinding, object> conflicts;

        public delegate void Resolver(IBinding binding);

        public CoreBinder()
        {
            bindings = new Dictionary<Type, Dictionary<string, IBinding>>();
            conflicts = new Dictionary<IBinding, object>();
        }

        virtual public IBinding Map<TKey, TValue>()
        {
            return Map(typeof(TKey), typeof(TValue));
        }

        virtual public IBinding Map<TKey>()
        {
            return Map(typeof(TKey), null);
        }

        virtual public IBinding Map(Type type)
        {
            return Map(type, null);
        }

        virtual public IBinding Map(Type key, object value)
        {
            var binding = GetRawBinding();
            binding.Map(key, value);
            return binding;
        }

        virtual public void ResolveBinding(IBinding binding)
        {
            string name = String.IsNullOrEmpty(binding.Name) ? "" : binding.Name;
            Dictionary<string, IBinding> values;
            //IBinding prevBinding = null;

            //if(!conflicts.ContainsKey(binding))
            //{
            //    conflicts[binding] = _name;
            //}

            if(bindings.ContainsKey(binding.Key))
            {
                //do it later
                //todo: validate binding names
                values = bindings[binding.Key];
                //if(values.ContainsKey(_name))
                //{
                //    if (prevBinding != null)
                //    {
                //        if (prevBinding == binding)
                //        {
                //            if (conflicts.ContainsKey(binding))
                //            {
                //                if (conflicts[binding] != binding.Name)
                //                    conflicts.Remove(binding);
                //            }
                //        }
                //        else
                //        {

                //        }
                //    }
                //}
                //else
                //{
                //    //leave it...
                //}
            }
            else
            {
                values = new Dictionary<string, IBinding>();
                bindings[binding.Key] = values;
            }

            //Remove nulloid bindings
            if (values.ContainsKey(name) && values[name].Equals(binding))
            {
                //UnityEngine.Debug.Log("Removing binding " + binding.Key);
                values.Remove(name);
            }

            //do not add duplicate bindings...
            if(!values.ContainsKey(name))
            {
                values[name] = binding;
            }

            //prevBinding = binding;
        }

        virtual public IBinding GetBinding(Type key)
        {
            return GetBinding(key, null);
        }

        virtual public IBinding GetBinding(Type key, string name)
        {
            name = String.IsNullOrEmpty(name) ? "" : name;
            if (bindings.ContainsKey(key))
            {
                var values = bindings[key];
                if (values.ContainsKey(name))
                    return values[name];
            }

            return null;
        }

        public void Debug()
        {
            foreach(var binding in bindings)
            {
                foreach(var b in binding.Value)
                    Console.WriteLine(b.Value.Key + " to " + b.Value.Value);
            }
        }

        virtual protected void resolver(IBinding binding)
        {
            ResolveBinding(binding);
            //ThrowConflictException();
        }

        virtual protected IBinding GetRawBinding()
        {
            return new Binding(resolver);
        }

        // Private Section
        private void ThrowConflictException()
        {
            if(conflicts.Count > 0)
            {
                foreach(var conflict in conflicts)
                {
                    throw new InectioException(conflict.Value + " is duplicate name for " + conflict.Key);
                }
            }
        }

        public void Dispose()
        {
            UnityEngine.Debug.Log("Disposing Bindings");
            bindings.Clear();
        }

        ~ CoreBinder()
        {
            Dispose();
        }
    }
}
