﻿using System;
using System.Reflection;

namespace Iniectio.Lite
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
            return obj;
        }

        public void RemoveAutoSignals(object target, bool enable)
        {
            var rBinder = reflectionBinder.Get(target.GetType());
            var methods = rBinder.listenMethods;
            var length = methods.Length;
            if (length == 0) return;
            for(int i = 0; i<length; i++)
            {
                if (methods[i].listenType != Listen.ListenType.AUTO) continue;
                var binding = injectionBinder.GetBinding(methods[i].type);
                if (binding != null)
                {
                    var signal = binding.Value as ISignal;
                    if (signal != null)
                    {
                        if (enable)
                            AddDelegate(target, signal, methods[i].info);
                        else
                            RemoveDelegate(target, signal, methods[i].info);
                    }
                }
            }
        }

        public object GetInstance(object value)
        {
            return injectionFactory.CreateInstance(value);
        }

        public void Inject(object target, bool forceInject = false)
        {
            //todo: map all relfections to reflection binder. so when u get the same view again you can get the maps directly...
            var reflected = reflectionBinder.Get(target.GetType());
            injectProperties(reflected.properties, target);
            injectFields(reflected.fields, target);
            injectMethods(reflected.methods, target);
            injectListenMethods(reflected.listenMethods, target);
        }

        public void OnRemove(object target)
        {
            var rBinder = reflectionBinder.GetBinding(target.GetType());
            if(rBinder != null)
            {
                var reflected = rBinder.Value as ReflectedItems;
                //todo: remove all bindings and also remove listen method delegates for the target object...
                var methods = reflected.listenMethods;
                foreach(var method in methods)
                {
                    //todo: remove delegate attached delegate for the current method...
                    if (method.listenType == Listen.ListenType.AUTO) continue;
                    var binding = injectionBinder.GetBinding(method.type);
                    var obj = binding.Value as ISignal;
                    if (obj != null)
                        RemoveDelegate(target, obj, method.info);
                }
            }

            reflectionBinder.OnRemove();
        }

        private object getValue(Type type, string name)
        {
            var obj = GetInstance(injectionBinder.GetBinding(type, name) as IInjectionBinding);
            Inject(obj); // applying injections for each binding itself...
            return obj;
        }

        private void injectProperties(PropertyAttributes[] properties, object target)
        {
            if(properties.Length > 0)
            {
                foreach (var prop in properties)
                {
                    //UnityEngine.Debug.Log("prop " + prop.type);
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

        private void injectListenMethods(AutoListenMethodAttributes[] methods, object target)
        {
            if(methods.Length > 0)
            {
                foreach(var method in methods)
                {
                    var binding = injectionBinder.GetBinding(method.type);
                    var obj = GetInstance(binding) as ISignal;
                    //UnityEngine.Debug.Log("Siganl " + obj);
                    if (obj != null)
                        AddDelegate(target, obj, method.info);
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
                //UnityEngine.Debug.Log("Removing Listener Generic" + method.Name);
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
