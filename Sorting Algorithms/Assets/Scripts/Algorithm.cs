using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Algorithm : MonoBehaviour, IAlgorithmAble {

    [SerializeField]
    protected float seconds = 1f;

    // value1 <- pivot value, value2 <- compare value (usually)
    protected int value1 = Util.INIT_STATE, value2 = Util.INIT_STATE - 1;
    protected bool isSortingComplete = false, isReadyForNextMove = false;

    protected Vector3 aboveHolder = new Vector3(0f, 0.5f, 0f);
    protected List<int> prevHighlight = new List<int>();
    protected Dictionary<string, List<string>> skipDict = new Dictionary<string, List<string>>();

    protected PseudoCodeViewer pseudoCodeViewer, pseudoCodeViewerFixed;

    protected virtual void Awake()
    {
        //displayUnitManager = displayUnitManagerObj.GetComponent(typeof(DisplayUnitManager)) as DisplayUnitManager;
        //displayUnitManager.Algorithm = this;
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

    public bool IsReadyForNextMove
    {
        get { return isReadyForNextMove; }
        set { isReadyForNextMove = value; }
    }

    // For visuals on the blackboard
    public virtual string GetComparison()
    {
        if (value1 < value2)
            return value1 + " < " + value2;
        else if (value1 > value2)
            return value1 + " > " + value2;
        else
            return value1 + " = " + value2;
    }

    public Vector3 AboveHolder
    {   
        set { aboveHolder = value; }
    }

    public PseudoCodeViewer PseudoCodeViewer
    {
        get { return pseudoCodeViewer; }
        set { pseudoCodeViewer = value; pseudoCodeViewer.SetAlgorithm(this); }
    }

    public PseudoCodeViewer PseudoCodeViewerFixed
    {
        get { return pseudoCodeViewerFixed; }
        set { pseudoCodeViewerFixed = value; pseudoCodeViewerFixed.SetAlgorithm(this); }
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
        skipDict.Add(Util.SKIP_NO_ELEMENT, new List<string>());
        skipDict[Util.SKIP_NO_ELEMENT].Add(Util.FIRST_INSTRUCTION);
        skipDict[Util.SKIP_NO_ELEMENT].Add(Util.FIRST_LOOP);
        skipDict[Util.SKIP_NO_ELEMENT].Add(Util.UPDATE_LOOP_INST);
        skipDict[Util.SKIP_NO_ELEMENT].Add(Util.END_LOOP_INST);
        skipDict[Util.SKIP_NO_ELEMENT].Add(Util.FINAL_INSTRUCTION);
        skipDict[Util.SKIP_NO_ELEMENT].Add(Util.INCREMENT_VAR_I);
        skipDict[Util.SKIP_NO_ELEMENT].Add(Util.SET_VAR_J);
        skipDict[Util.SKIP_NO_ELEMENT].Add(Util.UPDATE_VAR_J);
    }

    // ---------------------------- Overriden in the algorithm class which inherite this base class ----------------------------
    public abstract string GetAlgorithmName();
    public abstract void ResetSetup();

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

    public abstract IEnumerator Tutorial(GameObject[] list);
    public abstract void ExecuteOrder(InstructionBase instruction, int instructionNr, bool increment);
    public abstract Dictionary<int, InstructionBase> UserTestInstructions(InstructionBase[] list);
}

