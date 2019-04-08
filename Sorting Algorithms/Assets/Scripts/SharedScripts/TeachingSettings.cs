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
    protected TeachingModeEditor teachingModeEditor;
    protected enum TeachingModeEditor { Demo, UserTest }

    [SerializeField]
    protected AlgorithmSpeedEditor algorithmSpeedEditor;
    protected enum AlgorithmSpeedEditor { Slow, Normal, Fast, Test }

    [SerializeField]
    protected DifficultyEditor difficultyEditor;
    protected enum DifficultyEditor { beginner, intermediate, advanced, examination }


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

    // All input from interactable settings menu goes through here
    public override void UpdateInteraction(string sectionID, string itemID, string itemDescription)
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
