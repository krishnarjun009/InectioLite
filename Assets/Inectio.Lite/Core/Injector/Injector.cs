using System;
using System.Reflection;

namespace Inectio.Lite
{
    public class Injector
    {
        //New Implementtion
        public IInjectionBinder injectionBinder { get; set; }
        private InjectionFactory injectionFactory;
        private IReflectionBinder reflectionBinder;

        public Injector()
        {
            injectionFactory = new InjectionFactory();
            reflectionBinder = new ReflectionBinder();
        }

        public object GetInstance(IInjectionBinding binding)
        {
            var obj = injectionFactory.GetInstance(binding);
            Inject(obj);
            return obj;
        }

        public object GetInstance(object value)
        {
            return injectionFactory.CreateInstance(value);
        }

        public void Inject(object target)
        {
            //todo: map all relfections to reflection binder. so when u get the same view again you can get the maps directly...
            var reflected = reflectionBinder.Get(target.GetType());
            injectProperties(reflected.properties, target);
            injectFields(reflected.fields, target);
            injectMethods(reflected.methods, target);
        }

        public void RemoveInjections(object target)
        {
            var rBinder = reflectionBinder.GetBinding(target.GetType());
            if(rBinder != null)
            {
                var reflected = rBinder.Value as ReflectedItems;
                //todo: remove all bindings and also remove listen method delegates for the target object...
                var methods = reflected.methods;
                foreach(var method in methods)
                {
                    if(method.methodListenType != null)
                    {
                        //todo: remove delegate attached delegate for the current method...
                        var binding = injectionBinder.GetBinding(method.methodListenType);
                        var obj = GetInstance(binding) as ISignal;
                        if (obj != null)
                            RemoveDelegate(target, obj, method.info);
                    }
                }
            }
        }

        private object getValue(Type type, object name)
        {
            return GetInstance(injectionBinder.GetBinding(type, name) as IInjectionBinding);
        }

        private void injectProperties(PropertyAttributes[] properties, object target)
        {
            if(properties.Length > 0)
            {
                foreach (var prop in properties)
                {
                    prop.info.SetValue(target, getValue(prop.type, prop.name), null);
                }
            }
        }

        private void injectFields(FieldAttributes[] fields, object target)
        {
            if (fields.Length > 0)
            {
                foreach (var field in fields)
                {
                    field.info.SetValue(target, getValue(field.type, field.name));
                }
            }
        }

        private void injectMethods(MethodAttributes[] methods, object target)
        {
            if (methods.Length > 0)
            {
                foreach (var method in methods)
                {
                    if (method.methodParams != null)
                    {
                        var parms = method.methodParams;
                        var length = parms.Length;
                        object[] paramsData = new object[parms.Length];
                        for (int i = 0; i < length; i++)
                        {
                            paramsData[i] = getValue(parms[i].type, parms[i].name);
                        }
                        method.info.Invoke(target, paramsData);
                    }
                    if(method.methodListenType != null)
                    {
                        //todo: Create delegate for this method and signal.
                        //UnityEngine.Debug.Log("Creating delegate here " + method.methodListenType);
                        var binding = injectionBinder.GetBinding(method.methodListenType);
                        var obj = GetInstance(binding) as ISignal;
                        UnityEngine.Debug.Log("Siganl " + obj);
                        if(obj != null)
                            AddDelegate(target, obj, method.info);
                    }
                }
            }
        }

        //both add and remove delegate methods are taken from strange since it is common function...
        private void AddDelegate(object target, ISignal signal, MethodInfo method)
        {
            if (signal.GetType().BaseType.IsGenericType)
            {
                //UnityEngine.Debug.Log("Adding delegate to " + target);
                var toAdd = Delegate.CreateDelegate(signal.Listener.GetType(), target, method);
                signal.Listener = Delegate.Combine(signal.Listener, toAdd);
            }
            else
            {
                //UnityEngine.Debug.Log("nono generic Adding delegate to " + target);
                var s = signal as Signal;
                s.AddListener((Action)Delegate.CreateDelegate(typeof(Action), target, method));
            }
        }

        private void RemoveDelegate(object target, ISignal signal, MethodInfo method)
        {
            if (signal.GetType().BaseType.IsGenericType)
            {
                Delegate toRemove = Delegate.CreateDelegate(signal.Listener.GetType(), target, method);
                signal.Listener = Delegate.Remove(signal.Listener, toRemove);
            }
            else
            {
                //UnityEngine.Debug.Log("Removing Listener " + method.Name);
                var s = signal as Signal;
                s.RemoveListener((Action)Delegate.CreateDelegate(typeof(Action), target, method));
            }
        }
    }
}
