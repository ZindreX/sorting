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
    protected StepByStepManager stepByStepManager;
    protected UserTestManager userTestManager;

    [SerializeField]
    protected AlgorithmUserController algorithmUserController;

    [SerializeField]
    private GameObject sortAlgorithmsObj, displayUnitManagerObj, sortingTableObj;

    private Vector3[] holderPositions;
    protected WaitForSeconds loading = new WaitForSeconds(1f);

    private void Awake()
    {
        // >>> Objects
        holderManager = GetComponent(typeof(HolderManager)) as HolderManager;
        elementManager = GetComponent(typeof(ElementManager)) as ElementManager;
        stepByStepManager = GetComponent(typeof(StepByStepManager)) as StepByStepManager;
        userTestManager = GetComponent(typeof(UserTestManager)) as UserTestManager;
        displayUnitManager = displayUnitManagerObj.GetComponent(typeof(DisplayUnitManager)) as DisplayUnitManager;
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
        // Get values from settings
        algorithmName = sortSettings.Algorithm; // string name
        int numberOfElements = sortSettings.NumberOfElements;
        bool allowDuplicates = sortSettings.Duplicates;
        string sortingCase = sortSettings.SortingCase;
        float algorithmSpeed = sortSettings.AlgorithmSpeed;

        // Algorithm setup
        sortAlgorithm = (SortAlgorithm)GrabAlgorithmFromObj(); // SortAlgorithm object
        sortAlgorithm.InitTeachingAlgorithm();
        sortAlgorithm.MainManager = this; // Set main manager
        sortAlgorithm.DemoStepDuration = new WaitForSeconds(algorithmSpeed); // set algorithm step duration speed

        // Algorithm manager setup
        algorithmManagerBase = ActivateDeactivateSortingManagers(algorithmName);
        algorithmManagerBase.InitSortingManager(this);

        sortAlgorithm.PseudoCodeViewer = displayUnitManager.PseudoCodeViewer;

        // Init display unit manager
        displayUnitManager.InitDisplayUnitManager(sortAlgorithm);
        displayUnitManager.SetTextWithIndex(UtilSort.RIGHT_BLACKBOARD, algorithmName, 0);

        // Init holder manager
        holderManager.InitManager();
        holderManager.CreateObjects(numberOfElements, null);
        HolderPositions = holderManager.GetHolderPositions();

        // Init element manager
        elementManager.InitManager();
        elementManager.CreateObjects(numberOfElements, HolderPositions, allowDuplicates, sortingCase);

        // Prepare difficulty level related stuff for user test
        if (sortSettings.TeachingMode == Util.USER_TEST)
        {
            if (sortSettings.Difficulty <= Util.INTERMEDIATE)
            {
                displayUnitManager.PseudoCodeViewer.PseudoCodeSetup();
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
        }
        
        // Hide menu and display sorting table
        StartCoroutine(ActivateTaskObjects(true));

        displayUnitManager.SetText(UtilSort.SORT_TABLE_TEXT, "Click start to play");
    }

    /* --------------------------------------- Destroy & Restart ---------------------------------------
     * > Called from UserController
     * > Destroys all the gameobjects
     */
    public override void DestroyAndReset()
    {
        algorithmStarted = false;

        // Stop ongoing actions
        UserStoppedAlgorithm = true;

        // Destroy sorting elements
        elementManager.DestroyAndReset();

        // Destroy holders
        holderManager.DestroyAndReset();

        // Reset algorithm
        sortAlgorithm.ResetSetup();

        // Reset displays
        //displayUnitManager.ResetDisplays();
        displayUnitManager.DestroyDisplaysContent(); //::::

        // Hide sorting table and bring back menu
        StartCoroutine(ActivateTaskObjects(false));

        //
        controllerReady = false;

        // Cleanup pseudocode
        sortAlgorithm.PseudoCodeViewer.DestroyPseudoCode();

        // test stuff
        switch (sortSettings.TeachingMode)
        {
            case Util.DEMO: break;
            case Util.STEP_BY_STEP: stepByStepManager.ResetState(); break;
            case Util.USER_TEST: userTestManager.ResetState(); break;
            default: Debug.Log("Teaching mode '" + sortSettings.TeachingMode + "' not found"); break;
        }
    }

    /* --------------------------------------- Demo ---------------------------------------
     * - Gives a visual presentation of <sorting algorithm>
     * - Player can't interact with sorting elements (*fix)
    */
    public override void PerformAlgorithmDemo()
    {
        displayUnitManager.SetText(UtilSort.SORT_TABLE_TEXT, "Watch and learn");

        elementManager.InteractionWithSortingElements(false);
        //MergeSort.MergeSortStandard(elementManager.SortingElements);
        //Debug.Log("----------------------------------------------------------------------");
        StartCoroutine(sortAlgorithm.Demo(elementManager.SortingElements));
    }

    protected override void DemoUpdate()
    {

    }

    /* --------------------------------------- Step-By-Step ---------------------------------------
     * - Gives a visual presentation of <sorting algorithm>
     * - Player can't directly interact with sorting elements, but can use controllers to progress
     *   one step of a time / or back
    */
    public override void PerformAlgorithmStepByStep()
    {
        displayUnitManager.SetText(UtilSort.SORT_TABLE_TEXT, "Use grip buttons\n to progress");

        // Getting instructions for this sample of sorting elements
        elementManager.InteractionWithSortingElements(false);
        stepByStepManager.Init(sortAlgorithm.UserTestInstructions(algorithmManagerBase.CopyFirstState(elementManager.SortingElements)));
    }

    // Input from user during Step-By-Step (increment/decrement)
    public void PlayerStepByStepInput(bool increment)
    {
        if (ControllerReady)
            stepByStepManager.NotifyUserInput(increment);
    }

    protected override void StepByStepUpdate()
    {
        if (stepByStepManager.PlayerMove && stepByStepManager.IsValidStep)
        {
            stepByStepManager.PlayerMove = false;
            InstructionBase instruction = stepByStepManager.GetStep();
            
            //Debug.Log(">>> " + instruction.Instruction);
            //Debug.Log("InstructionNr.: " + instruction.INSTRUCION_NR);
            //Debug.Log(tutorialStep.CurrentInstructionNr);


            bool gotSortingElement = !sortAlgorithm.SkipDict[UtilSort.SKIP_NO_ELEMENT].Contains(instruction.Instruction);
            sortAlgorithm.ExecuteStepByStepOrder(instruction, gotSortingElement, stepByStepManager.PlayerIncremented);
        }
    }

    /* --------------------------------------- User Test ---------------------------------------
     * - Gives a visual presentation of elements used in <sorting algorithm>
     * - Player needs to interact with the sorting elements to progress through the algorithm
    */
    public override void PerformAlgorithmUserTest()
    {
        displayUnitManager.SetText(UtilSort.SORT_TABLE_TEXT, "Use trigger buttons to interact\n with the sorting elements");

        // Enable interaction
        elementManager.InteractionWithSortingElements(true);

        // Getting instructions for this sample of sorting elements
        Dictionary<int, InstructionBase> instructions = sortAlgorithm.UserTestInstructions(algorithmManagerBase.CopyFirstState(elementManager.SortingElements));

        // Initialize user test
        userTestManager.InitUserTest(instructions, algorithmManagerBase.MovesNeeded, FindNumberOfUserAction(instructions));

        // Set start time
        userTestManager.SetStartTime();

        userTestReady = true; // debugging
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

    // Finish off user test
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


    // --------------------------------------- Other functions --------------------------------------- 

    // Makes specific objects visible/invisible based on when they should be active
    protected override IEnumerator ActivateTaskObjects(bool active)
    {

        sortSettings.FillTooltips("Loading setup...");
        displayUnitManager.SetTextWithIndex(UtilSort.RIGHT_BLACKBOARD, "Loading setup", 1);
        yield return loading;
        displayUnitManager.SetTextWithIndex(UtilSort.RIGHT_BLACKBOARD, "Loading complete!", 1);

        // Settings menu
        sortSettings.SetSettingsActive(!active);

        // Sorting table
        sortingTableObj.SetActive(active);

        yield return loading;

        if (active)
            displayUnitManager.SetTextWithIndex(UtilSort.RIGHT_BLACKBOARD, "Teaching mode: " + sortSettings.TeachingMode, 1);

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
                BucketSortManager bucketSortManager = GetComponentInChildren<BucketSortManager>();
                bucketSortManager.enabled = true;

                BucketManager bucketManager = GetComponentInChildren<BucketManager>();
                bucketManager.enabled = true;
                bucketManager.InitManager();

                GetComponentInChildren<BubbleSortManager>().enabled = false;
                GetComponentInChildren<InsertionSortManager>().enabled = false;
                GetComponentInChildren<MergeSortManager>().enabled = false;

                return bucketSortManager;

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

    // Get algorithm component
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
                if (stepByStepManager.PlayerMove && stepByStepManager.IsValidStep)
                {
                    stepByStepManager.PlayerMove = false;
                    InstructionBase instruction = stepByStepManager.GetStep();
                    Debug.Log(">>> " + instruction.Instruction);
                    //Debug.Log("InstructionNr.: " + instruction.INSTRUCION_NR);
                    //Debug.Log(tutorialStep.CurrentInstructionNr);


                    bool gotSortingElement = !sortAlgorithm.SkipDict[UtilSort.SKIP_NO_ELEMENT].Contains(instruction.Instruction);
                    sortAlgorithm.ExecuteStepByStepOrder(instruction, gotSortingElement, stepByStepManager.PlayerIncremented);
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
