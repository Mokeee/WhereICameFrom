using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;

[InitializeOnLoad]
public class GloriaDefaultScene : Editor
{
    static GloriaDefaultScene()
    {
        EditorSceneManager.newSceneCreated += AddCustomDefaultObjects;
    }

    private static void AddCustomDefaultObjects(Scene scene, NewSceneSetup setup, NewSceneMode mode)
    {
        string folder = "Assets/ScriptableObjects/";
        //Load channels
        SceneSwitchChannel sceneChannel = AssetDatabase.LoadAssetAtPath(folder + "SceneManagement/SceneSwitchChannel.asset", typeof(SceneSwitchChannel)) as SceneSwitchChannel;
        var go = SceneLoadRegister.SpawnRegister(null);
        go.GetComponent<SceneLoadRegister>().channel = sceneChannel;


        LocalizationChannel localeChannel = AssetDatabase.LoadAssetAtPath(folder + "Localization/LocalizationChannel.asset", typeof(LocalizationChannel)) as LocalizationChannel;
        go = SceneLocalizer.SpawnLocalizer(null);
        go.GetComponent<SceneLocalizer>().channel = localeChannel;

        Theme theme = AssetDatabase.LoadAssetAtPath(folder + "Themes/Theme.asset", typeof(Theme)) as Theme;
        go = ThemeManager.SpawnThemeManager(null);
        go.GetComponent<ThemeManager>().theme = theme;
    }
}
