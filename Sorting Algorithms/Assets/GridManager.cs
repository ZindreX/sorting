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
                node.GetComponent<GridNode>().InitGridNode(new int[2] { z, x }, false);
                gridNodes[z, x] = node.GetComponent<GridNode>();
            }
        }
    }

    protected override void CreateEdges(string mode)
    {
        Debug.Log("TODO: create edges!");
    }

    /* Returns a matrix of the neighboors of parameter node
     * 
     * |0   0   0
     * |  O O O
     * |0 O X O 0   Neighboors (X: node, O: depht=1, 0: depth=2)
     * |  O O O
     * |0   0   0
    */
    public GridNode[,] GetNeighboors(GridNode node, bool doubleDepth)
    {
        // Init rows and columns variables
        int rows = 3, cols = 3, depth = 1;
        if (doubleDepth)
        {
            rows += 2;
            cols += 2;
            depth = 2;
        }

        // Neighboors matrix
        GridNode[,] neighboors = new GridNode[rows, cols];

        // Cell values for the input node
        int[] cell = node.Cell;
        int z = cell[0], x = cell[1];

        for (int i=0; i < rows; i++)
        {
            for (int j=0; j < cols; j++)
            {
                // Calibrating for correct cell in grid
                int cellZ = (z + i) - depth;
                int cellX = (x + j) - depth;

                // Adding if neighboor is inside the grid and is initialized
                if (IsValidNeighboor(z, x, cellZ, cellX, doubleDepth))
                {
                    neighboors[i, j] = gridNodes[cellZ, cellX];
                    gridNodes[cellZ, cellX].CurrentColor = Color.red; // Debugging color
                }
            }
        }
        gridNodes[z, x].CurrentColor = Color.green; // Debugging color
        return neighboors;
    }

    private bool IsValidNeighboor(int z, int x, int cellZ, int cellX,bool doubleDepth)
    {
        if (cellZ < 0 || cellX < 0 || cellZ >= rows || cellX >= cols) // Inside grid
            return false;
        else if (gridNodes[cellZ, cellX] == null) // exists
            return false;
        else
            return (doubleDepth) ? Mathf.Abs(z - cellZ) % 2 == 0 && Mathf.Abs(x - cellX) % 2 == 0 : true;  // double depth


    }
}
