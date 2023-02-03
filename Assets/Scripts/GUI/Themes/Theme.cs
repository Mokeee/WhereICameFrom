using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Theme", menuName = "ScriptableObjects/Theme", order = 1)]
public class Theme : ScriptableObject
{
    public Color titleTextColor = Color.black;
    public Color subtitleTextColor = Color.black;
    public Color contentTextColor = Color.black;
    public Color buttonTextColor = Color.black;
    public Color linkTextColor = Color.black;

    public TMP_FontAsset titleFont;
    public TMP_FontAsset subtitleFont;
    public TMP_FontAsset contentFont;
    public TMP_FontAsset buttonFont;

    public FontStyles titleStyle;
    public FontStyles subtitleStyle;
    public FontStyles contentStyle;
    public FontStyles buttonStyle;

    [Header("Default Button")]
    public ColorBlock defaultButton;
    [Header("Confirm Button")]
    public ColorBlock confirmButton;
    [Header("Deny Button")]
    public ColorBlock denyButton;
}
