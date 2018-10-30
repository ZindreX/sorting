

public abstract class InstructionBase {

    /* -------------------------------------------- Instruction class for user test --------------------------------------------
     * 
     * 
     * 
    */

    protected bool isCompare, isSorted;
    protected string instruction, status;

    public InstructionBase(string instruction, bool isCompare, bool isSorted)
    {
        this.instruction = instruction;
        status = Util.NOT_EXECUTED;
        this.isCompare = isCompare;
        this.isSorted = isSorted;
    }

    public string Instruction
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

    public bool HasBeenExecuted()
    {
        return status == Util.EXECUTED_INST;
    }


    // Debug checker
    public abstract string DebugInfo();


}
