using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildrenTest : MonoBehaviour {

	// Use this for initialization
	void Start () {

        var child = GetComponentsInChildren<Transform>();
        foreach (var c in child)
            Debug.Log(c.name);

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
