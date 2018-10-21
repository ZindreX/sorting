using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PseudoCodeViewer))]
public abstract class Algorithm : MonoBehaviour, IAlgorithmAble {

    [SerializeField]
    protected float seconds = 1f;

    // value1 <- pivot value, value2 <- compare value (usually)
    protected int value1, value2;
    protected bool isSortingComplete = false, isReadyForNextMove = false;

    protected Vector3 aboveHolder = new Vector3(0f, 0.1f, 0f); // y: 0.5f ?
    protected List<int> prevHighlight = new List<int>();


    [SerializeField]
    protected GameObject pseudoCodeViewerObj;
    protected PseudoCodeViewer pseudoCodeViewer;

    protected virtual void Awake()
    {
        pseudoCodeViewer = pseudoCodeViewerObj.GetComponent(typeof(PseudoCodeViewer)) as PseudoCodeViewer;
        pseudoCodeViewer.SetAlgorithm(this);
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

    //protected abstract string GetPseudoCode(int codeLine);

    // first instruction line of code
    public abstract int FirstInstructionCodeLine();
    // final instruction line of code
    public abstract int FinalInstructionCodeLine();

    //public List<string> GetPseudoCode() { return new List<string>(); } // change later

    public abstract IEnumerator Tutorial(GameObject[] list);
    public abstract void ExecuteOrder(InstructionBase instruction, int instructionNr, bool increment);
    public abstract Dictionary<int, InstructionBase> UserTestInstructions(InstructionBase[] list);
}

