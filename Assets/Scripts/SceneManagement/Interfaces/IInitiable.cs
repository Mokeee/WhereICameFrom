using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IInitiable
{
    UnityAction<float> OnUpdateProgress { get; set; }
    UnityAction<IInitiable> OnCompleted { get; set; }
    List<string> Coroutines { get; set; }
    /// <summary>
    /// Registers all coroutines for initiation and registers initiable.
    /// </summary>
    void RegisterForLoading();
    /// <summary>
    /// Gets called after registration, starts coroutines.
    /// </summary>
    /// <returns></returns>
    IEnumerator InitializeLogic();
    /// <summary>
    /// Is called for every coroutine that finishes.
    /// 
    /// Removes coroutine from waiting list and updates the progress.
    /// </summary>
    /// <param name="method"></param>
    void FinishedCoroutine(string method);
}
