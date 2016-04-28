using UnityEngine;
using System.Collections.Generic;
using LitJson;


public class Test1 : MonoBehaviour {

	// Use this for initialization
	void Start () {
        NetSystem.instance.GetAllData();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
