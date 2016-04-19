using UnityEngine;
using System.Collections;
using DG.Tweening;
public class TypeGroup : MonoBehaviour {

    public static TypeGroup instance;

    public RectTransform[] subs;
    public int? selectedIndex = null;
    public float movedDistance;
    public bool isMoving = false;
    float _sizeOfSearchUI = 40;
    // Use this for initialization
    void Awake ()
    {
        InitPosition();
        instance = this;
        for (int i = 0; i < subs.Length; i++)
        {
            subs[i].GetComponent<SubClass>().indexInGroup = i;
        }
	}
	
    public void OpenPanel()
    {
        RectTransform thisRectTransform = GetComponent<RectTransform>();
        thisRectTransform.DOAnchorPos(new Vector2(thisRectTransform.anchoredPosition.x,
                                                    thisRectTransform.sizeDelta.y / 2 + _sizeOfSearchUI), 0.5f);
    }
    public void ClosePanel()
    {
        RectTransform thisRectTransform = GetComponent<RectTransform>();
        thisRectTransform.DOAnchorPos(new Vector2(thisRectTransform.anchoredPosition.x,
                                                    -thisRectTransform.sizeDelta.y / 2), 0.5f);
    }
    public void InitPosition()
    {
        RectTransform thisRectTransform = GetComponent<RectTransform>();
        thisRectTransform.anchoredPosition = new Vector2(thisRectTransform.sizeDelta.x / 2, -thisRectTransform.sizeDelta.y / 2);
    }
}
