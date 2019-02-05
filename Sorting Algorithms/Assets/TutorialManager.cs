using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour {

    /* ****** Tutorial tasks ******
     * 0) Init
     * 1) Teleport point
     * 2) Teleport area
     * 3) Grab/move object
     * 4) Click button
     * 5) Use portal
     * 
    */

    public readonly int FINAL_TUTORIAL_TASK = 1, TASK_COMPLETE = -1;
    public readonly char SPLIT = ':';

    private Dictionary<int, List<string>> tutorialTasks;
    private int currentTask = 0;
    private int currentSubTask = 0, prevSubtask = -1, numberOfSubTasks;
    private bool currentTaskComplete;

    private Vector3 playerPosition, taskStartPosition;

    [SerializeField]
    private GameObject[] infoPanels;

    [SerializeField]
    private GameObject player;

    [SerializeField]
    private BoxCollider[] taskAreas;

    [SerializeField]
    private TextMesh debugText;

    private void Awake()
    {
        tutorialTasks = new Dictionary<int, List<string>>();

        // Init task
        tutorialTasks.Add(0, new List<string>());
        tutorialTasks[0].Add("Information:This tutorial will teach you the\n basics of the VR controllers, such\n as teleporting and interaction\n with objects.\n\n Step forward to continue.");
        // Teleporting sub task
        tutorialTasks[0].Add("Teleporting point: Click the touch pad to aim,\n then release to teleport to your\n new destination.\n\n Aim for the teleport point\n to your right.");

        // Teleport area
        tutorialTasks.Add(1, new List<string>());
        tutorialTasks[1].Add("Teleporting area: Use the same technique as\n in the previous task. \n\n Aim for the teleporting area\n to your left.");
    }

    // Use this for initialization
    void Start () {
        // Number of sub tasks
        numberOfSubTasks = tutorialTasks[currentTask].Count;
    }
	
	// Update is called once per frame
	void Update () {
        // No more tasks
        if (currentTask > FINAL_TUTORIAL_TASK)
        {
            debugText.text = "All tasks completed.";
            return;
        }
        
        // Get current position of the player (camera object)
        playerPosition = player.transform.position;

        // Set up if new task is assigned to player
        if (prevSubtask != currentSubTask && currentSubTask < numberOfSubTasks)
            SetupMainOrSubTask();

        // Checks whether task is completed by player
        PerformTask();

        // Prepare for next main task
        if (currentTaskComplete)
        {
            // Next main task
            currentTask++;
            // Number of sub tasks
            numberOfSubTasks = tutorialTasks[currentTask].Count;
            // Reset counters for sub tasks
            currentSubTask = 0;
            prevSubtask = -1;
            currentTaskComplete = false;
        }

        debugText.text = "Task: " + currentTask + ", subtask: " + currentSubTask + "/" + numberOfSubTasks;
	}

    // Sets up a new tutorial task (update information panels, add stuff?)
    private void SetupMainOrSubTask()
    {
        // Player position when tutorial task starts
        taskStartPosition = player.transform.position;

        // 0: title, 1: information
        string[] taskSplit = tutorialTasks[currentTask][currentSubTask].Split(SPLIT);
        infoPanels[currentTask].transform.Find("Panel/Title").GetComponent<TextMesh>().text = taskSplit[0];
        infoPanels[currentTask].transform.Find("Panel/Information").GetComponent<TextMesh>().text = taskSplit[1];

        prevSubtask++;
    }


    private void PerformTask()
    {
        int validationValue = 0;
        switch (currentTask)
        {
            case 0: validationValue = Task1(); break;
            case 1: validationValue = Task2(); break;
        }

        if (validationValue >= 0)
            currentSubTask += validationValue;
        else
            currentTaskComplete = true;
    }

    // **** Task completed methods ****

    /* Task 1
     * - Sub task 1: Walk forward towards the information panel
     * - Sub task 2: Teleport to the point to the right of the player
    */
    private int Task1()
    {
        switch (currentSubTask)
        {
            case 0: case 1: return taskAreas[currentSubTask].GetComponent<BoxCollider>().bounds.Contains(player.transform.position) ? 1 : 0;
            default: return TASK_COMPLETE;
        }
    }

    private int Task2()
    {
        return 0;
    }

}
