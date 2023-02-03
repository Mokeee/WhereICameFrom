using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.EventSystems;

public class Tooltip : Dialog, ITooltipable
{
    public Action OnClose;

    public float tooltipLifetime = 1.0f;
    private float tooltipLifetimeDelta;

    public bool spawnedTooltip;
    private bool mouseEntered;

    private ITooltipable source;

    /// <summary>
    /// Sets the tooltip's keyword and hence it's text.
    /// </summary>
    /// <param name="tooltipKeyword">Tooltip keyword.</param>
    /// <param name="position">World space tooltip position</param>
    /// <param name="parent">Parent tooltip, is null if there is none</param>
    public void SetTooltipAndShow(string tooltipKeyword, Vector2 position, Tooltip parent, ITooltipable source)
    {
        this.source = source;

        tooltipLifetimeDelta = 0;
        OnClose = null;

        if (parent != null)
        {
            parent.spawnedTooltip = true;
            OnClose += parent.OnChildHidden;
        }

        contentText.tooltipParent = this;

        SetSizeAndPosition(new Vector2(), position);
        Show(tooltipKeyword);

        StartCoroutine(CorrectPosition());
    }

    //Position relative to bottom right and in worls space
    //yield one frame and correct position for sizeDelta
    private IEnumerator CorrectPosition()
    {
        yield return null;

        var textRect = contentText.GetComponent<RectTransform>();
        var rect = GetComponent<RectTransform>();
        rect.sizeDelta = textRect.sizeDelta + new Vector2(2, 2);
        //rect.localPosition += new Vector3(textRect.sizeDelta.x / 2.0f, textRect.sizeDelta.y / 2.0f, 0);
    }

    public void CloseTooltip()
    {
        if (Hide())
        {
            OnClose.Invoke();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseEntered = true;
        tooltipLifetimeDelta = 0;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (mouseEntered && !spawnedTooltip)
            CloseTooltip();

        mouseEntered = false;
    }

    private void OnChildHidden()
    {
        spawnedTooltip = false;
    }

    void Update()
    {
        if (!mouseEntered && !spawnedTooltip && !source.MouseOnTooltip())
        {
            tooltipLifetimeDelta += Time.deltaTime;
            if (tooltipLifetimeDelta >= tooltipLifetime)
                CloseTooltip();
        }
    }

    public bool MouseOnTooltip()
    {
        return mouseEntered;
    }
}
