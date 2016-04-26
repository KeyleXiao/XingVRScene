using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using LitJson;

public class Test111 : MonoBehaviour
{
    public GameObject canvas;
    public GameObject subClass;
    // Use this for initialization
    void Start()
    {
        //GameObject go = new GameObject("new", typeof(RectTransform),typeof(GridLayoutGroup));
        //go.transform.parent = canvas.transform;
        //go.GetComponent<RectTransform>().sizeDelta=new Vector2(Screen.width, Screen.height);
        //go.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        //go.GetComponent<GridLayoutGroup>().constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        //go.GetComponent<GridLayoutGroup>().constraintCount = 2;
        //go.GetComponent<GridLayoutGroup>().cellSize = new Vector2(Screen.width / 2, 40);

        //InitializeSubClass(new string[] { "hah", "sss", "hah", "sss", "hah", "sss", "hah", "sss", "hah", "sss" }, go.transform);

        string sData = System.IO.File.ReadAllText(Application.streamingAssetsPath + "/JsonConfig.txt");
        AppDatas.InitJsonConfig(sData);

        // Error.instance.ThrowError("error", () => Application.Quit());

    }

    // Update is called once per frame
    void Update()
    {

    }
    void InitializeSubClass(string[] names, Transform parent)
    {
        int rowCount = names.Length / 2;
        rowCount = names.Length % 2 == 1 ? rowCount + 1 : rowCount;
        GameObject subClasses = new GameObject("subClass", typeof(RectTransform), typeof(GridLayoutGroup));
        subClasses.GetComponent<GridLayoutGroup>().constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        subClasses.GetComponent<GridLayoutGroup>().constraintCount = 2;

        subClasses.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, rowCount * 40);
        foreach (var n in names)
        {
            GameObject sub = Instantiate(subClass) as GameObject;
            sub.GetComponentInChildren<Text>().text = n;
            sub.transform.parent = parent.transform;
        }
    }
}

