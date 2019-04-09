using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class TeachingSettings : SettingsBase {

    protected float algorithmSpeed;
    protected string algorithm, teachingMode;
    protected int difficulty, algSpeed;
    protected bool stepBack;

    [Space(5)]
    [Header("Teaching settings")]
    [SerializeField]
    protected bool useDebuggingSettings;

    [SerializeField]
    protected bool pseudocodeLineNr, pseudocodeStep;

    [SerializeField]
    protected TeachingModeEditor teachingModeEditor;
    protected enum TeachingModeEditor { Demo, UserTest }

    [SerializeField]
    protected AlgorithmSpeedEditor algorithmSpeedEditor;
    protected enum AlgorithmSpeedEditor { Slow, Normal, Fast, Test }

    [SerializeField]
    protected DifficultyEditor difficultyEditor;
    protected enum DifficultyEditor { beginner, intermediate, advanced, examination }

    protected virtual void Start()
    {
        // Debugging editor (fast edit settings)
        if (useDebuggingSettings)
            GetSettingsFromEditor();
        else
        {
            // Init settings
            TeachingMode = Util.DEMO;
            AlgorithmSpeedLevel = 1;
            Difficulty = 0;
        }
    }


    // Init settings menu from editor
    protected virtual void GetSettingsFromEditor()
    {
        switch ((int)teachingModeEditor)
        {
            case 0: TeachingMode = Util.DEMO; break;
            case 1: TeachingMode = Util.USER_TEST; break;
        }

        AlgorithmSpeedLevel = (int)algorithmSpeedEditor;
        Difficulty = (int)difficultyEditor;
    }

    protected override void InitButtons()
    {
        InitButtonState(Util.TEACHING_MODE, teachingMode);
        InitButtonState(Util.OPTIONAL, Util.PSEUDOCODE_STEP, pseudocodeStep);
        InitButtonState(Util.OPTIONAL, Util.PSEUDOCODE_LINE_NR, pseudocodeLineNr);
        InitButtonState(Util.DIFFICULTY, Util.DIFFICULTY, difficulty);
        InitButtonState(Util.DEMO_SPEED, Util.DEMO_SPEED, algSpeed);
    }

    // All input from interactable settings menu goes through here
    public override void UpdateInteraction(string sectionID, string itemID, string itemDescription)
    {
        switch (sectionID)
        {
            case Util.ALGORITHM: Algorithm = itemID; break;
            case Util.TEACHING_MODE: TeachingMode = itemID; break;
            case Util.DIFFICULTY: Difficulty = Util.difficultyConverterDict.FirstOrDefault(x => x.Value == itemID).Key; break;
            case Util.DEMO_SPEED: AlgorithmSpeedLevel = Util.algorithSpeedConverterDict.FirstOrDefault(x => x.Value == itemID).Key; break;
            case Util.OPTIONAL:
                switch (itemID)
                {
                    case Util.PSEUDOCODE_STEP: PseudocodeStep = Util.ConvertStringToBool(itemDescription); break;
                    case Util.PSEUDOCODE_LINE_NR: PseudocodeLineNr = Util.ConvertStringToBool(itemDescription); break;
                    default: Debug.LogError("Couldn't update: section = " + sectionID + ", item = " + itemID + ", description = " + itemDescription); break;
                }

                break; // TODO
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

    public bool IsUserTest()
    {
        return teachingMode == Util.USER_TEST;
    }

    public bool StepBack
    {
        get { return stepBack; }
        set { stepBack = value; }
    }

    // Beginner, Intermediate, or Examination
    public int Difficulty
    {
        get { return difficulty; }
        set { difficulty = value; }
    }

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
            InitButtonState(Util.DEMO_SPEED, Util.DEMO_SPEED, value);
            switch (value)
            {
                case 0: algorithmSpeed = 2f; break;
                case 1: algorithmSpeed = 1f; break;
                case 2: algorithmSpeed = 0.5f; break;
                case 3: algorithmSpeed = 0f; break;
            }
        }
    }

    public bool PseudocodeLineNr
    {
        get { return pseudocodeLineNr; }
        set { pseudocodeLineNr = value; }
    }

    public bool PseudocodeStep
    {
        get { return pseudocodeStep; }
        set { pseudocodeStep = value; }
    }


    // Instantiates the algorithm/task, user needs to click a start button to start the task (see method below)
    public void InstantiateTask()
    {
        MainManager.InstantiateSetup();
    }

    // Start/stop algorithm in game
    public void StartStopTask()
    {
        if (!MainManager.AlgorithmInitialized)
            StartTask();
        else
            StopTask();
    }

    // Start task from in game
    private void StartTask()
    {
        switch (teachingMode)
        {
            case Util.DEMO: case Util.STEP_BY_STEP: break;
            case Util.USER_TEST:
                if (difficulty <= Util.PSEUDO_CODE_HIGHTLIGHT_MAX_DIFFICULTY)
                    AlgorithmSpeedLevel = 1; // Normal speed
                else
                    AlgorithmSpeedLevel = 3; // 0 Sec per step -- no "lag"
                break;
        }

        MainManager.StartAlgorithm();
    }

    // Stop task from in game
    private void StopTask()
    {
        MainManager.SafeStop();
    }

    protected abstract MainManager MainManager { get; set; }


}
