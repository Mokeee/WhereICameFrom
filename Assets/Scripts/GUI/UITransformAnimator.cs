using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Flags]
public enum UIAnimationType : short
{
    None = 0,
    Translation = 1,
    Scale = 2,
    Rotation = 4
}

public class UITransformAnimator : MonoBehaviour
{
    public UIAnimationType animationType;
    public float animationDuration;
    public AnimationCurve translationCurve;
    public AnimationCurve scaleCurve;
    public AnimationCurve rotationCurve;

    public Vector2 TranslationFactor = new Vector2();
    public Vector2 ScaleFactor = new Vector2();
    public Vector3 RotationFactor = new Vector3();

    private float startpoint;
    private float direction;

    public Vector2 TranslationTarget = new Vector2();
    public Vector3 ScaleTarget = new Vector3();
    public Vector3 RotationTarget = new Vector3();

    private Queue<Coroutine> coroutines = new Queue<Coroutine>();

    public void StartAnimationForward(RectTransform rectTransform = null, float animationDuration = 1.0f)
    {
        CheckCoroutines(rectTransform);
        coroutines.Enqueue(StartCoroutine(Animate(rectTransform, animationDuration, 0, 1)));
    }

    public void StartAnimationBackward(RectTransform rectTransform = null, float animationDuration = 1.0f)
    {
        CheckCoroutines(rectTransform);
        coroutines.Enqueue(StartCoroutine(Animate(rectTransform, animationDuration, 1, -1)));
    }

    private void CheckCoroutines(RectTransform rectTransform)
    {
        if (coroutines.Count > 0)
        {
            if (rectTransform == null)
                TryGetComponent<RectTransform>(out rectTransform);


            if ((animationType & UIAnimationType.Translation) == UIAnimationType.Translation)
                rectTransform.anchoredPosition = TranslationTarget;
            if ((animationType & UIAnimationType.Scale) == UIAnimationType.Scale)
                rectTransform.localScale = ScaleTarget;
            if ((animationType & UIAnimationType.Rotation) == UIAnimationType.Rotation)
                rectTransform.rotation = Quaternion.Euler(RotationTarget);
            StopAllCoroutines();
            coroutines.Clear();
        }
    }

    private IEnumerator Animate(RectTransform rectTransform, float animationDuration, float start, float dir)
    {
        startpoint = start;
        direction = dir;

        if (rectTransform == null)
            TryGetComponent<RectTransform>(out rectTransform);

        if ((animationDuration == 1.0f || animationDuration <= 0) && this.animationDuration > 0)
            animationDuration = this.animationDuration;

        if (rectTransform == null)
            throw new NullReferenceException("No RectTransform to animate!");

        bool translate = (animationType & UIAnimationType.Translation) == UIAnimationType.Translation;
        bool scale = (animationType & UIAnimationType.Scale) == UIAnimationType.Scale;
        bool rotate = (animationType & UIAnimationType.Rotation) == UIAnimationType.Rotation;

        float deltaTime = 0;
        Vector2 position = rectTransform.anchoredPosition;
        Vector2 initialScale = (Vector2)rectTransform.localScale;
        Vector3 rotation = rectTransform.rotation.eulerAngles;

        TranslationTarget = position + TranslationFactor * direction;
        ScaleTarget = initialScale + ScaleFactor * direction;
        RotationTarget = rotation + RotationFactor * direction;

        while (deltaTime < animationDuration)
        {
            deltaTime += Time.deltaTime;
            float t = Mathf.Clamp(Mathf.Abs(startpoint - (deltaTime / animationDuration)), 0, 1);

            if (translate) { Translate(rectTransform, position, direction * Mathf.Abs(startpoint - translationCurve.Evaluate(t))); };
            if (scale) { Scale(rectTransform, initialScale, direction * Mathf.Abs(startpoint - scaleCurve.Evaluate(t))); };
            if (rotate) { Rotate(rectTransform, rotation, direction * Mathf.Abs(startpoint - rotationCurve.Evaluate(t))); };

            yield return null;
        }


        if (translate) { Translate(rectTransform, position, direction); };
        if (scale) { Scale(rectTransform, initialScale, direction); };
        if (rotate) { Rotate(rectTransform, rotation, direction); };

        coroutines.Dequeue();
    }

    private void Translate(RectTransform rectTransform, Vector2 position, float t)
    {
        Vector2 newPos = position;
        newPos += TranslationFactor * t;

        rectTransform.anchoredPosition = newPos;
    }
    private void Scale(RectTransform rectTransform, Vector2 scale, float t)
    {
        Vector2 newScale = scale;
        newScale += ScaleFactor * t;

        rectTransform.localScale = newScale;
    }
    private void Rotate(RectTransform rectTransform, Vector3 rotation, float t)
    {
        Vector3 newRot = rotation;
        var mod = new Vector3(360, 360, 360);
        newRot += RotationFactor * t + mod;
        newRot.x %= mod.x;
        newRot.y %= mod.x;
        newRot.z %= mod.x;

        rectTransform.localRotation = Quaternion.Euler(newRot);
    }
}
