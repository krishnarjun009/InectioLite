using System;
using System.Collections.Generic;

namespace Inectio.Lite
{
    public class RootContext
    {
        private IInjectionBinder _injectionBinder;
        private static RootContext singletonContext;

        public static RootContext firstContext { get { return singletonContext; }}
        public IInjectionBinder injectionBinder { get { return _injectionBinder ?? (_injectionBinder = new InjectionBinder()); } }

        //public IInectioCommandBinder _commandBinder;

        //public IInectioCommandBinder commandBinder
        //{
        //    get { return _commandBinder ?? (_commandBinder = new InectioCommandBinder()); }
        //}

        public RootContext()
        {
            singletonContext = null;
            if (singletonContext == null)
                singletonContext = this;
            Initialize();
        }

        virtual public void MapBindings()
        {
            
        }

        private void Initialize()
        {
            MapBindings();
        }
    }
}
