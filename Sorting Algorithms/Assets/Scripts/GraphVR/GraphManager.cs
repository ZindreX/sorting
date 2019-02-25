using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GraphSettings))]
public abstract class GraphManager : MainManager {

    protected int MAX_NODES;

    protected GameObject nodePrefab, undirectedEdgePrefab, directedEdgePrefab, symmetricDirectedEdgePrefab;

    protected GraphSettings gs;
    protected GraphAlgorithm algorithm;

    protected virtual void Awake()
    {
        // Get settings from editor
        gs = GetComponent(typeof(GraphSettings)) as GraphSettings;
        gs.PrepareSettings();

        // Activate/deactivate components (Grid / Tree / Random)
        ActivateDeactivateGraphComponents(gs.Graphstructure);

        // Prefabs (editor clean up)
        nodePrefab = gs.nodePrefab;
        undirectedEdgePrefab = gs.undirectedEdgePrefab;
        directedEdgePrefab = gs.directedEdgePrefab;
        symmetricDirectedEdgePrefab = gs.symmetricDirectedEdgePrefab;

        // Algorithm
        algorithm = gs.GetGraphAlgorithm();
        algorithm.GraphStructure = gs.Graphstructure;
        algorithm.Seconds = gs.AlgorithmSpeed;
        algorithm.ListVisual = gs.ListVisual;
        
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
        switch (gs.TeachingMode)
        {
            case UtilGraph.DEMO:
                switch (gs.UseAlgorithm)
                {
                    case UtilGraph.BFS: StartCoroutine(((BFS)algorithm).Demo(startNode)); break;
                    case UtilGraph.DFS:
                        ((DFS)algorithm).VisistLeftFirst = gs.VisitLeftFirst;

                        if (true)
                            StartCoroutine(((DFS)algorithm).Demo(startNode));
                        else
                        {
                            //algorithm.ListVisual.AddListObject(GetNode(startNode[0], startNode[1]).NodeAlphaID);
                            StartCoroutine(((DFS)algorithm).DemoRecursive(startNode));
                        }                   
                        break;


                    case UtilGraph.DIJKSTRA:
                        int[] endNodeCell = gs.EndNode();
                        Node endNode = GetNode(endNodeCell[0], endNodeCell[1]);
                        endNode.IsEndNode = true;
                        StartCoroutine(((Dijkstra)algorithm).Demo(startNode, endNode));
                        break;

                    //case UtilGraph.TREE_PRE_ORDER_TRAVERSAL:
                    //    Debug.Log(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");
                    //    TraverseGraph(algorithm, GetNode(startNode[0], startNode[1]));
                    //    break;
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
            StartCoroutine(BacktrackShortestPaths(algorithm.Seconds));
            backtracking = true;
        }
            
	}

    public void CreateGraph()
    {
        CreateNodes(algorithm.AlgorithmName);
        CreateEdges(gs.EdgeType);
    }

    // Keeps only one graph structure active
    private void ActivateDeactivateGraphComponents(string graphStructure)
    {
        switch (graphStructure)
        {
            case UtilGraph.GRID_GRAD:
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
                edge = Instantiate(undirectedEdgePrefab, centerPos, Quaternion.identity);
                edge.AddComponent<UnDirectedEdge>();
                edge.GetComponent<UnDirectedEdge>().InitUndirectedEdge(node1, node2, edgeCost, UtilGraph.GRID_GRAD);
                break;

            case UtilGraph.DIRECED_EDGE:
                bool pathBothWaysActive = RollSymmetric();

                if (pathBothWaysActive)
                    edge = Instantiate(symmetricDirectedEdgePrefab, centerPos, Quaternion.identity);
                else
                    edge = Instantiate(directedEdgePrefab, centerPos, Quaternion.identity);

                edge.AddComponent<DirectedEdge>();
                edge.GetComponent<DirectedEdge>().InitDirectedEdge(node1, node2, edgeCost, UtilGraph.GRID_GRAD, pathBothWaysActive);
                break;
        }
        edge.transform.Rotate(0, angle, 0, Space.Self);
        edge.GetComponent<Edge>().SetLength(length);   
    }

    private bool RollSymmetric()
    {
        switch (gs.Graphstructure)
        {
            case UtilGraph.GRID_GRAD: case UtilGraph.RANDOM_GRAPH: return Random.Range(0, 10) < 7;
            case UtilGraph.TREE_GRAPH: return false;
            default: Debug.LogError("Symmetric option not set for '" + gs.Graphstructure + "'."); break;
        }
        return false;
    }

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

    protected abstract IEnumerator BacktrackShortestPaths(float seconds);



}
