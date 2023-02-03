using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ThemeManager))]
public class ThemeManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        ThemeManager lT = (ThemeManager)target;

        var property = serializedObject.FindProperty("theme");
        EditorGUILayout.PropertyField(property, true);

        if (GUILayout.Button("Update Theme"))
        {
            lT.UpdateTheme();
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(lT);
            serializedObject.ApplyModifiedProperties();
        }
    }
}