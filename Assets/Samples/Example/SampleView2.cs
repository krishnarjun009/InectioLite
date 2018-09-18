using UnityEngine;
using Iniectio.Lite;
using System;

namespace Sample
{
    public class SampleView2 : View
    {
        [Inject] private TestSignal testSignal;

        public override void OnRegister()
        {
            testSignal.AddListener(OnTestSignal);
        }

        public override void OnRemove()
        {
            testSignal.RemoveListener(OnTestSignal);
        }

        private void OnTestSignal()
        {
            Debug.Log("SampleView 2");
        }
    }
}
