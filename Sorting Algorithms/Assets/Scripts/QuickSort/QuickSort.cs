using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSort : Algorithm {

    [SerializeField]
    private GameObject pivotHolderObject;

    private Dictionary<int, string> pseudoCode;

    public override string GetAlgorithmName()
    {
        return Util.QUICK_SORT;
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
        return 1;
    }

    public override int FinalInstructionCodeLine()
    {
        return 9;
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

    public override IEnumerator Tutorial(GameObject[] list)
    {
        Debug.Log("TODO: implement tutorial");
        yield return new WaitForSeconds(seconds);
    }

    public override Dictionary<int, InstructionBase> UserTestInstructions(InstructionBase[] list)
    {
        throw new System.NotImplementedException();
    }

    public override void ExecuteOrder(InstructionBase instruction, int instructionNr, bool increment)
    {

    }
}
