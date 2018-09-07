using UnityEngine;
using Inectio.Lite;
using System;

namespace Sample
{
    public class SampleView1 : View
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
            Debug.Log("SampleView 1 " + gameObject.name);
        }

        [Listen(typeof(TestSignal))]
        private void ListenMethodTest()
        {
            Debug.Log("SampleView 1 at Listen method " + gameObject.name);
        }
    }
}
