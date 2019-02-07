

public class InsertionSortInstruction : InstructionSingleElement {

    protected bool isPivot;

    public InsertionSortInstruction(string instruction, int instructionNr, int i, int j, int k, int sortingElementID, int value, bool isCompare, bool isSorted, bool isPivot, int holderID, int nextHolderID)
        : base(instruction, instructionNr, i, j, k, sortingElementID, holderID, nextHolderID, value, isCompare, isSorted)
    {
        this.isPivot = isPivot;
    }

    public string GetCombinedID()
    {
        return sortingElementID + "/" + holderID;
    }

    public bool IsPivot
    {
        get { return isPivot; }
        set { isPivot = value; }
    }

    // Debug checker
    public override string DebugInfo()
    {
        return base.DebugInfo() + "| : " + holderID + " -> " + UtilSort.TranslateNextHolder(nextHolderID) + ", P=" + isPivot;
    }

}
