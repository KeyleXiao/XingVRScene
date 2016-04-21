using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class TouchZoomManager : MonoBehaviour,IDragHandler,IPointerDownHandler,IPointerUpHandler {

	public float initMousePositionX;
	public float initMousePositionY;

	// Use this for initialization
	void Start () {
		
	}

	public void OnDrag(PointerEventData eventData)
	{
		
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		initMousePositionX = Input.mousePosition.x;
	}

	public void OnPointerUp(PointerEventData eventData)
	{
        //if (initMousePositionX > Input.mousePosition.x) {
        //	Debug.Log ("Left");
        //	UI3Dmanager.instance.CardChangeNext ();
        //}
        //else if (initMousePositionX < Input.mousePosition.x) {
        //	Debug.Log ("Right");
        //	UI3Dmanager.instance.CardChangeBack ();
        //}
        if (Mathf.Abs(initMousePositionX - Input.mousePosition.x) < 1.0f)
        {
            AppDatas.IndexOfSelected = ScrollView3dUI.instance._cardQueue[1].IndexOfInfo;
            Debug.Log(AppDatas.IndexOfSelected);
            return;
        }
        if (initMousePositionX > Input.mousePosition.x)
        {
            Debug.Log("Left");
            ScrollView3dUI.instance.ScrollDown();
        }
        else if (initMousePositionX < Input.mousePosition.x)
        {
            Debug.Log("Right");
            ScrollView3dUI.instance.ScrollUp();

        }

    }


}
