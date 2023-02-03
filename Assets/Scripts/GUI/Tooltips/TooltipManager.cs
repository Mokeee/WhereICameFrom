using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipManager : MonoBehaviour
{
    public GameObject tooltipPrefab;
    public Transform tooltipParent;

    public Queue<Tooltip> unusedTooltips;
    // Start is called before the first frame update
    void Start()
    {
        unusedTooltips = new Queue<Tooltip>();
    }

    public void ShowTooltip(string keyword, Vector2 position, Tooltip parent, ITooltipable source)
    {
        var tooltip = (unusedTooltips.Count == 0)
            ? Instantiate(tooltipPrefab, tooltipParent).GetComponent<Tooltip>()
            : unusedTooltips.Dequeue();
        tooltip.SetTooltipAndShow(keyword, position, parent, source);
        tooltip.OnClose += source.CloseTooltip;
        tooltip.OnClose += () => { unusedTooltips.Enqueue(tooltip); };
    }


}
