using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class SettingsBase : MonoBehaviour {

    protected float algorithmSpeed;
    protected string algorithm, teachingMode;
    protected int difficulty, algSpeed;

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

    // Old to be removed
    protected Dictionary<string, OnOffButton> buttons;
    [SerializeField]
    protected OnOffButton startButton;




    // New
    protected Dictionary<string, Section> settingsSections;
    //protected Dictionary<string, SettingsMenuItem> settingsMenuItems;

    [SerializeField]
    protected ToggleButton startButton2;


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




        // new stuff
        settingsSections = new Dictionary<string, Section>();
        Component[] sectionComponents = GetComponentsInChildren<Section>();
        foreach (Section section in sectionComponents)
        {
            string sectionID = section.SectionID;
            if (!settingsSections.ContainsKey(sectionID))
                settingsSections[sectionID] = section;
            else
                Debug.LogError("Section already in dict: " + sectionID);
        }

        Debug.Log("Number of sections added: " + settingsSections.Count);



        //settingsMenuItems = new Dictionary<string, SettingsMenuItem>();
        //Component[] newButtonComponents = GetComponentsInChildren<SettingsMenuItem>();
        //foreach (SettingsMenuItem item in newButtonComponents)
        //{
        //    string itemID = item.ItemID;
        //    if (!settingsMenuItems.ContainsKey(itemID))
        //        settingsMenuItems[itemID] = item;
        //    else
        //        Debug.LogError("Item already in dict: " + itemID);
        //}
    }


    //
    public virtual void PrepareSettings()
    {
        switch ((int)teachingModeEditor)
        {
            case 0: TeachingMode = Util.DEMO; break;
            case 1: TeachingMode = Util.STEP_BY_STEP; break;
            case 2: TeachingMode = Util.USER_TEST; break;
        }

        AlgorithmSpeedLevel = (int)algorithmSpeedEditor;
        Difficulty = (int)difficultyEditor;

    }

    // All input from interactable settings menu goes through here
    public virtual void UpdateValueFromSettingsMenu(string sectionID, string itemID)
    {
        switch (sectionID)
        {
            case Util.ALGORITHM: Algorithm = itemID; break;
            case Util.TEACHING_MODE: TeachingMode = itemID; break;
                //case Util.DIFFICULTY: Difficulty = itemID; break;
                //case Util.ALGORITHM_SPEED: AlgorithmSpeed = itemID; break;

        }
    }

    public string Algorithm
    {
        get { return algorithm; }
        set { algorithm = value; FillTooltips("Algorithm:\n" + value); InitButtonState(Util.ALGORITHM, value); } //SetActiveButton(value); }
    }

    public string TeachingMode
    {
        get { return teachingMode; }
        set { teachingMode = value; FillTooltips("Teaching mode:\n" + value); InitButtonState(Util.TEACHING_MODE, value); } // SetActiveButton(value); }
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
        set { difficulty = value; FillTooltips("Difficulty:\n" + value); InitButtonState(Util.DIFFICULTY, Util.DIFFICULTY, value); } // SetActiveButton(Util.ConvertDifficulty(value)); }
    }

    // OLD
    public float AlgorithmSpeed
    {
        get { return algorithmSpeed; }
    }

    public int AlgorithmSpeedLevel
    {
        get { return algSpeed; }
        set
        {
            algSpeed = value;
            switch (value)
            {
                case 0: algorithmSpeed = 2f; break;
                case 1: algorithmSpeed = 1f; break;
                case 2: algorithmSpeed = 0.5f; break;
                case 3: algorithmSpeed = 0f; break;
            }
            InitButtonState(Util.ALGORITHM_SPEED, Util.ALGORITHM_SPEED, value);
        }
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
            MainManager.DestroyAndReset();
        }
        //SetActiveButton("Start");
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

    // OLD: To be removed!
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

    // new stuff
    protected void InitButtonState(string sectionID, string itemID)
    {
        if (settingsSections.ContainsKey(sectionID))
        {
            Section section = settingsSections[sectionID];

            section.InitItem(itemID);

        }
        else
            Debug.LogError("No section: " + sectionID);
    }

    protected void InitButtonState(string sectionID, string itemID, int value)
    {
        if (settingsSections.ContainsKey(sectionID))
        {
            Section section = settingsSections[sectionID];

            section.InitItem(itemID, value);

        }
        else
            Debug.LogError("No section: " + sectionID);
    }

    protected void InitButtonState(string sectionID, string itemID, bool value)
    {
        if (settingsSections.ContainsKey(sectionID))
        {
            Section section = settingsSections[sectionID];

            section.InitItem(itemID, value);

        }
        else
            Debug.LogError("No section: " + sectionID);
    }

    protected void HideSubSections()
    {
        foreach (KeyValuePair<string, OnOffButton> entry in buttons)
        {
            OnOffButton button = entry.Value;
            if (button.HasSubSection() && !button.State)
            {
                //Debug.Log("Has sub section: " + entry.Key);
                button.HideSubsection();
            }
            //else
            //    Debug.Log("Has no sub section: " + entry.Key);
        }
    }


    protected abstract MainManager MainManager { get; set; }

    public void SetSettingsActive(bool active)
    {
        Util.HideObject(gameObject, active);
    }
}
