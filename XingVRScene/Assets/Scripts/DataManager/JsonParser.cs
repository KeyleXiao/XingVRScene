using UnityEngine;
using System.Collections;
using LitJson;
using System.IO;
using System.Collections.Generic;

public class JsonParser : MonoBehaviour {

	public static JsonData studioJsonData;
	public static JsonData configData;

	public IEnumerator GetAllJsonDataByType(string type,int length,JsonData studioJsonData)
	{
		for (int i = 0; i < length; i++) {
			yield return null;
			//yield return StartCoroutine (LoadData("JsonData_" + type + "_" + i,studioJsonData));
		}
	}

	public static IEnumerator LoadData(string dataName,System.Action< JsonData > func)
	{
		WWW www = new WWW("file://" + Application.streamingAssetsPath + "/" + dataName + ".txt");
		yield return www;
		studioJsonData = JsonMapper.ToObject(www.text);
		func (studioJsonData);
//		int[] nums = new int[]{ 1, 2, 3, 4 };
//		Debug.Log (JsonMapper.ToObject (JsonMapper.ToJson (nums)).ToJson ());
//		studioJsonData = JsonMapper.ToObject (JsonMapper.ToJson (nums));
	}

	public static IEnumerator LoadConfig(string configName,System.Action<JsonData> func)
	{
		WWW www = new WWW("file://" + Application.streamingAssetsPath + "/" + configName + ".txt");
		yield return www;
		func (JsonMapper.ToObject(www.text));
	}
}
