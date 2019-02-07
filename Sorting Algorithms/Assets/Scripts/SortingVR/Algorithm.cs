using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Algorithm : MonoBehaviour, IAlgorithm {

    protected float seconds = 1f;

    // value1 <- pivot value, value2 <- compare value (usually)
    protected int value1 = UtilSort.INIT_STATE, value2 = UtilSort.INIT_STATE - 1;
    protected bool isSortingComplete = false;// isReadyForNextMove = false;

    //protected Vector3 aboveHolder = new Vector3(0f, 0.5f, 0f);
    protected List<int> prevHighlight = new List<int>();
    protected Dictionary<string, List<string>> skipDict = new Dictionary<string, List<string>>();

    protected PseudoCodeViewer pseudoCodeViewer;

    protected virtual void Awake()
    {
        AddSkipAbleInstructions();
    }

    public bool IsSortingComplete
    {
        get { return isSortingComplete; }
        set { isSortingComplete = value; }
    }

    public float Seconds
    {
        get { return seconds; }
        set { seconds = value; }
    }

    public PseudoCodeViewer PseudoCodeViewer
    {
        get { return pseudoCodeViewer; }
        set { pseudoCodeViewer = value; pseudoCodeViewer.SetAlgorithm(this); }
    }

    // Reach to line of code which are not mentioned in instructions
    public void HighllightPseudoLine(int lineNr, Color color)
    {
        pseudoCodeViewer.ChangeColorOfText(lineNr, color);
    }

    public Dictionary<string, List<string>> SkipDict
    {
        get { return skipDict; }
    }

    // Instructions which the user don't need to perform any actions to proceed
    public virtual void AddSkipAbleInstructions()
    {
        skipDict.Add(UtilSort.SKIP_NO_ELEMENT, new List<string>());
        skipDict[UtilSort.SKIP_NO_ELEMENT].Add(UtilSort.FIRST_INSTRUCTION);
        skipDict[UtilSort.SKIP_NO_ELEMENT].Add(UtilSort.FIRST_LOOP);
        skipDict[UtilSort.SKIP_NO_ELEMENT].Add(UtilSort.UPDATE_LOOP_INST);
        skipDict[UtilSort.SKIP_NO_ELEMENT].Add(UtilSort.END_LOOP_INST);
        skipDict[UtilSort.SKIP_NO_ELEMENT].Add(UtilSort.FINAL_INSTRUCTION);
        skipDict[UtilSort.SKIP_NO_ELEMENT].Add(UtilSort.INCREMENT_VAR_I);
        skipDict[UtilSort.SKIP_NO_ELEMENT].Add(UtilSort.SET_VAR_J);
        skipDict[UtilSort.SKIP_NO_ELEMENT].Add(UtilSort.UPDATE_VAR_J);
    }

    // ---------------------------- Overriden in the algorithm class which inherite this base class ----------------------------
    public abstract string AlgorithmName { get; }
    public virtual void ResetSetup()
    {
        value1 = UtilSort.INIT_STATE;
        value2 = UtilSort.INIT_STATE;
    }

    // To do stuff important for individual classes
    public abstract void Specials(string method, int number, bool activate);


    /* Collects one line of code at a time and rewrites it into pseudocode
     * > a method called PseudoCode(...) must be created aswell (this is where the pseudocode will be placed
     * 
    */
    public abstract string CollectLine(int lineNr);

    // first instruction line of code
    public abstract int FirstInstructionCodeLine();
    // final instruction line of code
    public abstract int FinalInstructionCodeLine();

    //public List<string> GetPseudoCode() { return new List<string>(); } // change later

    /* Tutorial of the chosen sorting algorithm
     * - No interaction from the user (except for settings)
    */
    public abstract IEnumerator Tutorial(GameObject[] list);

    /* Step by step execution of the chosen sorting algorithm
     * - The user can progress through the algorithm one step at the time
    */
    public abstract void ExecuteStepByStepOrder(InstructionBase instruction, bool gotElement, bool increment);

    /* Almost the same method as for the Step by step teaching method
     * - Only does the visualization of the pseudocode
    */
    public abstract IEnumerator UserTestHighlightPseudoCode(InstructionBase instruction, bool gotSortingElement);

    /* Creates a instruction dictionary
     * - Used in: User test, Step by Step
    */
    public abstract Dictionary<int, InstructionBase> UserTestInstructions(InstructionBase[] list);
}

