using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoaderExtension : MonoBehaviour
{
    public SceneSwitchSO switchSO;
    public SceneSwitchChannel switchChannel;

    public void ChangeScene()
    {
        switchChannel.RaiseSwitchRequestEvent(switchSO);
    }
}
