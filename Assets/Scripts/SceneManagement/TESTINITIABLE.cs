using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TESTINITIABLE : MonoBehaviour, IInitiable
{
    public float loadDuration = 1.0f;
    public UnityAction<float> OnUpdateProgress { get; set; }
    public UnityAction<IInitiable> OnCompleted { get; set; }
    public List<string> Coroutines { get; set; }

    private float progress;

    public void FinishedCoroutine(string method)
    {
        progress++;

        OnUpdateProgress.Invoke(progress / Coroutines.Count);

        if (progress == Coroutines.Count)
            OnCompleted.Invoke(this);
    }

    public IEnumerator InitializeLogic()
    {
        foreach (var routine in Coroutines)
        {
            yield return StartCoroutine(routine);
            FinishedCoroutine(routine);
        }
    }

    public void RegisterForLoading()
    {
        progress = 0;
        Coroutines = new List<string>();
        Coroutines.Add("LoadTest");
        Coroutines.Add("LoadTest");
        Coroutines.Add("LoadTest");

        FindObjectOfType<SceneLoadRegister>().RegisterInitiable(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        RegisterForLoading();
    }

    private IEnumerator LoadTest()
    {
        yield return new WaitForSeconds(loadDuration);
    }
}
