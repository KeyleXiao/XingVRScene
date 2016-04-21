using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class ScrollView3dUI : MonoBehaviour
{
    public static ScrollView3dUI instance;

    public RectTransform cardUIContainer;
    public RectTransform cardPosOfEnd;
    public List<RectTransform> positionList = new List<RectTransform>();

    public List<CardUI> _cardQueue = new List<CardUI>();
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
        positionList.Add(cardPosOfEnd);

        //for (int i = _cardPositionNum; i > 0; i--)
        for (int i = 0; i < _cardPositionNum; i++)
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
        for (int i = 0; i < 9; i++)
        {
            GameObject m_tmpCardObj = Instantiate(Resources.Load("NewCardUI") as GameObject, Vector3.zero, Quaternion.identity) as GameObject;
            CardUI m_cardUICom = m_tmpCardObj.GetComponent<CardUI>();
            m_cardUICom.InitCardPosition(i);
            m_cardUICom.InitCardInfo(i);
            _cardQueue.Add(m_cardUICom);
            m_tmpCardObj.transform.SetParent(cardUIContainer, false);
            m_cardUICom.transform.SetAsFirstSibling();
        }
    }

    public void ScrollUp()
    {
        CardUI m_temp = _cardQueue[_cardQueue.Count - 1];
        _cardQueue.RemoveAt(_cardQueue.Count - 1);
        _cardQueue.Insert(0, m_temp);

        for (int i = 0; i < _cardQueue.Count; i++)
        {
            _cardQueue[i].SetPosition(i);
            if (i == 0)
            {
                _cardQueue[i].thisRectTransform.anchoredPosition = positionList[i].anchoredPosition;
                _cardQueue[i].thisRectTransform.rotation = positionList[i].rotation;
                _cardQueue[i].transform.SetAsFirstSibling();

            }
            else
            {
                _cardQueue[i].thisRectTransform.DOAnchorPos(positionList[i].anchoredPosition, .2f);
                _cardQueue[i].thisRectTransform.DORotate(positionList[i].rotation.eulerAngles, .2f);
                _cardQueue[i].transform.SetAsFirstSibling();
            }
        }
    }
    public void ScrollDown()
    {

        CardUI m_temp = _cardQueue[0];
        _cardQueue.RemoveAt(0);
        _cardQueue.Add(m_temp);
        for (int i = 0; i < _cardQueue.Count; i++)
        {
            if (i == _cardQueue.Count - 1)
            {
                _cardQueue[i].thisRectTransform.anchoredPosition = positionList[i].anchoredPosition;
                _cardQueue[i].thisRectTransform.rotation = positionList[i].rotation;
                _cardQueue[i].transform.SetAsFirstSibling();
            }
            else
            {
                _cardQueue[i].thisRectTransform.DOAnchorPos(positionList[i].anchoredPosition, .3f);
                _cardQueue[i].thisRectTransform.DORotate(positionList[i].rotation.eulerAngles, .3f);
                _cardQueue[i].transform.SetAsFirstSibling();
            }
        }
    }
}
