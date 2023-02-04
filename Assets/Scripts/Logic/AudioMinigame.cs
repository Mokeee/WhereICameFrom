using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AudioMinigame : MonoBehaviour
{
    public List<int> solution;

    public UnityEvent OnWrongEvent = new UnityEvent();
    public UnityEvent OnCorrectEvent = new UnityEvent();

    private List<int> current = new List<int>();

    public void Reset()
    {
        current = new List<int>();
    }

    public void RegisterAnswer(int index)
    {
        Debug.Log(index);
        current.Add(index);

        if (current.Count == solution.Count)
        {
            bool same = true;
            int i = 0;
            foreach(var ix in current)
            {
                same = same && (ix == solution[i]);
                i++;
            }
            if (same)
                OnCorrectEvent.Invoke();
            else
                OnWrongEvent.Invoke();

            Reset();
        }
    }
}
