using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine.UI;

public class uGUI_FileBrowser : MonoBehaviour {

    public GameObject containerPanel;
    public GameObject fileButtonTemplate, folderButtonTemplate;
    public GameObject currentPathText;

    public string next = "";

    public string currentDirectory = "";

    public string[] supportedExtensions;
    public Sprite[] supportedIcons;

    public Sprite DirectoryIcon, UnknownFileIcon;

    List<GameObject> tempButtons = new List<GameObject>();

    public bool multiSelect = true;

    public string selectedFile = "";

    public List<string> selectedFiles = new List<string>();
    public List<string> clearSelectedFiles = new List<string>();

    public bool drawFilesOnly = false;
    public Color selectedColor = Color.cyan;
    public void OpenFolder(string folder)
    {
        currentDirectory = folder;
        OnDirectoryExpand(folder);
        LoadFolders();
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
                prev +=  dirs[i];
            }
        }
        currentDirectory = prev;
        OnDirectoryExpand(prev,true);
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
        OnDirectoryExpand(next,true);
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
        selectedFiles.Clear();
        clearSelectedFiles.Clear();
        selectedFile = "";
        currentPathText.GetComponent<InputField>().text = currentDirectory;
        for (int i = 0; i < tempButtons.Count; i++)
        {
            GameObject.Destroy(tempButtons[i]);
        }
         tempButtons.Clear();
         if (drawFilesOnly == false)
         {
             string[] folders = Directory.GetDirectories(currentDirectory, "*", SearchOption.TopDirectoryOnly);
             for (int i = 0; i < folders.Length; i++)
             {
                 int q = i;
                 GameObject newFile = (GameObject)Instantiate(folderButtonTemplate, Vector3.zero, folderButtonTemplate.transform.rotation);
                 newFile.transform.SetParent(containerPanel.transform);
                 DirectoryInfo dl = new DirectoryInfo(folders[i]);
                 newFile.transform.GetChild(0).GetComponent<Text>().text = dl.Name;
                 newFile.GetComponent<Button>().onClick.AddListener(delegate { OpenFolder(folders[q]); });
                 tempButtons.Add(newFile);
             }
         }
        LoadFiles();
    }

    public void LoadFiles()
    {
        string[] filePaths = Directory.GetFiles(currentDirectory);

        for (int i = 0; i < filePaths.Length; i++)
        {
            int q = i;
            GameObject newFile = (GameObject)Instantiate(fileButtonTemplate, Vector3.zero, fileButtonTemplate.transform.rotation);
            newFile.transform.SetParent(containerPanel.transform);
            newFile.transform.GetChild(0).GetComponent<Text>().text = Path.GetFileName(filePaths[i]);
            tempButtons.Add(newFile);
            newFile.GetComponent<Button>().onClick.AddListener(delegate { SelectFile(filePaths[q],newFile.GetComponent<Image>()); });
            //Detect File Extension
            bool found = false;
            for (int z = 0; z < supportedExtensions.Length; z++)
            {
                if (Path.GetExtension(filePaths[i]) == "." + supportedExtensions[z])
                {
                    newFile.transform.GetChild(1).GetComponent<Image>().sprite = supportedIcons[z];
                    found = true;
                }
            }
            if (found == false)
            {
                newFile.transform.GetChild(1).GetComponent<Image>().sprite = UnknownFileIcon;
            }
            
        }
    }

    public void SelectFile(string filename,Image img) //Called each time an item is selected
    {
        if (multiSelect == false)
        {
            for (int i = 0; i < tempButtons.Count; i++)
            {
                tempButtons[i].GetComponent<Image>().color = fileButtonTemplate.GetComponent<Image>().color;
            }
            img.color = selectedColor;
            selectedFile = filename;
        }
        else
        {
            if (Input.GetKey(KeyCode.LeftControl))
            {
                if (clearSelectedFiles.Contains(Path.GetFileName(filename)) == false)
                {
                    selectedFiles.Add(filename);
                    clearSelectedFiles.Add(Path.GetFileName(filename));
                    for (int i = 0; i < tempButtons.Count; i++)
                    {
                        if (clearSelectedFiles.Contains(tempButtons[i].transform.GetChild(0).GetComponent<Text>().text) == false)
                        {
                            tempButtons[i].GetComponent<Image>().color = fileButtonTemplate.GetComponent<Image>().color;
                        }
                        else
                        {
                            tempButtons[i].GetComponent<Image>().color = selectedColor;
                        }
                    }
                }
                else
                {
                    selectedFiles.Remove(filename);
                    clearSelectedFiles.Remove(Path.GetFileName(filename));
                    for (int i = 0; i < tempButtons.Count; i++)
                    {
                        if (clearSelectedFiles.Contains(tempButtons[i].transform.GetChild(0).GetComponent<Text>().text) == false)
                        {
                            tempButtons[i].GetComponent<Image>().color = fileButtonTemplate.GetComponent<Image>().color;
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
                selectedFiles.Clear();
                clearSelectedFiles.Clear();
                selectedFiles.Add(filename);
                clearSelectedFiles.Add(Path.GetFileName(filename));
                for (int i = 0; i < tempButtons.Count; i++)
                {
                    if (clearSelectedFiles.Contains(tempButtons[i].transform.GetChild(0).GetComponent<Text>().text) == false)
                    {
                        tempButtons[i].GetComponent<Image>().color = fileButtonTemplate.GetComponent<Image>().color;
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
            Debug.Log(selectedFile + " - Has been selected!");
        }
        else
        {
            for (int i = 0; i < selectedFiles.Count; i++)
            {
                Debug.Log(selectedFiles[i] + " - Has been selected!");
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

    public void NavigateTo(string directory)
    {
        currentDirectory = directory;
        currentPathText.GetComponent<InputField>().text = currentDirectory;
        LoadFolders();
    }

    //___________________________________________________________________________________//

    //Here are all the available built-in events.

    public void OnItemSelect(string selectedItem) //Called each time an item is selected/unselected
    {

    }

    public void OnDirectoryExpand(string directory, bool isFromBackForward=false) //Called each time user opens a new directory/folder
    {

    }

    public void OnDraw() //Called each time files&folders draw
    {

    }

  
   
}
