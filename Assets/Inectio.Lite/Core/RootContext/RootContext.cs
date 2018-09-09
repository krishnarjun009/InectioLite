
using System;
using System.Collections.Generic;

namespace Inectio.Lite
{
    public class RootContext
    {
        private IInjectionBinder _injectionBinder;
        public static RootContext singletonContext;
        private static event Action<IView> injectHandler;
        private static List<IView> views = new List<IView>();

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
            //injectHandler += InjectViewHanlder;
            if (singletonContext == null)
                singletonContext = this;
            Initialize();
        }

        public static void AddView(IView view)
        {
            //UnityEngine.Debug.Log("Adding view " + view);
            if (!views.Contains(view))
                views.Add(view);
        }

        //~ RootContext()
        //{
        //    injectHandler -= InjectViewHanlder;
        //}

        private void InjectViewHanlder(IView view)
        {
            injectionBinder.TryToInject(view);
        }

        virtual public void MapBindings()
        {
            
        }

        private void Initialize()
        {
            MapBindings();
            foreach(var view in views)
            {
                UnityEngine.Debug.Log("View " + view);
                injectionBinder.TryToInject(view);
            }
        }

        ~ RootContext()
        {
            singletonContext = null;
        }
    }
}
