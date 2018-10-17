

public class InsertionSortInstruction : InstructionBase {

    protected int sortingElementID, value, holderID, nextHolderID, i, j;
    protected bool isPivot;

    public InsertionSortInstruction(int sortingElementID, int holderID, int nextHolderID, int i, int j, string instruction, int value, bool isPivot, bool isCompare, bool isSorted)
        : base(instruction, isCompare, isSorted)
    {
        this.sortingElementID = sortingElementID;
        this.value = value;
        this.holderID = holderID;
        this.nextHolderID = nextHolderID; // -1: not going anywhere
        this.isPivot = isPivot;
        // For step by step tutorial
        this.i = i;
        this.j = j;
    }

    public int SortingElementID
    {
        get { return sortingElementID; }
    }

    public int Value
    {
        get { return value; }
    }

    public int HolderID
    {
        get { return holderID; }
        set { holderID = value; }
    }

    public int NextHolderID
    {
        get { return nextHolderID; }
        set { nextHolderID = value; }
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

    public int I
    {
        get { return i; }
    }

    public int J
    {
        get { return j; }
    }

    // Debug checker
    public override string DebugInfo()
    {
        return "[" + value + "]: " + holderID + " -> " + Util.TranslateNextHolder(nextHolderID) + ", P=" + isPivot + ", C=" + isCompare + ", S=" + isSorted + ", Inst: " + instruction;
    }

}
