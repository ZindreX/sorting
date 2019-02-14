using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GraphSettings))]
public abstract class GraphManager : MonoBehaviour {

    protected int MAX_NODES;

    protected GameObject nodePrefab;
    protected Edge edgePrefab;

    private bool userTestReady;

    protected GraphSettings gs;
    protected GraphAlgorithm algorithm;

    protected virtual void Awake()
    {
        // Get settings from editor
        gs = GetComponent(typeof(GraphSettings)) as GraphSettings;
        gs.PrepareSettings();

        // Activate/deactivate components (Grid / Tree / Random)
        ActivateDeactivateGraphComponents(gs.Graphstructure);

        // Prefabs (cleaness)
        nodePrefab = gs.nodePrefab;
        edgePrefab = gs.edgePrefab;

        // Algorithm
        algorithm = gs.GetGraphAlgorithm();
        algorithm.Seconds = gs.AlgorithmSpeed;
    }

    // Use this for initialization
    void Start () {
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
                    case UtilGraph.BFS: TraverseGraph(algorithm, GetNode(startNode[0], startNode[1])); break;
                    case UtilGraph.DFS:
                        ((DFS)algorithm).VisistLeftFirst = gs.VisitLeftFirst;
                        TraverseGraph(algorithm, GetNode(startNode[0], startNode[1]));
                        break;


                    case UtilGraph.DIJKSTRA:
                        int[] endNode = gs.EndNode();
                        ShortestPath(algorithm, GetNode(startNode[0], startNode[1]), GetNode(endNode[0], endNode[1]));
                        break;
                }
                break;

            case UtilGraph.USER_TEST:
                if (gs.UseAlgorithm == UtilGraph.BFS)
                {
                    List<int> visitOrder = ((BFS)algorithm).VisitNodeOrder(GetNode(startNode[0], startNode[1]));

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

    private void TraverseGraph(GraphAlgorithm algorithm, Node startNode)
    {
        StartCoroutine(algorithm.GetComponent<ITraverse>().Demo(startNode));
    }

    private void ShortestPath(GraphAlgorithm algorithm, Node from, Node to)
    {
        StartCoroutine(algorithm.GetComponent<IShortestPath>().Demo(from, to));

    }

	// Update is called once per frame
	void Update () {
        if (userTestReady)
            Debug.Log("Ready for user test!");
	}

    public void CreateGraph()
    {
        MAX_NODES = GetMaxNumberOfNodes();
        Debug.Log("Max nodes: " + MAX_NODES);
        CreateNodes("");
        CreateEdges("");
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
