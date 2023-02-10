using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class EncryptedNote : MonoBehaviour
{
    public string crypt;

    public bool encrypt;
    public int skipToPosition;

    public Image note;
    public UnityEvent OnNoteEncrypted = new UnityEvent();

    public Sprite cleanfeed;
    public Sprite encrypted;
    /*
    private string cleanfeed;
    private string encrypted;

    private char[] cryptChars;
    // Start is called before the first frame update
    public void InitCrypt()
    {
        cryptChars = crypt.ToCharArray();
        cleanfeed = note.text;

        encrypted = "";
        int i = 0;
        foreach(var character in cleanfeed)
        {
            if (i <= skipToPosition)
            {
                encrypted += character;
            }
            else if (character != ' ')
                encrypted += cryptChars[Random.Range(0, cryptChars.Length)];
            else
                encrypted += " ";

            i++;
        }
    }

    public void UpdateNote()
    {
        if (encrypt)
            note.text = encrypted;
        else
            note.text = cleanfeed;
    }
    */
    public void UpdateNote()
    {
        if (encrypt)
            note.sprite = encrypted;
        else
            note.sprite = cleanfeed;
    }

    private void Update()
    {
        if (Keyboard.current.tabKey.isPressed && Keyboard.current.cKey.isPressed)
        {
            if(encrypt)
            {
                encrypt = false;
                OnNoteEncrypted.Invoke();
                UpdateNote();
            }
        }
    }
}
