

public class ListVisualInstruction : InstructionBase {

    private Node node;
    private int index;
    private string case1, case2;

    public ListVisualInstruction(string instruction, int instructionNr, Node node) : base(instruction, instructionNr)
    {
        this.node = node;
    }

    public ListVisualInstruction(string instruction, int instructionNr, Node node, int index) : base(instruction, instructionNr)
    {
        this.node = node;
        this.index = index;
    }

    public ListVisualInstruction(string instruction, int instructionNr, Node node, int index, string case1, string case2) : base(instruction, instructionNr)
    {
        this.node = node;
        this.index = index;
        this.case1 = case1;
        this.case2 = case2;
    }

    public Node Node
    {
        get { return node; }
    }

    public int Index
    {
        get { return index; }
    }

    public string GetCase(bool hasNodeRepresentation)
    {
        return hasNodeRepresentation ? case2 : case1;
    }
}
