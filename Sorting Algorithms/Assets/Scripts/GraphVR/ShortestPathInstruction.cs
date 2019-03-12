

public class ShortestPathInstruction : InstructionLoop {

    private Node currentNode, connectedNode;
    private Edge currentEdge, prevEdge;
    private int nodeDist, edgeCost; // needed??? 

    public ShortestPathInstruction(string instruction, int instructionNr, Node currentNode, Node connectedNode, Edge currentEdge) : base(instruction, instructionNr, Util.NO_VALUE)
    {
        this.currentNode = currentNode;
        this.connectedNode = connectedNode;
        this.currentEdge = currentEdge;
    }

    public ShortestPathInstruction(string instruction, int instructionNr, int i, Node currentNode, Node connectedNode, Edge currentEdge) : base(instruction, instructionNr, i)
    {
        this.currentNode = currentNode;
        this.connectedNode = connectedNode;
        this.currentEdge = currentEdge;
    }

    public ShortestPathInstruction(string instruction, int instructionNr, int i, int j, Node currentNode, Node connectedNode, Edge currentEdge) : base(instruction, instructionNr, i, j)
    {
        this.currentNode = currentNode;
        this.connectedNode = connectedNode;
        this.currentEdge = currentEdge;
    }

    public ShortestPathInstruction(string instruction, int instructionNr, int i, int j, int k, Node currentNode, Node connectedNode, Edge currentEdge) : base(instruction, instructionNr, i, j, k)
    {
        this.currentNode = currentNode;
        this.connectedNode = connectedNode;
        this.currentEdge = currentEdge;
    }

    public ShortestPathInstruction(string instruction, int instructionNr, Node currentNode, Node connectedNode) : base(instruction, instructionNr, Util.NO_VALUE)
    {
        this.currentNode = currentNode;
        this.connectedNode = connectedNode;
    }

    public ShortestPathInstruction(string instruction, int instructionNr, Node connectedNode, Edge prevEdge) : base(instruction, instructionNr, Util.NO_VALUE)
    {
        this.connectedNode = connectedNode;
        this.prevEdge = prevEdge;
    }

    public ShortestPathInstruction(string instruction, int instructionNr, Node connectedNode, int index) : base(instruction, instructionNr, index)
    {
        this.connectedNode = connectedNode;
    }

    public Node CurrentNode
    {
        get { return currentNode; }
    }

    public Node ConnectedNode
    {
        get { return connectedNode; }
    }

    public Edge CurrentEdge
    {
        get { return currentEdge; }
    }

    public Edge PrevEdge
    {
        get { return prevEdge; }
    }

    public int NodeDist
    {
        get { return nodeDist; }
    }

    public int EdgeCost
    {
        get { return edgeCost; }
    }

    public int NodeDistAndEdgeCostTotal()
    {
        return nodeDist + edgeCost;
    }

    public override bool HasBeenExecuted()
    {
        return base.HasBeenExecuted();
    }

    public override string DebugInfo()
    {
        return base.DebugInfo() + " | dist=" + nodeDist + ", edge cost=" + edgeCost;
    }
}
