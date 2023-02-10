using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(UITransformAnimator))]
[RequireComponent(typeof(UIFader))]
[RequireComponent(typeof(TextMeshProUGUI))]
public class AnimatedTimer : MonoBehaviour
{
    public int maxSteps;
    public float stepSize;
    public bool countDown;

    private UITransformAnimator transformAnimator;
    private UIFader fader;
    private TextMeshProUGUI label;

    public void StartTimer()
    {
        fader = GetComponent<UIFader>();
        transformAnimator = GetComponent<UITransformAnimator>();
        label = GetComponent<TextMeshProUGUI>();
        fader.visible = true;
        fader.animationDuration = stepSize;
        transformAnimator.animationDuration = stepSize;

        StopAllCoroutines();
        StartCoroutine(AnimateTimer());
    }

    private IEnumerator AnimateTimer()
    {
        float stepTimeDelta = 0;
        int step = 0;
        while(step < maxSteps)
        {
            stepTimeDelta = stepTimeDelta % stepSize;
            fader.visible = true;
            fader.HideFromEvent();
            GetComponent<RectTransform>().localScale = Vector3.one;
            transformAnimator.StartAnimationForward();

            label.text = (countDown) ? (maxSteps - step).ToString() : (step + 1).ToString();
            stepTimeDelta += Time.deltaTime;
            yield return new WaitForSeconds(stepSize);
            step++;
        }
    }
}
