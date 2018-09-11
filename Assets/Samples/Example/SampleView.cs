﻿using System;
using Inectio.Lite;
using UnityEngine;

namespace Sample
{
    public class SampleView : View
    {
        [Inject] private SampleData data { get; set; }
        //Inject] private TestSignal testSignal;
        [Inject] private commandsignal commandsignal { get; set; }
        [Inject] private commandsignal2 commandsignal2 { get; set; }
        //[Inject("another")] private commandsignal commandsignal11 { get; set; }
        //[Inject] private commandsignal1 commandsignal1 { get; set; }

        protected override void Awake()
		{
            base.Awake();       
            data.Print();
            //testSignal.Dispatch();

            //Debug.Log("Creating gameobject");
            //GameObject go = new GameObject();
            //var c = go.AddComponent<SampleView1>();
            //go.transform.SetParent(transform, false);
            //c.testSignal.Dispatch();
		}

        IAsTest asTest = new asTest();
        public bool enableDebug = false;
		private void Update()
		{
            commandsignal.Dispatch(4,6, "Hello", 9);
            commandsignal2.Dispatch(4, 6, "Krishna", 9);
            //commandsignal11.DispatchToAll(1, "another");
            //commandsignal1.DispatchToAll(10, "anji1");
            //testSignal.DispatchToAll();
            //var test = asTest as asTest;
            //if(enableDebug)
            //    test.Print();
		}

		//[Inject]
        private void TestMethod(SampleData data)
        {
            Debug.Log("Method injection");
            data.Print();
            Debug.Log("------");
        }

        //[Listen(typeof(commandsignal))]
        //private void both(int i)
        //{
        //    //Debug.Log("both " + str);
        //}
	}

    public interface IAsTest
    {
        
    }

    public class asTest : IAsTest
    {
        public void Print()
        {
            Debug.Log("As Test");
        }
    }

    public class asTest1 : IAsTest
    {
        
    }
}
