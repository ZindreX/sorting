using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : GraphManager {

    private int rows, cols, gridSpace;

    //private Vector3[,] grid;
    private GridNode[,] gridNodes;

    private HashSet<int> usedNodes;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override int GetMaxNumberOfNodes()
    {
        return rows * cols;
    }

    protected override void InitGraph(int[] graphStructure)
    {
        // Init graph strucutre values
        rows = graphStructure[0];
        cols = graphStructure[1];
        gridSpace = graphStructure[2];
    }

    protected override void CreateNodes()
    {
        gridNodes = new GridNode[rows, cols];

        // Generate grid
        for (int z = 0; z < rows; z++)
        {
            int zPos = UtilGraph.GRAPH_MIN_Z + z * gridSpace;
            for (int x = 0; x < cols; x++)
            {
                int xPos = UtilGraph.GRAPH_MAX_X - x * gridSpace;
                GameObject node = Instantiate(nodePrefab, new Vector3(xPos, 0f, zPos), Quaternion.identity);
                node.AddComponent<GridNode>();
                node.GetComponent<GridNode>().InitGridNode(false);
                gridNodes[z, x] = node.GetComponent<GridNode>();
            }
        }

        // Find start node
    }

    protected override void CreateEdges(string mode)
    {
        throw new System.NotImplementedException();
    }

}
