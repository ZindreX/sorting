using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

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

    
    // ************** DEBUGGING ****************
    [SerializeField]
    private TeachingModeEditor teachingModeEditor;
    private enum TeachingModeEditor { tutorial, stepByStep, userTest }

    [SerializeField]
    private DifficultyEditor difficultyEditor;
    private enum DifficultyEditor { beginner, intermediate, examination }

    [SerializeField]
    private NumberofElementsEditor numberofElementsEditor;
    private enum NumberofElementsEditor { two, four, six, eight }

    [SerializeField]
    private bool allowDupEditor = false;

    [SerializeField]
    private CaseEditor sortingCaseEditor;
    private enum CaseEditor { none, best, worst }

    [SerializeField]
    private AlgorithmSpeed algorithmSpeed;
    private enum AlgorithmSpeed { slow, normal, fast }

    private void SettingsFromEditor()
    {
        switch ((int)teachingModeEditor)
        {
            case 0: teachingMode = Util.TUTORIAL; break;
            case 1: teachingMode = Util.STEP_BY_STEP; break;
            case 2: teachingMode = Util.USER_TEST; break;
        }

        switch ((int)difficultyEditor)
        {
            case 0: difficulty = Util.BEGINNER; break;
            case 1: difficulty = Util.INTERMEDIATE; break;
            case 2: difficulty = Util.EXAMINATION; break;
        }

        switch ((int)sortingCaseEditor)
        {
            case 0: sortingCase = Util.NONE; break;
            case 1: sortingCase = Util.BEST_CASE; break;
            case 2: sortingCase = Util.WORST_CASE; break;
        }

        switch ((int)numberofElementsEditor)
        {
            case 0: numberOfElements = 2; break;
            case 1: numberOfElements = 4; break;
            case 2: numberOfElements = 6; break;
            case 3: numberOfElements = 8; break;
        }

        switch ((int)algorithmSpeed)
        {
            case 0: algorithm.Seconds = 2f; break;
            case 1: algorithm.Seconds = 1f; break;
            case 2: algorithm.Seconds = 0.5f; break;
        }

        allowDuplicates = allowDupEditor;

        Debug.Log("Teachingmode: " + teachingMode + ", difficulty: " + difficulty + ", case: " + sortingCase + ", #: " + numberOfElements + ", allowdup: " + allowDuplicates);
    }
    // ********** DEBUGGING **************



    // Algorithm settings
    private int numberOfElements = 8;
    private string algorithmName, teachingMode = Util.TUTORIAL, difficulty = Util.BEGINNER, sortingCase = Util.NONE;
    private bool allowDuplicates = true, userStoppedAlgorithm = false, beginnerWait = false;
    private Vector3[] holderPositions;

    [SerializeField]
    private GameObject displayUnitManagerObj, teleportToSettings;

    // Base object instances
    protected DisplayUnitManager displayUnitManager;
    protected HolderManager holderManager;
    protected ElementManager elementManager;
    protected UserTestManager userTestManager;
    protected TutorialStep tutorialStep;
    protected Algorithm algorithm;

    protected virtual void Awake()
    {
        // *** Objects ***
        displayUnitManager = displayUnitManagerObj.GetComponent(typeof(DisplayUnitManager)) as DisplayUnitManager;

        holderManager = GetComponent(typeof(HolderManager)) as HolderManager;
        elementManager = GetComponent(typeof(ElementManager)) as ElementManager;
        userTestManager = GetComponent(typeof(UserTestManager)) as UserTestManager;
        tutorialStep = GetComponent(typeof(TutorialStep)) as TutorialStep;

        // Setup algorithm in their respective <Algorithm name>Manager
        algorithm = InstanceOfAlgorithm;
        algorithmName = algorithm.AlgorithmName;
        algorithm.PseudoCodeViewer = displayUnitManager.PseudoCodeViewer;
        algorithm.PseudoCodeViewerFixed = displayUnitManager.PseudoCodeViewerFixed;

        // Debugging
        SettingsFromEditor();
    }


    // Use this for initialization
    void Start()
    {
        displayUnitManager.BlackBoard.ChangeText(displayUnitManager.BlackBoard.TitleIndex, algorithmName);
        displayUnitManager.BlackBoard.ChangeText(displayUnitManager.BlackBoard.TextIndex, "");
    }

    // Update is called once per frame
    void Update()
    {
        // If the user clicked the stop button in game
        if (userStoppedAlgorithm)
            return;

        if (algorithm.IsSortingComplete)
        {
            if (IsUserTest() && userTestManager.TimeSpent == 0)
            {
                userTestManager.SetEndTime();
                userTestManager.CalculateScore();
                displayUnitManager.BlackBoard.ChangeText(displayUnitManager.BlackBoard.TextIndex, userTestManager.GetExaminationResult());
            }
            displayUnitManager.BlackBoard.ChangeText(displayUnitManager.BlackBoard.TitleIndex, "Sorting Completed!");
        }
        else
        {
            if (IsTutorialStep())
            {
                if (tutorialStep.PlayerMove && tutorialStep.IsValidStep)
                {
                    tutorialStep.PlayerMove = false;
                    InstructionBase instruction = tutorialStep.GetStep();

                    if (instruction.Instruction == Util.FIRST_INSTRUCTION)
                    {
                        tutorialStep.FirstInstruction = true;
                        algorithm.HighllightPseudoLine(algorithm.FirstInstructionCodeLine(), Util.HIGHLIGHT_COLOR);
                    }
                    else if (tutorialStep.FirstInstruction)
                    {
                        tutorialStep.FirstInstruction = false;
                        algorithm.HighllightPseudoLine(algorithm.FirstInstructionCodeLine(), Util.BLACKBOARD_TEXT_COLOR);
                    }

                    if (instruction.Instruction == Util.FINAL_INSTRUCTION)
                    {
                        tutorialStep.FinalInstruction = true;
                        algorithm.HighllightPseudoLine(algorithm.FinalInstructionCodeLine(), Util.HIGHLIGHT_COLOR);
                    }
                    else if (tutorialStep.FinalInstruction)
                    {
                        tutorialStep.FinalInstruction = false;
                        algorithm.HighllightPseudoLine(algorithm.FinalInstructionCodeLine(), Util.BLACKBOARD_TEXT_COLOR);
                    }

                    if (instruction.Instruction != Util.FIRST_INSTRUCTION && instruction.Instruction != Util.FINAL_INSTRUCTION)
                        algorithm.ExecuteStepByStepOrder(instruction, tutorialStep.CurrentInstructionNr, tutorialStep.PlayerIncremented);
                }
            }
            else if (IsUserTest()) // User test
            {
                //if (userTestReady) // debugging
                //{
                //    string holderIDTest = "";
                //    for (int x = 0; x < elementManager.SortingElements.Length; x++)
                //    {
                //        SortingElementBase s = elementManager.GetSortingElement(x).GetComponent<SortingElementBase>();
                //        holderIDTest += "[" + s.Value + "|" + ((InsertionSortInstruction)s.Instruction).HolderID + "], ";
                //    }
                //    displayUnitManager.BlackBoard.ChangeText(textIndex, holderIDTest);
                //}

                // First check if user test setup is complete
                if (userTestManager.HasInstructions() && !beginnerWait)
                {
                    // Check if user has done a move, and is ready for next round
                    if (elementManager.CurrentMoving != null)
                    {
                        // Dont do anything while moving element
                    }
                    else if (userTestManager.ReadyForNext == userTestManager.UserActionToProceed)
                    {
                        // Reset counter
                        userTestManager.ReadyForNext = 0;

                        // Checking if all sorting elements are sorted
                        if (!userTestManager.HasInstructions() && elementManager.AllSorted())
                        {
                            algorithm.IsSortingComplete = true;
                            Debug.LogError("Any state where it's entering here???"); // ???
                        }
                        else
                        {
                            // Still some elements not sorted, so go on to next round
                            bool hasInstruction = userTestManager.IncrementToNextInstruction();

                            // Hot fix - solve in some other way?
                            if (hasInstruction)
                                userTestManager.ReadyForNext += PrepareNextInstruction(userTestManager.GetInstruction());
                            else if (elementManager.AllSorted())
                                StartCoroutine(FinishUserTest());

                        }
                    }
                    displayUnitManager.BlackBoard.ChangeText(displayUnitManager.BlackBoard.TextIndex, userTestManager.FillInBlackboard());
                }
            }
        }
    }

    /* --------------------------------------- Instatiate Setup ---------------------------------------
     * > Called from UserController
     * > Creates the holders and the sorting elements
    */
    public void InstantiateSetup()
    {
        holderManager.CreateObjects(NumberOfElements, null);
        HolderPositions = holderManager.GetHolderPositions();
        //elementManager.CreateObjects(NumberOfElements, HolderPositions);
        elementManager.CreateObjects(NumberOfElements, HolderPositions, allowDuplicates, sortingCase);

        // Display on blackboard
        displayUnitManager.BlackBoard.ChangeText(displayUnitManager.BlackBoard.TitleIndex, algorithmName);
        displayUnitManager.BlackBoard.ChangeText(displayUnitManager.BlackBoard.TextIndex, teachingMode);
        
        switch (teachingMode)
        {
            case Util.TUTORIAL: PerformAlgorithmTutorial(); break;
            case Util.STEP_BY_STEP: PerformAlgorithmTutorialStep(); break;
            case Util.USER_TEST: PerformAlgorithmUserTest(); break;
        }
        userStoppedAlgorithm = false;
        teleportToSettings.GetComponent<TeleportPoint>().markerActive = false;
    }

    /* --------------------------------------- Destroy & Restart ---------------------------------------
     * > Called from UserController
     * > Destroys all the gameobjects
     */
    public void DestroyAndReset()
    {
        userStoppedAlgorithm = true;
        holderManager.DestroyObjects();
        elementManager.DestroyObjects();
        if (algorithm.IsSortingComplete)
            algorithm.IsSortingComplete = false;

        algorithm.ResetSetup();
        displayUnitManager.ResetDisplays();
        teleportToSettings.GetComponent<TeleportPoint>().markerActive = true;
    }

    // --------------------------------------- Getters and setters ---------------------------------------

    public Algorithm Algorithm
    {
        get { return algorithm; }
    }

    // Number of elements used
    public int NumberOfElements
    {
        get { return numberOfElements; }
        set { numberOfElements = value; displayUnitManager.BlackBoard.ChangeText(displayUnitManager.BlackBoard.TextIndex, "Number of elements: " + value); }
    }

    // The positions of the holders
    public Vector3[] HolderPositions
    {
        get { return holderPositions; }
        set { holderPositions = value; }
    }

    // A boolean used to wait entering the update cycle while beginner's help (pseudocode) is being written
    public bool BeginnerWait
    {
        get { return beginnerWait; }
        set { beginnerWait = value; }
    }

    // Tutorial, Step-By-Step, or User Test
    public string TeachingMode
    {
        get { return teachingMode; }
        set { teachingMode = value; displayUnitManager.BlackBoard.ChangeText(displayUnitManager.BlackBoard.TextIndex, "Teaching mode: " + value); }
    }

    // Beginner, Intermediate, or Examination
    public string Difficulty
    {
        get { return difficulty; }
        set { difficulty = value; displayUnitManager.BlackBoard.ChangeText(displayUnitManager.BlackBoard.TextIndex, "Difficulty: " + value); }
    }

    // None, Best-case, Worst-case (not implemented yet)
    public string SortingCase
    {
        set { sortingCase = value; displayUnitManager.BlackBoard.ChangeText(displayUnitManager.BlackBoard.TextIndex, "Case activated: " + value); }
    }

    // Duplicates can occour in the problem sets (not implemented yet)
    public bool Duplicates
    {
        set { allowDuplicates = value; displayUnitManager.BlackBoard.ChangeText(displayUnitManager.BlackBoard.TextIndex, "Duplicates: " + Util.EnabledToString(value)); }
    }

    // Returns the holder (might change, since insertion sort is the only with some modifications) ***
    public virtual HolderBase GetCorrectHolder(int index)
    {
        return holderManager.GetHolder(index);
    }

    // Input from user during Step-By-Step (increment/decrement)
    public void PlayerStepByStepInput(bool increment)
    {
        tutorialStep.NotifyUserInput(increment);
    }

    // Check if it's a Tutorial (including stepbystep for simplicity, might fix this later)
    public bool IsTutorial()
    {
        return teachingMode == Util.TUTORIAL || teachingMode == Util.STEP_BY_STEP;
    }

    // Check if it's StepByStep (not used?)
    public bool IsTutorialStep()
    {
        return teachingMode == Util.STEP_BY_STEP;
    }

    // Check if it's UserTest
    public bool IsUserTest()
    {
        return teachingMode == Util.USER_TEST;
    }

    /* --------------------------------------- Tutorial ---------------------------------------
     * - Gives a visual presentation of <sorting algorithm>
     * - Player can't interact with sorting elements (*fix)
    */
    public void PerformAlgorithmTutorial()
    {
        Debug.Log(">>> Performing " + algorithmName + " tutorial.");
        elementManager.InteractionWithSortingElements(false);
        StartCoroutine(algorithm.Tutorial(elementManager.SortingElements));
    }

    /* --------------------------------------- Step-By-Step ---------------------------------------
     * - Gives a visual presentation of <sorting algorithm>
     * - Player can't directly interact with sorting elements, but can use controllers to progress
     *   one step of a time / or back
    */
    public void PerformAlgorithmTutorialStep()
    {
        // Getting instructions for this sample of sorting elements
        elementManager.InteractionWithSortingElements(false);
        tutorialStep.Init(algorithm.UserTestInstructions(CopyFirstState(elementManager.SortingElements)));
    }

    /* --------------------------------------- User Test ---------------------------------------
     * - Gives a visual presentation of elements used in <sorting algorithm>
     * - Player needs to interact with the sorting elements to progress through the algorithm
    */
    public void PerformAlgorithmUserTest()
    {
        Debug.Log(">>> Performing " + algorithmName + " user test.");
        elementManager.InteractionWithSortingElements(true);
        
        // Getting instructions for this sample of sorting elements
        Dictionary<int, InstructionBase> instructions = algorithm.UserTestInstructions(CopyFirstState(elementManager.SortingElements));

        // Initialize user test
        userTestManager.InitUserTest(instructions, MovesNeeded, FindNumberOfUserAction(instructions));

        // Set start time
        userTestManager.SetStartTime();

        //DebugCheckInstructions(instructions); // Debugging

        userTestReady = true; // debugging
    }
    private bool userTestReady = false; // debugging

    //
    private IEnumerator FinishUserTest()
    {
        yield return new WaitForSeconds(algorithm.Seconds);
        algorithm.IsSortingComplete = true;
        displayUnitManager.PseudoCodeViewer.RemoveHightlight();
        for (int x = 0; x < numberOfElements; x++)
        {
            Util.IndicateElement(elementManager.GetSortingElement(x));
            elementManager.GetSortingElement(x).transform.rotation = Quaternion.identity;
            yield return new WaitForSeconds(algorithm.Seconds / 2);
        }
    }

    // Finds the number of instructions which the player has to do something to progress0 5   
    private int FindNumberOfUserAction(Dictionary<int, InstructionBase> instructions)
    {
        int count = 0;
        for (int x=0; x < instructions.Count; x++)
        {
            if (algorithm.SkipDict.ContainsKey(Util.SKIP_NO_ELEMENT) && algorithm.SkipDict.ContainsKey(Util.SKIP_NO_DESTINATION)) {
                if (!algorithm.SkipDict[Util.SKIP_NO_ELEMENT].Contains(instructions[x].Instruction) && !algorithm.SkipDict[Util.SKIP_NO_DESTINATION].Contains(instructions[x].Instruction))
                    count++;
            }
        }
        return count;
    }


    // --------------------------------------- To be implemented in subclasses ---------------------------------------

    // Returns the instance of the algorithm being runned 
    protected abstract Algorithm InstanceOfAlgorithm { get; }

    // Moves needed to progress to next instruction
    protected abstract int MovesNeeded { get; }

    /* Prepares the next instruction based on the algorithm being runned
     * - Sends instruction to the next sorting element the user should move
     * - Beginners (difficulty) will be shown steps on pseudoboard and given some hints
     * - Skips instructions which doesn't contain any elements nor destination
    */
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
