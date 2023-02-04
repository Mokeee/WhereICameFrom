using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using System.Text.RegularExpressions;

public class TextTyper : MonoBehaviour
{
    public float textSpeed = 0.2f;
    public TextMeshProUGUI textRenderer;

    public float deltaTime;
    public bool showText;
    public bool interactable = true;

    private string dialogueStartEvent;
    private string dialogueStopEvent;

    private int textIndex;
    public List<string> snippets;
    private bool enterLocked;

    [Header("Audio Events")]
    public SoundEvent OnStartText = new SoundEvent();
    public SoundEvent OnTextFinished = new SoundEvent();
    public SoundEvent OnSnippetFinished = new SoundEvent();


    public void ShowText(string text)
    {
        textIndex = 0;
        snippets = new List<string>();
        Regex r = new Regex(@"([a-zA-Z])*(['-])*\w+");
        var matches = r.Matches(text);

        int wordsPerSplit = 12;
        int splits = Mathf.FloorToInt(matches.Count / (wordsPerSplit * 1.0f));

        for (int i = 0; i < splits; i++)
        {
            int endIndex = (i == splits - 1) ? text.Length - 1 : matches[(i + 1) * wordsPerSplit].Index;
            int length =  endIndex - matches[i * wordsPerSplit].Index;
            snippets.Add(text.Substring(matches[i * wordsPerSplit].Index, length));
        }

        if (splits == 0)
            snippets.Add(text);

        ShowText();
    }

    public void ShowText()
    {
        deltaTime = 0;
        showText = true;
        OnStartText.Invoke(dialogueStartEvent);
    }

    public void ToggleDialogPause(bool isPaused)
    {
        showText = !isPaused;
        StopAllCoroutines();
        enterLocked = isPaused;
    }

    // Update is called once per frame
    void Update()
    {
        //if (showText)
        //{
        //    deltaTime += Time.deltaTime * textSpeed;
        //    int charCount = Mathf.RoundToInt(deltaTime);

        //    charCount = Mathf.Clamp(charCount, 0, text.Length);

        //    textRenderer.text = text.Substring(0, charCount);

        //    if (charCount == text.Length)
        //    {
        //        showText = false;
        //        deltaTime = 0.0f;
        //        OnTextFinished.Invoke();
        //    }
        //}

        if (showText && textIndex < snippets.Count)
        {
            deltaTime += Time.deltaTime * textSpeed;
            int charCount = Mathf.RoundToInt(deltaTime);

            charCount = Mathf.Clamp(charCount, 0, snippets[textIndex].Length);

            textRenderer.text = snippets[textIndex].Substring(0, charCount);

            if (charCount >= snippets[textIndex].Length)
            {
                showText = false;

                OnSnippetFinished.Invoke(dialogueStopEvent);
                if (textIndex != snippets.Count - 1)
                    textRenderer.text = textRenderer.text + " [...]";
                else
                    OnTextFinished.Invoke(dialogueStopEvent);

            }
        }

        if (interactable && (Input.GetKey(KeyCode.Return) || Input.GetMouseButtonDown(0)) && !enterLocked)
        {
            enterLocked = true;
            EnterKeyFunction();
            StartCoroutine(LockEnter());
        }
    }
    public void EnterKeyFunction()
    {
        if (textIndex < snippets.Count)
        {
            if (Mathf.RoundToInt(deltaTime) < snippets[textIndex].Length)
                deltaTime = snippets[textIndex].Length;
            else
                Proceed();
        }
    }
    private void Proceed()
    {
        showText = true;
        deltaTime = 0.0f;
        textIndex++;
        if (textIndex < snippets.Count)
            OnStartText.Invoke(dialogueStartEvent);
    }

    private IEnumerator LockEnter()
    {
        yield return new WaitForSeconds(0.3f);
        enterLocked = false;
    }
}

[System.Serializable]
public class SoundEvent : UnityEvent<string>
{ }