using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class Prompt : MonoBehaviour
{
    public UnityEvent OnShowPrompt = new UnityEvent();
    public UnityEvent OnClosePrompt = new UnityEvent();
    public UnityEvent OnError = new UnityEvent();

    public TextMeshProUGUI placeholderText;
    public TMP_InputField inputText;
    private FolderUI folder;
    private string input;

    private void Start()
    {
        inputText = GetComponent<TMP_InputField>();
    }

    public void ReceiveInput(string input)
    {
        this.input = input;
    }

    public void ShowPrompt(FolderUI requestingFolder)
    {
        folder = requestingFolder;
        placeholderText.text = string.Format("Enter Word with {0} Characters...", folder.keyword.Length);
        OnShowPrompt.Invoke();
    }

    public void SendInput()
    {
        if (folder.TryKeyword(input))
            OnClosePrompt.Invoke();
        else
            OnError.Invoke();
    }
}
