using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "SceneSwitchChannel", menuName = "ScriptableObjects/Channels/SceneSwitchChannel", order = 1)]
public class SceneSwitchChannel : ScriptableObject
{
    public UnityAction<SceneSwitchSO> OnSwitchRequested;

    public UnityAction<float> OnSwitchProgressUpdated;

    public UnityAction OnSwitchFinished;

    public void RaiseSwitchRequestEvent(SceneSwitchSO sceneToLoad)
    {
        if (OnSwitchRequested != null)
        {
            OnSwitchRequested.Invoke(sceneToLoad);
        }
        else
        {
            Debug.LogWarning("A Scene loading was requested, but nobody picked it up." +
                "Check why there is no SceneLoader already present, " +
                "and make sure it's listening on this Load Event channel.");
        }
    }

    public void RaiseSwitchFinishedEvent()
    {
        if (OnSwitchFinished != null)
        {
            OnSwitchFinished.Invoke();
        }
        else
        {
            Debug.LogWarning("A Scene was loaded, but nobody picked it up." +
                "Check why there is no SceneLoader already present, " +
                "and make sure it's listening on this Load Event channel.");
        }
    }
    public void RaiseSwitchProgessUpdateEvent(float progress)
    {
        if (OnSwitchProgressUpdated != null)
        {
            OnSwitchProgressUpdated.Invoke(progress);
        }
        else
        {
            Debug.LogWarning("A Scene loading progress was updated, but nobody picked it up." +
                "Check why there is no SceneLoader already present, " +
                "and make sure it's listening on this Load Event channel.");
        }
    }
}
