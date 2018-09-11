﻿using System;
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
