using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BubbleSort))]
public class BubbleSortManager : AlgorithmManagerBase {

    private BubbleSort bubbleSort;

    protected override void Awake()
    {
        bubbleSort = GetComponent(typeof(BubbleSort)) as BubbleSort;
        base.Awake();
    }

    protected override Algorithm InstanceOfAlgorithm
    {
        get { return bubbleSort; }
    }

    protected override int MovesNeeded
    {
        get { return 2; }
    }

    private List<string> skipInst = new List<string>() { Util.FIRST_INSTRUCTION, Util.UPDATE_LOOP_INST, Util.END_LOOP_INST, Util.FINAL_INSTRUCTION };
    protected override int PrepareNextInstruction(InstructionBase instruction)
    {
        if (skipInst.Contains(instruction.Instruction))
            return 2;

        // Get the next two instructions
        BubbleSortInstruction bubbleInstruction = (BubbleSortInstruction)instruction;

        // Find the sorting elements for this instruction
        BubbleSortElement s1 = elementManager.GetSortingElement(bubbleInstruction.SortingElementID1).GetComponent<BubbleSortElement>();
        BubbleSortElement s2 = elementManager.GetSortingElement(bubbleInstruction.SortingElementID2).GetComponent<BubbleSortElement>();
            
        // Hand the instructions out
        s1.Instruction = bubbleInstruction;
        s2.Instruction = bubbleInstruction;

        // Give next move permission
        s1.NextMove = true;
        s2.NextMove = true;

        Debug.Log("Round " + userTestManager.CurrentInstructionNr + ": " + bubbleInstruction.DebugInfo());


        if (false) // help enabled
        {

        }
        else
        {
            if (Util.skipAbleInstructions.Contains(instruction.Instruction)) // skipping until next user move
                return 2;
        }
        return 0;
    }

    protected override InstructionBase[] CopyFirstState(GameObject[] sortingElements)
    {
        BubbleSortInstruction[] elementStates = new BubbleSortInstruction[sortingElements.Length];

        for (int i = 0; i < sortingElements.Length; i++)
        {
            BubbleSortElement element = sortingElements[i].GetComponent<BubbleSortElement>();
            int sortingElementID = element.SortingElementID;
            int holderID = element.CurrentStandingOn.HolderID;
            int value = element.Value;
            bool isCompare = element.IsCompare;
            bool isSorted = element.IsSorted;
            elementStates[i] = new BubbleSortInstruction(sortingElementID, Util.NO_DESTINATION, holderID, Util.NO_DESTINATION, value, Util.NO_VALUE, Util.NO_VALUE, Util.NO_VALUE, Util.INIT_INSTRUCTION, isCompare, isSorted);
        }
        return elementStates;
    }
}
