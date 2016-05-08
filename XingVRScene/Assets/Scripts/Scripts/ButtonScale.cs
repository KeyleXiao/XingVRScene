using UnityEngine;
using System.Collections;

public class ButtonScale : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
