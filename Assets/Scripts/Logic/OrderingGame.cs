using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderingGame : MonoBehaviour
{
    public void RaiseInOrder(GameObject child)
    {
        int index = child.transform.GetSiblingIndex();
        index = Mathf.Max(0, index - 1);
        child.transform.SetSiblingIndex(index);
    }

    public void DescendInOrder(GameObject child)
    {
        int index = child.transform.GetSiblingIndex();
        index = Mathf.Min(index + 1, transform.childCount - 1);
        child.transform.SetSiblingIndex(index);
    }
}
