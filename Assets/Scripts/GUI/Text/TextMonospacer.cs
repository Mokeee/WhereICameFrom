using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextMonospacer : MonoBehaviour
{
    public TMP_InputField label;
    public string emSpacing = "1.2";

    public void MonospaceText(string text)
    {
        if (text.Length == 1)
        {
            string formatted = string.Format("<mspace={0}em>{1}", emSpacing, text);
            label.text = formatted;
            label.caretPosition = formatted.Length;
        }
    }
}
