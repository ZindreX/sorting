using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GraphManager : MonoBehaviour {

    protected int MAX_NODES;

    [SerializeField]
    protected Node nodePrefab;
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
        InitGraph(GetGraphStructure("Grid"));
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
            case "Grid":
                //  GRID_MIN_X = -8, GRID_MAX_X = 8, GRID_MIN_Z = 0, GRID_MAX_Z = 16, GRID_SPACE = 4;
                int[] gridStructure = new int[5];
                gridStructure[0] = -8;
                gridStructure[1] = 8;
                gridStructure[2] = 0;
                gridStructure[3] = 16;
                gridStructure[4] = 4;
                return gridStructure;
            case "Tree":
                return null;
            default: return null;
        }
    }

    protected abstract void InitGraph(int[] graphStructure);
    protected abstract void CreateNodes();
    protected abstract void CreateEdges(string mode);
    protected abstract int GetMaxNumberOfNodes();
}
