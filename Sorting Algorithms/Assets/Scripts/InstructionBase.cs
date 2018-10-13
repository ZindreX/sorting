

public abstract class InstructionBase {

    /* -------------------------------------------- Instruction class for user test --------------------------------------------
     * 
     * 
     * 
    */

    protected int sortingElementID, holderID, value;
    protected bool isCompare, isSorted;
    protected string instruction, status;

    public InstructionBase(int sortingElementID, int holderID, int value, string instruction, bool isCompare, bool isSorted)
    {
        this.sortingElementID = sortingElementID;
        this.holderID = holderID;
        this.value = value;
        this.instruction = instruction;
        status = "Not performed";
        this.isCompare = isCompare;
        this.isSorted = isSorted;
    }

    public int SortingElementID
    {
        get { return sortingElementID; }
    }

    public int HolderID
    {
        get { return holderID; }
        set { holderID = value; }
    }

    public int Value
    {
        get { return value; }
    }

    public string ElementInstruction
    {
        get { return instruction; }
        set { instruction = value; }
    }

    public string Status
    {
        get { return status; }
        set { status = value; }
    }

    public bool IsCompare
    {
        get { return isCompare; }
        set { isCompare = value; }
    }

    public bool IsSorted
    {
        get { return isSorted; }
        set { isSorted = value; }
    }

    public string GetCombinedID()
    {
        return sortingElementID + "/" + holderID;
    }


    // Debug checker
    public abstract string DebugInfo();


}
