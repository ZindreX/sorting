

public class InsertionSortInstruction : InstructionBase {

    private int nextHolderID;
    private bool isPivot;

    public InsertionSortInstruction(int sortingElementID, int holderID, int nextHolderID, string instruction, int value, bool isPivot, bool isCompare, bool isSorted)
        : base(sortingElementID, holderID, value, instruction, isCompare, isSorted)
    {
        this.nextHolderID = nextHolderID; // -1: not going anywhere
        this.isPivot = isPivot;
    }

    public int NextHolderID
    {
        get { return nextHolderID; }
        set { nextHolderID = value; }
    }

    public bool IsPivot
    {
        get { return isPivot; }
        set { isPivot = value; }
    }

    // Debug checker
    public override string DebugInfo()
    {
        return "[" + value + "]: " + holderID + " -> " + Util.TranslateNextHolder(nextHolderID) + ", P=" + isPivot + ", C=" + isCompare + ", S=" + isSorted + ", Inst: " + instruction;
    }

}
