using UnityEngine;
using Iniectio.Lite;
using System;

namespace Sample
{
    public class SampleView1 : View
    {
        [Inject] private TestSignal testSignal;
        [Inject] private SampleData data;
        [Inject] private ISample sample;

		public override void OnRegister()
		{
            testSignal.AddListener(OnTestSignal);
		}

		public override void OnRemove()
		{
            testSignal.RemoveListener(OnTestSignal);
		}

		private void Update()
		{
            data.Print();
		}

		private void OnTestSignal()
        {
            //Debug.Log("SampleView 1 " + gameObject.name);
        }

        //[Listen(typeof(TestSignal))]
        private void ListenMethodTest()
        {
            data.Print();
            //Debug.Log("SampleView 1 at Listen method " + gameObject.name);
        }
    }
}
