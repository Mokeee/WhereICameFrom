using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Startup : MonoBehaviour
{
    public float textDelay;

    public bool skipStartUp;

    public CanvasGroup backgroundImage;
    public Image reflectionOverlay;
    public Image borderImage;
    public GameObject screenParent;

    public UnityEvent OnStartIntro = new UnityEvent();
    public UnityEvent OnStartBoot = new UnityEvent();
    public UnityEvent OnShowLogIn = new UnityEvent();
    public UnityEvent OnStartUpLogIn = new UnityEvent();

    [Header("Animation")]
    /// <summary>
    /// Curve used to fade the canvas group.
    /// </summary>
    public AnimationCurve fadeCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    /// <summary>
    /// Duration of the fading animation.
    /// </summary>
    public float animationDuration = 0.5f;


    // Start is called before the first frame update
    void Start()
    {
        screenParent.SetActive(false);

        if (!skipStartUp)
            StartCoroutine(StartUpSequenceIntro());
        else
            StartCoroutine(StartUpSequenceLoggedInAnimation());
    }

    private IEnumerator StartUpSequenceIntro()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        yield return new WaitForSeconds(textDelay);
        //Show Intro Text

        //Wait For End of Text
        OnStartIntro.Invoke();
    }

    public void StartOSBoot()
    {
        StartCoroutine(DelayedStartOSBoot());
    }

    private IEnumerator DelayedStartOSBoot()
    {
        yield return new WaitForSeconds(1.5f);
        OnStartBoot.Invoke();
    }

    public void ShowLogIn()
    {
         OnShowLogIn.Invoke();
    }

    public void StartLogIn()
    {
        StartCoroutine(StartUpSequenceLoggedInAnimation());
    }

    private IEnumerator StartUpSequenceLoggedInAnimation()
    {
        //Toggle backgrounds
        float deltaTime = 0;

        reflectionOverlay.color = new Color(1, 1, 1, 0.15f);
        Color targetBorderColor = new Color(128/255f, 164/255f, 196/255f);
        while (deltaTime < animationDuration)
        {
            backgroundImage.alpha = fadeCurve.Evaluate(deltaTime / animationDuration);
            reflectionOverlay.color = new Color(1, 1, 1, Mathf.Lerp(0.15f, 0.8f, deltaTime / animationDuration));
            borderImage.color = Color.Lerp(Color.white, targetBorderColor, deltaTime / animationDuration);
            deltaTime += Time.fixedDeltaTime;
            yield return null;
        }

        backgroundImage.alpha = 1;
        reflectionOverlay.color = new Color(1, 1, 1, 0.8f);
        screenParent.SetActive(true);

        //Give Player Control
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }
}
