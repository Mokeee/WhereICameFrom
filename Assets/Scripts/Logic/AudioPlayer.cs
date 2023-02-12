using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using FMOD.Studio;
using FMODUnity;
using TMPro;

public class AudioPlayer : MonoBehaviour
{
    [SerializeField]
    public FloatEvent OnPlayback = new FloatEvent();
    public float volume;
    public EventReference soundReference;
    public FMOD.Studio.STOP_MODE stopMode = FMOD.Studio.STOP_MODE.IMMEDIATE;
    public bool isLooping;
    public TextMeshProUGUI label;
    private EventInstance instance;

    private EventDescription eventDescription;
    private int clipLength;
    int tpos = 0;
    bool isStopped = true;

    public string playLabel;
    public string pauseLabel;

    public void PlayAudio()
    {
        instance = RuntimeManager.CreateInstance(soundReference);
        instance.getDescription(out eventDescription);
        eventDescription.getLength(out clipLength);
        instance.start();

        if(label != null)
            label.text = pauseLabel;
        isStopped = false;
    }

    public void SetPlayBack(float percentage)
    {
        instance.setTimelinePosition((int)(clipLength * percentage));
    }

    public void SetIntensity(float intensity)
    {
        instance.setParameterByName("Intensity", intensity);
    }

    public void ToggleAudio()
    {
        if(isStopped)
        {
            PlayAudio();

            return;
        }
        bool paused = false;
        instance.getPaused(out paused);

        if (label != null)
        {
            if (paused)
                label.text = pauseLabel;
            else
                label.text = playLabel;
        }

        instance.setPaused(!paused);
    }

    public void StopAudio()
    {
        instance.stop(stopMode);
        instance.release();
        if (label != null)
            label.text = playLabel;
        isStopped = true;
    }

    private void Update()
    {
        if (clipLength == 0)
            return;

        instance.getTimelinePosition(out tpos);
        OnPlayback.Invoke(tpos / (clipLength * 1.0f));

        if (tpos / (clipLength * 1.0f) >= 1.0f && !isLooping)
            StopAudio();
    }
}

[System.Serializable]
public class FloatEvent : UnityEvent<float> { }