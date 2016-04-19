using UnityEngine;
using System.Collections;

public class StartLogic : MonoBehaviour {

	public GameObject[] allBG;
	int index = 1;

	// Use this for initialization
	void Start () {
	
	}
	
	public void ClickThisBG()
	{
		foreach (GameObject item in allBG) {
			item.SetActive (false);
		}

		if (index > 2) {
			Application.LoadLevel (Application.loadedLevel + 1);
		} else {
			allBG [index].SetActive (true);
			index++;
		}
	}
}
