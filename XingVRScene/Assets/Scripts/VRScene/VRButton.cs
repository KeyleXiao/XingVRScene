using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum ButtonType
{
	Road,
	Coach,
	Info,
	Link
}

public class VRButton : MonoBehaviour {

	public ButtonType buttonType;
	public int thisID;
	public string linkUrl;
	public Transform this3DPoint;
	public Image thisImage;
	public Text thisText;
	public Button thisButton;
	private Camera mCamera;

	// Use this for initialization
	void Start () {
		mCamera = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Camera> ();
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 pos = mCamera.WorldToScreenPoint (this3DPoint.position);
		transform.position = new Vector3 (pos.x,pos.y,transform.position.z);
		//Debug.Log (Quaternion.Angle (this3DPoint.rotation, VRSceneManager.instance.gameObject.transform.rotation));
		if (Quaternion.Angle (this3DPoint.rotation, VRSceneManager.instance.gameObject.transform.rotation) > 90.0f) {
			thisImage.enabled = false;
			thisText.enabled = false;
			thisButton.enabled = false;
		} else {
			thisImage.enabled = true;
			thisText.enabled = true;
			thisButton.enabled = true;
		}
	}

	public void ClickButton()
	{
		if (buttonType == ButtonType.Coach) {
			UImanager.instance.ShowCoach (thisID);
		}
		else if(buttonType == ButtonType.Link){
			Application.OpenURL (linkUrl);
		}
	}
}
