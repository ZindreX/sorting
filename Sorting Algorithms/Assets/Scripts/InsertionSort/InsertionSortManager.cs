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

    protected override int PrepareNextInstruction(int instNr)
    {
        // Get the next instruction
        InsertionSortInstruction instruction = (InsertionSortInstruction)userTestManager.GetInstruction(instNr);

        // Get the Sorting element
        InsertionSortElement sortingElement = elementManager.GetSortingElement(instruction.SortingElementID).GetComponent<InsertionSortElement>();

        // Hands out the next instruction
        sortingElement.ElementInstruction = instruction;

        // Give this sorting element permission to give feedback to progress to next intstruction
        sortingElement.NextMove = true;

        Debug.Log("Round " + instNr + ": " + userTestManager.GetInstruction(instNr).DebugInfo());

        return SkipOrHelp(instruction);
    }

    protected override int SkipOrHelp(InstructionBase instruction)
    {
        // Display help on blackboard
        if (false) // help enabled
        {

        }
        else
        {
            if (((InsertionSortInstruction)instruction).NextHolderID == Util.NO_INSTRUCTION) // skipping until next (user) move
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
            elementStates[i] = new InsertionSortInstruction(sortingElementID, holderID, Util.NO_INSTRUCTION, Util.INIT_INSTRUCTION, value, isPivot, isCompare, isSorted);
        }
        return elementStates;
    }

    // Add instructions for later use on blackboard (?)
    protected override Dictionary<int, string> CreatePseudoCode()
    {
        Dictionary<int, string> pseudoCode = new Dictionary<int, string>();
        pseudoCode.Add(0, "i <- 1");
        pseudoCode.Add(1, "while (i < length(list))");
        pseudoCode.Add(2, "   j <- i - 1");
        pseudoCode.Add(3, "   while (j > 0 and list[i] < list[j])");
        pseudoCode.Add(4, "       swap list[i] and list[j]");
        pseudoCode.Add(5, "       j -= 1");
        pseudoCode.Add(6, "   end while");
        pseudoCode.Add(7, "   i += 1");
        pseudoCode.Add(8, "end while");
        return pseudoCode;
    }
}
