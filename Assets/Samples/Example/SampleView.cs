using System;
using Inectio.Lite;
using UnityEngine;

namespace Sample
{
    public class SampleView : View
    {
        [Inject] private SampleData data { get; set; }
        [Inject] private TestSignal testSignal;

        protected override void Awake()
		{
            base.Awake();       
            data.Print();
            testSignal.Dispatch();

            Debug.Log("Creating gameobject");
            GameObject go = new GameObject();
            var c = go.AddComponent<SampleView1>();
            go.transform.SetParent(transform, false);
            //c.testSignal.Dispatch();
		}

		private void Update()
		{
            testSignal.Dispatch();
		}

		//[Inject]
        private void TestMethod(SampleData data)
        {
            Debug.Log("Method injection");
            data.Print();
            Debug.Log("------");
        }
	}
}
