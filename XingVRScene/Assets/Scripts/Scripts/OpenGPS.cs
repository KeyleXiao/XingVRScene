using UnityEngine;
using System.Collections;

public class OpenGPS : MonoBehaviour
{
    public static float lat, lng;

    bool gpsIsOk, WifiIsOk;
    IEnumerator Start()
    {
        yield return null;
        gpsIsOk = WifiIsOk = false;
#if UNITY_EDITOR

        lat = 50;
        lng = 50;

        gpsIsOk = WifiIsOk = true;


#elif UNITY_IOS || UNITY_ANDROID
        TestWifi();
        TestGPS();
#endif
        yield return new WaitUntil(() =>
                                    {
                                        return gpsIsOk && WifiIsOk;
                                    });
        NetSystem.instance.GetJsonConfig(NextScene());


    }

    IEnumerator NextScene()
    {
        float i = 0;
        while (i < 1)
        {
            i += 0.2f * Time.deltaTime;
            Loading.instance.SetLoadingValue(i);
            yield return null;
        }
    }

    void TestWifi()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            PlatformDialog.SetButtonLabel("刷新", "退出");
            PlatformDialog.Show("网络错误", "请在设置中打开网络连接", PlatformDialog.Type.OKCancel,
                                    () =>
                                    {
                                        TestWifi();
                                    },
                                    () =>
                                    {
                                        Application.Quit();
                                    });
        }
        else
        {
            WifiIsOk = true;
        }
    }
    void TestGPS()
    {
        StartCoroutine(StartTestGPS());
    }
    IEnumerator StartTestGPS()
    {
        if (!Input.location.isEnabledByUser)
        {
            PlatformDialog.SetButtonLabel("刷新", "退出");
            PlatformDialog.Show("GPS错误", "请在设置中打开GPS", PlatformDialog.Type.OKCancel,
                                    () =>
                                    {
                                        TestGPS();
                                    },
                                    () =>
                                    {
                                        Application.Quit();
                                    });
        }
        else
        {
            
            Input.location.Start();
            
            int maxWait = 20;
            while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
            {
                yield return new WaitForSeconds(1);
                maxWait--;
            }
            
            if (maxWait < 1)
            {
                Input.location.Stop();

                PlatformDialog.SetButtonLabel("刷新", "退出");
                PlatformDialog.Show("GPS错误", "GPS time out", PlatformDialog.Type.OKCancel,
                                        () =>
                                        {
                                            TestGPS();
                                        },
                                        () =>
                                        {
                                            Application.Quit();
                                        });
                
                yield break;
            }

            // Connection has failed
            if (Input.location.status == LocationServiceStatus.Failed)
            {
                Input.location.Stop();

                PlatformDialog.SetButtonLabel("刷新", "退出");
                PlatformDialog.Show("GPS错误", "Unable to determine device location", PlatformDialog.Type.OKCancel,
                                        () =>
                                        {
                                            TestGPS();
                                        },
                                        () =>
                                        {
                                            Application.Quit();
                                        });
                yield break;
            }
            else
            {
                lat = Input.location.lastData.latitude;
                lng = Input.location.lastData.longitude;
                gpsIsOk = true;
                Input.location.Stop();

            }
        }
    }
}
