using System;
using UnityEngine;

namespace Inectio.Lite
{
    public class View : MonoBehaviour, IView
    {

        [Inject]
        virtual public void OnRegister()
        {
        }

        virtual public void OnRemove()
        {
        }

        //[Inject]
        private void InjectThis()
        {
            //UnityEngine.Debug.Log("Inecting by default " + gameObject.name);
            var context = RootContext.singletonContext;
            context.injectionBinder.TryToInject(this);
        }

        protected virtual void Awake()
        {
        }

        protected virtual void Start()
        {
        }

        protected virtual void OnDestroy()
        {
            OnRemove();
            var context = RootContext.singletonContext;
            context.injectionBinder.RemoveView(this);
        }

        public void SetUp()
        {
            InjectThis();
        }
    }

    public class InectorProvider
    {
        public InectorProvider(IView view)
        {

        }
    }
}
