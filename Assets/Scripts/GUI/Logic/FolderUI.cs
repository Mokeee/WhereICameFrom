using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

public class FolderUI : MonoBehaviour
{
    public bool unlocked;
    public Sprite lockSprite;
    public Sprite unlockSprite;

    public Image spriteHolder;

    public string keyword;

    [SerializeField]
    public ShowPromptEvent OnShowPrompt = new ShowPromptEvent();
    public UnityEvent OnShowContent = new UnityEvent();

    private void Start()
    {
        UpdateSprite();
    }

    public void OpenFolder()
    {
        if (!unlocked)
            OnShowPrompt.Invoke(this);
        else
            OnShowContent.Invoke();
    }

    public bool TryKeyword(string input)
    {
        unlocked = string.Equals(input, keyword);

        if (unlocked)
        {
            OnShowContent.Invoke();
            UpdateSprite();
        }

        return unlocked;
    }

    private void UpdateSprite()
    {
        spriteHolder.sprite = (unlocked) ? unlockSprite : lockSprite;
    }
}

[Serializable]
public class ShowPromptEvent : UnityEvent<FolderUI> { }