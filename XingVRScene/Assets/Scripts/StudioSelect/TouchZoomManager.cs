using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class TouchZoomManager : MonoBehaviour,IDragHandler,IPointerDownHandler,IPointerUpHandler {

	public float initMousePositionX;
	public float initMousePositionY;
    public GameObject loading;
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

        if (Mathf.Abs(initMousePositionX - Input.mousePosition.x) < 1.0f)
        {
            AppDatas.IndexOfSelected = ScrollView3dUI.instance._cardQueue[1].IndexOfInfo;
            Debug.Log(AppDatas.DataSelected.ToJson());
            NetSystem.instance.AddVisit((string)AppDatas.DataSelected["StudioName"]);
            Debug.Log(AppDatas.IndexOfSelected);
            loading.SetActive(true);
            StartCoroutine(NextScence());
            return;
        }
        if (initMousePositionX > Input.mousePosition.x)
        {
            ScrollView3dUI.instance.ScrollDown();
        }
        else if (initMousePositionX < Input.mousePosition.x)
        {
            ScrollView3dUI.instance.ScrollUp();

        }

    }

    IEnumerator NextScence()
    {
        float i = 0;
        while (i < 1)
        {
            i += 0.5f * Time.deltaTime;
            Loading.instance.SetLoadingValue(i);
            yield return null;
        }
    }
}
