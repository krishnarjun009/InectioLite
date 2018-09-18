using System;
using System.Collections.Generic;

namespace Iniectio.Lite
{
    public class RootContext
    {
        private IInjectionBinder _injectionBinder;
        private ICommandBinder _commandBinder;
        private static RootContext singletonContext;

        public static RootContext firstContext { get { return singletonContext; }}
        public IInjectionBinder injectionBinder { get { return _injectionBinder ?? (_injectionBinder = new InjectionBinder()); } }
        public ICommandBinder commandBinder { get { return _commandBinder; } }

        public RootContext()
        {
            singletonContext = null;
            if (singletonContext == null)
                singletonContext = this;
            initialize();
        }

        virtual public void mapBindings()
        {
            
        }

        virtual public void OnRemove()
        {
            commandBinder.OnRemove();
            //UnityEngine.Debug.Log("On removing is calling from context");
        }

        private void initialize()
        {
            addCoreComponents();
            mapBindings();
        }

        private void addCoreComponents()
        {
            injectionBinder.Map(typeof(IInjectionBinder), injectionBinder); // self inject...
            injectionBinder.Map<ICommandBinder, CommandBinder>();
            _commandBinder = injectionBinder.GetInstance<ICommandBinder>();
            injectionBinder.TryToInject(_commandBinder);
            injectionBinder.UnBind<IInjectionBinder>();
        }
    }
}
