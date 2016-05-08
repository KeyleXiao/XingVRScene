using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OnButtonDown : MonoBehaviour {

    public Image icon;
    public Sprite iconBefore, iconAfter;
    public Image background;
    public Color colorBeforButtonDown, colorAfterButtonDown;
    public Animator buttonAnim;

    public Image BG;
    public Sprite imageOfSelected;
    static Sprite defaultimage;

    void Awake()
    {
        if (defaultimage == null)
        {
            Texture2D _tex = Resources.Load("jianshen") as Texture2D;
            defaultimage = Sprite.Create(_tex, new Rect(0, 0, _tex.width, _tex.height), new Vector2(0.5f, 0.5f));
        }
    }

    public void Open()
    {
        icon.sprite = iconAfter;
        background.color = colorAfterButtonDown;
        buttonAnim.SetBool("open", true);
        BG.sprite = imageOfSelected;
    }

    public void Close()
    {
        icon.sprite = iconBefore;
        background.color = colorBeforButtonDown;
        buttonAnim.SetBool("open", false);
        BG.sprite = defaultimage;
    }
}
