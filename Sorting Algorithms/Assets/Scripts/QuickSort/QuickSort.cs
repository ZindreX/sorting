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

    public override Dictionary<int, string> PseudoCode
    {
        get { return pseudoCode; }
        set { pseudoCode = value; }
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
}
