using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class uGUI_Popup : MonoBehaviour {

    public bool open = false; //The state of the popup box. True = opened, false = closed

    public GameObject popupPanel;
    public GameObject inputfieldUI;
    public GameObject optionButtonTemplate;

    public List<string> allItems = new List<string>();
    List<GameObject> tempOptions = new List<GameObject>();

    public int currentItem = 0;
    public void TogglePopup() //This void toggles the popup panel
    {
        popupPanel.SetActive(!popupPanel.activeSelf);

        if (popupPanel.activeSelf == true)
        {
            OnPopupOpened();
        }
        else
        {
            OnPopupClosed();
        }
    }

    public void UpdateUI() //Updates the selected object UI
    {
        inputfieldUI.GetComponent<InputField>().text = allItems[currentItem];
    }

    public void SelectItem(int index) //Called each time an item button is pressed
    {
        currentItem = index;
        TogglePopup();
        UpdateUI();
        OnItemSelect(index);
    }

    public void GenerateOptions()
    {
        for (int i = 0; i < tempOptions.Count; i++)
        {
            GameObject.Destroy(tempOptions[i]);
        }
        tempOptions.Clear();
        popupPanel.SetActive(true);
        for (int i = 0; i < allItems.Count; i++)
        {
            int t = i;
            GameObject go = (GameObject)Instantiate(optionButtonTemplate, Vector3.zero, optionButtonTemplate.transform.rotation);
            go.transform.SetParent(popupPanel.transform);
            go.transform.GetChild(0).GetComponent<Text>().text = allItems[i];
            go.GetComponent<Button>().onClick.AddListener(delegate { SelectItem(t); });
            tempOptions.Add(go);
        }
        popupPanel.SetActive(false);
    }

    void Start()
    {
        GenerateOptions();
        UpdateUI();
    }

    //___________________________________________________________________________________//

    //Here are all the available built-in functions

    public void AddOption(string name)
    {
        allItems.Add(name);
        GenerateOptions();
    } //Adding item with the given string as name

    public void RemoveOption(int index, string name = "")
    {
        if (name == "")
        {
            allItems.RemoveAt(index);
        }
        else
        {
            allItems.Remove(name);
        }
        GenerateOptions();
    } // Remove item at the given index if you let the name string emtpy, or remove the item with the given name and ignores the index
  
    public void AddOptions(string[] names) //Add items with the given strings
    {
        allItems.AddRange(names);
        GenerateOptions();
    }

    public void RemoveOptions(int index, int count) //Remove items from the current index to index + count
    {
        allItems.RemoveRange(index, count);
        GenerateOptions();
    }

   

    //___________________________________________________________________________________//

    //Here are all the available built-in events.
    private void OnItemSelect(int selectedItemIndex) //This is called each time an item is selected. The selectedItemIndex is the index of the item being selected.
    {
      
    }

    private void OnPopupOpened() //This is called each time the popup is opened
    {

    }

    private void OnPopupClosed() //This is called each time the popup is closed
    {

    }
}
