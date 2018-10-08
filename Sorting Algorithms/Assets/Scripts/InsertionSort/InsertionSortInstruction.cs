﻿

public class InsertionSortInstruction : InstructionBase {


    private int sortingElementID, value, holderID, nextHolderID;
    private bool isPivot;

    public InsertionSortInstruction(int sortingElementID, int holderID, int nextHolderID, string instruction, int value, bool isPivot, bool isCompare, bool isSorted)
        : base(instruction, isCompare, isSorted)
    {
        this.sortingElementID = sortingElementID;
        this.value = value;
        this.holderID = holderID;
        this.nextHolderID = nextHolderID; // -1: not going anywhere
        this.isPivot = isPivot;
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

    // Debug checker
    public override string DebugInfo()
    {
        return "[" + value + "]: " + holderID + " -> " + TranslateNextHolder() + ", P=" + isPivot + ", C=" + isCompare + ", S=" + isSorted + ", Inst: " + instruction;
    }

    private string TranslateNextHolder()
    {
        return (nextHolderID == Util.NO_INSTRUCTION) ? "N/A" : nextHolderID.ToString();
    }

}
