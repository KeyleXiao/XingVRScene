using UnityEngine;
using System.Collections.Generic;
using LitJson;


public class Test1 : MonoBehaviour {

    public GameObject o;
    public UnityEngine.UI.GridLayoutGroup panerl;
	// Use this for initialization
	void Start () {
        panerl.cellSize = new Vector2(Screen.width / 2, 40);
        for (int i = 0; i < 10; i++)
        {
            GameObject go = Instantiate(o);
            go.transform.parent = panerl.transform;
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
