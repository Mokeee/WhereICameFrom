using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDialog
{
    /// <summary>
    /// Activates the dialog.
    /// </summary>
    bool Show();

    /// <summary>
    /// Activates the dialog from events.
    /// </summary>
    void ShowFromEvent();

    /// <summary>
    /// Deactivates the dialog.
    /// </summary>
    bool Hide();

    /// <summary>
    /// Deactivates the dialog from events.
    /// </summary>
    void HideFromEvent();

    /// <summary>
    /// What happens after the dialog is activated.
    /// </summary>
    void OnEntry();

    /// <summary>
    /// What happens before the dialog is deactivated.
    /// </summary>
    void OnExit();

    void SetSizeAndPosition(Vector2 size, Vector2 position);

    void SetContent(DialogContent content);
}
