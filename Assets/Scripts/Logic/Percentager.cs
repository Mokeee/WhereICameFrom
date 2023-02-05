using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Percentager : MonoBehaviour, IPointerClickHandler
{
    public FloatEvent OnPercentageFound;

    public void OnPointerClick(PointerEventData eventData)
    {
        var trans = GetComponent<RectTransform>();
        Vector2 originPos = (Vector2)trans.position;
        float xPos = eventData.position.x - originPos.x;
        OnPercentageFound.Invoke(xPos / trans.rect.width);
    }
}
