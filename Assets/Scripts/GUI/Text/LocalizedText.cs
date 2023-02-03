using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System.Text.RegularExpressions;

[RequireComponent(typeof(TMP_Text))]
public class LocalizedText : MonoBehaviour, ITooltipable
{
    public TextType textType;
    public TMP_Text Text { get; internal set; }
    public string Keyword { get; internal set; }
    public Tooltip tooltipParent;

    private bool isHovering;
    private bool tooltipSpawned;

    public float tooltipAppearanceMeantime = 1.0f;
    private float tooltipTimeDelta;
    private int currentLink;

    private SceneLocalizer sceneLocalizer;

#if UNITY_EDITOR
    [MenuItem("GameObject/UI/LocalizedText")]
    static void CreateLocalizedText(MenuCommand menuCommand)
    {
        var go = SpawnLocalizedText(menuCommand.context as GameObject);
        // Register the creation in the undo system
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
        Selection.activeObject = go;
    }

    public static GameObject SpawnLocalizedText(GameObject parent)
    {
        GameObject go = new GameObject("Localized Text", typeof(TextMeshProUGUI), typeof(LocalizedText));

        var text = go.GetComponent<TextMeshProUGUI>();
        text.text = "New Text";
        text.alignment = TextAlignmentOptions.Center;
        // Ensure it gets reparented if this was a context click (otherwise does nothing)
        GameObjectUtility.SetParentAndAlign(go, parent);

        return go;
    }
#endif

    void Start()
    {
        Text = ((TMP_Text)GetComponent(typeof(TMP_Text)));
        RetrieveKeyword();
        sceneLocalizer = (SceneLocalizer)FindObjectsOfType(typeof(SceneLocalizer))[0];
        sceneLocalizer.RegisterText(this);
    }

    public void RetrieveKeyword()
    {
        if (Keyword == null || Keyword == "")
            Keyword = Text.text.Trim();
    }

    public void SetNewKeyword(string keyword)
    {
        Keyword = keyword;
    }

    public void UpdateLocalization(string translation)
    {
        if (Text == null)
            Text = ((TMP_Text)GetComponent(typeof(TMP_Text)));

        Text.text = translation;

        if (gameObject.activeInHierarchy)
            StartCoroutine(ColorLinks(translation));
    }

    private IEnumerator ColorLinks(string translation)
    {
        yield return null;
        foreach (var link in Text.textInfo.linkInfo)
        {
            string linkText = link.GetLinkText();
            var regex = new Regex(">" + linkText + "<", RegexOptions.IgnoreCase);
            Color linkColor = GameObject.FindGameObjectWithTag("ThemeManager").GetComponent<ThemeManager>().theme.linkTextColor;
            translation = regex.Replace(translation, "><color=#" + ColorUtility.ToHtmlStringRGB(linkColor) + regex.Match(translation) + "/color><");
        }

        Text.text = translation;
    }

    public string GetKeyword()
    {
        if (Keyword != null && Keyword != "")
            return Keyword;
        else
            return ((TMP_Text)GetComponent(typeof(TMP_Text))).text.Trim();
    }

    private void Update()
    {
        if (isHovering && !tooltipSpawned)
        {
            if (!tooltipSpawned)
            {
                tooltipTimeDelta += Time.deltaTime;

                if (tooltipTimeDelta >= tooltipAppearanceMeantime)
                {
                    currentLink = TMP_TextUtilities.FindIntersectingLink(Text, Mouse.current.position.ReadValue(), Camera.current);
                    int wordIndex = TMP_TextUtilities.FindIntersectingWord(Text, Mouse.current.position.ReadValue(), Camera.current);
                    if (wordIndex != -1 && currentLink != -1)
                    {
                        var wordInfo = Text.textInfo.wordInfo[wordIndex];
                        var linkInfo = Text.textInfo.linkInfo[currentLink];
                        string tooltipKey = "tooltip_" + linkInfo.GetLinkID();
                        GameObject.FindObjectOfType<TooltipManager>().ShowTooltip(tooltipKey,
                            transform.TransformPoint(wordInfo.textComponent.textInfo.characterInfo[wordInfo.lastCharacterIndex].topRight),
                            tooltipParent,
                            this);
                    }

                    tooltipSpawned = true;
                }
            }
            else
                tooltipTimeDelta = 0;
        }
        else
        {
            tooltipTimeDelta = 0;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
    }

    public void CloseTooltip()
    {
        tooltipSpawned = false;
    }

    public bool MouseOnTooltip()
    {
        return (isHovering) ? currentLink == TMP_TextUtilities.FindIntersectingLink(Text, Mouse.current.position.ReadValue(), Camera.current) : false;
    }
}
