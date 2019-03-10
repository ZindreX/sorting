using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

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


    protected Dictionary<string, Section> settingsSections;

    private void Awake()
    {
        // Get all sections from the settings menu
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
    }


    // Init settings menu from editor
    protected virtual void GetSettingsFromEditor()
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
    protected abstract void InitButtons();

    // All input from interactable settings menu goes through here
    public virtual void UpdateValueFromSettingsMenu(string sectionID, string itemID, string itemDescription)
    {
        switch (sectionID)
        {
            case Util.ALGORITHM: Algorithm = itemID; break;
            case Util.TEACHING_MODE: TeachingMode = itemID; break;
            case Util.DIFFICULTY: Difficulty = Util.difficultyConverterDict.FirstOrDefault(x => x.Value == itemID).Key; break;
            case Util.DEMO_SPEED: AlgorithmSpeedLevel = Util.algorithSpeedConverterDict.FirstOrDefault(x => x.Value == itemID).Key; break;
            case Util.READY: InstantiateTask(); break;
            case Util.START: StartStopTask(); break;
        }
    }

    public string Algorithm
    {
        get { return algorithm; }
        set { algorithm = value; }
    }

    public string TeachingMode
    {
        get { return teachingMode; }
        set { teachingMode = value; }
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
        set { difficulty = value; }
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
            StartTask();
        else
            StopTask();
    }

    // Start task from in game
    private void StartTask()
    {
        MainManager.StartAlgorithm();
    }

    // Stop task from in game
    private void StopTask()
    {
        MainManager.UserStoppedAlgorithm = true;
        MainManager.DestroyAndReset();
    }

    // Set thje settings menu text
    public void FillTooltips(string text)
    {
        if (tooltips != null)
            tooltips.text = text;
    }

    // ----------------------- Init settings menu buttons -----------------------
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

    public void SetSettingsActive(bool active)
    {
        Util.HideObject(gameObject, active, true);
    }

    protected abstract MainManager MainManager { get; set; }
}
