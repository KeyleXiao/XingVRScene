using UnityEngine;
using System.Collections;

public class CardUI : MonoBehaviour {

    public RectTransform thisRectTransform;
    int _indexOfPosition;
    int? _indexOfInfo;
	// Use this for initialization
	void Awake ()
    {
        thisRectTransform = GetComponent<RectTransform>();
	}
	
    public void InitCardInfo(int p_index)
    {
        _indexOfInfo = p_index;

    }
    public void InitCardPosition(int p_index)
    {
        _indexOfPosition = p_index;
        thisRectTransform.anchoredPosition = ScrollView3dUI.instance.positionList[_indexOfPosition].anchoredPosition;
        thisRectTransform.rotation = ScrollView3dUI.instance.positionList[_indexOfPosition].rotation;
    }
}
