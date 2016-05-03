using UnityEngine;
using System.Collections;

public class OpenGPS : MonoBehaviour
{
    public static float lat, lng;
    IEnumerator Start()
    {
        yield return null;
#if  UNITY_EDITOR

        lat = 50;
        lng = 50;
#elif UNITY_IOS || UNITY_ANDROID
           if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Error.instance.ThrowError("无法连接到网络，请打开网络连接", () => Application.Quit());
        }
        else if (!Input.location.isEnabledByUser)
        {
            Error.instance.ThrowError("请在设置中打开GPS", () => Application.Quit());
        }
        else
        {

            // Start service before querying location
            Input.location.Start();

            // Wait until service initializes
            int maxWait = 20;
            while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
            {
                yield return new WaitForSeconds(1);
                maxWait--;
            }

            // Service didn't initialize in 20 seconds
            if (maxWait < 1)
            {
                Error.instance.ThrowError("GPS time out", () => Application.Quit());
                yield break;
            }

            // Connection has failed
            if (Input.location.status == LocationServiceStatus.Failed)
            {
                Error.instance.ThrowError("Unable to determine device location", () => Application.Quit());
                yield break;
            }
            else
            {
                lat = Input.location.lastData.latitude;
                lng = Input.location.lastData.longitude;
            }
            Input.location.Stop();   
        }
             
#endif
        NetSystem.instance.GetJsonConfig();
        float i = 0;
        while (i < 1)
        {
            i += 0.2f * Time.deltaTime;
            Loading.instance.SetLoadingValue(i);
            yield return null;
        }

    }


}
