

public class InstructionBase {

    /* -------------------------------------------- Instruction class for user test --------------------------------------------
     * 
     * 
     * 
    */
    public readonly int INSTRUCION_NR;

    protected bool isCompare, isSorted;
    protected string instruction, status;
    protected int i, j;

    public InstructionBase(string instruction, int instructionNr, int i, int j, bool isCompare, bool isSorted)
    {
        INSTRUCION_NR = instructionNr;
        this.instruction = instruction;
        status = Util.NOT_EXECUTED;
        this.isCompare = isCompare;
        this.isSorted = isSorted;
        // For step by step tutorial
        this.i = i;
        this.j = j;
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

    public virtual bool HasBeenExecuted()
    {
        return status == Util.EXECUTED_INST;
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
    public virtual string DebugInfo()
    {
        return "Instruction: " + instruction + ", Nr.: " + INSTRUCION_NR + ", i=" + i + "|j=" + j;
    }


}
