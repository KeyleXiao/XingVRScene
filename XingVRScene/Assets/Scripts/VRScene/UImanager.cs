using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UImanager : MonoBehaviour {

	public int nowSelectCoach;
	public Sprite[] coachBG;
	public Sprite[] coachAbility;
	public Sprite[] coachData;
	public GameObject[] allSceneUI;
	public GameObject[] allScene3DUI;
	public GameObject[] allCoach;
	public GameObject[] allCoachButton;
	static public UImanager instance;
	public Image coachBGUI;
	public Image coachAbibilityUI;
	public Image coachDataUI;
	public GameObject backButton;
	public GameObject gravityButton;
	public Loading Loading;
	private bool isGravityView;

	void Awake()
	{
		instance = this;
		//VRSceneManager.instance.OnVRImageLoadFinish+=OnDataLoadFinish;

	}

	// Use this for initialization
	void Start () {
		foreach (GameObject item in allCoachButton) {
			EventTriggerListener.Get (item).onClick = CoachButtonClick;
		}
	}

	public void OnDataLoadFinish()
	{
		backButton.SetActive (true);
		gravityButton.SetActive (true);
		Loading.gameObject.SetActive (false);
		ChangeSceneUI (0);
	}
	
	public void ChangeSceneUI(int id)
	{
		foreach (GameObject item in allSceneUI) {
			item.SetActive (false);
		}
		allSceneUI [id].SetActive (true);
		foreach (GameObject item2 in allScene3DUI) {
			item2.SetActive (false);
		}
		allScene3DUI [id].SetActive (true);
	}

	public void ShowCoach(int id)
	{
		ChangeCoach (0);
		foreach (GameObject item in allCoach) {
			item.SetActive (false);
		}
		allCoach [id].SetActive (true);
		foreach (GameObject item2 in allScene3DUI) {
			item2.SetActive (false);
		}
		allScene3DUI [id].SetActive (true);
	}

	public void CoachBack()
	{
		foreach (GameObject item in allCoach) {
			item.SetActive (false);
		}
	}

	void CoachButtonClick(GameObject clickedButton)
	{
		string[] strs = clickedButton.name.Split('_');
		ChangeCoach (int.Parse(strs[1]));
	}

	public void ShowBuy()
	{
		Application.OpenURL ("http://m.dianping.com/mobile/event/63868");
	}

	public void ChangeCoach(int id)
	{
		nowSelectCoach = id;
		coachAbibilityUI.gameObject.SetActive (false);
		coachDataUI.gameObject.SetActive (false);
		coachBGUI.sprite = coachBG[id];
	}

	public void ShowAbility()
	{
		coachAbibilityUI.gameObject.SetActive (true);
		coachDataUI.gameObject.SetActive (false);
		coachAbibilityUI.sprite = coachAbility[nowSelectCoach];
	}

	public void ShowData()
	{
		coachDataUI.gameObject.SetActive (true);
		coachDataUI.sprite = coachData[nowSelectCoach];
		coachAbibilityUI.gameObject.SetActive (false);
	}

	public void ChangeViewMethod()
	{
		isGravityView = !isGravityView;
		VRSceneManager.instance.ChangeView (isGravityView);
	}

	public void BackToMainButton()
	{
		StartCoroutine (VRSceneManager.instance.BackToMainSceneAsc ());
	}
}
