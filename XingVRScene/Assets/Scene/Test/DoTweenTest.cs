using UnityEngine;
using System.Collections;
using DG.Tweening;
public class DoTweenTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //WWWForm form = new WWWForm();
        //form.AddField("lat", 50);
        //form.AddField("lng", 50);
        //form.AddField("type", "武术");
        //WWW www = new WWW("http://139.196.41.254/configApi/bigapi.php", form);
        //yield return www;
        //Debug.Log(www.text);
        AppDatas.InitJsonConfig(System.IO.File.ReadAllText(Application.streamingAssetsPath + "/JsonConfig.txt"));
    }

    // Update is called once per frame
    void Update () {
	
	}
}
