using UnityEngine;
using System.Collections;

public class ErrorMessage : MonoBehaviour {
    public UnityEngine.UI.Text text;
    public UnityEngine.UI.Button button;

    public void Init(string message,UnityEngine.Events.UnityAction func)
    {
        RectTransform m_rectTransform = GetComponent<RectTransform>();
        m_rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height / 2);
        m_rectTransform.anchoredPosition = Vector2.zero;
        text.text = message;
        button.onClick.AddListener(func);
    }

}
