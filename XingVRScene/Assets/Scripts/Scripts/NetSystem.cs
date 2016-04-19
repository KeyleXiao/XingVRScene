using UnityEngine;
using System.Collections;

public class NetSystem : MonoBehaviour
{
    public static NetSystem instance;
    string _jsonConfigUrl;
    string _allDataUrl;
    string _subDataUrl;

	void Awake ()
    {
        instance = this;
        
	}
	

	void Update ()
    {
	
	}
}
