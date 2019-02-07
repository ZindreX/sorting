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

    protected virtual void Awake()
    {
        numberOfNodes = 10;
    }

    // Use this for initialization
    void Start () {
        InitGraph(GetGraphStructure(UtilGraph.TREE));
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
    }


    private int[] GetGraphStructure(string graphType)
    {
        switch (graphType)
        {
            case UtilGraph.GRID:
                // 0: rows, 1: columns, 2: grid space
                int[] gridStructure = new int[3];
                gridStructure[0] = 5;
                gridStructure[1] = 5;
                gridStructure[2] = 4;
                return gridStructure;
            case UtilGraph.TREE:
                // 0: level, 1: n tree, 2: node space x, 3: node space z
                int[] treeStructure = new int[4];
                treeStructure[0] = 2;
                treeStructure[1] = 2;
                treeStructure[2] = 4;
                treeStructure[3] = 4;
                return treeStructure;
            default: return null;
        }
    }

    protected abstract int GetMaxNumberOfNodes();
    protected abstract void InitGraph(int[] graphStructure);
    protected abstract void CreateNodes();
    protected abstract void CreateEdges(string mode);
}
