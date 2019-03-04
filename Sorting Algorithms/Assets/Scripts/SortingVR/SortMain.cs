using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortMain : MainManager {

    // Base object instances
    [SerializeField]
    private SortSettings sortSettings;
    private SortAlgorithm sortAlgorithm;

    // Managers
    private AlgorithmManagerBase algorithmManagerBase;
    private ElementManager elementManager;
    private HolderManager holderManager;

    // Visual
    protected DisplayUnitManager displayUnitManager;

    // Teaching modes
    protected StepByStepManager tutorialStep;
    protected UserTestManager userTestManager;

    [SerializeField]
    protected AlgorithmUserController algorithmUserController;

    [SerializeField]
    private GameObject sortAlgorithmsObj, displayUnitManagerObj, sortingTableObj;

    private Vector3[] holderPositions;

    private void Awake()
    {
        // >>> Objects
        holderManager = GetComponent(typeof(HolderManager)) as HolderManager;
        elementManager = GetComponent(typeof(ElementManager)) as ElementManager;
        tutorialStep = GetComponent(typeof(StepByStepManager)) as StepByStepManager;
        userTestManager = GetComponent(typeof(UserTestManager)) as UserTestManager;
        displayUnitManager = displayUnitManagerObj.GetComponent(typeof(DisplayUnitManager)) as DisplayUnitManager;
    }

    protected override void DemoUpdate()
    {

    }

    protected override void StepByStepUpdate()
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

    protected override void UserTestUpdate()
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
                        userTestManager.ReadyForNext += algorithmManagerBase.PrepareNextInstruction(userTestManager.GetInstruction());
                    else if (elementManager.AllSorted())
                        StartCoroutine(FinishUserTest());

                }
            }
            displayUnitManager.BlackBoard.ChangeText(displayUnitManager.BlackBoard.TextIndex, userTestManager.FillInBlackboard());
        }
    }

    protected override void TaskCompletedFinishOff()
    {
        if (sortSettings.IsUserTest() && userTestManager.TimeSpent == 0)
        {
            userTestManager.SetEndTime();
            userTestManager.CalculateScore();
            displayUnitManager.BlackBoard.ChangeText(displayUnitManager.BlackBoard.TextIndex, userTestManager.GetExaminationResult());
        }
        displayUnitManager.BlackBoard.ChangeText(displayUnitManager.BlackBoard.TitleIndex, "Sorting Completed!");
    }

    // --------------------------------------- Getters / Setters --------------------------------------- 

    public override TeachingAlgorithm GetTeachingAlgorithm()
    {
        return sortAlgorithm;
    }

    public AlgorithmManagerBase AlgorithmManagerBase
    {
        get { return algorithmManagerBase; }
    }

    public SortSettings SortSettings
    {
        get { return sortSettings; }
    }

    public override SettingsBase Settings
    {
        get { return sortSettings; }
    }

    public ElementManager ElementManager
    {
        get { return elementManager; }
    }

    public HolderManager HolderManager
    {
        get { return holderManager; }
    }

    // The positions of the holders
    public Vector3[] HolderPositions
    {
        get { return holderPositions; }
        set { holderPositions = value; }
    }

    /* --------------------------------------- Instatiate Setup ---------------------------------------
     * > Called from UserController
     * > Creates the holders and the sorting elements
    */
    public override void InstantiateSetup()
    {
        // From awake
        // Prepare settings from editor (debugging)
        //sortSettings.PrepareSettings();

        // >>> Algorithm
        algorithmName = sortSettings.Algorithm;
        sortAlgorithm = (SortAlgorithm)GrabAlgorithmFromObj(); // move to init
        sortAlgorithm.MainManager = this;
        sortAlgorithm.DemoStepDuration = new WaitForSeconds(sortSettings.AlgorithmSpeed);

        // Manager
        algorithmManagerBase = ActivateDeactivateSortingManagers(algorithmName);
        algorithmManagerBase.InitSortingManager(this);

        sortAlgorithm.PseudoCodeViewer = displayUnitManager.PseudoCodeViewer;
        displayUnitManager.SetAlgorithmForPseudo(sortAlgorithm);
        // awake end

        // From start
        // Right blackboard title / text
        displayUnitManager.BlackBoard.ChangeText(displayUnitManager.BlackBoard.TitleIndex, algorithmName);
        displayUnitManager.BlackBoard.ChangeText(displayUnitManager.BlackBoard.TextIndex, "Teaching mode: " + sortSettings.TeachingMode);

        // start end

        holderManager.CreateObjects(sortSettings.NumberOfElements, null);
        HolderPositions = holderManager.GetHolderPositions();
        elementManager.CreateObjects(sortSettings.NumberOfElements, HolderPositions, sortSettings.Duplicates, sortSettings.SortingCase);

        // Display on blackboard
        displayUnitManager.BlackBoard.ChangeText(displayUnitManager.BlackBoard.TitleIndex, algorithmName);
        displayUnitManager.BlackBoard.ChangeText(displayUnitManager.BlackBoard.TextIndex, sortSettings.TeachingMode);

        // Display pseudocode
        if (sortSettings.TeachingMode == Util.USER_TEST)
        {
            if (sortSettings.Difficulty <= Util.INTERMEDIATE)
            {
                displayUnitManager.PseudoCodeViewer.PseudoCodeSetup();
                //displayUnitManager.PseudoCodeViewerFixed.PseudoCodeSetup();
            }
            else if (sortSettings.Difficulty == Util.ADVANCED)
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
        ActivateTaskObjects(true);
    }

    protected override void ActivateTaskObjects(bool active)
    {
        // Settings menu
        sortSettings.SetSettingsActive(!active);

        // Sorting table
        sortingTableObj.SetActive(active);

    }

    // Keeps only one sorting algorithm manager active
    private AlgorithmManagerBase ActivateDeactivateSortingManagers(string sortAlgorithm)
    {
        switch (sortAlgorithm)
        {
            case Util.BUBBLE_SORT:
                GetComponentInChildren<BubbleSortManager>().enabled = true; //todo: change to a list instead
                GetComponentInChildren<InsertionSortManager>().enabled = false;
                GetComponentInChildren<BucketSortManager>().enabled = false;
                GetComponentInChildren<BucketManager>().enabled = false;
                GetComponentInChildren<MergeSortManager>().enabled = false;
                return GetComponentInChildren<BubbleSortManager>();

            case Util.INSERTION_SORT:
                GetComponentInChildren<BubbleSortManager>().enabled = false;
                GetComponentInChildren<InsertionSortManager>().enabled = true;
                GetComponentInChildren<BucketSortManager>().enabled = false;
                GetComponentInChildren<BucketManager>().enabled = false;
                GetComponentInChildren<MergeSortManager>().enabled = false;
                return GetComponentInChildren<InsertionSortManager>();

            case Util.BUCKET_SORT:
                GetComponentInChildren<BubbleSortManager>().enabled = false;
                GetComponentInChildren<InsertionSortManager>().enabled = false;
                GetComponentInChildren<BucketSortManager>().enabled = true;
                GetComponentInChildren<BucketManager>().enabled = true;
                GetComponentInChildren<MergeSortManager>().enabled = false;
                return GetComponentInChildren<BucketSortManager>();

            case Util.MERGE_SORT:
                GetComponentInChildren<BubbleSortManager>().enabled = false;
                GetComponentInChildren<InsertionSortManager>().enabled = false;
                GetComponentInChildren<BucketSortManager>().enabled = false;
                GetComponentInChildren<BucketManager>().enabled = false;
                GetComponentInChildren<MergeSortManager>().enabled = true;
                return GetComponentInChildren<MergeSortManager>();

            default: Debug.Log("Sorting algorithm '" + sortAlgorithm + "' not found."); break;
        }
        return null;
    }

    protected override TeachingAlgorithm GrabAlgorithmFromObj()
    {
        switch (algorithmName)
        {
            case Util.BUBBLE_SORT: return sortAlgorithmsObj.GetComponent<BubbleSort>();
            case Util.INSERTION_SORT: return sortAlgorithmsObj.GetComponent<InsertionSort>();
            case Util.BUCKET_SORT: return sortAlgorithmsObj.GetComponent<BucketSort>();
            case Util.MERGE_SORT: return sortAlgorithmsObj.GetComponent<MergeSort>();
            default: Debug.LogError("'" + algorithmName + "' not valid"); return null;
        }
    }

    /* --------------------------------------- Destroy & Restart ---------------------------------------
     * > Called from UserController
     * > Destroys all the gameobjects
     */
    public override void DestroyAndReset()
    {
        algorithmStarted = false;

        if (sortSettings.IsDemo() && !sortSettings.IsStepByStep() && !sortAlgorithm.IsTaskCompleted)
        {
            StartCoroutine(algorithmUserController.CreateWarningMessage("Can't stop during demo. See blackboard for progress.", UtilSort.ERROR_COLOR));
        }
        else
        {
            // Stop ongoing actions
            UserStoppedAlgorithm = true;

            // Destroy holders
            holderManager.DestroyAndReset();

            // Destroy sorting elements
            elementManager.DestroyAndReset();

            // Reset
            if (sortAlgorithm.IsTaskCompleted)
                sortAlgorithm.IsTaskCompleted = false;

            // Reset algorithm
            sortAlgorithm.ResetSetup();

            // Reset displays
            //displayUnitManager.ResetDisplays();
            displayUnitManager.DestroyDisplaysContent();

            // Hide sorting table and bring back menu
            ActivateTaskObjects(false);

            //
            controllerReady = false;
        }

        // Cleanup pseudocode
        sortAlgorithm.PseudoCodeViewer.DestroyPseudoCode();
    }

    // Input from user during Step-By-Step (increment/decrement)
    public void PlayerStepByStepInput(bool increment)
    {
        if (ControllerReady)
            tutorialStep.NotifyUserInput(increment);
    }

    // Give feedback to the user when the sorting task (user test) is completed)
    private IEnumerator FinishUserTest()
    {
        yield return sortAlgorithm.DemoStepDuration;
        sortAlgorithm.IsTaskCompleted = true;
        displayUnitManager.PseudoCodeViewer.RemoveHightlight();

        // Visual feedback (make each element "jump")
        for (int x = 0; x < sortSettings.NumberOfElements; x++)
        {
            UtilSort.IndicateElement(elementManager.GetSortingElement(x));
            elementManager.GetSortingElement(x).transform.rotation = Quaternion.identity;
            yield return sortAlgorithm.DemoStepDuration; // 1/2
        }
    }

    /* --------------------------------------- Demo ---------------------------------------
     * - Gives a visual presentation of <sorting algorithm>
     * - Player can't interact with sorting elements (*fix)
    */
    public override void PerformAlgorithmDemo()
    {
        Debug.Log(">>> Performing " + algorithmName + " demo.");
        elementManager.InteractionWithSortingElements(false);
        //MergeSort.MergeSortStandard(elementManager.SortingElements);
        //Debug.Log("----------------------------------------------------------------------");
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
        tutorialStep.Init(sortAlgorithm.UserTestInstructions(algorithmManagerBase.CopyFirstState(elementManager.SortingElements)));
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
        Dictionary<int, InstructionBase> instructions = sortAlgorithm.UserTestInstructions(algorithmManagerBase.CopyFirstState(elementManager.SortingElements));

        // Initialize user test
        userTestManager.InitUserTest(instructions, algorithmManagerBase.MovesNeeded, FindNumberOfUserAction(instructions));

        // Set start time
        userTestManager.SetStartTime();

        //DebugCheckInstructions(instructions); // Debugging

        userTestReady = true; // debugging
    }










    // DELETE::::

    // Update is called once per frame
    void OldUpdate()
    {

        // OBS: Not used, see mainManager

        if (sortAlgorithm.IsTaskCompleted)
        {
            if (sortSettings.IsUserTest() && userTestManager.TimeSpent == 0)
            {
                userTestManager.SetEndTime();
                userTestManager.CalculateScore();
                displayUnitManager.BlackBoard.ChangeText(displayUnitManager.BlackBoard.TextIndex, userTestManager.GetExaminationResult());
            }
            displayUnitManager.BlackBoard.ChangeText(displayUnitManager.BlackBoard.TitleIndex, "Sorting Completed!");
        }
        else
        {
            if (sortSettings.IsStepByStep())
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
            else if (sortSettings.IsUserTest()) // User test
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
                                userTestManager.ReadyForNext += algorithmManagerBase.PrepareNextInstruction(userTestManager.GetInstruction());
                            else if (elementManager.AllSorted())
                                StartCoroutine(FinishUserTest());

                        }
                    }
                    displayUnitManager.BlackBoard.ChangeText(displayUnitManager.BlackBoard.TextIndex, userTestManager.FillInBlackboard());
                }
            }
        }
    }
}
