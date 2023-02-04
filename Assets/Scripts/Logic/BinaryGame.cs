using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BinaryGame : MonoBehaviour
{
    //public string formula;
    public List<Toggle> inputToggles;
    public List<int> inputValues;
    public TextMeshProUGUI outputLabel;

    public bool mapToAlphabet;

    private string[] chars = new string[] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };
    
    public void UseFormula()
    {
        int[] values = new int[inputValues.Count];
        foreach (var toggle in inputToggles)
            values[inputToggles.IndexOf(toggle)] = (toggle.isOn) ? inputValues[inputToggles.IndexOf(toggle)] : 0;

        int value = ((values[0] + values[1]) * values[2] + values[2] - values[1] + values[3] + values[3] - (values[0] + values[1]));
        if (!mapToAlphabet)
            outputLabel.text = value.ToString();
        else
        {
            value = (value - 1 + chars.Length) % chars.Length;
            outputLabel.text = chars[value];
        }
    }
}
