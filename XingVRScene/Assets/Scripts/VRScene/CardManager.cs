using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using LitJson;
using System.IO;

public class CardManager: MonoBehaviour {

	public int nowIndex{ get; set;}
	public int thisID;
	public Image thisImageBG;
	public Image thisSprite;
	public Text thisName;

	public CardManager(int id)
	{
		nowIndex = id;
		//InitialData ();
	}

	public int GetID()
	{
		return nowIndex;
	}

	public void InitialData()
	{
		StartCoroutine ("LoadLogo");
		thisName.text = DataManager.allStudioJsonData [thisID] ["StudioData"] ["StudioName"].ToString ();
	}

	IEnumerator LoadLogo()
	{
		WWW www = new WWW ("file://" + DataManager.allStudioJsonData[thisID]["StudioData"] ["StudioLogo"].ToString());
		yield return www;
		if(www != null && string.IsNullOrEmpty(www.error))
		{
			Texture2D img = www.texture as Texture2D; 
			//byte[] data = img.EncodeToPNG();
			thisSprite.sprite = Sprite.Create(img, new Rect(0, 0, img.width, img.height), new Vector2(0, 0));
		}
	}
}
