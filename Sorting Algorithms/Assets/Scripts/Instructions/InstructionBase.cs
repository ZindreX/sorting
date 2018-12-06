

public class InstructionBase {

    /* -------------------------------------------- Instruction base --------------------------------------------
     * 
     * 
     * 
    */

    public readonly int INSTRUCION_NR;

    protected string instruction, status;

    public InstructionBase(string instruction, int instructionNr)
    {
        INSTRUCION_NR = instructionNr;
        this.instruction = instruction;
        status = Util.NOT_EXECUTED;
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
        return status == Util.EXECUTED_INST;
    }

    // Debug checker
    public virtual string DebugInfo()
    {
        return "Instruction: " + instruction + ", Nr.: " + INSTRUCION_NR + ", Status: " + status;
    }


}
