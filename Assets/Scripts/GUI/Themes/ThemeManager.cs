using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[ExecuteAlways]
public class ThemeManager : MonoBehaviour
{
    public Theme theme;

#if UNITY_EDITOR
    private void Update()
    {
        if (SceneManager.GetActiveScene().isDirty)
            UpdateTheme();
    }

    [MenuItem("GameObject/Setup/SceneLocalizer")]
    static void CreateThemeManager(MenuCommand menuCommand)
    {
        var go = SpawnThemeManager(menuCommand.context as GameObject);
        // Register the creation in the undo system
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
        Selection.activeObject = go;
    }

    public static GameObject SpawnThemeManager(GameObject parent)
    {
        GameObject go = new GameObject("ThemeManager", typeof(ThemeManager));

        // Ensure it gets reparented if this was a context click (otherwise does nothing)
        GameObjectUtility.SetParentAndAlign(go, parent);

        return go;
    }
#endif

    public void UpdateTheme()
    {
        UpdateTexts();
        UpdateButtons();
    }

    /// <summary>
    /// Updates every localized text in the scene.
    /// </summary>
    private void UpdateTexts()
    {
        var texts = GameObject.FindObjectsOfType<LocalizedText>();

        foreach (var text in texts)
        {
            if (text.Text == null)
                text.Text = text.gameObject.GetComponent<TMPro.TextMeshProUGUI>();

            switch (text.textType)
            {
                case TextType.Content:
                    text.Text.color = theme.contentTextColor;
                    text.Text.font = theme.contentFont;
                    text.Text.fontStyle = theme.contentStyle;
                    break;
                case TextType.Title:
                    text.Text.color = theme.titleTextColor;
                    text.Text.font = theme.titleFont;
                    text.Text.fontStyle = theme.titleStyle;
                    break;
                case TextType.Subtitle:
                    text.Text.color = theme.subtitleTextColor;
                    text.Text.font = theme.subtitleFont;
                    text.Text.fontStyle = theme.subtitleStyle;
                    break;
                case TextType.Button:
                    text.Text.color = theme.buttonTextColor;
                    text.Text.font = theme.buttonFont;
                    text.Text.fontStyle = theme.buttonStyle;
                    break;
            }
        }
    }

    private void UpdateButtons()
    {
        var buttons = GameObject.FindObjectsOfType<ThemeableButton>();

        foreach (var button in buttons)
        {
            switch (button.buttonType)
            {
                case ButtonType.Default:
                    button.GetComponent<Button>().colors = theme.defaultButton;
                    if (button.backgroundImage != null)
                        button.backgroundImage.color = theme.defaultButton.normalColor;
                    break;
                case ButtonType.Confirm:
                    button.GetComponent<Button>().colors = theme.confirmButton;
                    if (button.backgroundImage != null)
                        button.backgroundImage.color = theme.confirmButton.normalColor;
                    break;
                case ButtonType.Deny:
                    button.GetComponent<Button>().colors = theme.denyButton;
                    if (button.backgroundImage != null)
                        button.backgroundImage.color = theme.denyButton.normalColor;
                    break;
            }
        }
    }
}

public enum TextType
{
    Content,
    Title,
    Subtitle,
    Button,
    Error,
    None
}

public enum ButtonType
{
    Default,
    Confirm,
    Deny,
    None
}
