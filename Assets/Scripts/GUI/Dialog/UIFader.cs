using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CanvasGroup))]
public class UIFader : MonoBehaviour
{
    [Header("Animation")]
    /// <summary>
    /// Curve used to fade the canvas group.
    /// </summary>
    public AnimationCurve fadeCurve = AnimationCurve.EaseInOut(0,0,1,1);
    /// <summary>
    /// Duration of the fading animation.
    /// </summary>
    public float animationDuration = 0.5f;

    [Header("UI Events")]
    public UIAnimationStartEvent OnStartToShow = new UIAnimationStartEvent();
    public UnityEvent OnFadeInComplete = new UnityEvent();
    public UIAnimationStartEvent OnStartToHide = new UIAnimationStartEvent();
    public UnityEvent OnFadeOutComplete = new UnityEvent();

    [Header("States")]
    public bool visible;


    private IEnumerator FadeIn()
    {
        transform.SetAsLastSibling();
        float deltaTime = 0;
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();

        while (deltaTime < animationDuration)
        {
            canvasGroup.alpha = fadeCurve.Evaluate(deltaTime / animationDuration);
            deltaTime += Time.fixedDeltaTime;
            yield return null;
        }

        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
        visible = true;

        OnFadeInComplete.Invoke();
    }

    private IEnumerator FadeOut()
    {
        float deltaTime = 0;
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.blocksRaycasts = false;

        while (deltaTime < animationDuration)
        {
            canvasGroup.alpha = fadeCurve.Evaluate(1 - deltaTime / animationDuration);
            deltaTime += Time.fixedDeltaTime;
            yield return null;
        }

        canvasGroup.alpha = 0;
        visible = false;

        OnFadeOutComplete.Invoke();
    }

    public bool Show()
    {
        if (!visible)
        {
            OnEntry();
            StartCoroutine(FadeIn());
        }

        return !visible;
    }

    public void ShowFromEvent()
    {
        Show();
    }

    public bool Hide()
    {
        if (visible)
        {
            OnExit();
            StartCoroutine(FadeOut());
        }
        return visible;
    }

    public void HideFromEvent()
    {
        Hide();
    }

    public void OnEntry()
    {
        OnStartToShow.Invoke(GetComponent<RectTransform>(), animationDuration);
    }

    public void OnExit()
    {
        OnStartToHide.Invoke(GetComponent<RectTransform>(), animationDuration);
    }
}
