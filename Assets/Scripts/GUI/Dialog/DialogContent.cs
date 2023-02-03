using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Content of a dialog
/// </summary>
[CreateAssetMenu(fileName = "Dialog", menuName = "ScriptableObjects/Dialog", order = 1)]
public class DialogContent : ScriptableObject
{
    /// <summary>
    /// Header text.
    /// </summary>
    public string header;
    /// <summary>
    /// Subtitle text.
    /// </summary>
    public string subtitle;
    /// <summary>
    /// Content text.
    /// </summary>
    public string content;

    /// <summary>
    /// Flavor image.
    /// </summary>
    public Sprite image;
}
