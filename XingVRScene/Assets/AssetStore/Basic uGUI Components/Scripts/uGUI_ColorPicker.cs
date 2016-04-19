using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class uGUI_ColorPicker : MonoBehaviour {

    public Slider[] allSliders;
    public Text[] allTexts;
    public GameObject preview;

    private float rV = 255, gV = 255, bV = 255, aV = 255;

    private Color color = Color.white;

    public Vector4 hexColor(float r, float g, float b, float a)
    {
        Vector4 color = new Vector4(r / 255, g / 255, b / 255, a / 255);
        return color;
    }

    public void SetTextFromColor()
    {
        for (int id = 0; id < allSliders.Length; id++)
        {
            allTexts[id].text = allSliders[id].value.ToString();
        }
    }

    public void SetRGBA()
    {
        rV = allSliders[0].value;
        gV = allSliders[1].value;
        bV = allSliders[2].value;
        aV = allSliders[3].value;
        color = hexColor(rV, gV, bV, aV);
        OnColorChangeEvent();
        SetTextFromColor();
        SetPreview();
    }

    public void SetPreview()
    {
        preview.GetComponent<Image>().color = GetColor();
    }


    //___________________________________________________________________________________//

    //Here are all the available built-in functions

    public void ResetColor()
    {
        color = Color.white;
        rV = 255;
        gV = 255;
        bV = 255;
        aV = 255;
        allSliders[0].value = 255;
        allSliders[1].value = 255;
        allSliders[2].value = 255;
        allSliders[3].value = 255;
        SetTextFromColor();
        SetPreview();
    } //Reset all values

    public Color GetColor() //Returns the current color
    {
        return color;
    } 


    //___________________________________________________________________________________//

    //Here are all the available built-in events.

    public void OnPickButtonPress() //On Pick button press
    {
        //On button press commands

        ResetColor(); //Disable if you want to store the latest color
        gameObject.SetActive(false);
    }

    public void OnCancelButtonPress() //On Cancel button press
    {
        ResetColor();
        gameObject.SetActive(false);
    }

    public void OnColorChangeEvent() //Called each time the color changes.
    {

    }
}
