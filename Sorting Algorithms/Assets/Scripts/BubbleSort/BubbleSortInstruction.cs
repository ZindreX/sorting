

public class BubbleSortInstruction : InstructionBase {

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

    private int sortingElementID1, sortingElementID2, holderID1, holderID2, value1, value2;

    public BubbleSortInstruction(int seID1, int seID2, int hID1, int hID2, int value1, int value2, string instruction, bool isCompare, bool isSorted)
        : base(instruction, isCompare, isSorted)
    {
        sortingElementID1 = seID1;
        sortingElementID2 = seID2;
        holderID1 = hID1;
        holderID2 = hID2;
        this.value1 = value1;
        this.value2 = value2;
    }

    public int SortingElementID1
    {
        get { return sortingElementID1; }
    }

    public int SortingElementID2
    {
        get { return sortingElementID2; }
    }

    public int HolderID1
    {
        get { return holderID1; }
        set { holderID1 = value; }
    }

    public int HolderID2
    {
        get { return holderID2; }
    }

    public int Value1
    {
        get { return value1; }
    }

    public int Value2
    {
        get { return value2; }
    }

    public int SwitchToHolder(int sortingElementID)
    {
        return (sortingElementID == SortingElementID1) ? holderID2 : holderID1;
    }

    // Checks whether the parameter ID is the 1st element in the instruction
    public bool IsMain(int sortingElementID)
    {
        return SortingElementID1 == sortingElementID;
    }

    public int GetHolderFor(int sortingElementID)
    {
        return (sortingElementID == SortingElementID1) ? holderID1 : holderID2;
    }

    public int GetValueFor(int sortingElementID, bool other)
    {
        if (other)
            return (sortingElementID == SortingElementID1) ? value2 : value1;
        return (sortingElementID == SortingElementID1) ? value1 : value2;
    }

    public override string DebugInfo()
    {
        switch (instruction)
        {
            case Util.COMPARE_START_INST: return "[" + value1 + "] ? [" + value2 + "] ::: [" + sortingElementID1 + " | " + holderID1 + "] ? [" + sortingElementID2 + " | " + HolderID2 + "], C=" + isCompare + ", S=" + isSorted + ", Inst: " + instruction;
            case Util.SWITCH_INST: return "[" + value1 + "] <--> [" + value2 + "] ::: [" + sortingElementID1 + " | " + holderID1 + "] <--> [" + sortingElementID2 + " | " + HolderID2 + "], C=" + isCompare + ", S=" + isSorted + ", Inst: " + instruction;
            case Util.COMPARE_END_INST: return "[" + value1 + "] < [" + value2 + "] ::: [" + sortingElementID1 + " | " + holderID1 + "] | [" + sortingElementID2 + " | " + HolderID2 + "], C=" + isCompare + ", S=" + isSorted + ", Inst: " + instruction;
            default: return instruction + " has no case.";
        }
    }
}
