using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class UIDragger : MonoBehaviour, IDraggable
{
    public RectTransform hitbox;
    private Vector2 offset;
    private Vector2 startingPosition;

    public void InterruptDrag()
    {
        transform.position = startingPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (hitbox != null)
            if (!RectTransformUtility.RectangleContainsScreenPoint(hitbox, Mouse.current.position.ReadValue()))
            {
                eventData.pointerDrag = null;
                return;
            }

        startingPosition = (Vector2)transform.position;
        offset = startingPosition - Mouse.current.position.ReadValue();

        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Mouse.current.position.ReadValue() + offset;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }
}
