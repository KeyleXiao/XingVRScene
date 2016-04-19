//
// LC VR Kit
//
// Copyright (c) 2015 Laurel Code Inc.
// All rights reserved.
//
// Contact: Laurel Code Inc. (support@laurel-code.com)
//


using UnityEngine;


public class LCVRCalibration : MonoBehaviour
{
	[SerializeField] private LCVRHeadMouseRotation headMouseRotation;
	[SerializeField] private LCVRPopupLabel        label;

	private bool show = false;

	private LCVRHead.VIEW_MODE viewMode = LCVRHead.VIEW_MODE.SIDE_BY_SIDE;

	private void setViewMode(){
		foreach (LCVRHead head in LCVR.Instance.Heads) head.ViewMode = viewMode;
		label.Show(viewMode == LCVRHead.VIEW_MODE.SIDE_BY_SIDE ? "Side By Side View" : "Single View");
	}

	public void OnClicked(GameObject obj){
		if (obj.name == "HMDButton"){
		  viewMode = viewMode == LCVRHead.VIEW_MODE.SIDE_BY_SIDE ? LCVRHead.VIEW_MODE.SINGLE : LCVRHead.VIEW_MODE.SIDE_BY_SIDE;
			setViewMode();
		}
		else if (obj.name == "GearButton"){
			if (show){
				show = false;
				headMouseRotation.enabled = true;
			}
			else{
				show = true;
				headMouseRotation.enabled = false;
			}
		}
	}

	private AnimationCurve curve;
	private Keyframe[]     keys = new Keyframe[2];

	private float deg2Tan(float x){ return Mathf.Tan(Mathf.Deg2Rad * x) ; }
	private float tan2Deg(float x){ return Mathf.Atan(x) * Mathf.Rad2Deg; }

	private bool realtimePreview = true;

	private void resetKeys(){
		keys[0].time  = 0.0f;
		keys[0].value = 0.0f;
		keys[0].outTangent = 0.0f;

#if true
		// Default
		keys[1].time  =    2.107753f;
		keys[1].value = -0.09912411f;
		keys[1].inTangent = deg2Tan(-3.774765f);

#elif false
		// HACOSCO (Single View)
		keys[1].time  =  2.240645f  ;
		keys[1].value = -0.02495953f;
		keys[1].inTangent = deg2Tan(-1.131816f);
#elif false
		// HACOSCO DX (Side by Side)
		keys[1].time  =    2.107753f;
		keys[1].value = -0.09912411f;
		keys[1].inTangent = deg2Tan(-3.774765f);
#elif false
		// Google Cardboard
		keys[1].time  =  1.641127f ;
		keys[1].value = -0.1204107f;
		keys[1].inTangent = deg2Tan(-0.1204107f);
#elif false
		// Soyan
		keys[1].time  =  2.761703f ;
		keys[1].value = -0.1046399f;
		keys[1].inTangent = deg2Tan(-3.19311f);
#elif false
		// Andoer
		keys[1].time  =  2.761703f ;
		keys[1].value = -0.1046399f;
		keys[1].inTangent = deg2Tan(-3.19311f);
#elif false
		// GMYLE
		keys[1].time  =  2.184114f ;
		keys[1].value = -0.1200423f;
		keys[1].inTangent = deg2Tan(-4.7526708f);
#endif
	}

	private void setDirty(Camera eye){
		LCVRLens lens = eye.gameObject.GetComponent<LCVRLens>();
		if (lens != null) lens.SetDirty();
	}

	private bool enableDrag = true;

	private void execWindow(int id){
		int x = 10;
		int y = 20;

		bool dirty = false;

		{
			Keyframe key = keys[0];

			float prevDeg = tan2Deg(key.outTangent);

			GUI.Label(new Rect(x, y, 200, 20), "key0.outTangent(deg): " + prevDeg); y += 20;

			float newDeg = GUI.HorizontalSlider(new Rect(x, y, 600, 20), prevDeg, -90.0f, 0.0f); y += 20;

			if (newDeg != prevDeg){
				key.outTangent = deg2Tan(newDeg);

				curve.MoveKey(0, keys[0] = key);
				dirty = true;
			}
		}

		{
			Keyframe key = keys[1];

			GUI.Label(new Rect(x, y, 200, 20), "key1.time: " + key.time); y += 20;

			float newTime = GUI.HorizontalSlider(new Rect(x, y, 600, 20), key.time, 0.0000001f, 10.0f); y += 20;

			GUI.Label(new Rect(x, y, 200, 20), "key1.value: " + key.value); y += 20;

			float newVal  = GUI.HorizontalSlider(new Rect(x, y, 600, 20), key.value, -1.0f, 0.0f); y += 20;

			float prevDeg = tan2Deg(key.inTangent);

			GUI.Label(new Rect(x, y, 200, 20), "key1.inTangent(deg): " + prevDeg); y += 20;

			float  newDeg = GUI.HorizontalSlider(new Rect(x, y, 600, 20), prevDeg, -90.0f, 0.0f); y += 20;

			if (newTime != key.time || newVal != key.value || newDeg != prevDeg){
				key.time      = newTime;
				key.value     = newVal;
				key.inTangent = deg2Tan(newDeg);

				curve.MoveKey(1, keys[1] = key);
				dirty = true;
			}
		}

		y += 10;

		bool setDirty_ = false;

		if (GUI.Button(new Rect(x, y, 100, 20), "Reset")){
			resetKeys();
			curve.MoveKey(0, keys[0]);
			curve.MoveKey(1, keys[1]);
			setDirty_ = true;
		}

		x = 380;

		realtimePreview = GUI.Toggle(new Rect(x, y, 150, 20), realtimePreview, "Realtime Preview");

		x = 510;

		if (realtimePreview){ if (dirty) setDirty_ = true; }
		else if (GUI.Button(new Rect(x, y, 100, 20), "Update")) setDirty_ = true;

		if (setDirty_)
			foreach (LCVRHead head in LCVR.Instance.Heads){
				if (head.  LeftEye) setDirty(head.  LeftEye);
				if (head.CenterEye) setDirty(head.CenterEye);
				if (head. RightEye) setDirty(head. RightEye);
			}

		x  = 10;
		y += 30;

		enableDrag = GUI.Toggle(new Rect(x, y, 200, 20), enableDrag, "Enable Drag Window");

		if (enableDrag) GUI.DragWindow();
	}

	private const float WINDOW_W = 640.0f;
	private const float WINDOW_H = 250.0f;
	private Rect  windowRect     = new Rect((Screen.width - WINDOW_W) * 0.5f, (Screen.height - WINDOW_H) * 0.5f, WINDOW_W, WINDOW_H);

	private void OnGUI(){
		{
			float scale = Screen.height / 480.0f;
			GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(scale, scale, 1.0f));

			GUI.backgroundColor = Color.black;
		}

		if (show) windowRect = GUI.Window(0, windowRect, execWindow, "Lens Calibration"); 
	}

	private void setHMDTypeAndDistortionCurve(Camera eye, LCVRLens.HMD_TYPE hmdType, AnimationCurve curve){
		LCVRLens lens = eye.gameObject.GetComponent<LCVRLens>();
		if (lens != null){
			lens.HMDType         = hmdType;
			lens.DistortionCurve = curve  ;
		}
	}

	private void Start(){
#if UNITY_IOS || UNITY_ANDROID
		realtimePreview = false;
#endif

		setViewMode();

		keys[0] = new Keyframe();
		keys[1] = new Keyframe();

		resetKeys();

		curve = new AnimationCurve(keys);

		foreach (LCVRHead head in LCVR.Instance.Heads){
			if (head.  LeftEye) setHMDTypeAndDistortionCurve(head.  LeftEye, LCVRLens.HMD_TYPE.CUSTOM, curve);
			if (head.CenterEye) setHMDTypeAndDistortionCurve(head.CenterEye, LCVRLens.HMD_TYPE.CUSTOM, curve);
			if (head. RightEye) setHMDTypeAndDistortionCurve(head. RightEye, LCVRLens.HMD_TYPE.CUSTOM, curve);
		}
	}
}
