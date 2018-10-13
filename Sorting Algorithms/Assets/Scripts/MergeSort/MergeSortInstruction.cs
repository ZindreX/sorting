

public class MergeSortInstruction : InstructionBase
{
    public MergeSortInstruction(int sortingElementID, int holderID, int nextHolderID, string instruction, int value, bool isCompare, bool isSorted)
        : base(sortingElementID, holderID, value, instruction, isCompare, isSorted)
    {
    }

    public override string DebugInfo()
    {
        throw new System.NotImplementedException();
    }
}
