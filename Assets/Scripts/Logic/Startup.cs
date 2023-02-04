using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Startup : MonoBehaviour
{
    public float textDelay;
    public float animationDelay;

    public CanvasGroup backgroundImage;
    public Image reflectionOverlay;
    public GameObject screenParent;

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
        StartCoroutine(StartUpSequenceAnimation());
    }

    private IEnumerator StartUpSequenceIntro()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        yield return new WaitForSeconds(textDelay);
        //Show Intro Text

        //Wait For End of Text
    }

    private IEnumerator StartUpSequenceAnimation()
    {
        yield return new WaitForSeconds(animationDelay);
        //Show OS Loadup
        //Animate LogIn
        //Toggle backgrounds
        float deltaTime = 0;

        reflectionOverlay.color = new Color(1, 1, 1, 0.15f);

        while (deltaTime < animationDuration)
        {
            backgroundImage.alpha = fadeCurve.Evaluate(deltaTime / animationDuration);
            reflectionOverlay.color = new Color(1, 1, 1, Mathf.Lerp(0.15f, 0.8f, deltaTime / animationDuration));
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
