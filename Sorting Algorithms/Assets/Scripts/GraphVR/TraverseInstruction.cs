

public class TraverseInstruction : InstructionLoop {

    private Node node;
    private Edge prevEdge;
    private bool visitInst, traverseInst;

    public TraverseInstruction(string instruction, int instructionNr, Node node, bool visitInst, bool traverseInst) : base(instruction, instructionNr, Util.NO_INDEX_VALUE)
    {
        this.node = node;
        this.visitInst = visitInst;
        this.traverseInst = traverseInst;
    }

    public TraverseInstruction(string instruction, int instructionNr, int i, Node node, bool visitInst, bool traverseInst) : base(instruction, instructionNr, i)
    {
        this.node = node;
        this.visitInst = visitInst;
        this.traverseInst = traverseInst;
    }

    public TraverseInstruction(string instruction, int instructionNr, int i, int j, Node node, bool visitInst, bool traverseInst) : base(instruction, instructionNr, i, j)
    {
        this.node = node;
        this.visitInst = visitInst;
        this.traverseInst = traverseInst;
    }

    public TraverseInstruction(string instruction, int instructionNr, int i, int j, int k, Node node, bool visitInst, bool traverseInst) : base(instruction, instructionNr, i, j, k)
    {
        this.node = node;
        this.visitInst = visitInst;
        this.traverseInst = traverseInst;
    }

    public Node Node
    {
        get { return node; }
    }

    public bool VisitInst
    {
        get { return visitInst; }
    }

    public bool TraverseInst
    {
        get { return traverseInst; }
    }

    public Edge PrevEdge
    {
        get { return prevEdge; }
        set { prevEdge = value; }
    }

    public override string DebugInfo()
    {
        return base.DebugInfo() + " || Node: " + node.NodeAlphaID + ", visit instruction=" + visitInst + ", traverse instruction=" + traverseInst;
    }

    public override bool HasBeenExecuted()
    {
        return base.HasBeenExecuted();
    }
}
