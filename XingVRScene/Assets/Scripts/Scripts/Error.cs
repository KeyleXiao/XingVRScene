using UnityEngine;
using System.Collections;

public class Error : MonoBehaviour
{
    public static Error instance;
    public GameObject errorWindow;
    public GameObject loadingUI;
    GameObject canvas;
	// Use this for initialization
	//void Awake ()
 //   {
 //       instance = this;
 //       DontDestroyOnLoad(gameObject);
 //       canvas = GameObject.Find("Canvas");
	//}

 //   public void LoadingSomething(System.Func<bool> func,System.Action funcAfterFinish)
 //   {
        
 //   }


 //   public void ThrowError(string message,UnityEngine.Events.UnityAction func)
 //   {
 //       Debug.Log(message);
 //       GameObject m_go = Instantiate(errorWindow) as GameObject;
 //       m_go.transform.parent = canvas.transform;
 //       ErrorMessage m_errorWin = m_go.GetComponent<ErrorMessage>();
 //       m_errorWin.Init(message, func);
 //   }
 //   void OnLevelWasLoaded(int level)
 //   {
 //       canvas = GameObject.Find("Canvas");
 //   }
 //   IEnumerator OpenLoading(System.Func<IEnumerator> func, System.Action funcAfterFinish)
 //   {
 //       GameObject m_loadingUI = Instantiate(loadingUI) as GameObject;
 //       m_loadingUI.transform.parent = canvas.transform;

 //       yield return new WaitForSeconds(0.2f);
 //       yield return func;
 //       if (funcAfterFinish != null)
 //           funcAfterFinish();
 //       Destroy(m_loadingUI);
 //   }
}
