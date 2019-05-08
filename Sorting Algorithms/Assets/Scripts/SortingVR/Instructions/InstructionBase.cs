

public class InstructionBase {

    /* -------------------------------------------- Instruction base --------------------------------------------
     * 
     * 
     * 
    */

    protected int instructionNr;
    protected string instruction, status;

    public InstructionBase(string instruction, int instructionNr)
    {
        this.instructionNr = instructionNr;
        this.instruction = instruction;
        status = UtilSort.NOT_EXECUTED;
    }

    public int InstructionNr
    {
        get { return instructionNr; }
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

    public virtual bool HasBeenExecuted()
    {
        return status == UtilSort.EXECUTED_INST;
    }

    // Debug checker
    public virtual string DebugInfo()
    {
        return instructionNr + ": Instruction: " + instruction + ", Status: " + status;
    }


}
