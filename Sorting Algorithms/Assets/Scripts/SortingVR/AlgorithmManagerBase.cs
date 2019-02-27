using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(HolderManager))]
[RequireComponent(typeof(ElementManager))]
[RequireComponent(typeof(UserTestManager))]
[RequireComponent(typeof(StepByStepManager))]
[RequireComponent(typeof(SortAlgorithm))]
[RequireComponent(typeof(DisplayUnitManager))]
public abstract class AlgorithmManagerBase : MainManager {

    /* -------------------------------------------- Sorting Algorithm Manager Base ----------------------------------------------------
     * 
     * 
    */

    private Vector3[] holderPositions;

    [SerializeField]
    private GameObject displayUnitManagerObj, settingsObj;


    // Base object instances
    protected DisplayUnitManager displayUnitManager;
    protected HolderManager holderManager;
    protected ElementManager elementManager;
    protected UserTestManager userTestManager;
    protected StepByStepManager tutorialStep;
    protected SortAlgorithm sortAlgorithm;
    protected AlgorithmSettings algorithmSettings;

    [SerializeField]
    protected AlgorithmUserController algorithmUserController;

    protected virtual void Awake()
    {
        // *** Objects ***
        algorithmSettings = settingsObj.GetComponent(typeof(AlgorithmSettings)) as AlgorithmSettings;
        displayUnitManager = displayUnitManagerObj.GetComponent(typeof(DisplayUnitManager)) as DisplayUnitManager;

        holderManager = GetComponent(typeof(HolderManager)) as HolderManager;
        elementManager = GetComponent(typeof(ElementManager)) as ElementManager;
        userTestManager = GetComponent(typeof(UserTestManager)) as UserTestManager;
        tutorialStep = GetComponent(typeof(StepByStepManager)) as StepByStepManager;

        // Setup algorithm in their respective <Algorithm name>Manager
        teachingAlgorithm = InstanceOfAlgorithm;
        sortAlgorithm = InstanceOfAlgorithm;
        algorithmName = sortAlgorithm.AlgorithmName;

        // Set displays
        sortAlgorithm.PseudoCodeViewer = displayUnitManager.PseudoCodeViewer;
        displayUnitManager.SetAlgorithmForPseudo(sortAlgorithm);
    }


    // Use this for initialization
    void Start()
    {
        // Right blackboard title / text
        displayUnitManager.BlackBoard.ChangeText(displayUnitManager.BlackBoard.TitleIndex, algorithmName);
        displayUnitManager.BlackBoard.ChangeText(displayUnitManager.BlackBoard.TextIndex, "Teaching mode: " + algorithmSettings.TeachingMode);
    }

    // Update is called once per frame
    void Update()
    {
        // If the user clicked the stop button in game
        if (userStoppedAlgorithm)
            return;

        if (sortAlgorithm.IsTaskCompleted)
        {
            if (algorithmSettings.IsUserTest() && userTestManager.TimeSpent == 0)
            {
                userTestManager.SetEndTime();
                userTestManager.CalculateScore();
                displayUnitManager.BlackBoard.ChangeText(displayUnitManager.BlackBoard.TextIndex, userTestManager.GetExaminationResult());
            }
            displayUnitManager.BlackBoard.ChangeText(displayUnitManager.BlackBoard.TitleIndex, "Sorting Completed!");
        }
        else
        {
            if (algorithmSettings.IsStepByStep())
            {
                if (tutorialStep.PlayerMove && tutorialStep.IsValidStep)
                {
                    tutorialStep.PlayerMove = false;
                    InstructionBase instruction = tutorialStep.GetStep();
                    Debug.Log(">>> " + instruction.Instruction);
                    //Debug.Log("InstructionNr.: " + instruction.INSTRUCION_NR);
                    //Debug.Log(tutorialStep.CurrentInstructionNr);


                    bool gotSortingElement = !sortAlgorithm.SkipDict[UtilSort.SKIP_NO_ELEMENT].Contains(instruction.Instruction);
                    sortAlgorithm.ExecuteStepByStepOrder(instruction, gotSortingElement, tutorialStep.PlayerIncremented);
                }
            }
            else if (algorithmSettings.IsUserTest()) // User test
            {
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
                            sortAlgorithm.IsTaskCompleted = true;
                            Debug.LogError("Manage to enter this case???"); // ???
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
        holderManager.CreateObjects(algorithmSettings.NumberOfElements, null);
        HolderPositions = holderManager.GetHolderPositions();
        //elementManager.CreateObjects(NumberOfElements, HolderPositions);
        elementManager.CreateObjects(algorithmSettings.NumberOfElements, HolderPositions, algorithmSettings.Duplicates, algorithmSettings.SortingCase);

        // Display on blackboard
        displayUnitManager.BlackBoard.ChangeText(displayUnitManager.BlackBoard.TitleIndex, algorithmName);
        displayUnitManager.BlackBoard.ChangeText(displayUnitManager.BlackBoard.TextIndex, algorithmSettings.TeachingMode);

        // Display pseudocode
        if (algorithmSettings.TeachingMode == UtilSort.USER_TEST)
        {
            if (algorithmSettings.Difficulty <= UtilSort.INTERMEDIATE)
            {
                displayUnitManager.PseudoCodeViewer.PseudoCodeSetup();
                //displayUnitManager.PseudoCodeViewerFixed.PseudoCodeSetup();
            }
            else if (algorithmSettings.Difficulty == UtilSort.ADVANCED)
            {
                //isplayUnitManager.PseudoCodeViewer.PseudoCodeSetup();
                // Ideas for left/center blackboard?
            }
            else
            {
                // Ideas for left/center? (Examination)
            }
        }
        else
        {
            displayUnitManager.PseudoCodeViewer.PseudoCodeSetup();
            //displayUnitManager.PseudoCodeViewerFixed.PseudoCodeSetup();
        }

        switch (algorithmSettings.TeachingMode)
        {
            case UtilSort.DEMO: PerformAlgorithmDemo(); break;
            case UtilSort.STEP_BY_STEP: PerformAlgorithmStepByStep(); break;
            case UtilSort.USER_TEST: PerformAlgorithmUserTest(); break;
        }
        userStoppedAlgorithm = false;
        settingsObj.SetActive(false);
        controllerReady = true;

    }

    /* --------------------------------------- Destroy & Restart ---------------------------------------
     * > Called from UserController
     * > Destroys all the gameobjects
     */
    public void DestroyAndReset()
    {
        if (algorithmSettings.IsDemo() && !algorithmSettings.IsStepByStep() && !sortAlgorithm.IsTaskCompleted)
        {
            StartCoroutine(algorithmUserController.CreateWarningMessage("Can't stop during demo. See blackboard for progress.", UtilSort.ERROR_COLOR));
        }
        else
        {
            userStoppedAlgorithm = true;
            holderManager.DestroyObjects();
            elementManager.DestroyObjects();
            if (sortAlgorithm.IsTaskCompleted)
                sortAlgorithm.IsTaskCompleted = false;

            sortAlgorithm.ResetSetup();
            displayUnitManager.ResetDisplays();
            settingsObj.SetActive(true);
            controllerReady = false;
        }

        // Cleanup pseudocode
        //algorithm.PseudoCodeViewer.DestroyPseudoCode();
    }

    // --------------------------------------- Getters and setters ---------------------------------------

    public SortAlgorithm Algorithm
    {
        get { return sortAlgorithm; }
    }

    public AlgorithmSettings AlgorithmSettings
    {
        get { return algorithmSettings; }
    }

    // The positions of the holders
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

    // Input from user during Step-By-Step (increment/decrement)
    public void PlayerStepByStepInput(bool increment)
    {
        if (ControllerReady)
            tutorialStep.NotifyUserInput(increment);
    }



    /* --------------------------------------- Demo ---------------------------------------
     * - Gives a visual presentation of <sorting algorithm>
     * - Player can't interact with sorting elements (*fix)
    */
    public override void PerformAlgorithmDemo()
    {
        Debug.Log(">>> Performing " + algorithmName + " tutorial.");
        elementManager.InteractionWithSortingElements(false);
        StartCoroutine(sortAlgorithm.Demo(elementManager.SortingElements));
    }

    /* --------------------------------------- Step-By-Step ---------------------------------------
     * - Gives a visual presentation of <sorting algorithm>
     * - Player can't directly interact with sorting elements, but can use controllers to progress
     *   one step of a time / or back
    */
    public override void PerformAlgorithmStepByStep()
    {
        // Getting instructions for this sample of sorting elements
        elementManager.InteractionWithSortingElements(false);
        tutorialStep.Init(sortAlgorithm.UserTestInstructions(CopyFirstState(elementManager.SortingElements)));
    }

    /* --------------------------------------- User Test ---------------------------------------
     * - Gives a visual presentation of elements used in <sorting algorithm>
     * - Player needs to interact with the sorting elements to progress through the algorithm
    */
    public override void PerformAlgorithmUserTest()
    {
        Debug.Log(">>> Performing " + algorithmName + " user test.");
        elementManager.InteractionWithSortingElements(true);
        
        // Getting instructions for this sample of sorting elements
        Dictionary<int, InstructionBase> instructions = sortAlgorithm.UserTestInstructions(CopyFirstState(elementManager.SortingElements));

        // Initialize user test
        userTestManager.InitUserTest(instructions, MovesNeeded, FindNumberOfUserAction(instructions));

        // Set start time
        userTestManager.SetStartTime();

        //DebugCheckInstructions(instructions); // Debugging

        userTestReady = true; // debugging
    }

    // Give feedback to the user when the sorting task (user test) is completed)
    private IEnumerator FinishUserTest()
    {
        yield return new WaitForSeconds(sortAlgorithm.Seconds);
        sortAlgorithm.IsTaskCompleted = true;
        displayUnitManager.PseudoCodeViewer.RemoveHightlight();

        // Visual feedback (make each element "jump")
        for (int x = 0; x < algorithmSettings.NumberOfElements; x++)
        {
            UtilSort.IndicateElement(elementManager.GetSortingElement(x));
            elementManager.GetSortingElement(x).transform.rotation = Quaternion.identity;
            yield return new WaitForSeconds(sortAlgorithm.Seconds / 2);
        }
    }

    // --------------------------------------- To be implemented in subclasses ---------------------------------------

    // Returns the instance of the algorithm being runned 
    protected abstract SortAlgorithm InstanceOfAlgorithm { get; }

    // Moves needed to progress to next instruction
    protected abstract int MovesNeeded { get; }

    // Copies the first state of sorting elements into instruction, which can be used when creating instructions for user test
    protected abstract InstructionBase[] CopyFirstState(GameObject[] sortingElements);
}
