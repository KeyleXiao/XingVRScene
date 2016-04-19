using UnityEngine;
using System.Collections.Generic;

public class AppDatas
{
    public static Dictionary<string, string[]> JsonConfig
    {
        get
        {
            return _JsonConfig;
        }
    }
    static Dictionary<string, string[]> _JsonConfig;
    

    public static void InitJsonConfig(string strData)
    {
        try
        {
            _JsonConfig = new Dictionary<string, string[]>();

            LitJson.JsonData m_JsonData = LitJson.JsonMapper.ToObject(strData);
            foreach (var m_key in m_JsonData.Keys)
            {

                string[] m_strArrayNames = LitJson.JsonMapper.ToObject<string[]>(m_JsonData[m_key].ToJson());
                JsonConfig.Add(m_key, m_strArrayNames);
            }
        }
        catch(System.Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }

}
