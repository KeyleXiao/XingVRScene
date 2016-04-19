//
// LC VR Kit
//
// Copyright (c) 2015 Laurel Code Inc.
// All rights reserved.
//
// Contact: Laurel Code Inc. (support@laurel-code.com)
//


using UnityEngine;
using UnityEngine.UI;


public class LCVRConfig : MonoBehaviour
{
	[SerializeField] private GameObject          configWindow   ;
	[SerializeField] private LCVRSwitchOverlayUI switchOverlayUI;

	[SerializeField] private LCVRHead mainHead;
	[SerializeField] private Toggle   resetOnlyYR;

	protected virtual void UpdateChecks(){ resetOnlyYR.isOn = mainHead.ResetOnlyYR; }

	private void  open(){ configWindow.SetActive(true ); switchOverlayUI.  Lock(); UpdateChecks(); }
	private void close(){ configWindow.SetActive(false); switchOverlayUI.Unlock(); }

	public virtual void Clicked(GameObject obj){
		if (obj.name == "GearButton"){
			if (configWindow.activeSelf) close(); else open(); 
		}
		else if (obj.name == "ResetOnlyYRCheck") mainHead.ResetOnlyYR = resetOnlyYR.isOn;
	}

	private void Update(){}
	private void Start (){}
	private void Awake (){}
}
