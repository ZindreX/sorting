

public class TraverseInstruction : InstructionLoop {

    private Node node;
    private bool visited;

    public TraverseInstruction(string instruction, int instructionNr, Node node, bool visited) : base(instruction, instructionNr, Util.NO_INDEX_VALUE)
    {
        this.node = node;
        this.visited = visited;
    }

    public TraverseInstruction(string instruction, int instructionNr, int i, Node node, bool visited) : base(instruction, instructionNr, i)
    {
        this.node = node;
        this.visited = visited;
    }

    public TraverseInstruction(string instruction, int instructionNr, int i, int j, Node node, bool visited) : base(instruction, instructionNr, i, j)
    {
        this.node = node;
        this.visited = visited;
    }

    public TraverseInstruction(string instruction, int instructionNr, int i, int j, int k, Node node, bool visited) : base(instruction, instructionNr, i, j, k)
    {
        this.node = node;
        this.visited = visited;
    }

    public Node Node
    {
        get { return node; }
    }

    public override string DebugInfo()
    {
        return base.DebugInfo();
    }

    public override bool HasBeenExecuted()
    {
        return base.HasBeenExecuted();
    }
}
