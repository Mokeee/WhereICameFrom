using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SceneLoadRegister : MonoBehaviour
{
    public SceneSwitchChannel channel;

    public List<IInitiable> initiables;

    private float totalProgress;
    private int completedInitiables;

#if UNITY_EDITOR
    [MenuItem("GameObject/Setup/SceneLoadRegister")]
    static void CreateLoadRegister(MenuCommand menuCommand)
    {
        var go = SpawnRegister(menuCommand.context as GameObject);
        // Register the creation in the undo system
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
        Selection.activeObject = go;
    }

    public static GameObject SpawnRegister(GameObject parent)
    {
        GameObject go = new GameObject("SceneLoadRegister", typeof(SceneLoadRegister));

        // Ensure it gets reparented if this was a context click (otherwise does nothing)
        GameObjectUtility.SetParentAndAlign(go, parent);

        return go;
    }
#endif

    public void RegisterInitiable(IInitiable initiable)
    {
        if (!initiables.Contains(initiable))
            initiables.Add(initiable);
    }

    private void Awake()
    {
        ResetRegister();
        StartCoroutine(LoadSceneLogic());
    }

    public IEnumerator LoadSceneLogic()
    {
        //Wait for registration
        yield return null;

        if (initiables.Count == 0)
        {
            channel.RaiseSwitchProgessUpdateEvent(1.0f);
            channel.RaiseSwitchFinishedEvent();
            ResetRegister();
        }
        else
        {
            foreach (var initiable in initiables)
            {
                initiable.OnCompleted += CompleteInitiable;
                initiable.OnUpdateProgress += UpdateProgess;
                yield return StartCoroutine(initiable.InitializeLogic());
            }
        }
    }

    private void UpdateProgess(float progress)
    {
        totalProgress += progress / initiables.Count;
        channel.RaiseSwitchProgessUpdateEvent(totalProgress);
    }

    private void CompleteInitiable(IInitiable initiable)
    {
        completedInitiables++;

        if (completedInitiables == initiables.Count)
        {
            channel.RaiseSwitchFinishedEvent();
            ResetRegister();
        }
    }

    private void ResetRegister()
    {
        initiables = new List<IInitiable>();
        completedInitiables = 0;
        totalProgress = 0;
    }
}
