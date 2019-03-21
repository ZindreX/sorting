

public class ShortestPathInstruction : InstructionBase {

    private Node currentNode, connectedNode;
    private Edge currentEdge, prevEdge;
    private int index, connectedNodeNewDist;

    public ShortestPathInstruction(string instruction, int instructionNr, Node currentNode, Node connectedNode, Edge currentEdge) : base(instruction, instructionNr)
    {
        this.currentNode = currentNode;
        this.connectedNode = connectedNode;
        this.currentEdge = currentEdge;

        connectedNodeNewDist = CurrentNode.Dist + currentEdge.Cost;
    }

    //public ShortestPathInstruction(string instruction, int instructionNr, Node currentNode, Node connectedNode) : base(instruction, instructionNr)
    //{
    //    this.currentNode = currentNode;
    //    this.connectedNode = connectedNode;
    //}

    public ShortestPathInstruction(string instruction, int instructionNr, Node connectedNode, Edge prevEdge) : base(instruction, instructionNr)
    {
        this.connectedNode = connectedNode;
        this.prevEdge = prevEdge;
    }

    public ShortestPathInstruction(string instruction, int instructionNr, Node connectedNode, int index) : base(instruction, instructionNr)
    {
        this.connectedNode = connectedNode;
        this.index = index;
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

    public int ConnectedNodeNewDist
    {
        get { return connectedNodeNewDist; }
    }

    public override bool HasBeenExecuted()
    {
        return base.HasBeenExecuted();
    }

    public override string DebugInfo()
    {
        string shortestPathInfo = " | ";

        if (currentNode != null)
            shortestPathInfo += currentNode.NodeAlphaID + ".dist=" + currentNode.Dist + ", ";

        if (currentEdge != null)
            shortestPathInfo += currentEdge.name + ".cost=" + currentEdge.Cost + ", ";

        if (connectedNode != null)
            shortestPathInfo += connectedNode.NodeAlphaID + ".dist=" + connectedNode.Dist + ", ";

        return base.DebugInfo() + shortestPathInfo + "new dist=" + connectedNodeNewDist;
    }
}
