using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GraphSettings))]
public abstract class GraphManager : MonoBehaviour {

    protected int MAX_NODES;

    [SerializeField]
    protected GameObject nodePrefab;
    [SerializeField]
    protected Edge edgePrefab;

    //protected int numberOfNodes, numberOfEdges;

    protected GraphSettings gs;
    protected ITraverse traverseAlgorithm;
    protected IShortestPath shortestPathAlgorithm;

    protected virtual void Awake()
    {
        gs = GetComponent(typeof(GraphSettings)) as GraphSettings;
        gs.PrepareSettings();
        UtilGraph.seconds = gs.AlgorithmSpeed;
        ActivateDeactivateGraphComponents(gs.Graphstructure);
        Debug.Log("Graph structure: " + gs.Graphstructure);

        switch (gs.UseAlgorithm)
        {
            case UtilGraph.BFS: traverseAlgorithm = new BFS(); break;
            case UtilGraph.DFS: traverseAlgorithm = new DFS(); break;
            case UtilGraph.DIJKSTRA: shortestPathAlgorithm = new Dijkstra(); break;
        }
    }

    // Use this for initialization
    void Start () {
        InitGraph(gs.GraphSetup());
        CreateGraph();

        int[] startNode = gs.StartNode();
        switch (gs.UseAlgorithm)
        {
            case UtilGraph.BFS: TraverseGraph(traverseAlgorithm, GetNode(startNode[0], startNode[1])); break;
            case UtilGraph.DFS:
                ((DFS)traverseAlgorithm).VisistLeftFirst = gs.VisitLeftFirst;
                TraverseGraph(traverseAlgorithm, GetNode(startNode[0], startNode[1]));
                break;


            case UtilGraph.DIJKSTRA:
                int[] endNode = gs.EndNode();
                ShortestPath(shortestPathAlgorithm, GetNode(startNode[0], startNode[1]), GetNode(endNode[0], startNode[1]));
                break;
        }
    }
	

    private void TraverseGraph(ITraverse algorithm, Node startNode)
    {
        StartCoroutine(algorithm.Demo(startNode));
    }

    private void ShortestPath(IShortestPath algorithm, Node from, Node to)
    {
        StartCoroutine(algorithm.Demo(from, to));

    }

	// Update is called once per frame
	void Update () {
		
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

    public void ResetGraph()
    {

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


}
