using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;

public class DataManager : MonoBehaviour {

	public static List<JsonData> allStudioJsonData = new List<JsonData>();	//所有场馆json数据的list 按照json中的id号排列
	public static List<string> allStudioType = new List<string>();			//每个大类的名称List 按照config配置表顺序排列
	public static JsonData studioConfig;									//用来存放配置表
	public static float allDataCount;										//总的场馆数量
	public static int nowSelectID;											//记录当前选择的健身房ID
	float nowLoadingCount;

	void Awake()
	{
//		for (int i = 0; i < allStudioJsonData.Count; i++) {
//			allStudioJsonData.Remove(allStudioJsonData[i]);
//		}
	}

	// Use this for initialization
	void Start () 
	{
		GetConfigData ();
	}

	public void GetConfigData()	//获取场馆大类、小类及每个小类中场馆数量的配置表
	{
		StartCoroutine (JsonParser.LoadConfig("JsonConfig",GetConfigCallBack));
	}

	void GetConfigCallBack(JsonData configData)		
	{
        
		studioConfig = configData;

        allStudioType.Add (studioConfig[0]["Name"].ToString());
		for (int i = 1; i < studioConfig.Count; i++) {
			//Debug.Log (studioConfig[i]["Name"].ToString());
			if (studioConfig[i]["Name"].ToString() != studioConfig[i - 1]["Name"].ToString()) {
				allStudioType.Add (studioConfig [i] ["Name"].ToString ());
			}
		}
		for (int j = 0; j < studioConfig.Count; j++) {
			for (int k = 0; k < int.Parse(studioConfig[j]["Length"].ToString()); k++) {
				allDataCount = allDataCount + int.Parse (studioConfig [j] ["sub" + (k + 1)]["Length"].ToString ());
			}
		}
		GetAllStudioJsonData ();
	}

	public void GetAllStudioJsonData()
	{
		for (int i = 0; i < allStudioType.Count; i++) {
			for (int j = 0; j < int.Parse(studioConfig[i]["Length"].ToString()); j++) {
				for (int k = 0; k < int.Parse(studioConfig[i]["sub" + (j + 1)]["Length"].ToString()); k++) {
					StartCoroutine (JsonParser.LoadData("JsonData_"+ studioConfig[i]["Name"] + "_" + studioConfig[i]["sub" + (j + 1)]["Name"] + "_" + (k + 1),GetDataCallBack));
					nowLoadingCount++;
				}
			}
		}
	}

	void GetDataCallBack(JsonData jsonData)
	{
		allStudioJsonData.Add (jsonData);
		Debug.Log (jsonData.ToJson ());
		Loading.instance.SetLoadingValue (nowLoadingCount/allDataCount);
	}
}
