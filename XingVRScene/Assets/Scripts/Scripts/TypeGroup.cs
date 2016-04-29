using UnityEngine;
using System.Collections;
using DG.Tweening;
public class TypeGroup : MonoBehaviour {

    public static TypeGroup instance;

    public RectTransform[] subs;
    public int? selectedIndex = null;
    public float movedDistance;
    public bool isMoving = false;
    float _sizeOfSearchUI = 100;
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


    public void OnOpenButtonDown(int __indexInGroup,float __height,GameObject __sub)
    {
        TypeGroup m_typeGroup = TypeGroup.instance;
        if (m_typeGroup.isMoving)
            return;
        m_typeGroup.isMoving = true;
        if (m_typeGroup.selectedIndex.HasValue)
        {
            float m_movedDis = m_typeGroup.movedDistance;
            if (m_typeGroup.selectedIndex.Value == __indexInGroup)          //当同一个按钮时关闭
            {
                m_typeGroup.subs[selectedIndex.Value].GetComponent<OnButtonDown>().Close();
                for (int i = 0; i <= m_typeGroup.selectedIndex.Value; i++)
                {
                    m_typeGroup.subs[i].DOAnchorPos(new Vector2(m_typeGroup.subs[i].anchoredPosition.x,
                                                    m_typeGroup.subs[i].anchoredPosition.y - m_movedDis),
                                                    0.5f).OnComplete(
                                                        () =>
                                                        {
                                                            if (m_typeGroup.selectedIndex.Value == __indexInGroup)
                                                            {
                                                                m_typeGroup.movedDistance = 0;
                                                                if (m_typeGroup.selectedIndex != null)
                                                                    m_typeGroup.selectedIndex = null;
                                                                m_typeGroup.isMoving = false;
                                                                __sub.SetActive(false);
                                                            }
                                                        }
                                                        );
                }
            }
            else                                                    //不是同一个按钮时 先关闭在打开
            {
                m_typeGroup.subs[selectedIndex.Value].GetComponent<OnButtonDown>().Close();

                for (int i = 0; i <= m_typeGroup.selectedIndex.Value; i++)
                {
                    m_typeGroup.subs[i].DOAnchorPos(new Vector2(m_typeGroup.subs[i].anchoredPosition.x,
                                                    m_typeGroup.subs[i].anchoredPosition.y - m_movedDis),
                                                    0.5f).OnComplete(() => OnClose(__indexInGroup, __height, __sub));
                }
            }


        }
        else                                        //打开子标签
        {
            m_typeGroup.selectedIndex = __indexInGroup;
            m_typeGroup.movedDistance = __height;
            m_typeGroup.subs[selectedIndex.Value].GetComponent<OnButtonDown>().Open();

            __sub.SetActive(true);
            for (int i = 0; i <= __indexInGroup; i++)
            {
                m_typeGroup.subs[i].DOAnchorPos(new Vector2(m_typeGroup.subs[i].anchoredPosition.x,
                                                m_typeGroup.subs[i].anchoredPosition.y + __height),
                                                0.5f).OnComplete(() => m_typeGroup.isMoving = false);
            }
        }

    }

    public void OnClose(int __indexInGroup, float __height, GameObject __sub)
    {

        TypeGroup m_typeGroup = TypeGroup.instance;
        m_typeGroup.subs[selectedIndex.Value].FindChild("SubClass").gameObject.SetActive(false);

        __sub.SetActive(true);
        if (m_typeGroup.selectedIndex.Value != __indexInGroup)
        {
            m_typeGroup.movedDistance = __height;
            m_typeGroup.selectedIndex = __indexInGroup;
            m_typeGroup.subs[selectedIndex.Value].GetComponent<OnButtonDown>().Open();

            for (int i = 0; i <= __indexInGroup; i++)
            {
                m_typeGroup.subs[i].DOAnchorPos(new Vector2(m_typeGroup.subs[i].anchoredPosition.x,
                                                    m_typeGroup.subs[i].anchoredPosition.y + __height),
                                                    0.5f).OnComplete(() => m_typeGroup.isMoving = false);
            }
        }
    }
}
