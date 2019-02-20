using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GraphSettings))]
public abstract class GraphManager : MainManager {

    protected int MAX_NODES;

    protected GameObject nodePrefab;
    protected Edge edgePrefab;

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
        edgePrefab = gs.edgePrefab;

        // Algorithm
        algorithm = gs.GetGraphAlgorithm();
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
        int[] startNode = gs.StartNode();
        switch (gs.TeachingMode)
        {
            case UtilGraph.DEMO:
                switch (gs.UseAlgorithm)
                {
                    case UtilGraph.BFS: StartCoroutine(((BFS)algorithm).Demo(GetNode(startNode[0], startNode[1]))); break;
                    case UtilGraph.DFS:
                        ((DFS)algorithm).VisistLeftFirst = gs.VisitLeftFirst;
                        StartCoroutine(((DFS)algorithm).Demo(GetNode(startNode[0], startNode[1])));
                        //algorithm.listVisual.AddListObject(GetNode(startNode[0], startNode[1]).NodeAlphaID);
                        //StartCoroutine(((DFS)algorithm).DemoRecursive(GetNode(startNode[0], startNode[1])));
                        break;


                    case UtilGraph.DIJKSTRA:
                        int[] endNode = gs.EndNode();
                        StartCoroutine(((Dijkstra)algorithm).Demo(GetNode(startNode[0], startNode[1]), GetNode(endNode[0], endNode[1])));
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
	void Update () {
        //if (userTestReady)
            
	}

    public void CreateGraph()
    {
        MAX_NODES = GetMaxNumberOfNodes();
        Debug.Log("Max nodes: " + MAX_NODES);
        CreateNodes(algorithm.AlgorithmName);
        CreateEdges(algorithm.AlgorithmName);
    }

    // Keeps only one graph structure active
    private void ActivateDeactivateGraphComponents(string graphStructure)
    {
        switch (graphStructure)
        {
            case UtilGraph.GRID:
                GetComponent<GridManager>().enabled = true;
                GetComponent<TreeManager>().enabled = false;
                GetComponent<RandomGraphManager>().enabled = false;
                break;

            case UtilGraph.TREE:
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



}
