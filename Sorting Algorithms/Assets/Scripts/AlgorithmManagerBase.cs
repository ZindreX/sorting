using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Blackboard))]
[RequireComponent(typeof(PseudoCodeViewer))]
[RequireComponent(typeof(HolderManager))]
[RequireComponent(typeof(ElementManager))]
[RequireComponent(typeof(UserTestManager))]
[RequireComponent(typeof(TutorialStep))]
[RequireComponent(typeof(Algorithm))]
[RequireComponent(typeof(ScoreManager))]
public abstract class AlgorithmManagerBase : MonoBehaviour {

    /* -------------------------------------------- The main unit for the algorithm ----------------------------------------------------
     * >>> Status of algorithms:
     * ---------------------------------------------------------------------------------------------------------------------------------
     *    Algorithm name    |   Standard    |   Tutorial    |   User Test   |                   Comment
     * ---------------------------------------------------------------------------------------------------------------------------------
     *    Bubble Sort       |     Yes       |     Yes       |       Yes     | Works, but maybe add more algorithm details to blackboard
     *    Insertion Sort    |     Yes       |     Yes       |       Yes     | Almost done --> create a new tutorial (step by step)
     *    Merge Sort        |     Yes       |     No        |       No      | Tutorial not completed yet, user test not started
     *    Quick Sort        |      No       |     No        |       No      | -
     *    Bucket Sort       |     Yes       |     Yes*      |       No      | Tutorial works, but inner sorting in bucket not displayed.
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

    [SerializeField]
    private string teachingMode = Util.TUTORIAL_STEP;

    private string algorithmName; 
    private string[] rules = new string[Util.NUMBER_OF_RULE_TYPES];

    private Vector3[] holderPositions;

    [SerializeField]
    private GameObject blackboardObj;

    [SerializeField]
    protected GameObject pseudoCodeViewerObj;

    // Base object instances
    protected Blackboard blackboard;
    protected PseudoCodeViewer pseudoCodeViewer;
    protected HolderManager holderManager;
    protected ElementManager elementManager;
    protected UserTestManager userTestManager;
    protected TutorialStep tutorialStep;
    protected Algorithm algorithm;
    protected ScoreManager scoreManager;

    protected virtual void Awake()
    {
        if (IsValidSetup())
        {
            // ***  Rules ***
            InitRules();

            // *** Objects ***
            blackboard = blackboardObj.GetComponent(typeof(Blackboard)) as Blackboard;
            pseudoCodeViewer = pseudoCodeViewerObj.GetComponent(typeof(PseudoCodeViewer)) as PseudoCodeViewer;

            holderManager = GetComponent(typeof(HolderManager)) as HolderManager;
            elementManager = GetComponent(typeof(ElementManager)) as ElementManager;
            userTestManager = GetComponent(typeof(UserTestManager)) as UserTestManager;
            tutorialStep = GetComponent(typeof(TutorialStep)) as TutorialStep;
            scoreManager = GetComponent(typeof(ScoreManager)) as ScoreManager;

            // Setup algorithm in their respective <Algorithm name>Manager
            algorithm = InstanceOfAlgorithm;
            algorithmName = algorithm.GetAlgorithmName();
        }
    }

    // Use this for initialization
    void Start ()
    {
        blackboard.ChangeText(0, algorithmName);
    }

    // Update is called once per frame
    void Update()
    {
        if (algorithm.IsSortingComplete)
        {
            if (IsUserTest() && scoreManager.TimeSpent == 0)
            {
                scoreManager.SetEndTime();
                scoreManager.CalculateScore();
                blackboard.ChangeText(3, scoreManager.FillInBlackboard());
            }
            blackboard.ChangeText(1, "Sorting Completed!");
        }
        else
        {
            if (IsTutorialStep())
            {
                if (tutorialStep.PlayerMove && tutorialStep.IsValidStep)
                {
                    tutorialStep.PlayerMove = false;
                    //StartCoroutine(algorithm.ExecuteOrder(tutorialStep.GetInstruction(), tutorialStep.CurrentInstructionNr));
                    InstructionBase instruction = tutorialStep.GetStep();

                    if (instruction.ElementInstruction == Util.FIRST_INSTRUCTION || instruction.ElementInstruction == Util.FINAL_INSTRUCTION)
                    {
                        tutorialStep.FirstOrFinal = true;
                        PseudoCodeFirstFinal(instruction.ElementInstruction, Util.HIGHLIGHT_COLOR);
                    }
                    else if (tutorialStep.FirstOrFinal)
                    {
                        tutorialStep.FirstOrFinal = false;
                        PseudoCodeFirstFinal(Util.FIRST_INSTRUCTION, Util.BLACKBOARD_TEXT_COLOR);
                        PseudoCodeFirstFinal(Util.FINAL_INSTRUCTION, Util.BLACKBOARD_TEXT_COLOR);
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
                    else if (userTestManager.ReadyForNext == userTestManager.AlgorithmMovesNeeded)
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
                    blackboard.ChangeText(4, userTestManager.FillInBlackboard());
                    blackboard.ChangeText(3, scoreManager.FillInBlackboard());
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
            for (int x=0; x < elementManager.SortingElements.Length; x++)
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

    public Blackboard GetBlackBoard
    {
        get { return blackboard; }
    }

    public PseudoCodeViewer GetPseudoCodeViewer
    {
        get { return pseudoCodeViewer; }
    }

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

    //

    public bool IsTutorial()
    {
        return teachingMode == Util.TUTORIAL || teachingMode == Util.TUTORIAL_STEP;
    }

    public bool IsTutorialStep()
    {
        return teachingMode == Util.TUTORIAL_STEP;
    }

    public bool IsUserTest()
    {
        return teachingMode == Util.USER_TEST;
    }

    //

    public IEnumerator HighlightText(int lineNr, string text)
    {
        pseudoCodeViewer.SetCodeLine(lineNr, text, Util.HIGHLIGHT_COLOR);
        yield return new WaitForSeconds(algorithm.Seconds);
        pseudoCodeViewer.ChangeColorOfText(lineNr, Util.BLACKBOARD_TEXT_COLOR);
    }

    private void PseudoCodeFirstFinal(string instruction, Color color)
    {
        switch (instruction)
        {
            case Util.FIRST_INSTRUCTION:
                pseudoCodeViewer.CLEAN_HIGHTLIGHT(algorithm.FirstInstructionCodeLine() + 1, algorithm.FinalInstructionCodeLine());
                pseudoCodeViewer.ChangeColorOfText(algorithm.FirstInstructionCodeLine(), color);
                break;

            case Util.FINAL_INSTRUCTION:
                pseudoCodeViewer.CLEAN_HIGHTLIGHT(0, algorithm.FinalInstructionCodeLine() - 1);
                pseudoCodeViewer.ChangeColorOfText(algorithm.FinalInstructionCodeLine(), color);
                break;
        }
    }


    /* --------------------------------------- Tutorial ---------------------------------------
     * 
     * 
    */
    public void PerformAlgorithmTutorial()
    {
        Debug.Log(">>> Performing " + algorithmName + " tutorial.");
        StartCoroutine(algorithm.Tutorial(elementManager.SortingElements));

        //Debug.Log(DebugCheckGameObjects(InsertionSort.InsertionSortStandard(elementManager.SortingElements)));
        //Debug.Log(DebugCheckGameObjects(BucketSort.BucketSortStandard2(elementManager.SortingElements, numberOfElements)));
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
        userTestManager.InitUserTest(instructions, MovesNeeded);

        // Set start time
        scoreManager.SetStartTime();

        //DebugCheckInstructions(instructions); // Debugging
    }


    // --------------------------------------- To be implemented in subclasses ---------------------------------------

    // Returns the instance of the algorithm being runned 
    protected abstract Algorithm InstanceOfAlgorithm { get; }

    // Moves needed to progress to next instruction
    protected abstract int MovesNeeded { get; }

    // Prepares the next instruction based on the algorithm being runned
    protected abstract int PrepareNextInstruction(InstructionBase instruction);

    // TODO: Display stuff on the blackboard if help enabled
    protected abstract int SkipOrHelp(InstructionBase instruction);

    // Copies the first state of sorting elements into instruction, which can be used when creating instructions for user test
    protected abstract InstructionBase[] CopyFirstState(GameObject[] sortingElements);

    // Step by step tutorial
    //protected abstract IEnumerator ExecuteOrder(InstructionBase instruction, int instructionNr);




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
