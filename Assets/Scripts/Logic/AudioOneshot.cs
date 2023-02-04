using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioOneshot : MonoBehaviour
{
    public EventReference soundReference;

    public void PlayAudio()
    {
        RuntimeManager.PlayOneShot(soundReference);
    }
}
