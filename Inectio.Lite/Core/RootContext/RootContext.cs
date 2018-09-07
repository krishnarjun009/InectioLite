
using System;

namespace Inectio.Lite
{
    public class RootContext
    {
        private IInjectionBinder _injectionBinder;
        public static RootContext singletonContext;

        public IInjectionBinder injectionBinder
        {
            get { return _injectionBinder ?? (_injectionBinder = new InjectionBinder()); }
        }

        //public IInectioCommandBinder _commandBinder;

        //public IInectioCommandBinder commandBinder
        //{
        //    get { return _commandBinder ?? (_commandBinder = new InectioCommandBinder()); }
        //}

        public RootContext()
        {
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

        ~RootContext()
        {
            singletonContext = null;
        }
    }
}
