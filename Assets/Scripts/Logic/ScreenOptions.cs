using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using UnityEngine.Events;

public class ScreenOptions : MonoBehaviour
{
    public UnityEvent OnHideMenuTimer = new UnityEvent();
    public FloatEvent OnVolumeChanged = new FloatEvent();
    public float menuTimer = 3.0f;
    public float volume = 1.0f;
    public float volumeStepSize = 0.1f;

    Bus masterBus;

	void Start()
	{
		masterBus = FMODUnity.RuntimeManager.GetBus("bus:/Master");
	}

	public void TurnOff()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void TurnVolumeUp()
    {
        SetVolume(1f);
    }

    public void TurnVolumeDown()
    {
        SetVolume(-1f);
    }

    private void SetVolume(float stepSign)
    {
        StopAllCoroutines();
        volume = Mathf.Clamp01(volume + volumeStepSize * stepSign);
        masterBus.setVolume(volume);
        OnVolumeChanged.Invoke(volume);
        StartCoroutine(HoldMenu());
    }

    private IEnumerator HoldMenu()
    {
        yield return new WaitForSeconds(menuTimer);
        OnHideMenuTimer.Invoke();
    }
}
