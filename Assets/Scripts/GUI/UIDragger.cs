using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class UIDragger : MonoBehaviour, IDraggable
{
    public RectTransform hitbox;
    private Vector2 offset;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (hitbox != null)
            if (!RectTransformUtility.RectangleContainsScreenPoint(hitbox, Mouse.current.position.ReadValue()))
            {
                eventData.pointerDrag = null;
                return;
            }

        offset = (Vector2)transform.position - Mouse.current.position.ReadValue();

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
