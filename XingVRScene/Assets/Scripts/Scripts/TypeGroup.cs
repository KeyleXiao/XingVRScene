using UnityEngine;
using System.Collections;
using DG.Tweening;
public class TypeGroup : MonoBehaviour {

    public static TypeGroup instance;

    public RectTransform[] subs;
    public int? selectedIndex = null;
    public float movedDistance;
    public bool isMoving = false;
    float _sizeOfSearchUI = 120;
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
    public void ClosePanel(System.Action func)
    {
        RectTransform thisRectTransform = GetComponent<RectTransform>();
        thisRectTransform.DOAnchorPos(new Vector2(thisRectTransform.anchoredPosition.x,
                                                    -thisRectTransform.sizeDelta.y / 2), 0.5f).OnComplete(() => func());

        if (selectedIndex.HasValue)
            OnOpenButtonDown(selectedIndex.Value, movedDistance, subs[selectedIndex.Value].FindChild("SubClass").gameObject);
    }
    public void InitPosition()
    {
        RectTransform thisRectTransform = GetComponent<RectTransform>();
        thisRectTransform.anchoredPosition = new Vector2(thisRectTransform.sizeDelta.x / 2, -thisRectTransform.sizeDelta.y / 2);
    }


    public void OnOpenButtonDown(int __indexInGroup,float __height,GameObject __sub)
    {
        StartCoroutine(StartMoveUI(__indexInGroup, __height, __sub));
    }

    public IEnumerator OnClose(int __indexInGroup, float __height, GameObject __sub)
    {

        TypeGroup m_typeGroup = TypeGroup.instance;
        m_typeGroup.subs[selectedIndex.Value].FindChild("SubClass").gameObject.SetActive(false);

        __sub.SetActive(true);
        if (m_typeGroup.selectedIndex.Value != __indexInGroup)
        {
            m_typeGroup.movedDistance = __height;
            m_typeGroup.subs[selectedIndex.Value].GetComponent<OnButtonDown>().Close();
            m_typeGroup.selectedIndex = __indexInGroup;
            m_typeGroup.subs[selectedIndex.Value].GetComponent<OnButtonDown>().Open();

            int _uiComplete = 0;

            for (int i = 0; i <= __indexInGroup; i++)
            {
                m_typeGroup.subs[i].DOAnchorPos(new Vector2(m_typeGroup.subs[i].anchoredPosition.x,
                                                    m_typeGroup.subs[i].anchoredPosition.y + __height),
                                                    0.5f).OnComplete(() => _uiComplete++);
            }
            yield return new WaitUntil(() => _uiComplete == __indexInGroup + 1);
            m_typeGroup.isMoving = false;
        }
    }


    IEnumerator StartMoveUI(int __indexInGroup, float __height, GameObject __sub)
    {

        TypeGroup m_typeGroup = TypeGroup.instance;
        if (m_typeGroup.isMoving)
            yield break;
        m_typeGroup.isMoving = true;
        if (m_typeGroup.selectedIndex.HasValue)
        {
            float m_movedDis = m_typeGroup.movedDistance;
            if (m_typeGroup.selectedIndex.Value == __indexInGroup)          //当同一个按钮时关闭
            {
                int _uiComplete = 0;
                for (int i = 0; i <= m_typeGroup.selectedIndex.Value; i++)
                {
                    m_typeGroup.subs[i].DOAnchorPos(new Vector2(m_typeGroup.subs[i].anchoredPosition.x,
                                                    m_typeGroup.subs[i].anchoredPosition.y - m_movedDis),
                                                    0.5f).OnComplete(
                                                        () =>
                                                        {
                                                            _uiComplete++;
                                                        }
                                                        );
                }
                yield return new WaitUntil(() => _uiComplete == m_typeGroup.selectedIndex.Value + 1);
                if (m_typeGroup.selectedIndex.Value == __indexInGroup)
                {
                    m_typeGroup.movedDistance = 0;
                    m_typeGroup.subs[selectedIndex.Value].GetComponent<OnButtonDown>().Close();
                    if (m_typeGroup.selectedIndex != null)
                        m_typeGroup.selectedIndex = null;
                    m_typeGroup.isMoving = false;
                    __sub.SetActive(false);
                }
            }
            else                                                    //不是同一个按钮时 先关闭在打开
            {
                int _uiComplete = 0;

                for (int i = 0; i <= m_typeGroup.selectedIndex.Value; i++)
                {
                    m_typeGroup.subs[i].DOAnchorPos(new Vector2(m_typeGroup.subs[i].anchoredPosition.x,
                                                    m_typeGroup.subs[i].anchoredPosition.y - m_movedDis),
                                                    0.5f).OnComplete(() => _uiComplete++);
                }
                yield return new WaitUntil(() => _uiComplete == m_typeGroup.selectedIndex.Value + 1);
                //Debug.Log("complete");
                yield return OnClose(__indexInGroup, __height, __sub);
            }


        }
        else                                        //打开子标签
        {
            m_typeGroup.selectedIndex = __indexInGroup;
            m_typeGroup.movedDistance = __height;
            m_typeGroup.subs[selectedIndex.Value].GetComponent<OnButtonDown>().Open();

            __sub.SetActive(true);

            int _uiComplete = 0;

            for (int i = 0; i <= __indexInGroup; i++)
            {
                m_typeGroup.subs[i].DOAnchorPos(new Vector2(m_typeGroup.subs[i].anchoredPosition.x,
                                                m_typeGroup.subs[i].anchoredPosition.y + __height),
                                                0.5f).OnComplete(() => _uiComplete++);
            }
            yield return new WaitUntil(() => _uiComplete == __indexInGroup + 1);
            m_typeGroup.isMoving = false;
        }
    }
}
