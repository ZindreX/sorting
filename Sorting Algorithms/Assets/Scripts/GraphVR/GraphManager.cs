using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class GraphManager : MonoBehaviour {

    protected GameObject nodePrefab, undirectedEdgePrefab, directedEdgePrefab, symmetricDirectedEdgePrefab;
    protected GameObject nodeContainerObject, edgeContainerObject;

    protected bool isShortestPath;
    protected string algorithmName, graphStructure, edgeType;
    protected List<GameObject> edges;

    private Node startNode, endNode;
    private Dictionary<string, int> rngDict;

    private ListVisual listVisual;

    public void InitGraphManager(string algorithmName, string graphStructure, string edgeType, bool isShortestPath, Dictionary<string, int> rngDict, ListVisual listVisual)
    {
        this.algorithmName = algorithmName;
        this.graphStructure = graphStructure;
        this.edgeType = edgeType;
        this.isShortestPath = isShortestPath;
        this.rngDict = rngDict;
        this.listVisual = listVisual;

        // Init edges
        edges = new List<GameObject>();

        // Cleanup?
        GraphMain graphMain = GetComponent<GraphMain>();
        nodePrefab = graphMain.nodePrefab;
        undirectedEdgePrefab = graphMain.undirectedEdgePrefab;
        directedEdgePrefab = graphMain.directedEdgePrefab;
        symmetricDirectedEdgePrefab = graphMain.symmetricDirectedEdgePrefab;

        nodeContainerObject = graphMain.nodeContainerObject;
        edgeContainerObject = graphMain.edgeContainerObject;
    }

    public int LookUpRNGDict(string key)
    {
        return rngDict.ContainsKey(key) ? rngDict[key] : Util.NO_VALUE;
    }

    public void SetNode(int[] cell, bool startNode)
    {
        Debug.Log("Cell: " + cell[0] + ", " + cell[1] + ", start=" + startNode);
        Debug.Log("Node: " + GetNode(cell[0], cell[1]) == null);

        if (startNode)
        {
            StartNode = GetNode(cell[0], cell[1]);
        }
        else
        {
            EndNode = GetNode(cell[0], cell[1]);
        }
    }

    public Node StartNode
    {
        get { return startNode; }
        set { startNode = value; startNode.IsStartNode = true; }
    }

    public Node EndNode
    {
        get { return endNode; }
        set { endNode = value; endNode.IsEndNode = true; }
    }

    public void CreateGraph(string edgeBuildMode)
    {
        CreateNodes(edgeBuildMode);
        CreateEdges(edgeBuildMode);
    }

    private bool testIsland = false;
    protected void CreateEdge(Node node1, Node node2, Vector3 centerPos, float angle)
    {
        // Instantiate and fix edge
        GameObject edge = null;
        float length = UtilGraph.DistanceBetweenNodes(node1.transform, node2.transform);

        // Set cost if algorithm requires it
        int edgeCost = UtilGraph.NO_COST;
        if (isShortestPath)
            edgeCost = Random.Range(0, UtilGraph.EDGE_MAX_WEIGHT);

        // Initialize edge
        switch (edgeType)
        {
            case UtilGraph.UNDIRECTED_EDGE:
                edge = Instantiate(undirectedEdgePrefab, centerPos, Quaternion.identity);
                edge.AddComponent<UnDirectedEdge>();
                edge.GetComponent<UnDirectedEdge>().InitUndirectedEdge(node1, node2, edgeCost, UtilGraph.GRID_GRAPH);
                break;

            case UtilGraph.DIRECED_EDGE:
                bool pathBothWaysActive = RollSymmetricEdge();

                if (pathBothWaysActive)
                    edge = Instantiate(symmetricDirectedEdgePrefab, centerPos, Quaternion.identity);
                else
                    edge = Instantiate(directedEdgePrefab, centerPos, Quaternion.identity);

                edge.AddComponent<DirectedEdge>();
                edge.GetComponent<DirectedEdge>().InitDirectedEdge(node1, node2, edgeCost, UtilGraph.GRID_GRAPH, pathBothWaysActive);
                break;
        }
        edge.GetComponent<Edge>().SetAngle(angle);   
        edge.GetComponent<Edge>().SetLength(length);
        edge.transform.parent = edgeContainerObject.transform;
        edges.Add(edge);


        // Just for testing
        if (testIsland)
        {
            int[] cell = ((GridNode)node1).Cell;
            int nodeID = node1.NodeID;

            if (cell[0] >= 5 && cell[0] <= 25 && cell[1] >= 5 && cell[1] <= 25)
            //if (nodeID > 500 && nodeID < 550)
            {
                edge.GetComponent<Edge>().Cost = 100000;
                //edge.GetComponent<Edge>().CurrentColor = Color.white;
            }
        }
    }

    private bool RollSymmetricEdge()
    {
        switch (graphStructure)
        {
            case UtilGraph.GRID_GRAPH: case UtilGraph.RANDOM_GRAPH: return Util.RollRandom(LookUpRNGDict(UtilGraph.SYMMETRIC_EDGE_CHANCE));
            case UtilGraph.TREE_GRAPH: return false;
            default: Debug.LogError("Symmetric option not set for '" + graphStructure + "'."); break;
        }
        return false;
    }

    // Backtracks the path from input node
    public IEnumerator BacktrackShortestPath(Node node, WaitForSeconds demoStepDuration)
    {
        // Start backtracking from end node back to start node
        while (node != null)
        {
            // Node representation
            listVisual.RemoveCurrentNode();
            yield return demoStepDuration;

            // Change color of node
            node.CurrentColor = UtilGraph.SHORTEST_PATH_COLOR;

            // Change color of edge leading to previous node
            Edge backtrackEdge = node.PrevEdge;

            if (backtrackEdge == null)
                break;

            backtrackEdge.CurrentColor = UtilGraph.SHORTEST_PATH_COLOR;

            // Set "next" node
            if (backtrackEdge is DirectedEdge)
                ((DirectedEdge)backtrackEdge).PathBothWaysActive = true;

            node = backtrackEdge.OtherNodeConnected(node);

            if (backtrackEdge is DirectedEdge)
                ((DirectedEdge)backtrackEdge).PathBothWaysActive = false; // incase using same graph again at some point
            yield return demoStepDuration;

            listVisual.DestroyCurrentNode();

            yield return demoStepDuration;
        }
    }

    // User test backtracking
    public void ShortestPatBacktrackingInstructions(Dictionary<int, InstructionBase> instructions, int instNr)
    {
        // Start backtracking from end node back to start node
        Node node = EndNode;
        instructions.Add(instNr++, new InstructionBase(UtilGraph.MARK_END_NODE, instNr));

        // Fix list visual
        instructions.Add(instNr++, new ListVisualInstruction(UtilGraph.PREPARE_BACKTRACKING, instNr, node)); // Prepares backtracking list + moving current (end) node out of list

        // Set current node (remove from list)
        instructions.Add(instNr++, new ListVisualInstruction(UtilGraph.BACKTRACK_REMOVE_CURRENT_NODE, instNr, node));

        while (node != null)
        {
            // Backtrack by using each node's prevEdge
            Edge backtrackEdge = node.PrevEdge;

            // Stop when we reach the start node
            if (backtrackEdge == null)
                break;

            // Set "next" node
            if (backtrackEdge is DirectedEdge)
                ((DirectedEdge)backtrackEdge).PathBothWaysActive = true;

            // Set the next node via the prevEdge
            node = backtrackEdge.OtherNodeConnected(node);

            // Add instruction for traversing + list visual
            ListVisualInstruction removeCurrentNodeRep = new ListVisualInstruction(UtilGraph.BACKTRACK_REMOVE_CURRENT_NODE, instNr, node, backtrackEdge);
            instructions.Add(instNr++, new TraverseInstruction(UtilGraph.BACKTRACK, instNr, node, false, true, removeCurrentNodeRep));

            if (backtrackEdge is DirectedEdge)
                ((DirectedEdge)backtrackEdge).PathBothWaysActive = false; // incase using same graph again at some point

            //// Destroy node rep
            //instructions.Add(instNr++, new ListVisualInstruction(UtilGraph.DESTROY_CURRENT_NODE, instNr));
        }
        instructions.Add(instNr++, new ListVisualInstruction(UtilGraph.DESTROY_CURRENT_NODE, instNr));
    }


    // Reset graph (prepare for User Test)
    public virtual void ResetGraph()
    {
        foreach (GameObject edgeObj in edges)
        {
            edgeObj.GetComponent<Edge>().DisplayCost(false);
        }
    }

    public void MakeEdgeCostVisible(bool visible)
    {
        foreach (GameObject edgeObj in edges)
        {
            Edge edge = edgeObj.GetComponent<Edge>();

            if (!visible)
                edge.DisplayCost(false);
            else
            {
                Node node1 = edge.GetNode(1);
                Node node2 = edge.GetNode(2);

                if (node1.Traversed || node2.Traversed)
                    edge.DisplayCost(true);
            }
        }
    }


    // Delete graph
    public virtual void DeleteGraph()
    {
        foreach (GameObject edge in edges)
        {
            Destroy(edge);
        }
        startNode = null;
        endNode = null;
    }

    // ************************************ Abstract methods ************************************ 

    // Initialize the setup variables for the graph
    public abstract void InitGraph(int[] graphStructure);

    // Creates the nodes (and edges*)
    public abstract void CreateNodes(string structure);

    // Max #nodes (change/remove?)
    public abstract int GetMaxNumberOfNodes();

    // Get specific node
    public abstract Node GetNode(int a, int b);

    // Creates the edges of the graph (in case not already done)
    public abstract void CreateEdges(string mode);

    // #Edges
    public abstract int GetNumberOfEdges();

    // ------------------ Shortest path ------------------ 

    public abstract void SetAllNodesDist(int value);

    // Backtracks from all nodes in the given graph
    public abstract IEnumerator BacktrackShortestPathsAll(WaitForSeconds demoStepDuration);

}
