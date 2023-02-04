using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BorderDragBlocker : MonoBehaviour, IPointerEnterHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(eventData.pointerDrag != null)
            eventData.pointerDrag.GetComponent<IDraggable>().InterruptDrag();
        eventData.pointerDrag = null;
    }
}
