using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class SettingsBase : MonoBehaviour {

    protected float algorithmSpeed;
    protected string algorithm, teachingMode;
    protected int difficulty;

    [Header("Settings base")]

    [SerializeField]
    protected TextMeshPro tooltips;

    [Space(2)]
    [Header("Overall settings")]
    [SerializeField]
    protected bool useDebuggingSettings;
    [SerializeField]
    protected TeachingModeEditor teachingModeEditor;
    protected enum TeachingModeEditor { Demo, StepByStep, UserTest }

    [SerializeField]
    protected AlgorithmSpeedEditor algorithmSpeedEditor;
    protected enum AlgorithmSpeedEditor { Slow, Normal, Fast, Test }

    [SerializeField]
    protected DifficultyEditor difficultyEditor;
    protected enum DifficultyEditor { beginner, intermediate, advanced, examination }

    protected Dictionary<string, OnOffButton> buttons;
    [SerializeField]
    protected OnOffButton startButton;

    private void Awake()
    {
        buttons = new Dictionary<string, OnOffButton>();

        // Get all buttons in the settings menu
        Component[] buttonComponents = GetComponentsInChildren<OnOffButton>();
        foreach (OnOffButton component in buttonComponents)
        {
            buttons.Add(component.ButtonID, component);
        }

        // Also add the start button which is located near startpoint/working area
        buttons.Add("Start", startButton);
    }

    //
    public abstract void PrepareSettings();
    protected abstract MainManager MainManager { get; set; }

    public void SetSettingsActive(bool active)
    {
        gameObject.SetActive(active);
    }

    public string Algorithm
    {
        get { return algorithm; }
        set { algorithm = value; FillTooltips("Algorithm:\n" + value); SetActiveButton(value); }
    }

    public string TeachingMode
    {
        get { return teachingMode; }
        set { teachingMode = value; FillTooltips("Teaching mode:\n" + value); SetActiveButton(value); }
    }

    public virtual bool IsDemo()
    {
        return teachingMode == Util.DEMO;
    }

    public bool IsStepByStep()
    {
        return teachingMode == Util.STEP_BY_STEP;
    }

    public bool IsUserTest()
    {
        return teachingMode == Util.USER_TEST;
    }

    // Beginner, Intermediate, or Examination
    public int Difficulty
    {
        get { return difficulty; }
        set { difficulty = value; FillTooltips("Difficulty:\n" + value); SetActiveButton(Util.ConvertDifficulty(value)); }
    }

    public float AlgorithmSpeed
    {
        get { return algorithmSpeed; }
        set { algorithmSpeed = value; FillTooltips("Demo speed:\n" + value + " sec per step"); SetActiveButton(Util.ConvertAlgorithmSpeed(value)); }
    }

    // Instantiates the algorithm/task, user needs to click a start button to start the task (see method below)
    public void InstantiateTask()
    {
        MainManager.InstantiateSetup();
    }

    // Start/stop algorithm in game
    public void StartStopTask()
    {
        if (!MainManager.AlgorithmStarted)
            MainManager.StartAlgorithm();
        else
        {
            MainManager.UserStoppedAlgorithm = true; //
            //MainManager.GetTeachingAlgorithm().DemoStepDuration = new WaitForSeconds(0f);
            MainManager.DestroyAndReset();
        }
        SetActiveButton("Start");
    }

    // Start task from in game
    public void StartTask()
    {
        MainManager.StartAlgorithm();
    }

    // Stop task from in game
    public void StopTask()
    {
        MainManager.DestroyAndReset();
    }

    // Set thje settings menu text
    public void FillTooltips(string text)
    {
        if (tooltips != null)
            tooltips.text = text;
    }

    // Changes the state of the in game menu (marking the selected button with a different material/color)
    protected void SetActiveButton(string buttonID)
    {
        if (buttons.ContainsKey(buttonID))
        {
            OnOffButton button = buttons[buttonID];

            if (button != null)
            {
                if (button.IsOnOffButton)
                    button.ToggleState();
                else
                    button.ChangeState();
            }
            else
                Debug.LogError("Null button: ID= " + buttonID);

        }
        else
            Debug.LogError("No key: " + buttonID);
    }

    protected void HideSubSections()
    {
        foreach (KeyValuePair<string, OnOffButton> entry in buttons)
        {
            OnOffButton button = entry.Value;
            if (button.HasSubSection() && !button.State)
            {
                Debug.Log("Has sub section: " + entry.Key);
                button.HideSubsection();
            }
            //else
            //    Debug.Log("Has no sub section: " + entry.Key);
        }
    }

}
