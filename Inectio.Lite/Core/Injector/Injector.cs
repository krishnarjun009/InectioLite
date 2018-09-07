using System;

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
            return injectionFactory.GetInstance(binding);
        }

        public void Inject(object target)
        {
            //todo: map all relfections to reflection binder. so when u get the same view again you can get the maps directly...
            var reflected = reflectionBinder.Get(target.GetType());
            injectProperties(reflected.properties, target);
            injectFields(reflected.fields, target);
            injectMethods(reflected.methods, target);
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
}
