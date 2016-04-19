using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;

public class UI3Dmanager : MonoBehaviour {

	public List<RectTransform> positionList;
	public Sprite[] allCardIcon;
	public RectTransform cardUIContainer;
	public List<CardManager> allCard;
	public List<CardManager> allCardQueue;
	public RectTransform positionL;
	public RectTransform positionR;
	public int nowIndex;
	public int cardPositionNum;
	public int allCardNum;
	public Vector3 cardPositionOffset;
	public float cardPositionRotation;
	public static UI3Dmanager instance;
	public Color[] allRandomColor;
	public Sprite[] allLogo;
	public string[] allCardName; 
	public GameObject loading;
	public Image loadingProgress;
	public int cardShowNumber;

	// Use this for initialization
	void Start () {
		instance = this;
		InitialCardPosition ();
		InitailCardList ();
		//CardChangeNext ();
		//CardChangeNext (positionL);
	}

	public void InitialCardPosition()
	{
		for (int i = 0; i < cardPositionNum; i++) {
			GameObject tmpPoint = Instantiate (Resources.Load ("Point") as GameObject,Vector3.zero,Quaternion.identity) as GameObject;
			tmpPoint.transform.SetParent (cardUIContainer,false);
			tmpPoint.transform.position = cardUIContainer.position;
			tmpPoint.transform.position = tmpPoint.transform.position + cardPositionOffset * i;
			tmpPoint.transform.rotation = Quaternion.Euler (new Vector3 (0, 0, cardPositionRotation * i));
			positionList.Add (tmpPoint.GetComponent<RectTransform> ());
		}
	}

	public void InitailCardList()
	{
		for (int i = 0; i < Mathf.FloorToInt(DataManager.allDataCount); i++) {
			allCard.Add (new CardManager (i));
		}
		for (int j = Mathf.FloorToInt(DataManager.allDataCount - 1); j >= 0 ; j--) {
			if(j < cardPositionNum)
			{
				GameObject tmpObj = Instantiate (Resources.Load ("CardUI") as GameObject, Vector3.zero, Quaternion.identity) as GameObject;
				CardManager cManager = tmpObj.GetComponent<CardManager> ();
				cManager.thisID = allCard [j].GetID ();
				allCardQueue.Add(cManager);
				tmpObj.transform.SetParent (cardUIContainer, false);
				tmpObj.transform.position = positionList[j].position;
				tmpObj.transform.rotation = positionList[j].rotation;
				int rand = Random.Range (0, allRandomColor.Length);
				tmpObj.GetComponent<CardManager> ().thisImageBG.color = allRandomColor [rand];
				tmpObj.GetComponent<CardManager> ().InitialData ();
			}
		}
		CardManager tmp;
		int max = allCardQueue.Count / 2 ;
		for (int n = 0; n < max; n++) {
			tmp = allCardQueue [n];
			allCardQueue [n] = allCardQueue [allCardQueue.Count - n - 1];
			allCardQueue [allCardQueue.Count - n - 1] = tmp;
		}
		ChangeCardShow ();
//		for (int i = 0; i < positionList.Count; i++) {
//			GameObject tmpCard = Instantiate (Resources.Load ("CardUI") as GameObject, Vector3.zero, Quaternion.identity) as GameObject;
//			tmpCard.transform.SetParent (cardUIContainer, false);
//			allCard.Add (tmpCard.GetComponent<CardManager> ());
//			tmpCard.transform.position = positionList[i].position;
//			tmpCard.transform.rotation = positionList[i].rotation;
//		}
	}

	public void ChangeCardShow()
	{
		for (int i = 0; i < allCardQueue.Count; i++) {
			if (i > (cardShowNumber - nowIndex - 1)) {
				allCardQueue [i].gameObject.SetActive (false);
			} else {
				allCardQueue [i].gameObject.SetActive (true);
			}
		}
	}

	public void CardChangeNext()//RectTransform positionDestination)
	{
		if ((allCardQueue.Count - 1 + nowIndex) > 0) {
			ChangeIndex (-1);
			for (int i = 0; i < allCardQueue.Count; i++) {
				if ((i + nowIndex) < 0) {
					allCardQueue [i].transform.DOMove (positionL.position, 0.5f).SetEase(Ease.InOutExpo);
					allCardQueue [i].transform.DORotate (positionL.rotation.eulerAngles, 0.5f).SetEase(Ease.InOutExpo);
				} else {
					allCardQueue [i].transform.DOMove (positionList[i + nowIndex].position, 0.5f).SetEase(Ease.InOutExpo);
					allCardQueue [i].transform.DORotate (positionList[i + nowIndex].rotation.eulerAngles, 0.5f).SetEase(Ease.InOutExpo);
				}
			}
			ChangeCardShow ();
		}
	}

	public void CardChangeBack()
	{
		if(nowIndex < 0)
		{
			ChangeIndex (1);
			for (int i = 0; i < allCardQueue.Count; i++) {
				if ((i + nowIndex) >= 0) {
					allCardQueue [i].transform.DOMove (positionList[i + nowIndex].position, 0.5f).SetEase(Ease.InOutExpo);
					allCardQueue [i].transform.DORotate (positionList[i + nowIndex].rotation.eulerAngles, 0.5f).SetEase(Ease.InOutExpo);
				}
			}
			ChangeCardShow ();
		}
	}

	public void ChangeIndex(int val)
	{
		//if ((nowIndex + val) >= 0 && (nowIndex + val) < allCardNum) {
		nowIndex = nowIndex + val;
		DataManager.nowSelectID = Mathf.Abs(nowIndex);
		//Debug.Log (DataManager.nowSelectID);
	}

	public void ChangeScene()
	{
		loading.gameObject.SetActive (true);
		cardUIContainer.gameObject.SetActive (false);
		StartCoroutine ("LoadSceneAsc");
	}
	
	IEnumerator LoadSceneAsc()
	{
		AsyncOperation async = Application.LoadLevelAsync (Application.loadedLevel + 1);
		//loadingProgress.fillAmount =  async.progress;
		yield return async;
	}
}
