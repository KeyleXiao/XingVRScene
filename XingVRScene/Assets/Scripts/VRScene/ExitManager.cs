using UnityEngine;
using System.Collections;

public class ExitManager : MonoBehaviour {

	public int id;

	// Use this for initialization
	void Start () {
	
	}

	void OnMouseDown () {
		VRSceneManager.instance.ChangeVRScene (id);
		UImanager.instance.ChangeSceneUI (id);
	}
}
