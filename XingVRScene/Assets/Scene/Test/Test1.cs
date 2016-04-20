using UnityEngine;
using System.Collections.Generic;
using LitJson;


public class Test1 : MonoBehaviour {

	// Use this for initialization
	void Start () {
        string s = "[1.1,2.2,3.3,4.4,5.5,6.6]";
        JsonData data = JsonMapper.ToObject(s);
        Debug.Log(data.IsArray);
        Debug.Log(data.Count);
        Queue<int> intQueue = new Queue<int>();
        for (int i = 0; i < 10; i++)
        {
            intQueue.Enqueue(i);
        }
        var temp = intQueue.GetEnumerator();
        //Debug.Log(temp.Current);
        while (temp.MoveNext())
        {
            Debug.Log(temp.Current);
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
