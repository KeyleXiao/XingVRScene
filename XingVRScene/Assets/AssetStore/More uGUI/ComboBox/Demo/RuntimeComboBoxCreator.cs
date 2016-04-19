using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RuntimeComboBoxCreator : MonoBehaviour
{
    public Sprite UISprite;
    public Sprite Background;
    public Font font;

	private void Start()
	{
		var rtCanvas = GameObject.Find("Canvas").GetComponent<RectTransform>();

		var goComboBox = new GameObject("ComboBox Runtime");

		var rtComboBox = goComboBox.AddComponent<RectTransform>();
		rtComboBox.SetParent(rtCanvas, false);
		rtComboBox.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 160);
		rtComboBox.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 30);
		rtComboBox.anchoredPosition = Vector2.zero;

		var comboBox = goComboBox.AddComponent<ComboBox>();
		
		comboBox.UISprite = UISprite;
		comboBox.Background = Background;
		comboBox.font = font;

		comboBox.Start();

		comboBox.HideFirst = false;
		comboBox.AddItems("Select item..", "Item 1", "Item 2", "Item 3", "Item 4", new ComboBoxItem("Item 5", UISprite), UISprite);
		comboBox[3].IsDisabled = true;
	}
}
