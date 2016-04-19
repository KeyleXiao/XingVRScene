using UnityEngine;
using System.Collections;

public class Error : MonoBehaviour
{
    public static Error instance;
    public GameObject errorWindow;

    GameObject canvas;
	// Use this for initialization
	void Awake ()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
        canvas = GameObject.Find("Canvas");
	}
    public void ThrowError(string message,UnityEngine.Events.UnityAction func)
    {
        GameObject m_go = Instantiate(errorWindow) as GameObject;
        m_go.transform.parent = canvas.transform;
        ErrorMessage m_errorWin = m_go.GetComponent<ErrorMessage>();
        m_errorWin.Init(message, func);
    }
    void OnLevelWasLoaded(int level)
    {
        canvas = GameObject.Find("Canvas");
    }
}
