using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(LocalizationManager))]
public class LocalizationManagerEditor : Editor
{
    bool showLanguageSettings = true;
    List<TextAsset> assets;
    SystemLanguage language;
    public override void OnInspectorGUI()
    {
        LocalizationManager lM = (LocalizationManager)target;

        var property = serializedObject.FindProperty("channel");
        EditorGUILayout.PropertyField(property, true);

        language = lM.CurrentLanguage;
        language = (SystemLanguage)EditorGUILayout.EnumPopup("Current Language", lM.CurrentLanguage);
        lM.SetLanguage(language);

        if (GUILayout.Button("Change Language"))
        {
            lM.ChangeLanguage(language);
        }

        ShowLanguageSettings(lM);

        if (GUI.changed)
        {
            EditorUtility.SetDirty(lM);
            serializedObject.ApplyModifiedProperties();
        }
    }

    private void ShowLanguageSettings(LocalizationManager lM)
    {
        showLanguageSettings = EditorGUILayout.Foldout(showLanguageSettings, "Language Mapping");

        if (showLanguageSettings)
        {
            Dictionary<SystemLanguage, TextAsset> tempAssets = new Dictionary<SystemLanguage, TextAsset>();
            if (lM.localeAssets == null || lM.localeAssets.Count == 0)
            {
                tempAssets = new Dictionary<SystemLanguage, TextAsset>();
                for (int i = 0; i < (int)SystemLanguage.Unknown; i++)
                {
                    tempAssets.Add((SystemLanguage)i, new TextAsset());
                }
            }
            else
                tempAssets = lM.localeAssets;

            lM.localeAssets = new Dictionary<SystemLanguage, TextAsset>();

            if (assets == null)
            {
                assets = new List<TextAsset>();
            }
            else
            {
                foreach (var asset in assets)
                {
                    tempAssets[(SystemLanguage)assets.IndexOf(asset)] = asset;
                }

                assets.Clear();
            }
            foreach (var key in tempAssets.Keys)
            {
                Rect r = (Rect)EditorGUILayout.BeginHorizontal();
                GUI.Box(r, GUIContent.none);
                EditorGUILayout.LabelField(key.ToString(), GUILayout.MaxWidth(100));
                var textAsset = (TextAsset)EditorGUILayout.ObjectField(tempAssets[key], typeof(TextAsset), false);

                assets.Add(textAsset);

                EditorGUILayout.EndHorizontal();
            }
            foreach (var key in tempAssets.Keys)
            {
                lM.localeAssets.Add(key, assets[(int)key]);
            }
        }
    }
}
