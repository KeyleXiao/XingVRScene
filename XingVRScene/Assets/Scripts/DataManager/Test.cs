using UnityEngine;
using System.Collections;
using LitJson;

public class Test : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
		StartCoroutine (JsonParser.LoadData ("JsonData_type1_1", Func));
	}
	
	void Func(JsonData data)
	{
		Debug.Log (data.ToJson ());

	}
}
