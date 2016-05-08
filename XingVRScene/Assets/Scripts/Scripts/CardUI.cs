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
    public Image bg, logo;
    public Text distance, hasSee, pay, nameOfGym;
    public LitJson.JsonData data
    {
        get
        {
            return _data;
        }
    }

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
        gameObject.SetActive(true);

        _indexOfInfo = __index;
        if (_indexOfInfo >= 0 && _indexOfInfo < AppDatas.JsonDataList.Count)
        {
            _data = AppDatas.JsonDataList[_indexOfInfo.Value];
            //Debug.Log(_data.ToJson());
            string _strDistance = _data["StudioInfo"]["Address"]["Coordinate"]["distance"].ToJson();
            distance.text = (float.Parse(_strDistance) / 1000f).ToString("F1") + "km";
            hasSee.text = (string)_data["StudioInfo"]["Visit"];
            pay.text = (string)_data["StudioInfo"]["Buy"];
            nameOfGym.text = (string)_data["StudioName"];
//#if UNITY_EDITOR
            //StartCoroutine(DownloadLogo("http://www.0739i.com.cn/data/attachment/portal/201603/09/120158ksjocrjsoohrmhtg.jpg"));
//#elif UNITY_IOS || UNITY_ANDROID
            StartCoroutine(DownloadLogo((string)_data["StudioLogo"]));
//#endif

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


    IEnumerator DownloadLogo(string __url)
    {
        logo.gameObject.SetActive(false);
    //    float a = logo.color.a;
    //    logo.color = new Color(1, 1, 1, 0);
        WWW www = new WWW(__url);
        yield return www;
        if (string.IsNullOrEmpty(www.error))
        {
            logo.sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0.5f, .05f));
            logo.gameObject.SetActive(true);

        }
        else
        {
            PlatformDialog.SetButtonLabel("刷新", "退出");
            PlatformDialog.Show(
                "网络错误",
                www.error,
                PlatformDialog.Type.OKCancel,
                () => StartCoroutine(DownloadLogo(__url)),
                () => Application.Quit()
            );
            //Error.instance.ThrowError("网络错误" + www.error, () => StartCoroutine(DownloadLogo(__url)));
        }
    }
}
