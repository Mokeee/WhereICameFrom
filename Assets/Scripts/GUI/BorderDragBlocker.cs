using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BorderDragBlocker : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private CursorLockMode cursorState;
    private bool cursorVisibility;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(eventData.pointerDrag != null)
            eventData.pointerDrag.GetComponent<IDraggable>().InterruptDrag();
        eventData.pointerDrag = null;

        cursorState = Cursor.lockState;
        cursorVisibility = Cursor.visible;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Cursor.lockState = cursorState;
        Cursor.visible = cursorVisibility;
    }
}
