using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class EncryptedNote : MonoBehaviour
{
    public string crypt;

    public bool encrypt;

    public TextMeshProUGUI note;

    private string cleanfeed;
    private string encrypted;

    private char[] cryptChars;
    // Start is called before the first frame update
    public void InitCrypt()
    {
        cryptChars = crypt.ToCharArray();
        cleanfeed = note.text;

        encrypted = "";
        foreach(var character in cleanfeed)
        {
            if (character != ' ')
                encrypted += cryptChars[Random.Range(0, cryptChars.Length)];
            else
                encrypted += " ";
        }
    }

    public void UpdateNote()
    {
        if (encrypt)
            note.text = encrypted;
        else
            note.text = cleanfeed;
    }

    private void Update()
    {
        if (Keyboard.current.tabKey.isPressed && Keyboard.current.cKey.isPressed)
        {
            encrypt = false;
            UpdateNote();
        }
    }
}