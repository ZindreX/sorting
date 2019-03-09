using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class GraphManager : MonoBehaviour {

    protected GameObject nodePrefab, undirectedEdgePrefab, directedEdgePrefab, symmetricDirectedEdgePrefab;

    protected bool isShortestPath;
    protected string algorithmName, graphStructure, edgeType;

    private Node startNode, endNode;
    private Dictionary<string, int> rngDict;

    public void InitGraphManager(string algorithmName, string graphStructure, string edgeType, bool isShortestPath, Dictionary<string, int> rngDict)
    {
        this.algorithmName = algorithmName;
        this.graphStructure = graphStructure;
        this.edgeType = edgeType;
        this.isShortestPath = isShortestPath;
        this.rngDict = rngDict;

        GraphMain graphMain = GetComponent<GraphMain>();
        nodePrefab = graphMain.nodePrefab;
        undirectedEdgePrefab = graphMain.undirectedEdgePrefab;
        directedEdgePrefab = graphMain.directedEdgePrefab;
        symmetricDirectedEdgePrefab = graphMain.symmetricDirectedEdgePrefab;
    }

    public int LookUpRNGDict(string key)
    {
        return rngDict.ContainsKey(key) ? rngDict[key] : Util.NO_VALUE;
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
        }
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

    // Reset graph (prepare for User Test)
    public abstract void ResetGraph();

    // Delete graph
    public abstract void DeleteGraph();

    // Backtracks from all nodes in the given graph
    public abstract IEnumerator BacktrackShortestPathsAll(WaitForSeconds demoStepDuration);

}
