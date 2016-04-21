using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CardUI : MonoBehaviour
{

    public RectTransform thisRectTransform;
    public int? IndexOfInfo
    {
        get
        {
            return _indexOfInfo;
        }
    }
    public Text distance, hasSee, pay;

    LitJson.JsonData _data;
    int _indexOfPosition;
    int? _indexOfInfo;
	// Use this for initialization
	void Awake ()
    {
        thisRectTransform = GetComponent<RectTransform>();
	}
	
    public void InitCardInfo(int __index)
    {
        _indexOfInfo = __index;
        if (_indexOfInfo >= 0 && _indexOfInfo < AppDatas.JsonDataList.Count)
        {
            _data = AppDatas.JsonDataList[_indexOfInfo.Value];
            distance.text = (string)_data["distance"];
            hasSee.text = (string)_data["Visit"];
            pay.text = (string)_data["Buy"];

        }
        else
        {
            _data = null;
            gameObject.SetActive(false);
        }

    }
    public void InitCardPosition(int __index)
    {
        _indexOfPosition = __index;
        thisRectTransform.anchoredPosition = ScrollView3dUI.instance.positionList[_indexOfPosition].anchoredPosition;
        thisRectTransform.rotation = ScrollView3dUI.instance.positionList[_indexOfPosition].rotation;
    }
    public void SetPosition(int __index)
    {
        _indexOfPosition = __index;
    }
}
