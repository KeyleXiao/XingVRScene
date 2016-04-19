using UnityEngine;
using System.Collections;

public class SelectLogic : MonoBehaviour {

	public GameObject loading;
	public UnityStandardAssets.ImageEffects.Blur blurEffect;

	// Use this for initialization
	void Start () {
	
	}
	
	public void NextScene()
	{
		blurEffect.enabled = true;
		loading.SetActive (true);
		StartCoroutine ("LoadSceneAsc");	
	}

	IEnumerator LoadSceneAsc()
	{
		AsyncOperation async = Application.LoadLevelAsync (Application.loadedLevel + 1);
		//loadingProgress.fillAmount =  async.progress;
		yield return async;
	}
}
