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
    void Start()
    {
        instance = this;
        InitialCardPosition();
        InitailCardList();
        ChangeAlpha();

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
            _cardQueue.Add(m_cardUICom);
            m_tmpCardObj.transform.SetParent(cardUIContainer, false);
            m_cardUICom.transform.SetAsFirstSibling();
        }
        for (int i = 1; i < 9; i++)
        {
            _cardQueue[i].InitCardInfo(i - 1);
        }
    }

    public void ScrollUp()
    {
        if (_cardQueue[0].data == null)
        {
            return;
        }
        CardUI m_temp = _cardQueue[_cardQueue.Count - 1];
        _cardQueue.RemoveAt(_cardQueue.Count - 1);
        m_temp.InitCardInfo(_cardQueue[0].IndexOfInfo.Value - 1);
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
        ChangeAlpha();

    }
    public void ScrollDown()
    {
        if (_cardQueue[2].data == null)
        {
            return;
        }
        CardUI m_temp = _cardQueue[0];
        _cardQueue.RemoveAt(0);
        m_temp.InitCardInfo(_cardQueue[_cardQueue.Count - 1].IndexOfInfo.Value + 1);

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
        ChangeAlpha();

    }

    void ChangeAlpha()
    {
        for (int i = 1; i < _cardQueue.Count; i++)
        {
            if (_cardQueue[i].gameObject.activeSelf)
            {
                Color _color;
                _color = _cardQueue[i].bg.color; _color.a = 1 - (i - 1) * 0.15f;_cardQueue[i].bg.color = _color;
                _color = _cardQueue[i].logo.color; _color.a = 1 - (i - 1) * 0.15f; _cardQueue[i].logo.color = _color;
                _color = _cardQueue[i].distance.color; _color.a = 1 - (i - 1) * 0.15f; _cardQueue[i].distance.color = _color;
                _color = _cardQueue[i].hasSee.color; _color.a = 1 - (i - 1) * 0.15f; _cardQueue[i].hasSee.color = _color;
                _color = _cardQueue[i].pay.color; _color.a = 1 - (i - 1) * 0.15f; _cardQueue[i].pay.color = _color;
                _color = _cardQueue[i].nameOfGym.color; _color.a = 1 - (i - 1) * 0.15f; _cardQueue[i].nameOfGym.color = _color;
            }
        }
    }

}
