using UnityEngine;
using System.Collections;
using LitJson;


public class NetSystem : MonoBehaviour
{
    public static NetSystem instance;
    string _jsonConfigUrl = "http://139.196.41.254/configApi/configapi.php";
    string _allDataUrl = "http://139.196.41.254/configApi/nearpos.php";
    string _subDataUrl = "http://139.196.41.254/configApi/bigapi.php";

    string _jsonStatusKeyName = "code";
    string _jsonOkCode = "200";

	void Awake ()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
	}
	
    public void GetJsonConfig()
    {
        StartCoroutine(DownloadJsonConfig());
    }

    IEnumerator DownloadJsonConfig()
    {
        WWW www = new WWW(_jsonConfigUrl);
        yield return www;
        if (string.IsNullOrEmpty(www.error))
        {
            JsonData m_jd = JsonMapper.ToObject(www.text);
            if ((string)m_jd[_jsonStatusKeyName] == _jsonOkCode)
            {
                AppDatas.InitJsonConfig(m_jd["data"]);
            }
            else
            {
                Error.instance.ThrowError("网络错误" + (string)m_jd[_jsonStatusKeyName], () => GetJsonConfig());
            }
        }
        else
        {
            Error.instance.ThrowError("网络错误" + www.error, () => GetJsonConfig());
        }
    }

    public void GetSubData(string __name)
    {
        StartCoroutine(DownloadSubData(__name));
    }
    IEnumerator DownloadSubData(string __type)
    {
        WWWForm m_form = new WWWForm();
        m_form.AddField("lat", OpenGPS.lat.ToString());
        m_form.AddField("lng", OpenGPS.lng.ToString());
        m_form.AddField("type", __type);
        WWW m_www = new WWW(_subDataUrl, m_form);
        yield return m_www;
        if (string.IsNullOrEmpty(m_www.error))
        {
            JsonData m_jd = JsonMapper.ToObject(m_www.text);
            if ((string)m_jd[_jsonStatusKeyName] == _jsonOkCode)
            {
                AppDatas.InitJsonDataList(m_jd["data"].ToJson());
            }
            else
            {
                Debug.Log((string)m_jd[_jsonStatusKeyName]);
                Error.instance.ThrowError("网络错误" + (string)m_jd[_jsonStatusKeyName], () => GetSubData(__type));
            }
        }
        else
        {
            Error.instance.ThrowError("网络错误" + m_www.error, () => GetSubData(__type));
        }
    }

    public void GetAllData()
    {
        StartCoroutine(DownloadAllData());
    }

    IEnumerator DownloadAllData()
    {
        WWWForm m_form = new WWWForm();
        m_form.AddField("lat", OpenGPS.lat.ToString());
        m_form.AddField("lng", OpenGPS.lng.ToString());
        WWW m_www = new WWW(_allDataUrl, m_form);
        yield return m_www;
        if (string.IsNullOrEmpty(m_www.error))
        {
            JsonData m_jd = JsonMapper.ToObject(m_www.text);
            if ((string)m_jd[_jsonStatusKeyName] == _jsonOkCode)
            {
#if UNITY_EDITOR
                AppDatas.InitJsonDataList(System.IO.File.ReadAllText(Application.dataPath + "/1.txt"));
#elif UNITY_IOS || UNITY_ANDROID
            AppDatas.InitJsonDataList(m_jd["data"].ToJson());
#endif
            }
            else
            {
                Error.instance.ThrowError("网络错误" + (string)m_jd[_jsonStatusKeyName], () => GetAllData());
            }
        }
        else
        {
            Error.instance.ThrowError("网络错误" + m_www.error, () => GetAllData());
        }
    }
}
