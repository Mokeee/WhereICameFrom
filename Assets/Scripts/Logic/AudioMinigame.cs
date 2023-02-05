using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class AudioMinigame : MonoBehaviour
{
    public List<int> solution;
    public List<Toggle> states;

    public UnityEvent OnWrongEvent = new UnityEvent();
    public UnityEvent OnCorrectEvent = new UnityEvent();

    private List<int> current = new List<int>();

    public void Reset()
    {
        current = new List<int>();
        foreach (var t in states)
            t.isOn = false;
    }

    public void RegisterAnswer(int index)
    {
        states[current.Count].isOn = true;
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
