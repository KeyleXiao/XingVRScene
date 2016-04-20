using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class ScrollView3dUI : MonoBehaviour
{
    public static ScrollView3dUI instance;

    public RectTransform cardUIContainer;
    public List<RectTransform> positionList = new List<RectTransform>();


    Queue<CardUI> _cardQueue = new Queue<CardUI>();
    int _cardPositionNum = 8;
    Vector3 _cardPositionOffset = new Vector3(0, 15, 20);
    float _cardPositionRotation = 5;
    // Use this for initialization
    void Awake()
    {
        instance = this;
        InitialCardPosition();
        InitailCardList();
    }


    public void Init3DUI()
    {

    }

    public void Close3DUI()
    {

    }

    /// <summary>
    /// 初始化点位置
    /// </summary>
    void InitialCardPosition()
    {
        for (int i = _cardPositionNum; i > 0; i--)
        {
            GameObject tmpPoint = Instantiate(Resources.Load("Point") as GameObject, Vector3.zero, Quaternion.identity) as GameObject;
            tmpPoint.transform.SetParent(cardUIContainer, false);
            tmpPoint.transform.position = cardUIContainer.position;
            tmpPoint.transform.position = tmpPoint.transform.position + _cardPositionOffset * i;
            tmpPoint.transform.rotation = Quaternion.Euler(new Vector3(0, 0, _cardPositionRotation * i));
            positionList.Add(tmpPoint.GetComponent<RectTransform>());
        }
    }
    /// <summary>
    /// 以上初始化卡片信息
    /// </summary>
    void InitailCardList()
    {
        for (int i = 0; i < 8; i++)
        {
            GameObject m_tmpCardObj = Instantiate(Resources.Load("NewCardUI") as GameObject, Vector3.zero, Quaternion.identity) as GameObject;
            CardUI m_cardUICom = m_tmpCardObj.GetComponent<CardUI>();
            m_cardUICom.InitCardPosition(i);
            _cardQueue.Enqueue(m_cardUICom);
            m_tmpCardObj.transform.SetParent(cardUIContainer, false);
            m_cardUICom.transform.SetSiblingIndex(i);
        }
    }
}
