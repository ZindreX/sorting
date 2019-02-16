using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSort : SortAlgorithm {

    [SerializeField]
    private GameObject pivotHolderObject;

    private Dictionary<int, string> pseudoCode;

    public override string AlgorithmName
    {
        get { return UtilSort.QUICK_SORT; }
    }

    public override string CollectLine(int lineNr)
    {
        throw new System.NotImplementedException();
    }

    private string PseudoCode(int lineNr, int i, int j, bool increment)
    {
        throw new System.NotImplementedException();
    }

    public override int FirstInstructionCodeLine()
    {
        return 0;
    }

    public override int FinalInstructionCodeLine()
    {
        return 0;
    }

    public override void Specials(string method, int number, bool activate)
    {
        switch (method)
        {
            case "Somemethod": FirstInstructionCodeLine(); break; // example: some void method
        }
    }

    public override void ResetSetup()
    {
        throw new System.NotImplementedException();
    }

    public GameObject[] QuickSortStandard(GameObject[] list, int pivotPos)
    {
        //GameObject[] lessThanPivot = new GameObject[];

        return list;
    }

    public override IEnumerator Demo(GameObject[] list)
    {
        Debug.Log("TODO: implement tutorial");
        yield return new WaitForSeconds(seconds);
    }

    public override Dictionary<int, InstructionBase> UserTestInstructions(InstructionBase[] list)
    {
        throw new System.NotImplementedException();
    }

    public override void ExecuteStepByStepOrder(InstructionBase instruction, bool gotElement, bool increment)
    {
        throw new System.NotImplementedException();
    }

    public override IEnumerator UserTestHighlightPseudoCode(InstructionBase instruction, bool gotSortingElement)
    {
        throw new System.NotImplementedException();
    }
}
