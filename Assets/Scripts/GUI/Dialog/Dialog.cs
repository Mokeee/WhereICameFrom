using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class Dialog : MonoBehaviour, IDialog
{
    [Header("Animation")]
    /// <summary>
    /// Curve used to fade the canvas group.
    /// </summary>
    public AnimationCurve fadeCurve;
    /// <summary>
    /// Duration of the fading animation.
    /// </summary>
    public float animationDuration = 0.5f;

    [Header("Contents")]
    public DialogContent content;
    public Image flavorImage;
    public LocalizedText headerText;
    public LocalizedText subtitleText;
    public LocalizedText contentText;

    [Header("UI Events")]
    public UIAnimationStartEvent OnStartToShow = new UIAnimationStartEvent();
    public UnityEvent OnFadeInComplete = new UnityEvent();
    public UIAnimationStartEvent OnStartToHide = new UIAnimationStartEvent();
    public UnityEvent OnFadeOutComplete = new UnityEvent();

    [Header("States")]
    public bool visible;

    [HideInInspector]
    public SceneLocalizer sceneLocalizer;

    public bool Hide()
    {
        if (visible)
        {
            OnExit();
            StartCoroutine(FadeOut());
        }
        return visible;
    }

    public void OnEntry()
    {
        OnStartToShow.Invoke(GetComponent<RectTransform>(), animationDuration);
    }

    public void OnExit()
    {
        OnStartToHide.Invoke(GetComponent<RectTransform>(), animationDuration);
    }

    public void SetSizeAndPosition(Vector2 size, Vector2 position)
    {
        GetComponent<RectTransform>().sizeDelta = size;
        GetComponent<RectTransform>().localPosition = position;
    }

    public bool Show()
    {
        UpdateContent();
        if (!visible)
        {
            OnEntry();
            StartCoroutine(FadeIn());
        }

        return !visible;
    }

    public bool Show(DialogContent content)
    {
        SetContent(content);
        return Show();
    }

    public bool Show(string contentKeyword)
    {
        contentText.Keyword = contentKeyword;
        sceneLocalizer.TranslateText(contentText);
        if (!visible)
        {
            OnEntry();
            StartCoroutine(FadeIn());
        }

        return !visible;
    }

    // Start is called before the first frame update
    void Awake()
    {
        sceneLocalizer = (SceneLocalizer)FindObjectsOfType(typeof(SceneLocalizer))[0];
        visible = GetComponent<CanvasGroup>().blocksRaycasts;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetContent(DialogContent content)
    {
        this.content = content;
        UpdateContent();
    }

    private void UpdateContent()
    {
        if (subtitleText != null)
        {
            if (content.subtitle == "" || content.subtitle == null)
                subtitleText.gameObject.SetActive(false);
            else
            {
                subtitleText.gameObject.SetActive(true);
                subtitleText.SetNewKeyword(content.subtitle);
                sceneLocalizer.TranslateText(subtitleText);
            }
        }

        headerText.SetNewKeyword(content.header);
        contentText.SetNewKeyword(content.content);

        sceneLocalizer.TranslateText(headerText);
        sceneLocalizer.TranslateText(contentText);

        flavorImage.sprite = content.image;
    }

    private IEnumerator FadeIn()
    {
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

    public void ShowFromEvent()
    {
        Show();
    }

    public void HideFromEvent()
    {
        Hide();
    }
}

[Serializable]
public class UIAnimationStartEvent : UnityEvent<RectTransform, float>
{
}