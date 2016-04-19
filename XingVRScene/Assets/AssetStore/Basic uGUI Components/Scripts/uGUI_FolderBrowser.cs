using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine.UI;

public class uGUI_FolderBrowser : MonoBehaviour
{

    public GameObject containerPanel;
    public GameObject folderButtonTemplate;
    public GameObject currentPathText;

    public string next = "";

    public string currentDirectory = "";

    public Sprite DirectoryIcon;

    List<GameObject> tempButtons = new List<GameObject>();

    public bool multiSelect = true;

    public string selectedDirectory = "";

    public List<string> selectedDirectories = new List<string>();
    public List<string> clearSelectedDirectories = new List<string>();

    public Color selectedColor = Color.cyan;

    public void OpenFolder(string folder)
    {
        if (!Input.GetKey(KeyCode.LeftControl))
        {
            currentDirectory = folder;
            OnDirectoryExpand(folder);
            LoadFolders();
        }
    }

    public void GoBack()
    {
        next = currentDirectory;
        string prev = "";
        string current = currentDirectory;
        string[] dirs = current.Split('\\');
        for (int i = 0; i < dirs.Length - 1; i++)
        {
            if (prev != "")
            {
                prev += "\\" + dirs[i];
            }
            else
            {
                prev += dirs[i];
            }
        }
        currentDirectory = prev;
        OnDirectoryExpand(prev, true);
        LoadFolders();
    }

    public void Cancel()
    {
        for (int i = 0; i < tempButtons.Count; i++)
        {
            GameObject.Destroy(tempButtons[i]);
        }
        tempButtons.Clear();
        gameObject.SetActive(false);
    }

    public void GoForward()
    {
        currentDirectory = next;
        OnDirectoryExpand(next, true);
        LoadFolders();
    }

    public void GoTo()
    {
        currentDirectory = currentPathText.GetComponent<InputField>().text;
        LoadFolders();
    }

    public void LoadFolders()
    {
        OnDraw();
        selectedDirectories.Clear();
        clearSelectedDirectories.Clear();
        selectedDirectory = "";
        currentPathText.GetComponent<InputField>().text = currentDirectory;
        for (int i = 0; i < tempButtons.Count; i++)
        {
            GameObject.Destroy(tempButtons[i]);
        }
        tempButtons.Clear();
        string[] folders = Directory.GetDirectories(currentDirectory, "*", System.IO.SearchOption.TopDirectoryOnly);
        for (int i = 0; i < folders.Length; i++)
        {
            int q = i;
            GameObject newFile = (GameObject)Instantiate(folderButtonTemplate, Vector3.zero, folderButtonTemplate.transform.rotation);
            newFile.transform.SetParent(containerPanel.transform);
            DirectoryInfo dl = new DirectoryInfo(folders[i]);
            newFile.transform.GetChild(0).GetComponent<Text>().text = dl.Name;
            newFile.GetComponent<Button>().onClick.AddListener(delegate { OpenFolder(folders[q]); });
            newFile.GetComponent<Button>().onClick.AddListener(delegate { SelectDirectory(folders[q], newFile.GetComponent<Image>()); });
            tempButtons.Add(newFile);
        }
    }
    public void SelectDirectory(string filename, Image img) //Called each time an item is selected
    {
        
        if (multiSelect == false)
        {
            if (Input.GetKey(KeyCode.LeftControl))
            {
                for (int i = 0; i < tempButtons.Count; i++)
                {
                    tempButtons[i].GetComponent<Image>().color = folderButtonTemplate.GetComponent<Image>().color;
                }
                img.color = selectedColor;
                selectedDirectory = filename;
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.LeftControl))
            {
                if (clearSelectedDirectories.Contains(Path.GetFileName(filename)) == false)
                {
                    selectedDirectories.Add(filename);
                    clearSelectedDirectories.Add(Path.GetFileName(filename));
                    for (int i = 0; i < tempButtons.Count; i++)
                    {
                        if (clearSelectedDirectories.Contains(tempButtons[i].transform.GetChild(0).GetComponent<Text>().text) == false)
                        {
                            tempButtons[i].GetComponent<Image>().color = folderButtonTemplate.GetComponent<Image>().color;
                        }
                        else
                        {
                            tempButtons[i].GetComponent<Image>().color = selectedColor;
                        }
                    }
                }
                else
                {
                    selectedDirectories.Remove(filename);
                    clearSelectedDirectories.Remove(Path.GetFileName(filename));
                    for (int i = 0; i < tempButtons.Count; i++)
                    {
                        if (clearSelectedDirectories.Contains(tempButtons[i].transform.GetChild(0).GetComponent<Text>().text) == false)
                        {
                            tempButtons[i].GetComponent<Image>().color = folderButtonTemplate.GetComponent<Image>().color;
                        }
                        else
                        {
                            tempButtons[i].GetComponent<Image>().color = selectedColor;
                        }
                    }
                }
            }
            else
            {
                selectedDirectories.Clear();
                clearSelectedDirectories.Clear();
                selectedDirectories.Add(filename);
                clearSelectedDirectories.Add(Path.GetFileName(filename));
                for (int i = 0; i < tempButtons.Count; i++)
                {
                    if (clearSelectedDirectories.Contains(tempButtons[i].transform.GetChild(0).GetComponent<Text>().text) == false)
                    {
                        tempButtons[i].GetComponent<Image>().color = folderButtonTemplate.GetComponent<Image>().color;
                    }
                    else
                    {
                        tempButtons[i].GetComponent<Image>().color = selectedColor;
                    }
                }
            }
        }
        OnItemSelect(filename);
    }

    public void SelectFileButton()
    {
        if (multiSelect == false)
        {
            Debug.Log(selectedDirectory + " - Has been selected!");
        }
        else
        {
            for (int i = 0; i < selectedDirectories.Count; i++)
            {
                Debug.Log(selectedDirectories[i] + " - Has been selected!");
            }
        }
        Cancel();
    }

    void Start()
    {
        if (currentDirectory == "")
            currentDirectory = System.Environment.CurrentDirectory;
        LoadFolders();
    }

    //___________________________________________________________________________________//

    //Here are all the available built-in functions

    public void ForceRefresh()//Refresh the current directory
    {
        LoadFolders();
    }

    //___________________________________________________________________________________//

    //Here are all the available built-in events.

    public void OnItemSelect(string selectedItem) //Called each time an item is selected/diselected
    {

    }

    public void OnDirectoryExpand(string directory, bool isFromBackForward = false) //Called each time user opens a new directory/folder
    {

    }

    public void OnDraw() //Called each time files&folders draw
    {

    }


}
