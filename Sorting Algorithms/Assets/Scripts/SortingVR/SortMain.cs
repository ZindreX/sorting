using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortMain : MainManager {

    // Audio
    public readonly string SORTED_SOUND = "Sorted";

    [Space(10)]
    [Header("Sort main")]
    [Space(2)]
    // Base object instances
    [SerializeField]
    private SortSettings sortSettings;
    private SortAlgorithm sortAlgorithm;

    private SortingTable sortingTable;

    // Managers
    private AlgorithmManagerBase algorithmManagerBase;
    private ElementManager elementManager;
    private HolderManager holderManager;

    // Visual
    protected DisplayUnitManager displayUnitManager;

    [SerializeField]
    protected AlgorithmUserController algorithmUserController;

    [SerializeField]
    private GameObject sortAlgorithmsObj, displayUnitManagerObj;

    private Vector3[] holderPositions;

    protected override void Awake()
    {
        base.Awake();

        // >>> Objects
        holderManager = GetComponent(typeof(HolderManager)) as HolderManager;
        elementManager = GetComponent(typeof(ElementManager)) as ElementManager;
        demoManager = GetComponent(typeof(DemoManager)) as DemoManager;
        userTestManager = GetComponent(typeof(UserTestManager)) as UserTestManager;
        displayUnitManager = displayUnitManagerObj.GetComponent(typeof(DisplayUnitManager)) as DisplayUnitManager;

        sortingTable = FindObjectOfType<SortingTable>();
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

    private bool test;
    public bool Test { get; set; }

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
                    if (!safeStopChecklist[key])
                    {
                        readyForDestroy = false;
                        break;
                    }

                }
                ShutdownProcess(readyForDestroy);
                break;

            default: Debug.Log(">>>>>>>>> Unknown check '" + check + "'."); break;
        }
    }

    public override void InstantiateSafeStart()
    {
        base.InstantiateSafeStart();

        // Get values from settings
        algorithmName = sortSettings.Algorithm; // string name
        int numberOfElements = sortSettings.NumberOfElements;
        bool allowDuplicates = sortSettings.Duplicates;
        string sortingCase = sortSettings.SortingCase;
        float algorithmSpeed = sortSettings.AlgorithmSpeed;

        // Set min max values for sorting elements
        elementManager.SetMinMax(sortSettings.ElementMinValue, sortSettings.ElementMaxValue);

        // Algorithm manager setup
        algorithmManagerBase = SortingAlgorithmManager(algorithmName);
        algorithmManagerBase.InitSortingManager(this);

        // Algorithm setup
        sortAlgorithm = (SortAlgorithm)GrabAlgorithmFromObj(); // SortAlgorithm object
        sortAlgorithm.InitSortAlgorithm(this, algorithmSpeed);

        // Init display unit manager
        displayUnitManager.InitDisplayUnitManager();
        displayUnitManager.SetTextWithIndex(UtilSort.RIGHT_BLACKBOARD, algorithmName, 0);

        // Init holder manager
        holderManager.InitManager();
        holderManager.CreateObjects(numberOfElements, null);
        HolderPositions = holderManager.GetHolderPositions();

        // Init element manager
        elementManager.InitManager();
        elementManager.CreateObjects(numberOfElements, HolderPositions, allowDuplicates, sortingCase);
        sortAlgorithm.ListValues = elementManager.InitList();

        // Only Insertion sort using this method so far, here: create pivot holder
        sortAlgorithm.Specials(UtilSort.INIT, Util.NO_VALUE, false);


        // Prepare difficulty level related stuff for user test
        bool includeLineNr = Settings.PseudocodeLineNr;
        bool inDetailStep = Settings.PseudocodeStep;
        if (sortSettings.TeachingMode == Util.USER_TEST)
        {
            if (sortSettings.Difficulty <= Util.PSEUDO_CODE_MAX_DIFFICULTY)
            {
                displayUnitManager.PseudoCodeViewer.InitPseudoCodeViewer(sortAlgorithm, includeLineNr, inDetailStep);
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
            displayUnitManager.PseudoCodeViewer.InitPseudoCodeViewer(sortAlgorithm, includeLineNr, inDetailStep);
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
        displayUnitManager.DestroyDisplaysContent(); // pseudocode
        displayUnitManager.ResetDisplays();

        // Hide sorting table and bring back menu
        StartCoroutine(ActivateTaskObjects(false));


        // test stuff
        //switch (sortSettings.TeachingMode)
        //{
        //    case Util.DEMO: case Util.STEP_BY_STEP: demoManager.ResetState(); break;
        //    case Util.USER_TEST: userTestManager.ResetState(); break;
        //    default: Debug.Log("Teaching mode '" + sortSettings.TeachingMode + "' not found"); break;
        //}
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

            //foreach (KeyValuePair<int, InstructionBase> entry in instructions)
            //{
            //    Debug.Log(">>> " + entry.Key + ": " + entry.Value.DebugInfo());
            //}


            demoManager.InitDemo(instructions);

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
        displayUnitManager.SetText(UtilSort.SORT_TABLE_TEXT, "Use the trigger button to interact\n with the sorting elements");

        // Enable interaction
        elementManager.InteractionWithSortingElements(true);

        // Getting instructions for this sample of sorting elements
        Dictionary<int, InstructionBase> instructions = sortAlgorithm.UserTestInstructions(algorithmManagerBase.CopyFirstState(elementManager.SortingElements));

        // Initialize user test
        int numberOfUserActions = FindNumberOfUserAction(instructions);
        userTestManager.InitUserTest(instructions, algorithmManagerBase.MovesNeeded, numberOfUserActions, Settings.Difficulty);
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
                    Debug.Log("This update loop needs some fixes.... Ever getting into this case????"); // ???
                }
                else
                {
                    // Still some elements not sorted, so go on to next round
                    bool hasInstruction = userTestManager.IncrementToNextInstruction();

                    // Hot fix - solve in some other way?
                    if (hasInstruction)
                        userTestManager.ReadyForNext += algorithmManagerBase.PrepareNextInstruction(userTestManager.GetInstruction());
                    else if (elementManager.AllSorted())
                    {
                        WaitForSupportToComplete++;
                        StartCoroutine(FinishUserTest());
                    }
                }
            }

            if (Settings.IsUserTest() && sortSettings.UserTestScore)
                displayUnitManager.SetTextWithIndex(UtilSort.RIGHT_BLACKBOARD, userTestManager.FillInBlackboard(), 1);
        }
    }

    // Give feedback to the user when the sorting task (user test) is completed)
    protected override IEnumerator FinishUserTest()
    {
        yield return base.FinishUserTest();

        // Visual feedback (make each element "jump")
        for (int x = 0; x < sortSettings.NumberOfElements; x++)
        {
            UtilSort.IndicateElement(elementManager.GetSortingElement(x));
            elementManager.GetSortingElement(x).transform.rotation = Quaternion.identity;
            yield return finishStepDuration;
        }
        WaitForSupportToComplete--;
    }

    // Finish off user test
    protected override void TaskCompletedFinishOff()
    {
        if (sortSettings.IsUserTest() && sortSettings.UserTestScore)
        {
            //if (userTestManager.TimeSpent == 0)
            //{
            userTestManager.SetEndTime();
            userTestManager.CalculateScore();

            // Left blackboard
            displayUnitManager.SetTextWithIndex(UtilSort.LEFT_BLACKBOARD, "User test incorrect action details", 0);
            displayUnitManager.SetTextWithIndex(UtilSort.LEFT_BLACKBOARD, userTestManager.IncorrectActionDetails(), 1);

            // Right blackboard
            displayUnitManager.SetTextWithIndex(UtilSort.RIGHT_BLACKBOARD, "User test score", 0);
            displayUnitManager.SetTextWithIndex(UtilSort.RIGHT_BLACKBOARD, userTestManager.GetExaminationResult(), 1);
            //}
        }
        displayUnitManager.SetText(UtilSort.SORT_TABLE_TEXT, "Sorting Completed!");
    }


    // --------------------------------------- Other functions --------------------------------------- 

    // Makes specific objects visible/invisible based on when they should be active
    protected override IEnumerator ActivateTaskObjects(bool active)
    {
        sortSettings.FillTooltips("Loading setup...", false);
        yield return loading;

        if (feedbackDisplayText != null)
        {
            if (active)
                feedbackDisplayText.text = sortSettings.TeachingMode;
            else
                feedbackDisplayText.text = "";
        }

        // Settings menu
        sortSettings.ActiveInScene(!active);

        // Sorting table
        sortingTable.ActiveInScene(active);


        demoDevice.gameObject.SetActive(active && SortSettings.IsDemo());
        //Debug.Log(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> Demo device active: " + (active && SortSettings.IsDemo()));

        yield return loading;

        if (active)
            displayUnitManager.SetTextWithIndex(UtilSort.RIGHT_BLACKBOARD, "Teaching mode: " + sortSettings.TeachingMode, 1);
        else
            sortSettings.FillTooltips("Loading complete!", false);
    }

    // Keeps only one sorting algorithm manager active
    private AlgorithmManagerBase SortingAlgorithmManager(string sortAlgorithm)
    {
        switch (sortAlgorithm)
        {
            case Util.BUBBLE_SORT: return GetComponentInChildren<BubbleSortManager>();
            case Util.INSERTION_SORT: return GetComponentInChildren<InsertionSortManager>();
            case Util.BUCKET_SORT:
                int numberOfbuckets = sortSettings.NumberOfBuckets;

                BucketSortManager bucketSortManager = GetComponentInChildren<BucketSortManager>();
                bucketSortManager.NumberOfBuckets = numberOfbuckets;

                BucketManager bucketManager = GetComponentInChildren<BucketManager>(); // Move to BucketSortManager
                bucketManager.InitBucketManager(numberOfbuckets);
                bucketManager.InitManager();
                return bucketSortManager;

            case Util.MERGE_SORT: return GetComponentInChildren<MergeSortManager>();
            default: Debug.LogError("Sorting algorithm '" + sortAlgorithm + "' not found."); break;
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

    }


    // Variables to reset on destroy (player leaving scene without stopping demo/user test)
    private void OnDestroy()
    {
        SortingElementBase.SORTING_ELEMENT_NR = 0;
        HolderBase.HOLDER_NR = 0;
        Bucket.BUCKET_NR = 0;
    }
}
