using System;
using UnityEngine;

namespace Inectio.Lite
{
    public class View : MonoBehaviour, IView
    {
        /// <summary>
        /// OnRegister will execute after Awake method.
        /// </summary>
        virtual public void OnRegister()
        {
        }

        /// <summary>
        /// OnRemove Will execute in OnDestroy method.
        /// </summary>
        virtual public void OnRemove()
        {
        }

        /// <summary>
        /// Handles Auto Listen signals OnEnable
        /// </summary>
        virtual protected void OnEnable()
        {
            RootContext.firstContext.injectionBinder.OnAutoSignalHandler(true, this);// support for single context
        }

        /// <summary>
        /// Handles Auto Listen signals OnDisable
        /// </summary>
        virtual protected void OnDisable()
        {
            RootContext.firstContext.injectionBinder.OnAutoSignalHandler(false, this);
        }

        /// <summary>
        /// base awake is mandetary to call before your code.
        /// </summary>
        protected virtual void Awake()
        {
            var context = RootContext.firstContext;
            context.injectionBinder.TryToInject(this);
            OnRegister();
        }

        protected virtual void Start()
        {
        }

        /// <summary>
        /// base destroy is mandetary to call before your code to clean up.
        /// </summary>
        protected virtual void OnDestroy()
        {
            OnRemove();
            var context = RootContext.firstContext;
            context.injectionBinder.OnRemove(this);
        }
    }
}
