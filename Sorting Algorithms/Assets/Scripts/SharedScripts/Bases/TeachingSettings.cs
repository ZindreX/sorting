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

    [Space(5)]
    [Header("Pseudocode optional")]
    [SerializeField]
    protected bool pseudocodeLineNr;

    [SerializeField]
    protected bool pseudocodeStep;

    [SerializeField]
    protected bool userTestScore;

    [Header("Main settings")]
    [SerializeField]
    protected TeachingModeEditor teachingModeEditor;
    protected enum TeachingModeEditor { Demo, UserTest }

    [SerializeField]
    protected AlgorithmSpeedEditor algorithmSpeedEditor;
    protected enum AlgorithmSpeedEditor { Slow, Normal, Fast, SFast }

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
            AlgorithmSpeedLevel = 0;
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
        // Basic stuff
        InitButtonState(Util.TEACHING_MODE, teachingMode);
        InitButtonState(Util.DIFFICULTY, Util.DIFFICULTY, difficulty);
        InitButtonState(Util.DEMO_SPEED, Util.DEMO_SPEED, algSpeed);

        // Optional
        InitButtonState(Util.OPTIONAL, Util.PSEUDOCODE_STEP, pseudocodeStep);
        InitButtonState(Util.OPTIONAL, Util.PSEUDOCODE_LINE_NR, pseudocodeLineNr);
        InitButtonState(Util.OPTIONAL, Util.SCORE, userTestScore);
    }

    // All input from interactable settings menu goes through here
    public override void UpdateInteraction(string sectionID, string itemID, string itemDescription)
    {
        switch (sectionID)
        {
            case Util.ALGORITHM: Algorithm = itemID; break;
            case Util.TEACHING_MODE:
                TeachingMode = itemID;

                // Reset User test button (difficulty)
                if (TeachingMode == Util.DEMO && difficulty > 0)
                {
                    difficulty = 0;
                    InitButtonState(Util.DIFFICULTY, Util.DIFFICULTY, difficulty);
                }
                else if (TeachingMode == Util.USER_TEST) // Reset Demo button (speed)
                {
                    // Button gets reinitialized in method
                    AlgorithmSpeedLevel = 1;
                }

                break;

            case Util.DIFFICULTY:
                Difficulty = Util.difficultyConverterDict.FirstOrDefault(x => x.Value == itemID).Key;
                FillTooltips(FixNewLines(itemDescription), true);
                break;

            case Util.DEMO_SPEED:
                AlgorithmSpeedLevel = Util.algorithSpeedConverterDict.FirstOrDefault(x => x.Value == itemID).Key;
                break;

            case Util.OPTIONAL:
                switch (itemID)
                {
                    case Util.PSEUDOCODE_STEP:
                        bool stepActive = Util.ConvertStringToBool(itemDescription);
                        PseudocodeStep = stepActive;
                        if (stepActive)
                            FillTooltips("Pseudoline into more steps.", false);
                        else
                            FillTooltips("Pseudoline updates directly.", false);
                        break;

                    case Util.PSEUDOCODE_LINE_NR:
                        bool lineNrActive = Util.ConvertStringToBool(itemDescription);
                        PseudocodeLineNr = lineNrActive;
                        if (lineNrActive)
                            FillTooltips("Line nr will be displayed.", false);
                        else
                            FillTooltips("Line nr will not be displayed.", false);
                        break;

                    case Util.SCORE:
                        bool scoreActive = Util.ConvertStringToBool(itemDescription);
                        UserTestScore = scoreActive;
                        if (scoreActive)
                            FillTooltips("User test score enabled.", false);
                        else
                            FillTooltips("User test score disabled.", false);
                        break;

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
                case 0: algorithmSpeed = 3f; break;
                case 1: algorithmSpeed = 2f; break;
                case 2: algorithmSpeed = 1f; break;
                case 3: algorithmSpeed = 0.5f; break;
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

    public bool UserTestScore
    {
        get { return userTestScore; }
        set { userTestScore = value; }
    }


    // Instantiates the algorithm/task when the player has clicked the "ready" button
    public void InstantiateTask()
    {
        MainManager.InstantiateSafeStart();
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
            case Util.DEMO:
            case Util.STEP_BY_STEP:
                algorithmSpeed = AlgorithmSpeedLevel;
                break;

            case Util.USER_TEST:
                if (difficulty <= Util.PSEUDO_CODE_HIGHTLIGHT_MAX_DIFFICULTY)
                    AlgorithmSpeedLevel = 1; // Normal speed
                else
                    algorithmSpeed = 0f; //AlgorithmSpeedLevel = 3; // 0 Sec per step -- no "lag"
                break;
        }

        MainManager.StartAlgorithm();
    }

    // Stop task from in game
    private void StopTask()
    {
        MainManager.SafeStop();
    }

    // For listing items in the settings menu description (e.g. difficulty levels)
    private string FixNewLines(string itemDescription)
    {
        string[] headerBodySplit = itemDescription.Split(':');

        if (headerBodySplit.Length == 2)
        {
            string result = headerBodySplit[0]; // Header line

            string[] body = headerBodySplit[1].Split(','); // Body part

            foreach (string part in body)
            {
                result += "\n> " + part;
            }
            return result;
        }
        return itemDescription;
    }


    // ------------------------ Abstract methods ------------------------

    protected abstract MainManager MainManager { get; set; }
}
