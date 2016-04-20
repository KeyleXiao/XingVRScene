using UnityEngine;
using System.Collections;
using LitJson;


public class NetSystem : MonoBehaviour
{
    public static NetSystem instance;
    string _jsonConfigUrl;
    string _allDataUrl;
    string _subDataUrl;

    string _jsonStatusKeyName;
    enum _wwwStatus
    {
        ok,
        error
    }
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
            if ((int)m_jd[_jsonStatusKeyName] == (int)_wwwStatus.ok)
            {

            }
        }
    }
}
