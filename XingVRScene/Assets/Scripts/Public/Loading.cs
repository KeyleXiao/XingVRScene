using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Loading : MonoBehaviour {

	public Image thisImage;
	public static Loading instance;
	public bool isAnimation;

	// Use this for initialization
	void Start () {
		instance = this;
	}

	public void SetLoadingValue(float val)
	{
		thisImage.fillAmount = val;
		if (val >= 1.0f) {
			Application.LoadLevel (Application.loadedLevel + 1);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (isAnimation) {
			thisImage.fillAmount = thisImage.fillAmount + 0.5f * Time.deltaTime;
		}
	}
}
