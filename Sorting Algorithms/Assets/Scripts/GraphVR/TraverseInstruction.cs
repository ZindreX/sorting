

public class TraverseInstruction : InstructionBase {

    private Node node;
    private Edge prevEdge;
    private bool visitInst, traverseInst;

    public TraverseInstruction(string instruction, int instructionNr, Node node, bool visitInst, bool traverseInst) : base(instruction, instructionNr)
    {
        this.node = node;
        this.visitInst = visitInst;
        this.traverseInst = traverseInst;
    }

    public TraverseInstruction(string instruction, int instructionNr, Node node, Edge prevEdge, bool visitInst, bool traverseInst) : base(instruction, instructionNr)
    {
        this.node = node;
        this.prevEdge = prevEdge;
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
