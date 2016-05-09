using UnityEngine;
using System.Collections;
using LitJson;


public class NetSystem : MonoBehaviour
{
    public static NetSystem instance;
    string _jsonConfigUrl = "http://139.196.41.254/configApi/configapi.php";
    string _allDataUrl = "http://139.196.41.254/configApi/nearpos.php";
    string _subDataUrl = "http://139.196.41.254/configApi/bigapi.php";
    string _plusVisitTimes = "http://139.196.41.254/configApi/addvisit.php";

    string _jsonStatusKeyName = "code";
    string _jsonOkCode = "200";

	void Awake ()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
	}
	
    public void GetJsonConfig(IEnumerator func)
    {
        StartCoroutine(DownloadJsonConfig(func));
    }

    IEnumerator DownloadJsonConfig(IEnumerator func)
    {
        WWW www = new WWW(_jsonConfigUrl);
        yield return www;
        if (string.IsNullOrEmpty(www.error))
        {
            JsonData m_jd = JsonMapper.ToObject(www.text);
            if ((string)m_jd[_jsonStatusKeyName] == _jsonOkCode)
            {
                AppDatas.InitJsonConfig(m_jd["data"]);
                yield return func;
            }
            else
            {
                PlatformDialog.SetButtonLabel("刷新", "退出");
                PlatformDialog.Show(
                    "网络错误",
                    (string)m_jd[_jsonStatusKeyName],
                    PlatformDialog.Type.OKCancel,
                    () => GetJsonConfig(func),
                    () => Application.Quit()
                );
                //Error.instance.ThrowError("网络错误" + (string)m_jd[_jsonStatusKeyName], () => GetJsonConfig(func));
            }
        }
        else
        {
            PlatformDialog.SetButtonLabel("刷新", "退出");
            PlatformDialog.Show(
                "网络错误",
                www.error,
                PlatformDialog.Type.OKCancel,
                () => GetJsonConfig(func),
                () => Application.Quit()
            );
            //Error.instance.ThrowError("网络错误" + www.error, () => GetJsonConfig(func));
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
                ScrollView3dUI.instance.Init3DUI();

            }
            else
            {
                //Debug.Log((string)m_jd[_jsonStatusKeyName]);
                PlatformDialog.SetButtonLabel("刷新", "退出");
                PlatformDialog.Show(
                    "网络错误",
                    (string)m_jd[_jsonStatusKeyName],
                    PlatformDialog.Type.OKCancel,
                    () => GetSubData(__type),
                    () => Application.Quit()
                );
                //Error.instance.ThrowError("网络错误" + (string)m_jd[_jsonStatusKeyName], () => GetSubData(__type));
            }
        }
        else
        {
            PlatformDialog.SetButtonLabel("刷新", "退出");
            PlatformDialog.Show(
                "网络错误",
                m_www.error,
                PlatformDialog.Type.OKCancel,
                () => GetSubData(__type),
                () => Application.Quit()
            );
            //Error.instance.ThrowError("网络错误" + m_www.error, () => GetSubData(__type));
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
                //#if UNITY_EDITOR
                //                AppDatas.InitJsonDataList(System.IO.File.ReadAllText(Application.dataPath + "/1.txt"));
                //#elif UNITY_IOS || UNITY_ANDROID
            AppDatas.InitJsonDataList(m_jd["data"].ToJson());
//#endif
                ScrollView3dUI.instance.Init3DUI();
            }
            else
            {
                PlatformDialog.SetButtonLabel("刷新", "退出");
                PlatformDialog.Show(
                    "网络错误",
                    (string)m_jd[_jsonStatusKeyName],
                    PlatformDialog.Type.OKCancel,
                    () => GetAllData(),
                    () => Application.Quit()
                );
                //Error.instance.ThrowError("网络错误" + (string)m_jd[_jsonStatusKeyName], () => GetAllData());
            }
        }
        else
        {
            PlatformDialog.SetButtonLabel("刷新", "退出");
            PlatformDialog.Show(
                "网络错误",
                m_www.error,
                PlatformDialog.Type.OKCancel,
                () => GetAllData(),
                () => Application.Quit()
            );
            //Error.instance.ThrowError("网络错误" + m_www.error, () => GetAllData());
        }
    }

    public void AddVisit(string __name)
    {
        StartCoroutine(AddVisitTimes(__name));
    }
    IEnumerator AddVisitTimes(string __name)
    {
        WWWForm m_form = new WWWForm();
        m_form.AddField("name", __name);
        WWW m_www = new WWW(_plusVisitTimes, m_form);
        yield return m_www;
        if (string.IsNullOrEmpty(m_www.error))
        {
            JsonData m_jd = JsonMapper.ToObject(m_www.text);
            //Debug.Log(m_www.text);
            if ((int)m_jd[_jsonStatusKeyName] == 200)
            {
                yield break;
            }
            else
            {
                PlatformDialog.SetButtonLabel("刷新", "退出");
                PlatformDialog.Show(
                    "网络错误",
                    (string)m_jd[_jsonStatusKeyName],
                    PlatformDialog.Type.OKCancel,
                    () => AddVisit(__name),
                    () => Application.Quit()
                );
            }
        }
        else
        {
            PlatformDialog.SetButtonLabel("刷新", "退出");
            PlatformDialog.Show(
                "网络错误",
                m_www.error,
                PlatformDialog.Type.OKCancel,
                () => AddVisit(__name),
                () => Application.Quit()
            );
        }
    }
}
