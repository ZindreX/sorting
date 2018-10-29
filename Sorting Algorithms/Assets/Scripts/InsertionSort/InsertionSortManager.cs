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

    private List<string> skipInst = new List<string>() { Util.FIRST_INSTRUCTION, Util.FINAL_INSTRUCTION };
    protected override int PrepareNextInstruction(InstructionBase instruction)
    {
        //if (skipInst.Contains(instruction.ElementInstruction))
        //    return 1;


        // Get the next instruction
        InsertionSortInstruction insertionSortInstruction = (InsertionSortInstruction)instruction;
        Debug.Log("Round " + userTestManager.CurrentInstructionNr + ": " + insertionSortInstruction.DebugInfo());
        bool gotSortingElement = !skipInst.Contains(instruction.ElementInstruction);

        if (gotSortingElement)
        {
            // Get the Sorting element
            InsertionSortElement sortingElement = elementManager.GetSortingElement(insertionSortInstruction.SortingElementID).GetComponent<InsertionSortElement>();

            // Hands out the next instruction
            sortingElement.ElementInstruction = insertionSortInstruction;

            // Give this sorting element permission to give feedback to progress to next intstruction
            sortingElement.NextMove = true;
        }

        // Display help on blackboard
        if (HelpEnabled) // replace with teachingMode == Util.Beginner
        {
            ((InsertionSort)algorithm).UserTestDisplayHelp(instruction, gotSortingElement);
            if (gotSortingElement)
                return 0;
            return 1;
        }
        else if (insertionSortInstruction.NextHolderID == Util.NO_DESTINATION) // skipping until next (user) move
                return 1;
        else
            return 0;
    }

    public override HolderBase GetCorrectHolder(int index)
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
            elementStates[i] = new InsertionSortInstruction(sortingElementID, holderID, Util.NO_DESTINATION, Util.NO_VALUE, Util.NO_VALUE, Util.INIT_INSTRUCTION, value, isPivot, isCompare, isSorted);
        }
        return elementStates;
    }
}
