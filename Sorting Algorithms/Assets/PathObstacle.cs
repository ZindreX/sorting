using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathObstacle : MonoBehaviour {

    [Header("Obstacle")]
    [SerializeField]
    private ObstacleType obstacleType;
    private enum ObstacleType { Goal, Button, Task }

    private TutorialArea tutorialArea;

    private Dictionary<InteractButton, bool> buttons;

    [SerializeField]
    private int numberOfTasks;
    private int currentClearedTasks;


    private void Awake()
    {
        tutorialArea = GetComponentInParent<TutorialArea>();
        switch ((int) obstacleType)
        {
            case 0: tutorialArea.SubTaskCleared(true); break;
            case 1:
                // Find all buttons for this door
                Component[] components = GetComponentsInChildren<InteractButton>();

                // Init variables
                numberOfTasks = components.Length;
                currentClearedTasks = 0;

                // Create a dictionary to keep track of each activation
                buttons = new Dictionary<InteractButton, bool>();
                foreach (Component component in components)
                {
                    buttons.Add(component.GetComponent<InteractButton>(), false);
                }
                break;

            case 2:

                break;
        }
    }

    public int NumberOfTasks
    {
        get { return numberOfTasks; }
    }

    public void ReportFromButton(InteractButton button)
    {
        if (buttons.ContainsKey(button))
        {
            bool active = button.Activated;
            buttons[button] = active;

            if (active)
                currentClearedTasks++;
            else
                currentClearedTasks--;

            tutorialArea.SubTaskCleared(active);
        }

    }

    public void ReportSubTaskCleared()
    {
        currentClearedTasks++;
        tutorialArea.SubTaskCleared(true);
    }

}
