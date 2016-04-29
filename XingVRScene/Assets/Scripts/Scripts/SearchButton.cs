using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SearchButton : MonoBehaviour {

    public Image sesrchButtonUI, smallRangeUI, AllRangeUI;
    public GridLayoutGroup gridUI;
    public static SearchButton instance;
	// Use this for initialization
	void Start ()
    {
        instance = this;
        gridUI.spacing = new Vector2((Screen.width - 88) / 2, 0);
        AllRange();
	}

    void OnEnable()
    {
    }



    public void Search()
    {
        gridUI.gameObject.SetActive(true);
        sesrchButtonUI.gameObject.SetActive(false);
        ScrollView3dUI.instance.Close3DUI();
        smallRangeUI.color = AllRangeUI.color = Color.white;

    }

    public void SmallRange()
    {
        smallRangeUI.color = Color.red;
        TypeGroup.instance.gameObject.SetActive(true);
        TypeGroup.instance.OpenPanel();
    }

    public void AllRange()
    {
        NetSystem.instance.GetAllData();
        SearchButton.instance.CloseSearch();
        smallRangeUI.color = Color.white;
        TypeGroup.instance.ClosePanel();
        TypeGroup.instance.gameObject.SetActive(false);

    }
    public void CloseSearch()
    {
        gridUI.gameObject.SetActive(false);
        sesrchButtonUI.gameObject.SetActive(true);
        TypeGroup.instance.ClosePanel();
        TypeGroup.instance.gameObject.SetActive(false);
    }
}
