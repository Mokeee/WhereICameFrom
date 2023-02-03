using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class SceneLoader : MonoBehaviour
{
    [Header("Load Event")]
    //The load event we are listening to
    public SceneSwitchChannel _loadEventChannel = default;
    [Header("Loading Screen")]
    [SerializeField] private LoadingScreen _loadingInterface = default;

    private List<string> loadedAdditiveScenes;
    private AsyncOperation loadingOperation;

    public SceneSwitchSO currentSceneSwitch;
    private InputAction endLoadingScreenAction;
    private float totalProgress;
    private bool sceneReady;

    private void Start()
    {
        loadedAdditiveScenes = new List<string>();
        StartLoadScene(currentSceneSwitch);

        //Wait to you press the space key to activate the Scene
        endLoadingScreenAction = new InputAction(binding: "<Keyboard>/space");
        //Activate the Scene
        endLoadingScreenAction.performed += (InputAction.CallbackContext context) =>
        {
            sceneReady = true;
            endLoadingScreenAction.Disable();
        };

        _loadingInterface.OnFadeInComplete.AddListener(() => { loadingOperation.allowSceneActivation = true; });
    }

    private void OnEnable()
    {
        _loadEventChannel.OnSwitchRequested += StartLoadScene;
        _loadEventChannel.OnSwitchProgressUpdated += UpdateProgress;
    }
    private void OnDisable()
    {
        _loadEventChannel.OnSwitchRequested -= StartLoadScene;
        _loadEventChannel.OnSwitchProgressUpdated -= UpdateProgress;
    }

    private void StartLoadScene(SceneSwitchSO sceneSwitch)
    {
        currentSceneSwitch = sceneSwitch;

        UnloadAdditiveScenes();
        LoadScene();
    }

    private void LoadScene()
    {
        loadingOperation = SceneManager.LoadSceneAsync(currentSceneSwitch.newSceneName, currentSceneSwitch.loadMode);
        loadingOperation.completed += ActivateScene;

        if (currentSceneSwitch.showLoadingScreen)
            StartCoroutine(TrackProgress());
    }

    private void UnloadAdditiveScenes()
    {
        List<string> scenesToRemove = new List<string>();
        foreach (var sceneName in loadedAdditiveScenes)
        {
            if (sceneName != currentSceneSwitch.sceneDependency)
            {
                loadingOperation = SceneManager.UnloadSceneAsync(sceneName);
                scenesToRemove.Add(sceneName);
            }
        }

        foreach (var sceneName in scenesToRemove)
            loadedAdditiveScenes.Remove(sceneName);
    }

    private void ActivateScene(AsyncOperation action)
    {
        action.completed -= ActivateScene;

        if (currentSceneSwitch.loadMode == LoadSceneMode.Additive)
            loadedAdditiveScenes.Add(currentSceneSwitch.newSceneName);

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(currentSceneSwitch.newSceneName));
    }

    private IEnumerator TrackProgress()
    {
        sceneReady = false;

        if (!_loadingInterface.visible)
            loadingOperation.allowSceneActivation = false;

        if (!currentSceneSwitch.disableLoadingSceneByInput)
            _loadEventChannel.OnSwitchFinished += DisableLoadingScreen;

        _loadingInterface.Show();

        totalProgress = 0;

        //When the scene reaches 0.9f, it means that it is loaded
        //The remaining 0.1f are for the integration
        while (totalProgress < 1.0f)
        {
            _loadingInterface.UpdateProgress(totalProgress, false);

            yield return null;
        }

        _loadingInterface.UpdateProgress(totalProgress, currentSceneSwitch.disableLoadingSceneByInput);

        if (currentSceneSwitch.disableLoadingSceneByInput)
            endLoadingScreenAction.Enable();

        while (!sceneReady)
        {
            yield return null;
        }

        //Hide progress bar when loading is done
        _loadingInterface.Hide();
    }

    private void DisableLoadingScreen()
    {
        _loadEventChannel.OnSwitchFinished -= DisableLoadingScreen;
        sceneReady = true;
        endLoadingScreenAction.Disable();
    }

    private void UpdateProgress(float progress)
    {
        totalProgress = progress;
    }
}
