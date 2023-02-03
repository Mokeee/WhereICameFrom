using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : Dialog
{
    public Slider progressBar;
    public List<string> tips;
    public List<Sprite> backgrounds;
    public float cycleSpeed = 10.0f;
    private void Start()
    {
        if (GetComponent<CanvasGroup>().alpha == 1.0f)
            StartCoroutine(CycleContent());

        sceneLocalizer = (SceneLocalizer)FindObjectsOfType(typeof(SceneLocalizer))[0];

        OnFadeInComplete.AddListener(() => { StartCoroutine(CycleContent()); });
        OnFadeOutComplete.AddListener(() => { StopCoroutine(CycleContent()); });
    }

    public void UpdateProgress(float progress, bool waitingForInput)
    {
        subtitleText.gameObject.SetActive(true);
        progressBar.value = progress;
        if (waitingForInput)
        {
            subtitleText.SetNewKeyword("loadingscreen_finished");
            sceneLocalizer.TranslateText(subtitleText);
        }
        else
            subtitleText.UpdateLocalization(Mathf.Floor(progress * 100) + "%");
    }

    private IEnumerator CycleContent()
    {
        int index = Random.Range(0, tips.Count);
        while (true)
        {
            //Show next tip
            contentText.SetNewKeyword(tips[index]);
            content.image = backgrounds[index % backgrounds.Count];
            flavorImage.sprite = backgrounds[index % backgrounds.Count];
            sceneLocalizer.TranslateText(contentText);

            index += Random.Range(1, tips.Count);
            index %= tips.Count;

            yield return new WaitForSeconds(cycleSpeed);
        }
    }
}
