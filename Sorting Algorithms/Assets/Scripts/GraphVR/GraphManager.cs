using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class GraphManager : MonoBehaviour {

    protected GraphSettings gs;
    protected GraphAlgorithm algorithm;

    protected int START_X, START_Z, END_X, END_Z;
    private Node startNode, endNode;
    private Dictionary<string, int> rngDict;

    public void InitGraphManager(GraphSettings gs, GraphAlgorithm algorithm, Dictionary<string, int> rngDict)
    {
        this.gs = gs;
        this.algorithm = algorithm;
        this.rngDict = rngDict;
    }

    public int LookUpRNGDict(string key)
    {
        return rngDict.ContainsKey(key) ? rngDict[key] : Util.NO_VALUE;
    }

    public Node StartNode
    {
        get { return startNode; }
    }

    public Node EndNode
    {
        get { return endNode; }
    }

    public void SetupImportantNodes(int[] startNodeCell, int[] endNodeCell, bool beforeNodesExist)
    {
        if (beforeNodesExist)
        {
            START_X = startNodeCell[0];
            START_Z = startNodeCell[1];
            END_X = endNodeCell[0];
            END_Z = endNodeCell[1];
        }
        else
        {
            // Start node
            startNode = GetNode(START_X, START_Z);
            startNode.IsStartNode = true;

            // End node
            endNode = GetNode(END_X, END_Z);
            endNode.IsEndNode = true;
        }
    }

    public void CreateGraph()
    {
        CreateNodes(gs.EdgeMode);
        CreateEdges(gs.EdgeMode);
    }

    private bool testIsland = false;
    protected void CreateEdge(Node node1, Node node2, Vector3 centerPos, float angle)
    {
        // Instantiate and fix edge
        GameObject edge = null;
        float length = UtilGraph.DistanceBetweenNodes(node1.transform, node2.transform);

        // Set cost if algorithm requires it
        int edgeCost = UtilGraph.NO_COST;
        if (algorithm is IShortestPath)
            edgeCost = Random.Range(0, UtilGraph.EDGE_MAX_WEIGHT);

        // Initialize edge
        switch (gs.EdgeType)
        {
            case UtilGraph.UNDIRECTED_EDGE:
                edge = Instantiate(gs.undirectedEdgePrefab, centerPos, Quaternion.identity);
                edge.AddComponent<UnDirectedEdge>();
                edge.GetComponent<UnDirectedEdge>().InitUndirectedEdge(node1, node2, edgeCost, UtilGraph.GRID_GRAPH);
                break;

            case UtilGraph.DIRECED_EDGE:
                bool pathBothWaysActive = RollSymmetricEdge();

                if (pathBothWaysActive)
                    edge = Instantiate(gs.symmetricDirectedEdgePrefab, centerPos, Quaternion.identity);
                else
                    edge = Instantiate(gs.directedEdgePrefab, centerPos, Quaternion.identity);

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
        switch (gs.Graphstructure)
        {
            case UtilGraph.GRID_GRAPH: case UtilGraph.RANDOM_GRAPH: return Util.RollRandom(LookUpRNGDict(UtilGraph.SYMMETRIC_EDGE_CHANCE));
            case UtilGraph.TREE_GRAPH: return false;
            default: Debug.LogError("Symmetric option not set for '" + gs.Graphstructure + "'."); break;
        }
        return false;
    }

    // Backtracks the path from input node
    public IEnumerator BacktrackShortestPath(Node node, float seconds)
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
            yield return new WaitForSeconds(seconds);
        }
    }

    // ************************************ Abstract methods ************************************ 

    // Initialize the setup variables for the graph
    public abstract void InitGraph(int[] graphStructure);

    // Creates the nodes (and edges*)
    protected abstract void CreateNodes(string structure);

    // Max #nodes (change/remove?)
    public abstract int GetMaxNumberOfNodes();

    // Get specific node
    public abstract Node GetNode(int a, int b);

    // Creates the edges of the graph (in case not already done)
    protected abstract void CreateEdges(string mode);

    // #Edges
    public abstract int GetNumberOfEdges();

    // Reset graph (prepare for User Test)
    public abstract void ResetGraph();

    // Delete graph
    public abstract void DeleteGraph();

    // Backtracks from all nodes in the given graph
    public abstract IEnumerator BacktrackShortestPathsAll(float seconds);



}
