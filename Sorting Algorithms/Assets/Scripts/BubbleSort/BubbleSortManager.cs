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

    protected override int PrepareNextInstruction(InstructionBase instruction)
    {
        bool gotSortingElement = !bubbleSort.SkipDict[Util.SKIP_NO_ELEMENT].Contains(instruction.Instruction);
        bool noDestination = bubbleSort.SkipDict[Util.SKIP_NO_DESTINATION].Contains(instruction.Instruction);

        if (gotSortingElement)
        {
            // Get the next two instructions
            BubbleSortInstruction bubbleInstruction = (BubbleSortInstruction)instruction;

            // Find the sorting elements for this instruction
            BubbleSortElement s1 = elementManager.GetSortingElement(bubbleInstruction.SortingElementID1).GetComponent<BubbleSortElement>();
            BubbleSortElement s2 = elementManager.GetSortingElement(bubbleInstruction.SortingElementID2).GetComponent<BubbleSortElement>();
            
            // Hand the instructions out
            s1.Instruction = bubbleInstruction;
            s2.Instruction = bubbleInstruction;

            // Give this sorting element permission to give feedback to progress to next intstruction
            if (instruction.Instruction == Util.SWITCH_INST)
            {
                s1.NextMove = true;
                s2.NextMove = true;
            }
        }

        if (algorithmSettings.Difficulty <= Util.BEGINNER)
        {
            BeginnerWait = true;
            StartCoroutine(algorithm.UserTestHighlightPseudoCode(instruction, gotSortingElement));
        }

        if (gotSortingElement && !noDestination)
            return 0;
        return 2;
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
            elementStates[i] = new BubbleSortInstruction(Util.INIT_INSTRUCTION, 0, Util.NO_VALUE, Util.NO_VALUE, Util.NO_VALUE, sortingElementID, Util.NO_VALUE, holderID, Util.NO_VALUE, value, Util.NO_VALUE, false, false);
// new BubbleSortInstruction(sortingElementID, Util.NO_DESTINATION, holderID, Util.NO_DESTINATION, value, Util.NO_VALUE, Util.NO_VALUE, Util.NO_VALUE, Util.INIT_INSTRUCTION, 0, isCompare, isSorted);
        }
        return elementStates;
    }
}
