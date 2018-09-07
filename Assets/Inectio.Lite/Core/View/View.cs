using System;

namespace Inectio.Lite
{
    public class View : UnityEngine.MonoBehaviour, IView
    {
        [Inject]
        virtual public void OnRegister()
        {
        }

        virtual public void OnRemove()
        {
        }

        protected virtual void Awake()
        {
            var context = RootContext.singletonContext;
            context.injectionBinder.TryToInject(this);
        }

        protected virtual void OnDestroy()
        {
            OnRemove();
            var context = RootContext.singletonContext;
            context.injectionBinder.RemoveView(this);
        }
    }
}
