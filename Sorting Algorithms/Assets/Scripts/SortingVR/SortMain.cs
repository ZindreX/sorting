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

    [SerializeField]
    protected AlgorithmUserController algorithmUserController;

    [SerializeField]
    private GameObject sortAlgorithmsObj, displayUnitManagerObj, sortingTableObj;

    private Vector3[] holderPositions;

    protected override void Awake()
    {
        base.Awake();

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

    public override TeachingSettings Settings
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
    protected override void PerformCheckList(string check)
    {
        switch (check)
        {
            case START_UP_CHECK:
                break;


            case SHUT_DOWN_CHECK:
                // Get feedback from sub units whenever they are ready to shut down
                // if nodes etc. ready -> destroy
                displayUnitManager.SetText(UtilSort.SORT_TABLE_TEXT, "Stopping " + SortSettings.TeachingMode);

                bool readyForDestroy = true;
                foreach (KeyValuePair<string, bool> entry in safeStopChecklist)
                {
                    string key = entry.Key;
                    Debug.Log("Key: " + key);
                    if (!safeStopChecklist[key])
                    {
                        readyForDestroy = false;
                        Debug.Log("Not ready");
                        break;
                    }

                }

                if (readyForDestroy)
                {
                    Debug.Log("Starting safe shutdown");
                    checkListModeActive = false;
                    DestroyAndReset();
                }

                break;
            default: Debug.Log(">>>>>>>>> Unknown check '" + check + "'."); break;
        }
    }

    public override void InstantiateSetup()
    {
        base.InstantiateSetup();

        // Get values from settings
        algorithmName = sortSettings.Algorithm; // string name
        int numberOfElements = sortSettings.NumberOfElements;
        bool allowDuplicates = sortSettings.Duplicates;
        string sortingCase = sortSettings.SortingCase;
        float algorithmSpeed = sortSettings.AlgorithmSpeed;

        // Algorithm setup
        sortAlgorithm = (SortAlgorithm)GrabAlgorithmFromObj(); // SortAlgorithm object
        sortAlgorithm.InitSortAlgorithm(this, algorithmSpeed);

        // Algorithm manager setup
        algorithmManagerBase = ActivateDeactivateSortingManagers(algorithmName);
        algorithmManagerBase.InitSortingManager(this);

        // Init display unit manager
        bool includeLineNr = Settings.PseudocodeLineNr;
        bool inDetailStep = Settings.PseudocodeStep;
        displayUnitManager.InitDisplayUnitManager(sortAlgorithm, includeLineNr, inDetailStep);
        displayUnitManager.SetTextWithIndex(UtilSort.RIGHT_BLACKBOARD, algorithmName, 0);

        // Init holder manager
        holderManager.InitManager();
        holderManager.CreateObjects(numberOfElements, null);
        HolderPositions = holderManager.GetHolderPositions();

        // Init element manager
        elementManager.InitManager();
        elementManager.CreateObjects(numberOfElements, HolderPositions, allowDuplicates, sortingCase);

        // Only Insertion sort using this method so far, here: create pivot holder
        sortAlgorithm.Specials(UtilSort.INIT, Util.NO_VALUE, false);

        // Prepare difficulty level related stuff for user test
        if (sortSettings.TeachingMode == Util.USER_TEST)
        {
            if (sortSettings.Difficulty <= Util.PSEUDO_CODE_MAX_DIFFICULTY)
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

        // Pseudocode initialized
        sortAlgorithm.PseudoCodeInitilized = true; // Sort only

        
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
        base.DestroyAndReset();

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

        if (true)
        {
            // Getting instructions for this sample of sorting elements
            InstructionBase[] firstState = algorithmManagerBase.CopyFirstState(elementManager.SortingElements);
            Dictionary<int, InstructionBase> instructions = sortAlgorithm.UserTestInstructions(firstState);

            if (instructions == null)
                return;

            Debug.Log("Number of instructions: " + instructions.Count);

            stepByStepManager.InitDemo(instructions);

            newDemoImplemented = true;
        }
        else
            StartCoroutine(sortAlgorithm.Demo(elementManager.SortingElements));
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
        int numberOfUserActions = FindNumberOfUserAction(instructions);
        userTestManager.InitUserTest(instructions, algorithmManagerBase.MovesNeeded, numberOfUserActions);

        // Set start time
        userTestManager.SetStartTime();

        //foreach (KeyValuePair<int, InstructionBase> entry in instructions)
        //{
        //    Debug.Log(entry.Value.DebugInfo());
        //}
    }

    protected override void UserTestUpdate()
    {
        // First check if user test setup is complete (instructions available)
        if (userTestManager.HasInstructions())
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
        audioManager.Play("Finish");

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
        displayUnitManager.SetTextWithIndex(UtilSort.RIGHT_BLACKBOARD, "Sorting Completed!", 1);
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
        Util.HideObject(sortSettings.gameObject, !active, true);
        //sortSettings.SetSettingsActive(!active);

        // Sorting table
        Util.HideObject(sortingTableObj, active, true);
        demoDevice.gameObject.SetActive(active && SortSettings.IsDemo());
        Debug.Log(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> Demo device active: " + (active && SortSettings.IsDemo()));

        yield return loading;

        if (active)
            displayUnitManager.SetTextWithIndex(UtilSort.RIGHT_BLACKBOARD, "Teaching mode: " + sortSettings.TeachingMode, 1);
        else
        {
            sortSettings.FillTooltips("Loading complete!");
        }

    }

    // Keeps only one sorting algorithm manager active
    private AlgorithmManagerBase ActivateDeactivateSortingManagers(string sortAlgorithm)
    {
        switch (sortAlgorithm)
        {
            case Util.BUBBLE_SORT:
                GetComponentInChildren<BubbleSortManager>().enabled = true; // needed to enable/disable?
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

    public override void ToggleVisibleStuff()
    {
        Debug.Log("Toggle");
    }
}
