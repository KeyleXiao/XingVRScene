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
                PlatformDialog.SetButtonLabel("OK");
                PlatformDialog.Show("错误", "没有选择数据", PlatformDialog.Type.SubmitOnly, () => Application.Quit(), null);
                //Error.instance.ThrowError("没有选择数据", () => Application.Quit());
                return null;
            }
        }
    }
    static Dictionary<string, string[]> _JsonConfig;
    static JsonData _jsondataList;


    public static void InitJsonConfig(JsonData Data)
    {
        //try
        {

            _JsonConfig = new Dictionary<string, string[]>();
            string[] _type = new string[] { "格斗类", "大球类", "舞蹈类", "小球类", "健身类", "儿童类", "其他类" };
            for (int i = 0; i < Data.Count; i++)
            {
                string[] m_strArrayNames = new string[Data[i]["type" + i.ToString()].Count];
                for (int j = 0; j < Data[i]["type" + i.ToString()].Count; j++)
                {
                    m_strArrayNames[j] = ((string)Data[i]["type" + i.ToString()][j]["sub" + j.ToString()]["Name"]).Trim();
                }
                JsonConfig.Add(_type[i], m_strArrayNames);
            }
            //foreach (var item in JsonConfig)
            //{
            //    foreach (var v in item.Value)
            //    {
            //        Debug.Log(item.Key + " " + v);
            //    }
            //}
            
        }
        //catch (System.Exception ex)
        //{
        //    Debug.Log(ex.Message);
        //    Error.instance.ThrowError(ex.Message, () => Application.Quit());
        //    Debug.Log(ex.Message);
        //}
    }

    public static void InitJsonDataList(string strData)
    {
        try
        {
            _jsondataList = JsonMapper.ToObject(strData);
        }
        catch (System.Exception ex)
        {
            PlatformDialog.SetButtonLabel("OK");
            PlatformDialog.Show("错误", ex.Message, PlatformDialog.Type.SubmitOnly, () => Application.Quit(), null);
        }
    }
}
