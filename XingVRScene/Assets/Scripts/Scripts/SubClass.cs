using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class SubClass : MonoBehaviour {
    public int indexInGroup;
    int height;
    string typeName;
    static GameObject buttonPrefabs;
	// Use this for initialization

	void Start ()
    {
        typeName = transform.FindChild("Icon/BigTypeName").GetComponent<Text>().text;
        buttonPrefabs = Resources.Load("SubClassButton") as GameObject;
        int m_length = AppDatas.JsonConfig[typeName].Length;
        int m_h = m_length / 2;
        m_h = m_length % 2 == 1 ? m_h + 1 : m_h;
        height = m_h * 40;
        RectTransform m_rectTransform = transform.FindChild("SubClass").GetComponent<RectTransform>();
        m_rectTransform.sizeDelta = new Vector2(m_rectTransform.GetCanvas().GetComponent<RectTransform>().sizeDelta.x, height);
        
        m_rectTransform.anchoredPosition = new Vector2(m_rectTransform.sizeDelta.x / 2, -m_rectTransform.sizeDelta.y / 2);

        GridLayoutGroup m_grid = m_rectTransform.GetComponent<GridLayoutGroup>();
        m_grid.cellSize = new Vector2(m_rectTransform.GetCanvas().GetComponent<RectTransform>().sizeDelta.x / 2, 40);

        //以上初始化ui大小 适配位置
        //添加按钮事件
        
        GetComponentInChildren<Button>().onClick.AddListener(() => TypeGroup.instance.OnOpenButtonDown(indexInGroup, height, m_rectTransform.gameObject));

        
        foreach (var item in AppDatas.JsonConfig[typeName])
        {
            GameObject button = Instantiate(buttonPrefabs) as GameObject;
            button.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            Text text = button.GetComponentInChildren<Text>();
            text.text = item;
            button.transform.parent = m_rectTransform.transform;
            button.GetComponent<Button>().onClick.AddListener(() =>
                                                                    {
                                                                        NetSystem.instance.GetSubData(text.text);
                                                                        SearchButton.instance.CloseSearch();
                                                                        TypeGroup.instance.OnOpenButtonDown(indexInGroup, height, m_rectTransform.gameObject);
                                                                    }
                                                                );

        }

        m_rectTransform.gameObject.SetActive(false);
        
    }
	


    //void OnOpenButtonDown()
    //{
    //    TypeGroup m_typeGroup = TypeGroup.instance;
    //    if (m_typeGroup.isMoving)
    //        return;
    //    m_typeGroup.isMoving = true;
    //    if (m_typeGroup.selectedIndex.HasValue)
    //    {
    //        float m_movedDis = m_typeGroup.movedDistance;
    //        if (m_typeGroup.selectedIndex.Value == indexInGroup)
    //        {
    //            for (int i = 0; i <= m_typeGroup.selectedIndex.Value; i++)
    //            {
    //                m_typeGroup.subs[i].DOAnchorPos(new Vector2(m_typeGroup.subs[i].anchoredPosition.x,
    //                                                m_typeGroup.subs[i].anchoredPosition.y - m_movedDis),
    //                                                0.5f).OnComplete(
    //                                                    () =>
    //                                                    {
    //                                                        if (m_typeGroup.selectedIndex.Value == indexInGroup)
    //                                                        {
    //                                                            m_typeGroup.movedDistance = 0;
    //                                                            m_typeGroup.selectedIndex = null;
    //                                                            m_typeGroup.isMoving = false;
    //                                                        }
    //                                                    }
    //                                                    );
    //            }
    //        }
    //        else
    //        {
    //            for (int i = 0; i <= m_typeGroup.selectedIndex.Value; i++)
    //            {
    //                m_typeGroup.subs[i].DOAnchorPos(new Vector2(m_typeGroup.subs[i].anchoredPosition.x,
    //                                                m_typeGroup.subs[i].anchoredPosition.y - m_movedDis),
    //                                                0.5f).OnComplete(OnClose);
    //            }
    //        }
            

    //    }
    //    else
    //    {
    //        m_typeGroup.selectedIndex = indexInGroup;
    //        m_typeGroup.movedDistance = height;
    //        for (int i = 0; i <= indexInGroup; i++)
    //        {
    //            m_typeGroup.subs[i].DOAnchorPos(new Vector2(m_typeGroup.subs[i].anchoredPosition.x,
    //                                            m_typeGroup.subs[i].anchoredPosition.y + height),
    //                                            0.5f).OnComplete(() => m_typeGroup.isMoving = false);
    //        }
    //    }
       
    //}
    //void OnClose()
    //{
        
    //    TypeGroup m_typeGroup = TypeGroup.instance;
    //    if (m_typeGroup.selectedIndex.Value != indexInGroup)
    //    {
    //        m_typeGroup.movedDistance = height;
    //        m_typeGroup.selectedIndex = indexInGroup;

    //        for (int i = 0; i <= indexInGroup; i++)
    //        {
    //            m_typeGroup.subs[i].DOAnchorPos(new Vector2(m_typeGroup.subs[i].anchoredPosition.x,
    //                                                m_typeGroup.subs[i].anchoredPosition.y + height),
    //                                                0.5f).OnComplete(() => m_typeGroup.isMoving = false);
    //        }
    //    }
    //}

}
