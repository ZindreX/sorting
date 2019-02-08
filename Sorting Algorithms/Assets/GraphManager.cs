using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GraphManager : MonoBehaviour {

    protected int MAX_NODES;

    [SerializeField]
    protected GameObject nodePrefab;
    [SerializeField]
    protected Edge edgePrefab;

    protected Node[] nodes;
    protected Edge[] edges;

    protected int numberOfNodes, numberOfEdges;
    protected string activeGraphStructure;

    protected virtual void Awake()
    {
        activeGraphStructure = UtilGraph.GRID;
        ActivateDeactivateGraphComponents(activeGraphStructure);
    }

    // Use this for initialization
    void Start () {
        InitGraph(GetGraphStructure(activeGraphStructure));
        CreateGraph();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void CreateGraph()
    {
        MAX_NODES = GetMaxNumberOfNodes();
        Debug.Log("Max nodes: " + MAX_NODES);
        CreateNodes();
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

    private int[] GetGraphStructure(string graphType)
    {
        switch (graphType)
        {
            case UtilGraph.GRID:
                int[] gridStructure = new int[3];
                gridStructure[0] = 5;   // 0: rows
                gridStructure[1] = 5;   // 1: columns
                gridStructure[2] = 4;   // 2: grid space
                return gridStructure;

            case UtilGraph.TREE:
                int[] treeStructure = new int[4];
                treeStructure[0] = 2;   // 0: level (root only = 0)
                treeStructure[1] = 2;   // 1: n tree (binary, etc.)
                treeStructure[2] = 4;   // 2: node space x
                treeStructure[3] = 4;   // 3: node space z
                return treeStructure;

            default: return null;
        }
    }

    protected abstract int GetMaxNumberOfNodes();
    protected abstract void InitGraph(int[] graphStructure);
    protected abstract void CreateNodes();
    protected abstract void CreateEdges(string mode);
}
