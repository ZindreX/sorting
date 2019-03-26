

public class ShortestPathInstruction : InstructionBase {

    private Node currentNode, connectedNode;
    private Edge currentEdge, prevEdge;

    private Edge oldPrevEdge;
    private int connectedNodeOldDist, connectedNodeNewDist;
    private int index;

    private ListVisualInstruction listVisualInstruction;

    public ShortestPathInstruction(string instruction, int instructionNr, Node currentNode, Node connectedNode, Edge currentEdge) : base(instruction, instructionNr)
    {
        this.currentNode = currentNode;
        this.connectedNode = connectedNode;
        this.currentEdge = currentEdge;

        connectedNodeOldDist = connectedNode.Dist;
        connectedNodeNewDist = CurrentNode.Dist + currentEdge.Cost;
    }

    public ShortestPathInstruction(string instruction, int instructionNr, Node currentNode, Node connectedNode, Edge currentEdge, ListVisualInstruction listVisualInstruction) : base(instruction, instructionNr)
    {
        this.currentNode = currentNode;
        this.connectedNode = connectedNode;
        this.currentEdge = currentEdge;

        connectedNodeOldDist = connectedNode.Dist;
        connectedNodeNewDist = CurrentNode.Dist + currentEdge.Cost;
        this.listVisualInstruction = listVisualInstruction;
    }

    public ShortestPathInstruction(string instruction, int instructionNr, Node connectedNode, Edge prevEdge) : base(instruction, instructionNr)
    {
        this.connectedNode = connectedNode;

        if (connectedNode.PrevEdge != null)
            oldPrevEdge = connectedNode.PrevEdge;

        this.prevEdge = prevEdge;
    }

    public ShortestPathInstruction(string instruction, int instructionNr, Node connectedNode, Edge prevEdge, ListVisualInstruction listVisualInstruction) : base(instruction, instructionNr)
    {
        this.connectedNode = connectedNode;

        if (connectedNode.PrevEdge != null)
            oldPrevEdge = connectedNode.PrevEdge;

        this.prevEdge = prevEdge;
        this.listVisualInstruction = listVisualInstruction;
    }

    //public ShortestPathInstruction(string instruction, int instructionNr, Node connectedNode, int index) : base(instruction, instructionNr)
    //{
    //    this.connectedNode = connectedNode;
    //    this.index = index;
    //}

    //public ShortestPathInstruction(string instruction, int instructionNr, Node connectedNode, int index, ListVisualInstruction listVisualInstruction) : base(instruction, instructionNr)
    //{
    //    this.connectedNode = connectedNode;
    //    this.index = index;
    //    this.listVisualInstruction = listVisualInstruction;
    //}

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

    public Edge OldPrevEdge
    {
        get { return oldPrevEdge; }
    }

    public int ConnectedNodeOldDist
    {
        get { return connectedNodeOldDist; }
    }

    public int ConnectedNodeNewDist
    {
        get { return connectedNodeNewDist; }
    }

    public override bool HasBeenExecuted()
    {
        return base.HasBeenExecuted();
    }

    public ListVisualInstruction ListVisualInstruction
    {
        get { return listVisualInstruction; }
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
