using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Algorithm : MonoBehaviour, IAlgorithmAble {

    [SerializeField]
    protected float seconds = 1f;

    protected int pivotValue, compareValue;
    protected bool isSortingComplete = false, isReadyForNextMove = false;

    protected Vector3 aboveHolder = new Vector3(0f, 0.1f, 0f); // y: 0.5f ?
    protected List<int> prevHighlight = new List<int>();

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
        if (pivotValue < compareValue)
            return pivotValue + " < " + compareValue;
        else if (pivotValue > compareValue)
            return pivotValue + " > " + compareValue;
        else
            return pivotValue + " = " + compareValue;
    }

    // ---------------------------- Overriden in the algorithm class which inherite this base class ----------------------------
    public abstract string GetAlgorithmName();
    public abstract void ResetSetup();

    //protected abstract string GetPseudoCode(int codeLine);

    // first instruction line of code
    public abstract int FirstInstructionCodeLine();
    // final instruction line of code
    public abstract int FinalInstructionCodeLine();

    public abstract IEnumerator Tutorial(GameObject[] list);
    public abstract void ExecuteOrder(InstructionBase instruction, int instructionNr, bool increment);
    public abstract Dictionary<int, InstructionBase> UserTestInstructions(InstructionBase[] list);
}

