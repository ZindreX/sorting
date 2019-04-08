using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TutorialButtonTask : TutorialTask {

    private Dictionary<InteractButton, bool> buttons;

    protected override void Awake()
    {
        base.Awake();

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
    }

    public override void InitTask()
    {
        try
        {
            foreach (KeyValuePair<InteractButton, bool> entry in buttons)
            {
                InteractButton button = entry.Key;
                bool active = entry.Value;

                if (active)
                {
                    button.ButtonClicked();
                    buttons[button] = false;
                }

            }
        }
        catch (InvalidOperationException e)
        {
            return;
        }
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
            {
                currentClearedTasks--;
                tutorialArea.ReportBackwardProgress(this);
            }

            if (currentClearedTasks == numberOfTasks)
            {
                tutorialArea.ReportForwardProgress(this);
            }
        }
    }

    public override void StopTask()
    {

    }
}
