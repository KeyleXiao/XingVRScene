using UnityEngine;
using System.Collections.Generic;
using LitJson;
public class AppDatas
{
    public static Dictionary<string, string[]> JsonConfig
    {
        get
        {
            return _JsonConfig;
        }
    }
    public static JsonData JsonDataList
    {
        get
        {
            return _jsondataList;
        }
    }
    public static int? IndexOfSelected;
    public static JsonData DataSelected
    {
        get
        {
            if (IndexOfSelected.HasValue)
            {
                return JsonDataList[IndexOfSelected.Value];
            }
            else
            {
                Error.instance.ThrowError("没有选择数据", () => Application.Quit());
                return null;
            }
        }
    }
    static Dictionary<string, string[]> _JsonConfig;
    static JsonData _jsondataList;


    public static void InitJsonConfig(string strData)
    {
        try
        {
            _JsonConfig = new Dictionary<string, string[]>();

            JsonData m_JsonData = LitJson.JsonMapper.ToObject(strData);
            foreach (var m_key in m_JsonData.Keys)
            {

                //string[] m_strArrayNames = LitJson.JsonMapper.ToObject<string[]>(m_JsonData[m_key].ToJson());
                //JsonConfig.Add(m_key, m_strArrayNames);
                string[] m_strArrayNames = new string[(int)m_JsonData[m_key]["Length"]];
                
                for (int i = 1; i <= m_strArrayNames.Length; i++)
                {
                    m_strArrayNames[i - 1] = (string)m_JsonData[m_key]["sub" + i.ToString()]["Name"];
                }
                JsonConfig.Add((string)m_JsonData[m_key]["Name"], m_strArrayNames);
            }
        }
        catch(System.Exception ex)
        {
            Error.instance.ThrowError(ex.Message, () => Application.Quit());
            Debug.Log(ex.Message);
        }
    }

    public static void InitJsonDataList(string strData)
    {
        try
        {
            _jsondataList = JsonMapper.ToObject(strData);
        }
        catch (System.Exception ex)
        {
            Error.instance.ThrowError(ex.Message, () => Application.Quit());
        }
    }
}
