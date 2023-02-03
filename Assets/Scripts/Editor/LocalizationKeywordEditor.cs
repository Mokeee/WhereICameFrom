using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LocalizationKeywordEditor : EditorWindow
{
    public string baseFolder = "Locale";
    string keyword = "";
    string keywordLastUpdate = "";

    bool keywordFound;
    List<TextAsset> locales;
    Dictionary<string, LocaleReader> readers;

    // Add menu named "My Window" to the Window menu
    [MenuItem("Localization/LocalizationKeywordEditor")]
    static void InitLocalization()
    {
        Init("Localization", "");
    }

    // Add menu named "My Window" to the Window menu
    [MenuItem("Balancing/BalancingKeywordEditor")]
    static void InitBalancing()
    {
        Init("Balancing", "");
    }

    static void Init(string baseFolder, string keyword)
    {
        // Get existing open window or if none, make a new one:
        LocalizationKeywordEditor window = (LocalizationKeywordEditor)EditorWindow.GetWindow(typeof(LocalizationKeywordEditor));
        window.keyword = keyword;
        window.baseFolder=baseFolder;

        InitWindow(window);

        window.ShowModal();
    }

    static void InitWindow(LocalizationKeywordEditor window)
    {
        window.readers = LocaleReader.GetReaders(window.baseFolder, out window.locales);
    }

    public void InitEditor(string keyword)
    {
        LocalizationKeywordEditor.Init(baseFolder, keyword);
    }

    void OnGUI()
    {
        GUILayout.Label("Adding Keyword", EditorStyles.boldLabel);
        keyword = EditorGUILayout.TextField("Keyword", keyword);

        if (keyword != "" && keyword != null)
        {
            if (keyword != keywordLastUpdate)
            {
                keywordFound = LocaleReader.FindKeyword(keyword, readers);
                keywordLastUpdate = keyword;
            }

            if (!keywordFound && GUILayout.Button("Add Keyword"))
            {
                LocaleReader.AddKeyword(keyword, readers);
                keywordLastUpdate = "";
            }
            if (keywordFound && GUILayout.Button("Remove Keyword"))
            {
                LocaleReader.RemoveKeyword(keyword, readers);
                keywordLastUpdate = "";
            }

            if (!keywordFound)
                EditorGUILayout.LabelField("Keyword not found!");
            else
            {
                foreach(string language in readers.Keys)
                {
                    string translation = EditorGUILayout.TextField(language, readers[language].FindTranslation(keyword));
                    readers[language].SetTranslation(keyword, translation);
                }
            }
            if (GUILayout.Button("Save"))
                LocaleReader.SortAndSaveEdits(locales, readers);
        }

        if (GUILayout.Button("Save & Close"))
        {
            LocaleReader.SortAndSaveEdits(locales, readers);
            Close();
        }
    }
}