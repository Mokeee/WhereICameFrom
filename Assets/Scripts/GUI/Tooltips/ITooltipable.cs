using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public interface ITooltipable : IPointerEnterHandler, IPointerExitHandler
{
    void CloseTooltip();

    bool MouseOnTooltip();
}
