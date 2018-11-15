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
        bool gotSortingElement = !insertionSort.SkipDict[Util.SKIP_NO_ELEMENT].Contains(instruction.Instruction);
        bool noDestination = insertionSort.SkipDict[Util.SKIP_NO_DESTINATION].Contains(instruction.Instruction);

        if (gotSortingElement)
        {
            InsertionSortInstruction insertionSortInstruction = (InsertionSortInstruction)instruction;
            // Get the Sorting element
            InsertionSortElement sortingElement = elementManager.GetSortingElement(insertionSortInstruction.SortingElementID).GetComponent<InsertionSortElement>();

            // Hands out the next instruction
            sortingElement.Instruction = insertionSortInstruction;

            // Give this sorting element permission to give feedback to progress to next intstruction
            if (instruction.Instruction == Util.PIVOT_START_INST || instruction.Instruction == Util.PIVOT_END_INST || instruction.Instruction == Util.SWITCH_INST)
                sortingElement.NextMove = true;
        }

        // Display help on blackboard
        if (Difficulty == Util.BEGINNER)
        {
            BeginnerWait = true;
            StartCoroutine(algorithm.UserTestDisplayHelp(instruction, gotSortingElement));
        }


        if (gotSortingElement && !noDestination)
            return 0;
        return 1;
        // No help displaye

        //if (noDestination)
        //    return 1;
        //return 0;
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
