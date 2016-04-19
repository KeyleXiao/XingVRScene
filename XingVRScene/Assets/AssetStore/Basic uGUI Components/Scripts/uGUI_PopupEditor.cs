#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.UI;


[CustomEditor (typeof(uGUI_Popup))]
public class uGUI_PopupEditor : Editor {

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
        GUILayout.Label("uGUI Popup");
        if (systemSettings == true)
        {
            GUILayout.Label("");
            m_Property = m_Object.FindProperty("popupPanel");
            EditorGUILayout.PropertyField(m_Property, new GUIContent("Popup Panel GO :"), true);
            m_Object.ApplyModifiedProperties();
            m_Property = m_Object.FindProperty("inputfieldUI");
            EditorGUILayout.PropertyField(m_Property, new GUIContent("InputField GO :"), true);
            m_Object.ApplyModifiedProperties();
            m_Property = m_Object.FindProperty("optionButtonTemplate");
            EditorGUILayout.PropertyField(m_Property, new GUIContent("Option Button Tempate GO :"), true);
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
        m_Property = m_Object.FindProperty("allItems");
        EditorGUILayout.PropertyField(m_Property, new GUIContent("All Popup Items :"), true);
        m_Object.ApplyModifiedProperties();
        GUILayout.Label("");
        m_Property = m_Object.FindProperty("currentItem");
        EditorGUILayout.PropertyField(m_Property, new GUIContent("Current Selected Item :"), true);
        m_Object.ApplyModifiedProperties();
        
    }

    
}

/*
        m_Property = m_Object.FindProperty("");
        EditorGUILayout.PropertyField(m_Property, new GUIContent(""), true);
        m_Object.ApplyModifiedProperties();
 */
#endif