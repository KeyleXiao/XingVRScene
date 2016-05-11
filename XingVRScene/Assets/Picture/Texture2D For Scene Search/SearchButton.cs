using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SearchButton : MonoBehaviour {

    public Image sesrchButtonUI, smallRangeUI, AllRangeUI, BG;
    public GridLayoutGroup gridUI;
    public static SearchButton instance;
    Sprite defaultimage;
    // Use this for initialization
    void Start ()
    {
        if (defaultimage == null)
        {
            Texture2D _tex = Resources.Load("jianshen") as Texture2D;
            defaultimage = Sprite.Create(_tex, new Rect(0, 0, _tex.width, _tex.height), new Vector2(0.5f, 0.5f));
        }
        instance = this;
        gridUI.spacing = new Vector2((GetComponent<RectTransform>().GetCanvas().GetComponent<RectTransform>().sizeDelta.x - 176) / 2, 0);
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
        SmallRange();
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
        BG.sprite = defaultimage;
        //TypeGroup.instance.gameObject.SetActive(false);

    }
    public void CloseSearch()
    {
        
        TypeGroup.instance.ClosePanel(()=>
                                        {
                                            gridUI.gameObject.SetActive(false);
                                            sesrchButtonUI.gameObject.SetActive(true);
                                        });
        //TypeGroup.instance.gameObject.SetActive(false);
    }
}
