using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LocalizedText))]
public class TextFormatter : MonoBehaviour
{
    private TMPro.TMP_Text text;
    private string templateText;
    private void Start()
    {
    }

    private void SetupFormatter()
    {
        text = GetComponent<LocalizedText>().Text;
        templateText = text.text;
    }

    public void FormatInt(int number)
    {
        if (text == null)
            SetupFormatter();

        text.text = string.Format(templateText, number);
    }
}
