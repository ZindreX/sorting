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

    private string PseudoCode(int lineNr, int i, int j, bool increment)
    {
        switch (lineNr)
        {
            case 0: return "InsertionSort( List<int> list )";
            case 1: return "i = 1";
            case 2: return "while ( " + i + " < " + GetComponent<AlgorithmManagerBase>().NumberOfElements + " )";
            case 3: return "    " + j + " = " + i + " - 1";
            case 4: return "    while ( " + j + " >= 0 and " + value1 + " < " + value2 + " )";
            case 5: return "        swap " + value1 + " and " + value2;
            case 6: return "        " + j + " = " + (j + 1) + " - 1";
            case 7: return "    end while";
            case 8: return "    " + i + " = " + (i - 1) + " + 1";
            case 9: return "end while";
            default: return "X";
        }
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
