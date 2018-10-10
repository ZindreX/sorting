using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Algorithm : MonoBehaviour, IAlgorithmAble {


    [SerializeField]
    protected float seconds = 1f;

    protected int pivotValue, compareToValue;

    protected bool isSortingComplete = false, isReadyForNextMove = false;

    public bool IsSortingComplete
    {
        get { return isSortingComplete; }
        set { isSortingComplete = value; }
    }

    public bool IsReadyForNextMove
    {
        get { return isReadyForNextMove; }
        set { isReadyForNextMove = value; }
    }

    // For visuals on the blackboard
    public string GetComparison()
    {
        if (pivotValue < compareToValue)
            return pivotValue + " < " + compareToValue;
        else if (pivotValue > compareToValue)
            return pivotValue + " > " + compareToValue;
        else
            return pivotValue + " = " + compareToValue;
    }

    // ---------------------------- Overriden in the algorithm class which inherite this base class ----------------------------
    public abstract string GetAlgorithmName();
    public abstract void ResetSetup();
    public abstract Dictionary<int, string> PseudoCode { get; set; }

    public abstract IEnumerator Tutorial(GameObject[] list);
    public abstract Dictionary<int, InstructionBase> UserTestInstructions(InstructionBase[] list);
}

