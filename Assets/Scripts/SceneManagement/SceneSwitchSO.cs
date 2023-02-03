using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "SceneSwitch", menuName = "ScriptableObjects/SceneSwitch", order = 1)]
public class SceneSwitchSO : ScriptableObject
{
    /// <summary>
    /// Scene to load.
    /// </summary>
    public string newSceneName;
    /// <summary>
    /// Scene that must not be unloaded.
    /// </summary>
    public string sceneDependency;
    public LoadSceneMode loadMode;
    public bool showLoadingScreen;
    public bool disableLoadingSceneByInput;
}
