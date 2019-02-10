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

    protected int numberOfNodes, numberOfEdges;

    protected GraphSettings gs;

    protected virtual void Awake()
    {
        gs = GetComponent(typeof(GraphSettings)) as GraphSettings;
        gs.PrepareSettings();
        ActivateDeactivateGraphComponents(gs.Graphstructure);
        Debug.Log("Graph structure: " + gs.Graphstructure);
    }

    // Use this for initialization
    void Start () {
        InitGraph(gs.GraphSetup());
        CreateGraph();

        // Test
        switch (gs.UseAlgorithm)
        {
            case UtilGraph.BFS:
                //StartCoroutine(BFS.Demo(GetComponent<TreeManager>().Tree[0]));
                StartCoroutine(BFS.Demo(GetComponent<GridManager>().GridNodes[2, 2]));
                break;
            case UtilGraph.DFS:
                //StartCoroutine(DFS.Demo(GetComponent<TreeManager>().Tree[0], true));
                StartCoroutine(DFS.Demo(GetComponent<GridManager>().GridNodes[2, 2], true));
                break;
            case UtilGraph.DIJKSTRA:
                StartCoroutine(Dijkstra.Demo(GetComponent<GridManager>().GridNodes[0, 0]));
                break;
        }


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
                break;

            case UtilGraph.TREE:
                GetComponent<TreeManager>().enabled = true;
                GetComponent<GridManager>().enabled = false;
                break;

            default: Debug.Log("Graph structure '" + graphStructure + "' not found."); break;
        }
    }

    public void ResetGraph()
    {

    }


    protected abstract void InitGraph(int[] graphStructure);

    protected abstract void CreateNodes(string structure);
    public abstract int GetMaxNumberOfNodes();
    protected abstract List<Node> ConvertNodes();

    protected abstract void CreateEdges(string mode);
    public abstract int GetNumberOfEdges();

    // Algorithms
    protected abstract IEnumerator TraverseBFS(string config);
    protected abstract IEnumerator TraverseDFS(string config);


}
