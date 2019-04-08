using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialArea : Area {

    [Header("Tutorial area")]
    [SerializeField]
    private TutorialArea nextArea;

    private TutorialManager tutorialManager;
    private AudioManager audioManager;

    private Dictionary<TutorialTask, bool> subTasks;

    private int subtasksCompleted, numberOfSubtasks;
    private bool isAreaCleared;

    protected override void Awake()
    {
        base.Awake();

        tutorialManager = FindObjectOfType<TutorialManager>();
        audioManager = FindObjectOfType<AudioManager>();

        // Find tasks and sum the number of sub tasks
        TutorialTask[] tasks = GetComponentsInChildren<TutorialTask>();
        Debug.Log(">>> Init:: Area: " + AreaName + ": " + tasks.Length);
        if (tasks != null)
        {
            subTasks = new Dictionary<TutorialTask, bool>();
            foreach (TutorialTask subTask in tasks)
            {
                numberOfSubtasks++;
                subTasks[subTask] = false;
            }
        }
    }

    public TutorialArea NextArea
    {
        get { return nextArea; }
    }

    public bool IsAreaCleared
    {
        get { return isAreaCleared; }
    }

    // Initializes the tasks
    public override void InitTask()
    {
        if (subTasks != null)
        {
            foreach (KeyValuePair<TutorialTask, bool> entry in subTasks)
            {
                entry.Key.InitTask();
            }
        }
    }

    // Gets report from the task
    public void ReportForwardProgress(TutorialTask tutorialTask)
    {
        if (subTasks.ContainsKey(tutorialTask))
        {
            subTasks[tutorialTask] = true;
            subtasksCompleted++;

            if (subtasksCompleted >= numberOfSubtasks)
            {
                isAreaCleared = true;
                AreaCleared(true);
            }
        }
    }

    public void ReportBackwardProgress(TutorialTask tutorialTask)
    {
        if (subTasks.ContainsKey(tutorialTask))
        {
            subTasks[tutorialTask] = false;
            subtasksCompleted--;

            isAreaCleared = false;
            AreaCleared(false);
        }
    }

    private void AreaCleared(bool cleared)
    {
        Debug.Log("Area: " + AreaName + ",  cleared: " + cleared);
        if (cleared)
        {
            OpenPath();
            audioManager.Play("GoalCompleted");
        }
        else if (door.DoorOpen)
            door.CloseDoor();       
    }


    // Detecting when a player enters the area
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if (other.name == Util.HEAD_COLLIDER)
        {
            tutorialManager.PlayerInArea(this);
        }
    }

    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
        
        if (subTasks != null)
        {
            foreach (KeyValuePair<TutorialTask, bool> entry in subTasks)
            {
                entry.Key.StopTask();
            }
        }
    }



}
