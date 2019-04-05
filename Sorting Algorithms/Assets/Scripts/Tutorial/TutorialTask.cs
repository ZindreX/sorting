using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TutorialTask : MonoBehaviour {

    protected TutorialArea tutorialArea;
    
    [SerializeField]
    protected int numberOfTasks;
    protected int currentClearedTasks;

    protected bool taskCompleted;

    protected virtual void Awake()
    {
        tutorialArea = GetComponentInParent<TutorialArea>();
    }

    public int NumberOfTasks
    {
        get { return numberOfTasks; }
    }

    public void Progress()
    {
        currentClearedTasks++;

        if (currentClearedTasks == numberOfTasks)
        {
            taskCompleted = true;
            tutorialArea.ReportForwardProgress(this);
        }
    }

    public void BackwardProgress()
    {
        currentClearedTasks--;
        taskCompleted = false;
        tutorialArea.ReportBackwardProgress(this);
    }


    public abstract void InitTask();

    public abstract void StopTask();

    public virtual void ResetTask()
    {
        currentClearedTasks = 0;
        taskCompleted = false;
    }

}
