using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GraphSettings))]
public abstract class GraphManager : MainManager {

    protected GraphSettings gs;
    protected GraphAlgorithm algorithm;

    protected virtual void Awake()
    {
        // Get settings from editor
        gs = GetComponent(typeof(GraphSettings)) as GraphSettings;
        gs.PrepareSettings();

        // Activate/deactivate components (Grid / Tree / Random)
        ActivateDeactivateGraphComponents(gs.Graphstructure);

        // Algorithm
        algorithm = gs.GetGraphAlgorithm();
        algorithm.GraphStructure = gs.Graphstructure;
        algorithm.Seconds = gs.AlgorithmSpeed;
        algorithm.ListVisual = gs.ListVisual;
        algorithm.ShortestPathOneToAll = gs.ShortestPathOneToAll;
        algorithm.VisitLeftFirst = gs.VisitLeftFirst;
        
        // Pseudocode
        algorithm.PseudoCodeViewer = gs.PseudoCodeViewer;

    }

    // Use this for initialization
    void Start () {
        gs.PseudoCodeViewer.PseudoCodeSetup();

        // Get variables for graph setup
        InitGraph(gs.GraphSetup());

        // Create graph based on init variables
        CreateGraph();

        // Init teaching mode
        int[] startNodeCell = gs.StartNode();
        Node startNode = GetNode(startNodeCell[0], startNodeCell[1]);
        startNode.IsStartNode = true;
        switch (gs.TeachingMode)
        {
            case UtilGraph.DEMO:
                switch (gs.UseAlgorithm)
                {
                    case Util.BFS: StartCoroutine(((BFS)algorithm).Demo(startNode)); break;

                    case Util.DFS:
                        StartCoroutine(((DFS)algorithm).Demo(startNode));
                        break;

                    case Util.DFS_RECURSIVE:
                        //algorithm.ListVisual.AddListObject(GetNode(startNode[0], startNode[1]).NodeAlphaID);
                        StartCoroutine(((DFS)algorithm).DemoRecursive(startNode));
                        break;

                    case Util.DIJKSTRA:
                        int[] endNodeCell = gs.EndNode();
                        Node endNode = GetNode(endNodeCell[0], endNodeCell[1]);
                        endNode.IsEndNode = true;
                        StartCoroutine(((Dijkstra)algorithm).Demo(startNode, endNode));
                        //StartCoroutine(((Dijkstra)algorithm).DemoNoPseudocode(startNode, endNode));
                        break;
                }
                break;

            case UtilGraph.USER_TEST:
                if (gs.UseAlgorithm == UtilGraph.BFS)
                {
                    List<int> visitOrder = null; // ((BFS)algorithm).UserTestInstructions(GetNode(startNode[0], startNode[1]));

                    string result = "";
                    for (int x = 0; x < visitOrder.Count; x++)
                    {
                        result += visitOrder[x].ToString() + " -> ";
                    }
                    result.Substring(0, result.Length - 4);
                    Debug.Log(result);
                }

                PerformUserTest();
                break;
        }
    }

    // Bugged?
    //private void TraverseGraph(Node startNode)
    //{
    //    Debug.Log(">>> Algorithm: " + algorithm.AlgorithmName);
    //    Debug.Log("Test: " + algorithm.GetComponent(typeof(DFS)));
    //    StartCoroutine(algorithm.GetComponent<ITraverse>().Demo(startNode));
    //}

    //private void ShortestPath(Node from, Node to)
    //{
    //    StartCoroutine(algorithm.GetComponent<IShortestPath>().Demo(from, to));
    //}

    // Update is called once per frame
    private bool backtracking = false;
	void Update () {
        if (algorithm.IsTaskCompleted && !backtracking)
        {   
            Debug.Log("Starting backtracking");
            if (algorithm.ShortestPathOneToAll)
                StartCoroutine(BacktrackShortestPathsAll(algorithm.Seconds));
            else
            {
                int[] endNodeCell = gs.EndNode();
                Node endNode = GetNode(endNodeCell[0], endNodeCell[1]);
                StartCoroutine(BacktrackShortestPath(endNode, algorithm.Seconds));
            }
            backtracking = true;
        }
            
	}

    public void CreateGraph()
    {
        CreateNodes(gs.EdgeMode);
        CreateEdges(gs.EdgeMode);
    }

    // Keeps only one graph structure active
    private void ActivateDeactivateGraphComponents(string graphStructure)
    {
        switch (graphStructure)
        {
            case UtilGraph.GRID_GRAPH:
                GetComponent<GridManager>().enabled = true;
                GetComponent<TreeManager>().enabled = false;
                GetComponent<RandomGraphManager>().enabled = false;
                break;

            case UtilGraph.TREE_GRAPH:
                GetComponent<TreeManager>().enabled = true;
                GetComponent<GridManager>().enabled = false;
                GetComponent<RandomGraphManager>().enabled = false;
                break;

            case UtilGraph.RANDOM_GRAPH:
                GetComponent<RandomGraphManager>().enabled = true;
                GetComponent<TreeManager>().enabled = false;
                GetComponent<GridManager>().enabled = false;
                break;

            default: Debug.Log("Graph structure '" + graphStructure + "' not found."); break;
        }
    }


    private void PerformUserTest()
    {
        ResetGraph();
        userTestReady = true;
        Debug.Log("Ready for user test!");
    }


    protected override int PrepareNextInstruction(InstructionBase instruction)
    {
        bool gotNode = !algorithm.SkipDict[UtilSort.SKIP_NO_ELEMENT].Contains(instruction.Instruction);
        bool noDestination = algorithm.SkipDict[UtilSort.SKIP_NO_DESTINATION].Contains(instruction.Instruction);

        if (gotNode)
        {
            TraverseInstruction traverseInstruction = (TraverseInstruction)instruction;
            // Get the Sorting element
            Node node = null;//elementManager.GetSortingElement(insertionSortInstruction.SortingElementID).GetComponent<InsertionSortElement>();

            // Hands out the next instruction
            node.Instruction = traverseInstruction;

            // Give this sorting element permission to give feedback to progress to next intstruction
            if (instruction.Instruction == UtilSort.PIVOT_START_INST || instruction.Instruction == UtilSort.PIVOT_END_INST || instruction.Instruction == UtilSort.SWITCH_INST)
                node.NextMove = true;
        }

        // Display help on blackboard
        if (true) //algorithmSettings.Difficulty <= UtilSort.BEGINNER)
        {
            BeginnerWait = true;
            StartCoroutine(algorithm.UserTestHighlightPseudoCode(instruction, gotNode));
        }


        if (gotNode && !noDestination)
            return 0;
        return 1;
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
                bool pathBothWaysActive = RollSymmetric();

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
                edge.GetComponent<Edge>().CurrentColor = Color.white;
            }
        }
    }

    private bool RollSymmetric()
    {
        switch (gs.Graphstructure)
        {
            case UtilGraph.GRID_GRAPH: case UtilGraph.RANDOM_GRAPH: return Random.Range(UtilGraph.ROLL_MIN, UtilGraph.ROLL_MAX) < UtilGraph.SYMMETRIC_EDGE_CHANCE;
            case UtilGraph.TREE_GRAPH: return false;
            default: Debug.LogError("Symmetric option not set for '" + gs.Graphstructure + "'."); break;
        }
        return false;
    }

    // Backtracks the path from input node
    protected IEnumerator BacktrackShortestPath(Node node, float seconds)
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
    protected abstract void InitGraph(int[] graphStructure);

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
    protected abstract IEnumerator BacktrackShortestPathsAll(float seconds);



}
