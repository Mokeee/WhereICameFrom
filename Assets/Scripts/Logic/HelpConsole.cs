using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Text.RegularExpressions;

public class HelpConsole : MonoBehaviour
{
    public TextMeshProUGUI logLabel;

    public Dictionary<string, string> hints = new Dictionary<string, string>()
    {
        { "datura", "Look for irregularities in written last names, try to layer images, the 4 binary minigame inputs are reflected in another document, and some characters come not in the form of letters. You are searching for 8 letters." },
        { "senecio", "Matching ADA's jingle with the sound minigame reveals more folder contents. The titles of the appearing images can be input into the cross-word puzzle of the newspaper." },
        { "aletris", "The blueprint can be used as a textmask, revealing important words. The order of the blueprints is found in the sentences of the assembly robot within the dialog of ADA and Albert." }
    };

    public Dictionary<string, string> solutions = new Dictionary<string, string>()
    {
        { "datura", "magnaspes" },
        { "senecio", "damocles" },
        { "aletris", "eden" },
        { "note", "press tab + c in the encrypted note" }
    };

    private bool visible;
    private bool toggleLocked;

    string error = "<br><color=\"red\">Command{0} could not be parsed{1}</color>";

    public void Unlock()
    {
        toggleLocked = false;
    }

    public void ToggleConsole()
    {
        if (toggleLocked)
            return;

        if (!visible)
            GetComponent<UIFader>().ShowFromEvent();
        else
            GetComponent<UIFader>().HideFromEvent();

        toggleLocked = true;
        visible = !visible;
    }

    public void ParseInput(string text)
    {

        var matches = Regex.Matches(text, @"([a-zA-Z-])+");
        string parameter = "";

        if (matches.Count != 2)
            logLabel.text += string.Format(error, "", "...");
        else
        {
            if (Regex.Match(matches[1].Value, @"--([a-zA-Z])+").Success)
            {
                parameter = Regex.Match(matches[1].Value, @"([a-zA-Z])+").Value.ToLower();
            }

            if (matches[0].Value == "hint")
            {
                PrintCommand(hints, "hint", parameter);
            }
            else if (matches[0].Value == "password")
            {
                PrintCommand(solutions, "password", parameter);
            }
            else
                logLabel.text += string.Format(error, " <color=\"blue\">" + matches[0].Value+ "</color>", "...");
        }
    }

    private void PrintCommand(Dictionary<string, string> pool, string command, string parameter)
    {
        try
        {
            logLabel.text += "<br>" + pool[parameter];
        }
        catch
        {
            logLabel.text += string.Format(error, " <color=\"blue\">command</color>", ", unknown parameter <color=\"green\">" + parameter + "</color>");
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F1))
        {
            ToggleConsole();
        }
    }
}
