

public class MergeSortInstruction : InstructionSingleElementUpdate {

    public MergeSortInstruction(string instruction, int instructionNr, int i, int j, int k, int sortingElementID, int value, bool isCompare, bool isSorted, int holderID, int nextHolderID)
        : base(instruction, instructionNr, i, j, k, sortingElementID, holderID, nextHolderID, value, isCompare, isSorted)
    {

    }

    public override string DebugInfo()
    {
        throw new System.NotImplementedException();
    }
}
