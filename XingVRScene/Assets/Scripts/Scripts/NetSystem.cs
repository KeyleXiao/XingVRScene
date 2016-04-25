using UnityEngine;
using System.Collections;
using LitJson;


public class NetSystem : MonoBehaviour
{
    public static NetSystem instance;
    string _jsonConfigUrl;
    string _allDataUrl;
    string _subDataUrl = "http://139.196.41.254/configApi/bigapi.php";

    string _jsonStatusKeyName = "code";
    string _jsonOkCode = "200";

	void Awake ()
    {
        instance = this;
        
	}
	
    public void GetJsonConfig()
    {

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

            }
        }
        else
        {
            Debug.Log(www.error);
        }
    }

    public void GetSubData(string __name)
    {
        StartCoroutine(DownloadSubData(__name));
    }
    IEnumerator DownloadSubData(string __type)
    {
        WWWForm m_form = new WWWForm();
        m_form.AddField("lat", 50);
        m_form.AddField("lng", 50);
        m_form.AddField("type", __type);
        WWW m_www = new WWW(_subDataUrl, m_form);
        yield return m_www;
        if (string.IsNullOrEmpty(m_www.error))
        {
            JsonData m_jd = JsonMapper.ToObject(m_www.text);
            if ((string)m_jd[_jsonStatusKeyName] == _jsonOkCode)
            {
                Debug.Log(m_www.text);
            }
            else
            {

            }
        }
        else
        {
            Debug.Log(m_www.error);
        }
    }
}
