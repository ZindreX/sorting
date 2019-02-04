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

    public readonly int FINAL_TUTORIAL_TASK = 1;

    private Dictionary<int, string> tutorialTasks;
    private int currentTask = 0;
    private bool currentTaskReady = false, currentTaskComplete = false;
    private Vector3 playerPosition, taskStartPosition;

    [SerializeField]
    private GameObject player, infoPanel;

    [SerializeField]
    private TextMesh taskText;

    private void Awake()
    {
        tutorialTasks = new Dictionary<int, string>();
        // Init task
        tutorialTasks.Add(0, "This tutorial will teach you the basics\n of the VR controllers, such as\n teleporting and interaction with\n objects.\n\n Step forward to continue.");

        // Teleporting task
        tutorialTasks.Add(1, "How to teleport using Vive controller:\n - Use the touch surface to\n aim for teleporting areas.\n - Click the touch pad to teleport to new position");

    }

    // Use this for initialization
    void Start () {
 
	}
	
	// Update is called once per frame
	void Update () {
        playerPosition = player.transform.position;

        if (!currentTaskReady)
            SetupNewTask(currentTask);

        switch (currentTask)
        {
            case 0: currentTaskComplete = InitTask(); break;
            case 1: currentTaskComplete = TeleportTaskPart1(); break;
            case 2: currentTaskComplete = TeleportTaskPart2(); break;
        }

        if (currentTaskComplete)
        {
            currentTask++;
            currentTaskComplete = false;
            currentTaskReady = false;

            infoPanel.transform.position = new Vector3(playerPosition.x, 0.3f, playerPosition.z + 1f);
        }
	}

    private void SetupNewTask(int currentTask)
    {
        if (currentTask <= FINAL_TUTORIAL_TASK)
        {
            // Player position when tutorial task starts
            taskStartPosition = player.transform.position;
            taskText.text = tutorialTasks[currentTask];

            // Add and/or remove prefabs needed for the tutorial task
            switch (currentTask)
            {
            }

            currentTaskReady = true;
        }
    }

    // Simple walk forward task
    private bool InitTask()
    {
        return (playerPosition.z - taskStartPosition.z) >= 0.25;
    }

    private bool TeleportTaskPart1()
    {
        return playerPosition.x >= 3.5;
    }

    private bool TeleportTaskPart2()
    {
        return playerPosition.x >= 3.5;
    }

    private bool ClickButtonTask()
    {
        return false;
    }
}
