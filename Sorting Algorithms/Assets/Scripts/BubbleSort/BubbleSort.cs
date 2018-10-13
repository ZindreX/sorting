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
    public override Dictionary<int, InstructionBase> UserTestInstructions(InstructionBase[] sortingElements)
    {
        int N = sortingElements.Length, instructionNr = 0;

        Dictionary<int, InstructionBase> instructions = new Dictionary<int, InstructionBase>();

        for (int i = 0; i < N; i++)
        {
            for (int j = 0; j < N - i - 1; j++)
            {
                // Choose sorting elements to compare
                BubbleSortInstruction c1 = new BubbleSortInstruction(sortingElements[j].SortingElementID, sortingElements[j].HolderID, Util.NO_DESTINATION, sortingElements[j].Value, Util.COMPARE_START_INST, true, false);
                BubbleSortInstruction c2 = new BubbleSortInstruction(sortingElements[j+1].SortingElementID, sortingElements[j+1].HolderID, Util.NO_DESTINATION, sortingElements[j+1].Value, Util.COMPARE_START_INST, true, false);

                // Add this instruction
                instructions.Add(instructionNr++, c1);
                instructions.Add(instructionNr++, c2);

                if (c1.Value > c2.Value)
                {
                    // Switch their positions
                    int holder1 = c1.HolderID;
                    int holder2 = c2.HolderID;

                    instructions.Add(instructionNr++, new BubbleSortInstruction(c1.SortingElementID, c1.HolderID, c2.HolderID, sortingElements[j].Value, Util.SWITCH_INST, true, false));
                    instructions.Add(instructionNr++, new BubbleSortInstruction(c1.SortingElementID, c2.HolderID, c1.HolderID, sortingElements[j+1].Value, Util.SWITCH_INST, true, false));

                    InstructionBase temp = sortingElements[j];
                    sortingElements[j] = sortingElements[j + 1];
                    sortingElements[j + 1] = temp;

                    // Update holderID
                    sortingElements[j].HolderID = holder1;
                    sortingElements[j + 1].HolderID = holder2;
                }
                // Add this instruction
                instructions.Add(instructionNr++, new BubbleSortInstruction(sortingElements[j].SortingElementID, sortingElements[j].HolderID, Util.NO_DESTINATION, sortingElements[j].Value, Util.COMPARE_END_INST, false, false));
                instructions.Add(instructionNr++, new BubbleSortInstruction(sortingElements[j+1].SortingElementID, sortingElements[j+1].HolderID, Util.NO_DESTINATION, sortingElements[j+1].Value, Util.COMPARE_END_INST, false, false));
            }
            //sortingElements[N - i - 1].IsSorted = true;
            instructions[instructions.Count - 1].IsSorted = true;
        }
        for (int x=0; x < instructions.Count; x++)
        {
            Debug.Log(instructions[x].DebugInfo());
        }
        return instructions;
    }
    #endregion
}
