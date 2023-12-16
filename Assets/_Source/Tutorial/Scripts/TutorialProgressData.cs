using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TutorialProgressData
{
    public List<TutorialStep> finishedSteps = new List<TutorialStep>();

    public Action Updated;

    public void FinishedStep(TutorialStep step)
    {
        if (IsStepFinished(step) == false)
        {
            finishedSteps.Add(step);
            Updated?.Invoke();
        }
    }

    public bool IsStepFinished(TutorialStep step)
    {
        return finishedSteps.Contains(step);
    }
}