using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(UITransformAnimator))]
public class UITransformAnimatorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        UITransformAnimator anim = (UITransformAnimator)target;

        anim.animationType = (UIAnimationType)EditorGUILayout.EnumFlagsField("Animation Type", anim.animationType);

        if ((anim.animationType & UIAnimationType.Translation) == UIAnimationType.Translation)
            anim.TranslationFactor = EditorGUILayout.Vector2Field("Translation Factor", anim.TranslationFactor);
        if ((anim.animationType & UIAnimationType.Scale) == UIAnimationType.Scale)
            anim.ScaleFactor = EditorGUILayout.Vector2Field("Scale Factor", anim.ScaleFactor);
        if ((anim.animationType & UIAnimationType.Rotation) == UIAnimationType.Rotation)
            anim.RotationFactor = EditorGUILayout.Vector3Field("Rotation Factor", anim.RotationFactor);

        if(anim.animationType != UIAnimationType.None)
        {
            anim.animationDuration = EditorGUILayout.FloatField("Animation Duration", anim.animationDuration);

            var property = serializedObject.FindProperty("translationCurve");
            EditorGUILayout.PropertyField(property, true);
            property = serializedObject.FindProperty("scaleCurve");
            EditorGUILayout.PropertyField(property, true);
            property = serializedObject.FindProperty("rotationCurve");
            EditorGUILayout.PropertyField(property, true);
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(anim);
            serializedObject.ApplyModifiedProperties();
        }
    }
}