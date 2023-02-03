using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(LocalizedText))]

public class LocalizableTextEditor : Editor
{
    public override void OnInspectorGUI()
    {
        LocalizedText lT = (LocalizedText)target;

        lT.textType = (TextType)EditorGUILayout.EnumPopup("Type of text", lT.textType);

        if (GUILayout.Button("Edit Keyword"))
        {
            LocalizationKeywordEditor editorWindow = (LocalizationKeywordEditor)EditorWindow.GetWindow(typeof(LocalizationKeywordEditor), true, "Edit Keyword");
            editorWindow.baseFolder="Localization";
            editorWindow.Close();//If the window was alerady open, we should have something for clearing old data
            editorWindow.InitEditor(lT.GetKeyword());
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(lT);
            serializedObject.ApplyModifiedProperties();
        }
    }
}
