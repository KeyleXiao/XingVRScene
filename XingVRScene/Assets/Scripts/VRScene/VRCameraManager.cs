using UnityEngine;
using System.Collections;

public class VRCameraManager : MonoBehaviour {

	public float rotateYSpeed;
	float oldPositionY;
	float deltaPositionY;

	// Use this for initialization
	void Start () {
		oldPositionY = Input.acceleration.x;
	}
	
	// Update is called once per frame
	void Update () {
		deltaPositionY = oldPositionY - Input.acceleration.x;
		transform.Rotate (new Vector3(0 , deltaPositionY , 0) * rotateYSpeed);
		oldPositionY = Input.acceleration.x;
	}
}
