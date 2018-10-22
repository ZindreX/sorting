using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PseudoCodeViewer))]
public abstract class Algorithm : MonoBehaviour, IAlgorithmAble {

    [SerializeField]
    protected float seconds = 1f;

    // value1 <- pivot value, value2 <- compare value (usually)
    protected int value1 = Util.INIT_STATE, value2 = Util.INIT_STATE - 1;
    protected bool isSortingComplete = false, isReadyForNextMove = false;

    protected Vector3 aboveHolder = new Vector3(0f, 0.5f, 0f);
    protected List<int> prevHighlight = new List<int>();

    [SerializeField]
    protected GameObject pseudoCodeViewerObj;
    protected PseudoCodeViewer pseudoCodeViewer;

    protected virtual void Awake()
    {
        pseudoCodeViewer = pseudoCodeViewerObj.GetComponent(typeof(PseudoCodeViewer)) as PseudoCodeViewer;
        pseudoCodeViewer.SetAlgorithm(this); // just pass seconds maybe?
    }

    public bool IsSortingComplete
    {
        get { return isSortingComplete; }
        set { isSortingComplete = value; }
    }

    public float Seconds
    {
        get { return seconds; }
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

    // ---------------------------- Overriden in the algorithm class which inherite this base class ----------------------------
    public abstract string GetAlgorithmName();
    public abstract void ResetSetup();


    // Collects one line of code at a time and rewrites it into pseudocode | another methods must be created aswell *depends on the algorithm
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

