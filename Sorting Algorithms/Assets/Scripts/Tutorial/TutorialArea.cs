using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialArea : Area {

    [Header("Tutorial area")]
    [SerializeField]
    private TutorialArea nextArea;

    private TutorialManager tutorialManager;
    private AudioManager audioManager;

    private int achievedTasks, numberOfTasks;
    private bool cleared;

    protected override void Awake()
    {
        base.Awake();

        tutorialManager = FindObjectOfType<TutorialManager>();
        audioManager = FindObjectOfType<AudioManager>();

        PathObstacle[] obstacles = GetComponentsInChildren<PathObstacle>();
        if (obstacles != null)
        {
            foreach (PathObstacle po in obstacles)
            {
                numberOfTasks += po.NumberOfTasks;
            }
        }
    }

    public TutorialArea NextArea
    {
        get { return nextArea; }
    }

    public bool Cleared
    {
        get { return cleared; }
    }

    public override void InitArea()
    {
        switch (AreaName)
        {
            case "Graph":

                break;
        }
    }

    public void SubTaskCleared(bool cleared)
    {
        if (cleared)
        {
            achievedTasks++;

            if (achievedTasks == numberOfTasks)
            {
                cleared = true;
                AreaCleared(true);
            }
        }
        else
        {
            achievedTasks--;
            cleared = false;
            AreaCleared(false);
        }
    }

    private void AreaCleared(bool cleared)
    {
        if (cleared)
        {
            OpenPath();
            audioManager.Play("GoalCompleted");
        }
        else if (door.DoorOpen)
            door.CloseDoor();       
    }










    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        tutorialManager.PlayerInArea(this);
    }

    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
    }



}
