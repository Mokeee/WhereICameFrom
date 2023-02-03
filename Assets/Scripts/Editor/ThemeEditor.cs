using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Linq;
using TMPro;
using UnityEngine.UI;

[CustomEditor(typeof(Theme))]
public class ThemeEditor : Editor
{
    private bool showTextColors;
    private bool showTextFonts;
    private bool showTextStyles;
    private bool showButtons;
    public override void OnInspectorGUI()
    {
        Theme t = (Theme)target;

        showTextColors = EditorGUILayout.Foldout(showTextColors, "Text Colors");
        if (showTextColors)
            ShowField(t, typeof(Color));
        showTextFonts = EditorGUILayout.Foldout(showTextFonts, "Text Fonts");
        if (showTextFonts)
            ShowField(t, typeof(TMP_FontAsset));
        showTextStyles = EditorGUILayout.Foldout(showTextStyles, "Text Styles");
        if (showTextStyles)
            ShowField(t, typeof(FontStyles));
        showButtons = EditorGUILayout.Foldout(showButtons, "Buttons");
        if (showButtons)
            ShowField(t, typeof(ColorBlock));

        if (GUI.changed)
        {
            EditorUtility.SetDirty(t);
            serializedObject.ApplyModifiedProperties();
        }
    }

    private void ShowField(Theme theme, Type type)
    {
        var fields = theme.GetType().GetFields().Where(field => field.FieldType.Equals(type)).ToArray();
        foreach (var field in fields)
        {
            if (type == typeof(Color))
                CreateColorField(field, theme);
            if (type == typeof(FontStyles))
                CreateStyleField(field, theme);
            if (type == typeof(TMP_FontAsset) || type == typeof(ColorBlock))
                CreatePropertyField(field, theme);
        }
    }

    private void CreateColorField(System.Reflection.FieldInfo field, Theme theme)
    {
        var value = (Color)field.GetValue(theme);
        field.SetValue(theme, EditorGUILayout.ColorField(SplitCamelCaseExtension.SplitCamelCase(field.Name), value));
    }

    private void CreateStyleField(System.Reflection.FieldInfo field, Theme theme)
    {
        var value = (FontStyles)field.GetValue(theme);
        field.SetValue(theme, EditorGUILayout.EnumFlagsField(SplitCamelCaseExtension.SplitCamelCase(field.Name), value));
    }

    private void CreatePropertyField(System.Reflection.FieldInfo field, Theme theme)
    {
        var property = serializedObject.FindProperty(field.Name);
        EditorGUILayout.PropertyField(property, true);
    }
}

static class SplitCamelCaseExtension
{
    public static string SplitCamelCase(this string str)
    {
        TextInfo myTI = new CultureInfo("en-US", false).TextInfo;
        return myTI.ToTitleCase(Regex.Replace(Regex.Replace(str, @"(\P{Ll})(\P{Ll}\p{Ll})", "$1 $2"), @"(\p{Ll})(\P{Ll})", "$1 $2"));
    }
}