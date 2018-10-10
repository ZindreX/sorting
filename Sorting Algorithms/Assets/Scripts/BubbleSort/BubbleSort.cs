using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleSort : Algorithm {

    /* --------------------------------------------------- Bubble Sort --------------------------------------------------- 
     * Comparing 2 elements at a time
     * Moves the biggest until it reaches the end of the list
    */
    private Dictionary<int, string> pseudoCode;

    public override string GetAlgorithmName()
    {
        return Util.BUBBLE_SORT;
    }

    public override Dictionary<int, string> PseudoCode
    {
        get { return pseudoCode; }
        set { pseudoCode = value; }
    }

    public override void ResetSetup()
    {
        Debug.Log("Nothing to reset (?)");
    }

    #region Bubble Sort 1
    public static GameObject[] BubbleSort1(GameObject[] list)
    {
        for (int i = 0; i < list.Length; i++)
        {
            for (int j = 0; j < list.Length - i - 1; j++)
            {
                GameObject pivot = list[j];

                if (pivot.GetComponent<SortingElementBase>().Value > list[j + 1].GetComponent<SortingElementBase>().Value)
                {
                    list[j] = list[j + 1];
                    list[j + 1] = pivot;
                }
            }
        }
        return list;
    }
    #endregion

    #region Bubble Sort 2
    public static GameObject[] BubbleSort2(GameObject[] list)
    {
        bool exchanges = true;
        int N = list.Length;
        while (N > 0 && exchanges)
        {
            exchanges = false;
            for (int i = 0; i < list.Length - 1; i++)
            {
                if (list[i].GetComponent<SortingElementBase>().Value > list[i + 1].GetComponent<SortingElementBase>().Value)
                {
                    exchanges = true;
                    GameObject temp = list[i];
                    list[i] = list[i + 1];
                    list[i + 1] = temp;
                }
            }
            N -= 1;
        }
        return list;
    }
    #endregion

    #region Bubble Sort: All Moves Tutorial (Visual)
    public override IEnumerator Tutorial(GameObject[] list)
    {
        int N = list.Length;
        for (int i=0; i < N; i++)
        {
            for (int j = 0; j < N - i - 1; j++)
            {
                // Choose sorting elements to compare
                BubbleSortElement p1 = list[j].GetComponent<BubbleSortElement>();
                BubbleSortElement p2 = list[j + 1].GetComponent<BubbleSortElement>();

                // Change status
                p1.IsCompare = true;
                p2.IsCompare = true;

                // Update color on holders
                p1.transform.position += new Vector3(0f, 0.1f, 0f);
                p2.transform.position += new Vector3(0f, 0.1f, 0f);

                // Get their values
                pivotValue = p1.Value;
                compareToValue = p2.Value;

                // Update blackboard
                yield return new WaitForSeconds(seconds * 2);

                if (pivotValue > compareToValue)
                {
                    // Switch their positions
                    GameObject temp = list[j];
                    GameObject temp2 = list[j + 1];
                    Vector3 pos1 = list[j].transform.position, pos2 = list[j + 1].transform.position; // p1 & p2 old positions
                    p1.transform.position = pos2;
                    list[j] = temp2;

                    p2.transform.position = pos1;
                    list[j + 1] = temp;
                    yield return new WaitForSeconds(seconds);
                }
                p1.IsCompare = false;
                p1.CurrentStandingOn.CurrentColor = Util.STANDARD_COLOR;
                p2.IsCompare = false;
                p2.CurrentStandingOn.CurrentColor = Util.STANDARD_COLOR;
                yield return new WaitForSeconds(seconds);
            }
            list[N - i - 1].GetComponent<BubbleSortElement>().IsSorted = true;
            list[N - i - 1].transform.position += new Vector3(0f, 0.1f, 0f);
            yield return new WaitForSeconds(seconds);
        }
        isSortingComplete = true;
    }
    #endregion

    #region Bubble Sort: User Test
    public override Dictionary<int, InstructionBase> UserTestInstructions(InstructionBase[] list)
    {
        int N = list.Length, instructionNr = 0;

        Dictionary<int, InstructionBase> instructions = new Dictionary<int, InstructionBase>();

        for (int i = 0; i < N; i++)
        {
            for (int j = 0; j < N - i - 1; j++)
            {
                // Choose sorting elements to compare
                BubbleSortInstruction comparison = MakeInstruction(list[j], list[j + 1], Util.COMPARE_START_INST, true, false);

                // Add this instruction
                instructions.Add(instructionNr++, comparison);

                if (comparison.Value1 > comparison.Value2)
                {
                    // Switch their positions
                    instructions.Add(instructionNr++, MakeInstruction(list[j], list[j + 1], Util.SWITCH_INST, true, false));

                    int holder1 = ((BubbleSortInstruction)list[j]).HolderID1;
                    int holder2 = ((BubbleSortInstruction)list[j + 1]).HolderID1;
                    InstructionBase temp = list[j];
                    list[j] = list[j + 1];
                    list[j + 1] = temp;
                    // Update holderID
                    ((BubbleSortInstruction)list[j]).HolderID1 = holder1;
                    ((BubbleSortInstruction)list[j + 1]).HolderID1 = holder2;
                }
                // Add this instruction
                instructions.Add(instructionNr++, MakeInstruction(list[j], list[j + 1], Util.COMPARE_END_INST, false, false));
            }
            list[N - i - 1].IsSorted = true;
            instructions[instructions.Count - i - 1].IsSorted = true;
        }
        return instructions;
    }
    #endregion


    private BubbleSortInstruction MakeInstruction(InstructionBase inst1, InstructionBase inst2, string instruction, bool isCompare, bool isSorted)
    {
        int seID1 = ((BubbleSortInstruction)inst1).SortingElementID1;
        int seID2 = ((BubbleSortInstruction)inst2).SortingElementID1;
        int hID1 = ((BubbleSortInstruction)inst1).HolderID1;
        int hID2 = ((BubbleSortInstruction)inst2).HolderID1;
        int value1 = ((BubbleSortInstruction)inst1).Value1;
        int value2 = ((BubbleSortInstruction)inst2).Value1;
        return new BubbleSortInstruction(seID1, seID2, hID1, hID2, value1, value2, instruction, isCompare, isSorted);
    }

}
