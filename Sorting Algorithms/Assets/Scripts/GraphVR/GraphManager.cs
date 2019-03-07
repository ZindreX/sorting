using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class GraphManager : MonoBehaviour {

    protected GraphMain graphMain;
    protected GraphSettings graphSettings;
    protected GraphAlgorithm graphAlgorithm;

    private Node startNode, endNode;
    private Dictionary<string, int> rngDict;

    public void InitGraphManager(GraphSettings graphSettings, GraphAlgorithm graphAlgorithm, Dictionary<string, int> rngDict)
    {
        this.graphSettings = graphSettings;
        this.graphAlgorithm = graphAlgorithm;
        this.rngDict = rngDict;
    }

    public int LookUpRNGDict(string key)
    {
        return rngDict.ContainsKey(key) ? rngDict[key] : Util.NO_VALUE;
    }

    public Node StartNode
    {
        get { return startNode; }
        set { startNode = value; }
    }

    public Node EndNode
    {
        get { return endNode; }
        set { endNode = value; }
    }

    //public void CreateGraph()
    //{
    //    CreateNodes(graphSettings.EdgeMode);
    //    CreateEdges(graphSettings.EdgeMode);
    //}

    private bool testIsland = false;
    protected void CreateEdge(Node node1, Node node2, Vector3 centerPos, float angle)
    {
        // Instantiate and fix edge
        GameObject edge = null;
        float length = UtilGraph.DistanceBetweenNodes(node1.transform, node2.transform);

        // Set cost if algorithm requires it
        int edgeCost = UtilGraph.NO_COST;
        if (graphAlgorithm is IShortestPath)
            edgeCost = Random.Range(0, UtilGraph.EDGE_MAX_WEIGHT);

        // Initialize edge
        switch (graphSettings.EdgeType)
        {
            case UtilGraph.UNDIRECTED_EDGE:
                edge = Instantiate(graphSettings.undirectedEdgePrefab, centerPos, Quaternion.identity);
                edge.AddComponent<UnDirectedEdge>();
                edge.GetComponent<UnDirectedEdge>().InitUndirectedEdge(node1, node2, edgeCost, UtilGraph.GRID_GRAPH);
                break;

            case UtilGraph.DIRECED_EDGE:
                bool pathBothWaysActive = RollSymmetricEdge();

                if (pathBothWaysActive)
                    edge = Instantiate(graphSettings.symmetricDirectedEdgePrefab, centerPos, Quaternion.identity);
                else
                    edge = Instantiate(graphSettings.directedEdgePrefab, centerPos, Quaternion.identity);

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
        switch (graphSettings.Graphstructure)
        {
            case UtilGraph.GRID_GRAPH: case UtilGraph.RANDOM_GRAPH: return Util.RollRandom(LookUpRNGDict(UtilGraph.SYMMETRIC_EDGE_CHANCE));
            case UtilGraph.TREE_GRAPH: return false;
            default: Debug.LogError("Symmetric option not set for '" + graphSettings.Graphstructure + "'."); break;
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

    public int PrepareNextInstruction(InstructionBase instruction)
    {
        Debug.Log("Preparing next instruction: " + instruction.Instruction);
        Node node = null;
        bool gotNode = !graphAlgorithm.SkipDict[Util.SKIP_NO_ELEMENT].Contains(instruction.Instruction);
        bool noDestination = graphAlgorithm.SkipDict[Util.SKIP_NO_DESTINATION].Contains(instruction.Instruction);

        if (gotNode)
        {
            TraverseInstruction traverseInstruction = (TraverseInstruction)instruction;

            // Get the Sorting element
            node = traverseInstruction.Node;

            // Hands out the next instruction
            node.Instruction = traverseInstruction;

            // Set goal
            //posManager.CurrentGoal = node.gameObject;

            // Give this sorting element permission to give feedback to progress to next intstruction
            if (instruction.Instruction == UtilGraph.DEQUEUE_NODE_INST)
                node.NextMove = true;
        }

        // Display help on blackboard
        if (true) //algorithmSettings.Difficulty <= UtilSort.BEGINNER)
        {
            Debug.Log("Fixing pseudocode!");
            graphMain.BeginnerWait = true;
            StartCoroutine(graphAlgorithm.UserTestHighlightPseudoCode(instruction, gotNode && !noDestination));

            // Node representation
            switch (instruction.Instruction)
            {
                case UtilGraph.ENQUEUE_NODE_INST: graphAlgorithm.ListVisual.AddListObject(node); break;
                case UtilGraph.DEQUEUE_NODE_INST: graphAlgorithm.ListVisual.RemoveCurrentNode(); break;
                case UtilGraph.END_FOR_LOOP_INST: graphAlgorithm.ListVisual.DestroyCurrentNode(); break;
            }
        }

        Debug.Log("Got node: " + gotNode + ", no destination: " + noDestination);
        if (gotNode && !noDestination)
            return 0;
        Debug.Log("Nothing to do for player, get another instruction");
        return 1;
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
