

public class MergeSortInstruction : InstructionBase {

    public MergeSortInstruction(int sortingElementID, int holderID, int nextHolderID, int i, int j, string instruction, int instructionNr, int value, bool isCompare, bool isSorted)
        : base(instruction, instructionNr, i, j, isCompare, isSorted)
    {

    }

    public override string DebugInfo()
    {
        throw new System.NotImplementedException();
    }
}
