using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InsertionSort))]
public class InsertionSortManager : AlgorithmManagerBase {

    private InsertionSort insertionSort;

    protected override void Awake()
    {
        insertionSort = GetComponent(typeof(InsertionSort)) as InsertionSort;
        base.Awake();
    }

    protected override Algorithm InstanceOfAlgorithm
    {
        get { return insertionSort; }
    }

    protected override int MovesNeeded
    {
        get { return 1; }
    }

    protected override int PrepareNextInstruction(InstructionBase instruction)
    {
        // Get the next instruction
        InsertionSortInstruction insertionSortInstruction = (InsertionSortInstruction)instruction;

        // Get the Sorting element
        InsertionSortElement sortingElement = elementManager.GetSortingElement(insertionSortInstruction.SortingElementID).GetComponent<InsertionSortElement>();

        // Hands out the next instruction
        sortingElement.ElementInstruction = insertionSortInstruction;

        // Give this sorting element permission to give feedback to progress to next intstruction
        sortingElement.NextMove = true;

        Debug.Log("Round " + userTestManager.CurrentInstructionNr + ": " + insertionSortInstruction.DebugInfo());

        return SkipOrHelp(insertionSortInstruction);
    }

    protected override int SkipOrHelp(InstructionBase instruction)
    {
        // Display help on blackboard
        if (false) // help enabled
        {

        }
        else
        {
            if (((InsertionSortInstruction)instruction).NextHolderID == Util.NO_DESTINATION) // skipping until next (user) move
                return 1;
        }
        return 0;
    }

    protected override HolderBase GetCorrectHolder(int index)
    {
        HolderBase getHolder = holderManager.GetHolder(index);
        return (getHolder != null) ? getHolder : insertionSort.PivotHolder;
    }

    protected override InstructionBase[] CopyFirstState(GameObject[] sortingElements)
    {
        InsertionSortInstruction[] elementStates = new InsertionSortInstruction[sortingElements.Length];

        for (int i = 0; i < sortingElements.Length; i++)
        {
            InsertionSortElement element = sortingElements[i].GetComponent<InsertionSortElement>();
            int sortingElementID = element.SortingElementID;
            int holderID = element.CurrentStandingOn.HolderID;
            int value = element.Value;
            bool isPivot = element.IsPivot;
            bool isCompare = element.IsCompare;
            bool isSorted = element.IsSorted;
            elementStates[i] = new InsertionSortInstruction(sortingElementID, holderID, Util.NO_DESTINATION, Util.INIT_INSTRUCTION, value, isPivot, isCompare, isSorted);
        }
        return elementStates;
    }

    protected override IEnumerator ExecuteOrder(InstructionBase instruction, bool increment)
    {
        InsertionSortInstruction inst = (InsertionSortInstruction)instruction;
        InsertionSortElement sortingElement = elementManager.GetSortingElement(inst.SortingElementID).GetComponent<InsertionSortElement>();
        sortingElement.IsPivot = inst.IsPivot;
        sortingElement.IsCompare = inst.IsCompare;
        sortingElement.IsSorted = inst.IsSorted;

        HolderBase moveToHolder = null;
        if (sortingElement.CurrentStandingOn.HolderID == inst.HolderID)
            moveToHolder = GetCorrectHolder(inst.NextHolderID);
        else
            moveToHolder = GetCorrectHolder(inst.HolderID);

        Debug.Log("Moving from holder " + sortingElement.CurrentStandingOn.HolderID + " to holder " + moveToHolder.HolderID);
        switch (inst.ElementInstruction)
        {
            case Util.INIT_INSTRUCTION:
                sortingElement.transform.position = moveToHolder.transform.position + new Vector3(0f, 1f, 0f);
                yield return new WaitForSeconds(insertionSort.Seconds);
                break;
            case Util.PIVOT_START_INST:
                sortingElement.transform.position = moveToHolder.transform.position + new Vector3(0f, 1f, 0f);
                yield return new WaitForSeconds(insertionSort.Seconds);
                break;
            case Util.PIVOT_END_INST:
                sortingElement.transform.position = moveToHolder.transform.position + new Vector3(0f, 1f, 0f);
                yield return new WaitForSeconds(insertionSort.Seconds);
                break;
            case Util.COMPARE_START_INST:
                break;
            case Util.COMPARE_END_INST:
                break;
            case Util.SWITCH_INST:
                sortingElement.transform.position = moveToHolder.transform.position + new Vector3(0f, 1f, 0f);
                yield return new WaitForSeconds(insertionSort.Seconds);
                break;
            case Util.EXECUTED_INST:
                break;
        }
    }
}
