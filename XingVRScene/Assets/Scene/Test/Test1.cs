using UnityEngine;
using System.Collections.Generic;
using LitJson;


public class Test1 : MonoBehaviour {

	// Use this for initialization
	void Start () {
        string[] s = { "a", "b", "c", "d", "e", "f", "g" };
        LinkedList<string> intQueue = new LinkedList<string>(s);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
