using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OnButtonDown : MonoBehaviour {

    public Image icon;
    public Sprite iconBefore, iconAfter;
    public Image background;
    public Color colorBeforButtonDown, colorAfterButtonDown;
    public Animator buttonAnim;

    public void Open()
    {
        icon.sprite = iconAfter;
        background.color = colorAfterButtonDown;
        buttonAnim.SetBool("open", true);
    }

    public void Close()
    {
        icon.sprite = iconBefore;
        background.color = colorBeforButtonDown;
        buttonAnim.SetBool("open", false);
    }
}
