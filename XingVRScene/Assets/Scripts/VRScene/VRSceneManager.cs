using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using DG.Tweening;

public class VRSceneManager : MonoBehaviour {

	public Renderer SceneBall;
	public List<Texture> allScene;
	public GameObject VRHead;
	public System.Action OnVRImageLoadFinish;
	static public VRSceneManager instance;

	void Awake()
	{
		instance = this;
		//Debug.Log ("DataManager.nowSelectID " + DataManager.allStudioJsonData[DataManager.nowSelectID]);
	}

	// Use this for initialization
	void Start () {
		for (int i = 0; i < DataManager.allStudioJsonData[DataManager.nowSelectID]["StudioData"]["VRData"].Count; i++) {
			StartCoroutine (LoadImage(DataManager.allStudioJsonData [DataManager.nowSelectID] ["StudioData"] ["VRData"] ["Data" + (i + 1)] ["VRViewPhoto"].ToString(),i));
		}
	}

	void StartAnimation()
	{
		transform.DOLocalMove (new Vector3 (0, 0, 0), 2.0f).SetEase (Ease.OutQuad);
		transform.DOLocalRotate (new Vector3 (0, 0, 0), 2.0f).SetEase (Ease.OutQuad);
	}
	
	public void ChangeVRScene(int id)
	{
		SceneBall.material.mainTexture = allScene [id];
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape)) {
			Application.Quit ();
		}
	}

	public void ChangeView(bool isGravity)
	{
		VRHead.GetComponent<LCVRHead> ().enabled = isGravity;
		VRHead.GetComponent<LCVRHeadMouseRotation> ().enabled = isGravity;
		VRHead.GetComponent<LCVRLookingDown> ().enabled = isGravity;
		VRHead.GetComponent<MouseRotate> ().enabled = !isGravity;
	}

	public IEnumerator BackToMainSceneAsc()
	{
		DataManager.nowSelectID = 0;
		AsyncOperation async = Application.LoadLevelAsync ("SelectStage");
		//loadingProgress.fillAmount =  async.progress;
		yield return async;
	}

	IEnumerator LoadImage(string url,int id)
	{
		WWW www = new WWW (url);//"file://" + 
		yield return www;
		Debug.Log (url);
		if(www != null && string.IsNullOrEmpty(www.error))
		{
			allScene.Add (www.texture);
			if (id >= (DataManager.allStudioJsonData[DataManager.nowSelectID]["StudioData"]["VRData"].Count - 1)) {
				SceneBall.gameObject.SetActive (true);
				ChangeVRScene (0);
				UImanager.instance.OnDataLoadFinish ();
				StartAnimation ();
			}
		}
	}

}
