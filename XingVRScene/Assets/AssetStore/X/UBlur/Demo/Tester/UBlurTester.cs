using UnityEngine;
using X.UBlur;

public class UBlurTester : MonoBehaviour {

    public UBlur ublur;

    public void BlurOn() {
        if (ublur != null) {
            ublur.EnableBlur(true);
        }
    }

    public void BlurOff() {
        if (ublur != null) {
            ublur.EnableBlur(false);
        }
    }

    public void FisheyeOn() {
        if (ublur != null) {
            ublur.EnableFisheye(true);
        }
    }

    public void FisheyeOff() {
        if (ublur != null) {
            ublur.EnableFisheye(false);
        }
    }
}
