#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor (typeof(uGUI_FileBrowser))]
public class uGUI_FileBrowserEditor : Editor {

    private SerializedObject m_Object;
    private SerializedProperty m_Property;

    void OnEnable()
    {
        m_Object = new SerializedObject(target);
    }

    bool systemSettings = false;
    public override void OnInspectorGUI()
    {
        GUILayout.Label("");
        GUILayout.Label("uGUI File Browser");

        if (systemSettings == true)
        {
            GUILayout.Label("");
            m_Property = m_Object.FindProperty("containerPanel");
            EditorGUILayout.PropertyField(m_Property, new GUIContent("File Container Panel :"), true);
            m_Object.ApplyModifiedProperties();
            m_Property = m_Object.FindProperty("fileButtonTemplate");
            EditorGUILayout.PropertyField(m_Property, new GUIContent("File Button Template :"), true);
            m_Object.ApplyModifiedProperties();
            m_Property = m_Object.FindProperty("folderButtonTemplate");
            EditorGUILayout.PropertyField(m_Property, new GUIContent("Folder Button Template :"), true);
            m_Object.ApplyModifiedProperties();
            m_Property = m_Object.FindProperty("currentPathText");
            EditorGUILayout.PropertyField(m_Property, new GUIContent("Current Path InputField :"), true);
            m_Object.ApplyModifiedProperties();
            GUILayout.Label("");
            if (GUILayout.Button("Close System Settings"))
                systemSettings = false;
        }
        else
        {
            if (GUILayout.Button("Open System Settings"))
                systemSettings = true;
        }
        GUILayout.Label("");
        GUILayout.Label("Extension & Icon Settings");
        GUILayout.Label("");
        m_Property = m_Object.FindProperty("supportedExtensions");
        EditorGUILayout.PropertyField(m_Property, new GUIContent("Array Of Supported Extensions :"), true);
        m_Object.ApplyModifiedProperties();
        m_Property = m_Object.FindProperty("supportedIcons");
        EditorGUILayout.PropertyField(m_Property, new GUIContent("Array Of Supported Sprites :"), true);
        m_Object.ApplyModifiedProperties();
        GUILayout.Label("");
        m_Property = m_Object.FindProperty("UnknownFileIcon");
        EditorGUILayout.PropertyField(m_Property, new GUIContent("Unknown File Sprite :"), true);
        m_Object.ApplyModifiedProperties();
        m_Property = m_Object.FindProperty("DirectoryIcon");
        EditorGUILayout.PropertyField(m_Property, new GUIContent("Folder Sprite :"), true);
        m_Object.ApplyModifiedProperties();
        GUILayout.Label("");
        GUILayout.Label("File Browser Settings");
        GUILayout.Label("");
        m_Property = m_Object.FindProperty("currentDirectory");
        EditorGUILayout.PropertyField(m_Property, new GUIContent("Current Directory :"), true);
        m_Object.ApplyModifiedProperties();
        m_Property = m_Object.FindProperty("drawFilesOnly");
        EditorGUILayout.PropertyField(m_Property, new GUIContent("Draw Only Files :"), true);
        m_Object.ApplyModifiedProperties();
        m_Property = m_Object.FindProperty("selectedColor");
        EditorGUILayout.PropertyField(m_Property, new GUIContent("Selected Item Color :"), true);
        m_Object.ApplyModifiedProperties();
        m_Property = m_Object.FindProperty("multiSelect");
        EditorGUILayout.PropertyField(m_Property, new GUIContent("Allow Multi-Select :"), true);
        m_Object.ApplyModifiedProperties();
    }
}
#endif