using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HolderManager))]
[RequireComponent(typeof(ElementManager))]
[RequireComponent(typeof(UserTestManager))]
[RequireComponent(typeof(TutorialStep))]
[RequireComponent(typeof(Algorithm))]
[RequireComponent(typeof(DisplayUnitManager))]
public abstract class AlgorithmManagerBase : MonoBehaviour {

    /* -------------------------------------------- The main unit for the algorithm ----------------------------------------------------
     * >>> Status of algorithms:
     * ---------------------------------------------------------------------------------------------------------------------------------
     *    Algorithm name    |   Standard    |   Tutorial    |   User Test   |                   Comment
     * ---------------------------------------------------------------------------------------------------------------------------------
     *    Bubble Sort       |     Yes       |     Yes       |       Yes     | Complete, but needs more testing for bugs
     *    Insertion Sort    |     Yes       |     Yes       |       Yes     | Complete, but needs more testing for bugs
     *    Merge Sort        |     Yes       |     No        |       No      | Tutorial not completed yet, user test not started
     *    Quick Sort        |      No       |     No        |       No      | -
     *    Bucket Sort       |     Yes       |     Yes*      |       No      | Implement user test + get stuff up on the blackboard
     * ---------------------------------------------------------------------------------------------------------------------------------
     * 
    */

    // Algorithm settings
    [SerializeField]
    private int numberOfElements = 8;

    // Settings
    //[SerializeField]
    //private bool isTutorial = true;
    [SerializeField]
    private bool helpEnabled = false, noDuplicates = false, worstCase = false, bestCase = false;


    // Drop down in editor for teaching mode
    [SerializeField]
    private TeachingMode teachingMode;
    private enum TeachingMode
    {
        Tutorial,
        TutorialStep,
        UserTest
    };

    // Drop down in editor for difficulty
    [SerializeField]
    private DifficultyLevel difficultyLevel;
    private enum DifficultyLevel
    {
        Beginner,
        Intermediate,
        Examination,
    }

    private string algorithmName, difficultyLevelString, teachingModeString;
    private string[] rules = new string[Util.NUMBER_OF_RULE_TYPES];
    private int titleIndex = 0, textIndex = 1;

    private Vector3[] holderPositions;

    [SerializeField]
    private GameObject displayUnitManagerObj;

    // Base object instances
    protected DisplayUnitManager displayUnitManager;
    protected HolderManager holderManager;
    protected ElementManager elementManager;
    protected UserTestManager userTestManager;
    protected TutorialStep tutorialStep;
    protected Algorithm algorithm;

    protected virtual void Awake()
    {
        if (IsValidSetup())
        {
            // *** Mode and difficulty *** (drop down menu in editor)
            teachingModeString = ConvertMode(teachingMode);
            difficultyLevelString = ConvertDifficulty(difficultyLevel);

            // ***  Rules ***
            InitRules();

            // *** Objects ***
            displayUnitManager = displayUnitManagerObj.GetComponent(typeof(DisplayUnitManager)) as DisplayUnitManager;

            holderManager = GetComponent(typeof(HolderManager)) as HolderManager;
            elementManager = GetComponent(typeof(ElementManager)) as ElementManager;
            userTestManager = GetComponent(typeof(UserTestManager)) as UserTestManager;
            tutorialStep = GetComponent(typeof(TutorialStep)) as TutorialStep;

            // Setup algorithm in their respective <Algorithm name>Manager
            algorithm = InstanceOfAlgorithm;
            algorithmName = algorithm.GetAlgorithmName();
            algorithm.PseudoCodeViewer = displayUnitManager.PseudoCodeViewer;
            algorithm.PseudoCodeViewerFixed = displayUnitManager.PseudoCodeViewerFixed;

            SetAboveHolderForTeachingMode();
        }
    }


    // Use this for initialization
    void Start()
    {
        displayUnitManager.BlackBoard.ChangeText(titleIndex, algorithmName);
        displayUnitManager.BlackBoard.ChangeText(textIndex, teachingModeString);
    }

    // Update is called once per frame
    void Update()
    {
        if (algorithm.IsSortingComplete)
        {
            if (IsUserTest() && userTestManager.TimeSpent == 0)
            {
                userTestManager.SetEndTime();
                userTestManager.CalculateScore();
                displayUnitManager.BlackBoard.ChangeText(textIndex, userTestManager.GetExaminationResult());
            }
            displayUnitManager.BlackBoard.ChangeText(titleIndex, "Sorting Completed!");
        }
        else
        {
            if (IsTutorialStep())
            {
                if (tutorialStep.PlayerMove && tutorialStep.IsValidStep)
                {
                    tutorialStep.PlayerMove = false;
                    InstructionBase instruction = tutorialStep.GetStep();

                    if (instruction.ElementInstruction == Util.FIRST_INSTRUCTION)
                    {
                        tutorialStep.FirstInstruction = true;
                        algorithm.HighllightPseudoLine(algorithm.FirstInstructionCodeLine(), Util.HIGHLIGHT_COLOR);
                    }
                    else if (tutorialStep.FirstInstruction)
                    {
                        tutorialStep.FirstInstruction = false;
                        algorithm.HighllightPseudoLine(algorithm.FirstInstructionCodeLine(), Util.BLACKBOARD_TEXT_COLOR);
                    }

                    if (instruction.ElementInstruction == Util.FINAL_INSTRUCTION)
                    {
                        tutorialStep.FinalInstruction = true;
                        algorithm.HighllightPseudoLine(algorithm.FinalInstructionCodeLine(), Util.HIGHLIGHT_COLOR);
                    }
                    else if (tutorialStep.FinalInstruction)
                    {
                        tutorialStep.FinalInstruction = false;
                        algorithm.HighllightPseudoLine(algorithm.FinalInstructionCodeLine(), Util.BLACKBOARD_TEXT_COLOR);
                    }

                    if (instruction.ElementInstruction != Util.FIRST_INSTRUCTION && instruction.ElementInstruction != Util.FINAL_INSTRUCTION)
                        algorithm.ExecuteOrder(instruction, tutorialStep.CurrentInstructionNr, tutorialStep.PlayerIncremented);
                }
            }
            else if (IsTutorial())
            {
                //blackboard.SetResultText(algorithm.GetComparison());
            }
            else // User test
            {
                // First check if user test setup is complete
                if (userTestManager.HasInstructions())
                {
                    // Check if user has done a move, and is ready for next round
                    if (elementManager.CurrentMoving != null)
                    {
                        // Dont do anything while moving element
                    }
                    else if (userTestManager.ReadyForNext == userTestManager.UserActionToProceed)
                    {
                        // Reset
                        userTestManager.ReadyForNext = 0;

                        // Checking if all sorting elements are sorted
                        if (elementManager.AllSorted())
                            algorithm.IsSortingComplete = true;
                        else
                        {
                            // Still some elements not sorted, so go on to next round
                            userTestManager.IncrementToNextInstruction();
                            userTestManager.ReadyForNext += PrepareNextInstruction(userTestManager.GetInstruction());
                        }
                    }
                    displayUnitManager.BlackBoard.ChangeText(textIndex, userTestManager.FillInBlackboard());
                }
            }
        }
    }

    // Check if valid is possible to perform - TODO: Fix elsewhere so this isn't needed
    private bool IsValidSetup()
    {
        return (!worstCase && !bestCase) || (worstCase != bestCase);
    }

    /* --------------------------------------- Creation with rules ---------------------------------------
     * rules[0]: duplicates etc.
     * rules[1]: worst-/bestcase
    */
    private void InitRules()
    {
        if (noDuplicates)
            rules[0] = Util.DUPLICATES;

        if (worstCase)
            rules[1] = Util.WORST_CASE;
        else if (bestCase)
            rules[1] = Util.BEST_CASE;
    }

    /* --------------------------------------- Instatiate Setup ---------------------------------------
     * > Called from UserController
     * > Creates the holders and the sorting elements
    */
    public void InstantiateSetup()
    {
        holderManager.CreateObjects(numberOfElements, null);
        HolderPositions = holderManager.GetHolderPositions();
        elementManager.CreateObjects(numberOfElements, HolderPositions);
        // Disable Drag if tutorial
        if (IsTutorial())
        {
            for (int x = 0; x < elementManager.SortingElements.Length; x++)
            {
                elementManager.GetSortingElement(x).GetComponent<Drag>().enabled = false; // not working
            }
        }
    }

    /* --------------------------------------- Destroy & Restart ---------------------------------------
     * > Called from UserController
     * > Destroys all the gameobjects
     */
    public void DestroyAndReset()
    {
        holderManager.DestroyObjects();
        elementManager.DestroyObjects();
        if (algorithm.IsSortingComplete)
            algorithm.IsSortingComplete = false;

        algorithm.ResetSetup();
    }

    // --------------------------------------- Getters and setters ---------------------------------------

    protected string GetAlgorithmName
    {
        get { return algorithm.GetAlgorithmName(); }
    }

    public int NumberOfElements
    {
        get { return numberOfElements; }
    }

    public bool HelpEnabled
    {
        get { return helpEnabled; }
    }

    public Vector3[] HolderPositions
    {
        get { return holderPositions; }
        set { holderPositions = value; }
    }

    // Returns the holder (might change, since insertion sort is the only with some modifications) ***
    public virtual HolderBase GetCorrectHolder(int index)
    {
        return holderManager.GetHolder(index);
    }

    public void PlayerStepByStepInput(bool increment)
    {
        tutorialStep.NotifyUserInput(increment);
    }

    public void SetDifficulty(string difficultyLevel)
    {
        difficultyLevelString = difficultyLevel;
        userTestManager.DifficultyLevel = difficultyLevel;
        switch (difficultyLevel)
        {
            case Util.BEGINNER: userTestManager.DifficultyMultiplier = 1; break;
            case Util.INTERMEDIATE: userTestManager.DifficultyMultiplier = 2; break;
            case Util.EXAMINATION: userTestManager.DifficultyMultiplier = 3; break;
            default: Debug.LogError("Difficulty '" + difficultyLevel + "' not implemented."); break;
        }
    }

    private string ConvertMode(TeachingMode value)
    {
        switch ((int)value)
        {
            case 0: return Util.TUTORIAL;
            case 1: return Util.TUTORIAL_STEP;
            case 2: return Util.USER_TEST;
            default: return "Cannot convert '" + value + "'";
        }
    }

    private TeachingMode ConvertMode(string value)
    {
        switch (value)
        {
            case Util.TUTORIAL: return TeachingMode.Tutorial;
            case Util.TUTORIAL_STEP: return TeachingMode.TutorialStep;
            case Util.USER_TEST: return TeachingMode.UserTest;
            default:
                Debug.Log("Value '" + value + "' not valid, setting to Tutorial");
                return TeachingMode.Tutorial;
        }
    }

    private string ConvertDifficulty(DifficultyLevel value)
    {
        switch ((int)value)
        {
            case 0: return Util.BEGINNER;
            case 1: return Util.INTERMEDIATE;
            case 2: return Util.EXAMINATION;
            default: return "Cannot convert '" + value + "'";
        }
    }

    private DifficultyLevel ConvertDifficulty(string value)
    {
        switch (value)
        {
            case Util.BEGINNER: return DifficultyLevel.Beginner;
            case Util.INTERMEDIATE: return DifficultyLevel.Intermediate;
            case Util.EXAMINATION: return DifficultyLevel.Examination;
            default:
                Debug.Log("Value not valid, setting to beginner level");
                return DifficultyLevel.Beginner;
        }
    }

    // **

    public void SetAboveHolderForTeachingMode()
    {
        switch (teachingModeString)
        {
            case Util.TUTORIAL: algorithm.AboveHolder = new Vector3(0f, 0.1f, 0f); break;
            case Util.TUTORIAL_STEP: algorithm.AboveHolder = new Vector3(0f, 0.5f, 0f); break;
            case Util.USER_TEST: algorithm.AboveHolder = new Vector3(0f, 0.5f, 0f); break;
            default: Debug.Log("Teaching mode '" + teachingMode + "' hasnt't defined aboveHolder variable"); break;
        }
    }

    public bool IsTutorial()
    {
        return teachingModeString == Util.TUTORIAL || teachingModeString == Util.TUTORIAL_STEP;
    }

    public bool IsTutorialStep()
    {
        return teachingModeString == Util.TUTORIAL_STEP;
    }

    public bool IsUserTest()
    {
        return teachingModeString == Util.USER_TEST;
    }




    /* --------------------------------------- Tutorial ---------------------------------------
     * 
     * 
    */
    public void PerformAlgorithmTutorial()
    {
        Debug.Log(">>> Performing " + algorithmName + " tutorial.");
        StartCoroutine(algorithm.Tutorial(elementManager.SortingElements));
    }

    public void PerformAlgorithmTutorialStep()
    {
        // Getting instructions for this sample of sorting elements
        tutorialStep.Init(algorithm.UserTestInstructions(CopyFirstState(elementManager.SortingElements)));
    }

    /* --------------------------------------- User Test ---------------------------------------
     * 
     * 
    */
    public void PerformAlgorithmUserTest()
    {
        Debug.Log(">>> Performing " + algorithmName + " user test.");
        
        // Getting instructions for this sample of sorting elements
        Dictionary<int, InstructionBase> instructions = algorithm.UserTestInstructions(CopyFirstState(elementManager.SortingElements));

        // Initialize user test
        userTestManager.InitUserTest(instructions, MovesNeeded, FindNumberOfUserAction(instructions));

        // Set start time
        userTestManager.SetStartTime();

        //DebugCheckInstructions(instructions); // Debugging
    }

    private int FindNumberOfUserAction(Dictionary<int, InstructionBase> instructions)
    {
        int count = 0;
        for (int x=0; x < instructions.Count; x++)
        {
            if (!algorithm.SkipAbleInstructions.Contains(instructions[x].ElementInstruction))
                count++;
        }
        return count;
    }


    // --------------------------------------- To be implemented in subclasses ---------------------------------------

    // Returns the instance of the algorithm being runned 
    protected abstract Algorithm InstanceOfAlgorithm { get; }

    // Moves needed to progress to next instruction
    protected abstract int MovesNeeded { get; }

    // Prepares the next instruction based on the algorithm being runned
    protected abstract int PrepareNextInstruction(InstructionBase instruction);

    // Copies the first state of sorting elements into instruction, which can be used when creating instructions for user test
    protected abstract InstructionBase[] CopyFirstState(GameObject[] sortingElements);



    // --------------------------------------- Debugging ---------------------------------------

    private void DebugCheckInstructions(Dictionary<int, InstructionBase> dict)
    {
        for (int x = 0; x < dict.Count; x++)
        {
            Debug.Log(dict[x].DebugInfo());
        }
        Debug.Log("");
    }

    private string DebugCheckGameObjects(GameObject[] list)
    {
        string result = "";
        foreach (GameObject obj in list)
        {
            result += obj.GetComponent<SortingElementBase>().Value + ", ";
        }
        return result;
    }
}
