

public class MergeSortInstruction : InstructionSingleElement {

    public MergeSortInstruction(string instruction, int instructionNr, int i, int j, int k, int sortingElementID, int holderID, int nextHolderID, int value, bool isCompare, bool isSorted)
    : base(instruction, instructionNr, i, j, k, sortingElementID, holderID, nextHolderID, value, isCompare, isSorted)
    {

    }

    public override string DebugInfo()
    {
        return base.DebugInfo() + " | HolderID = " + holderID + ", Next holder ID = " + nextHolderID;
    }
}
