using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : GraphManager {

    private int gridMinX, gridMaxX, gridMinZ, gridMaxZ, gridSpace, rows, cols;

    private Vector3[,] grid;
    private Node[,] gridNodes;

    private HashSet<int> usedNodes;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override int GetMaxNumberOfNodes()
    {
        return ((gridMaxX - gridMinX) / gridSpace + 1) * ((gridMaxZ - gridMinZ) / gridSpace + 1);
    }

    protected override void InitGraph(int[] graphStructure)
    {
        // Init graph strucutre values
        gridMinX = graphStructure[0];
        gridMaxX = graphStructure[1];
        gridMinZ = graphStructure[2];
        gridMaxZ = graphStructure[3];
        gridSpace = graphStructure[4];

        // Calculate number of rows & columns
        rows = (gridMaxX - gridMinX) / gridSpace + 1;
        cols = (gridMaxZ - gridMinZ) / gridSpace + 1;

        // Create grid
        grid = new Vector3[rows, cols];
        for (int z = 0; z < rows; z++)
        {
            int zPos = gridMinZ + z * gridSpace;
            for (int x = 0; x < cols; x++)
            {
                int xPos = gridMaxX - x * gridSpace;
                grid[z, x] = new Vector3(xPos, 0f, zPos);
            }
        }
    }

    protected override void CreateNodes()
    {
        gridNodes = new Node[rows, cols];
        gridNodes[0,2] = Instantiate(nodePrefab, grid[0,2], Quaternion.identity);

        for (int z = 0; z < rows; z++)
        {
            for (int x = 0; x < cols; x++)
            {
                if (x % 2 == 0 && z % 2 == 0)
                    gridNodes[z, x] = Instantiate(nodePrefab, grid[z, x], Quaternion.identity);
            }
        }

    }

    protected override void CreateEdges(string mode)
    {
        throw new System.NotImplementedException();
    }

}
