

public class BubbleSortInstruction : InstructionDoubleElementUpdate {

    /* -------------------------------------------- Instruction class for user test --------------------------------------------
     * Sorting element 1: sortindElementID1 + holderID1 + value1 
     * Sorting element 2: sortindElementID2 + holderID2 + value2
     * [ Sorting Element 1 ] [ Sorting Element 2 ]
     * 
     * > Instructions   : element value status                      : note
     * - Compare_start  : mix                                       : compares the values
     * - Compare_end    : sorting element 1 is the smallest element : evaluation/switching is complete
     * - Switch_inst    : sorting element 2 is the smalled element  : switching in process
     * 
    */

    public BubbleSortInstruction(string instruction, int instructionNr, int i, int j, int k, int seID1, int seID2, int hID1, int hID2, int value1, int value2, bool isCompare, bool isSorted)
        : base(instruction, instructionNr, i, j, k, seID1, seID2, hID1, hID2, value1, value2, isCompare, isSorted)
    {

    }

    public int SwitchToHolder(int sortingElementID)
    {
        return (sortingElementID == SortingElementID1) ? holderID2 : holderID1;
    }

    public int GetHolderFor(int sortingElementID)
    {
        return (sortingElementID == SortingElementID1) ? holderID1 : holderID2;
    }

    public bool IsElementSorted(int sortingElementID)
    {
        return IsSorted && (holderID1 == 0 && sortingElementID == SortingElementID1 || (sortingElementID == sortingElementID2));
    }

    public override string DebugInfo()
    {
        switch (instruction)
        {
            case Util.COMPARE_START_INST: return base.DebugInfo() + ": [" + value1 + "] ? [" + value2 + "] ::: [" + sortingElementID1 + " | " + holderID1 + "] ? [" + sortingElementID2 + " | " + HolderID2 + "], C=" + isCompare + ", S=" + isSorted;
            case Util.SWITCH_INST: return base.DebugInfo() + ": [" + value1 + "] <--> [" + value2 + "] ::: [" + sortingElementID1 + " | " + holderID1 + "] <--> [" + sortingElementID2 + " | " + HolderID2 + "], C=" + isCompare + ", S=" + isSorted;
            case Util.COMPARE_END_INST: return base.DebugInfo() + ": [" + value1 + "] < [" + value2 + "] ::: [" + sortingElementID1 + " | " + holderID1 + "] | [" + sortingElementID2 + " | " + HolderID2 + "], C=" + isCompare + ", S=" + isSorted;
            default: return instruction + " has no case.";
        }
    }
}
