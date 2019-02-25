using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : GraphManager {

    public readonly int MAX_ROWS = 5, MAX_COLUMNS = 5;

    private int rows, cols, gridSpace;

    private GridNode[,] gridNodes;

    public override int GetMaxNumberOfNodes()
    {
        return rows * cols;
    }

    public GridNode[,] GridNodes
    {
        get { return gridNodes; }
    }

    protected override void InitGraph(int[] graphStructure)
    {
        // Init graph strucutre values
        rows = graphStructure[0];
        cols = graphStructure[1];
        gridSpace = graphStructure[2];
    }

    protected override void CreateNodes(string s)
    {
        gridNodes = new GridNode[rows, cols];
        int startX = (UtilGraph.GRAPH_MAX_X - ((MAX_ROWS - cols) / 2) * gridSpace);

        // Generate grid
        for (int z = 0; z < rows; z++)
        {
            int zPos = UtilGraph.GRAPH_MIN_Z + z * gridSpace;
            for (int x = 0; x < cols; x++)
            {
                int xPos = startX - x * gridSpace;
                GameObject node = Instantiate(nodePrefab, new Vector3(xPos, 0f, zPos), Quaternion.identity);
                node.AddComponent<GridNode>();
                node.GetComponent<GridNode>().InitGridNode(algorithm.AlgorithmName, new int[2] { z, x });
                gridNodes[z, x] = node.GetComponent<GridNode>();
            }
        }

        //GetNeighboors(gridNodes[2, 2], true);
        //CompassNeighbors(gridNodes[2, 2]);
    }

    public override int GetNumberOfEdges()
    {
        return (int)Mathf.Pow(rows - 1, cols) + (int)Mathf.Pow(cols - 1, rows); // complete graph
    }

    protected override void CreateEdges(string mode)
    {
        for (int row=0; row < rows; row++)
        {
            for (int col=0; col < cols; col++)
            {
                switch (mode)
                {
                    case UtilGraph.FULL_EDGES: FullGrid(row, col); break;
                    case UtilGraph.PARTIAL_EDGES: PartialGrid2(row, col); break;
                    default: Debug.LogError("'" + mode + "' mode not implemented!"); break;
                }
            }
        }
    }

    // Builds efficiently a full grid
    private void FullGrid(int row, int col)
    {
        Vector3 n1, n2;

        // Adding edges to odd rows and even columns, or even rows and odd columns
        if (row % 2 != 0 && col % 2 == 0 || row % 2 == 0 && col % 2 != 0)
        {
            n1 = gridNodes[row, col].transform.position;
            List<GridNode> neighbors = GetCompassNeighbors(gridNodes[row, col]); //GetAllNeighbors(gridNodes[row, col]);

            for (int i = 0; i < neighbors.Count; i++)
            {
                GridNode neighbor = neighbors[i];
                if (neighbor != null)
                    n2 = neighbors[i].transform.position;
                else
                    continue;

                // Find center between nodes
                Vector3 centerPos = new Vector3(n1.x + n2.x, 0f, n1.z + n2.z) / 2;

                // Find angle
                float angle = 0f;
                if (row == neighbors[i].Cell[0])
                    angle = Mathf.Atan2(0, n2.x - n1.x) * Mathf.Rad2Deg;
                else
                    angle = Mathf.Atan2(gridSpace, n2.x - n1.x) * Mathf.Rad2Deg;

                // Initialize edge
                // Fix: South pointing directed egdes swapped nodes
                if (i == 2)
                    CreateEdge(neighbors[i], gridNodes[row, col], centerPos, angle);
                else
                    CreateEdge(gridNodes[row, col], neighbors[i], centerPos, angle);
            }
        }
    }

    // TESTING *****
    private void PartialGrid2(int row, int col)
    {
        Vector3 n1, n2;
        GridNode[,] neighbors = GetNeighbors(gridNodes[row, col], false);
        n1 = gridNodes[row, col].transform.position;

        for (int i=0; i < neighbors.GetLength(0); i++)
        {
            for (int j=0; j < neighbors.GetLength(1); j++)
            {
                GridNode neighbor = neighbors[i, j];

                // 2nd node cant be the same as the 1st
                if (neighbor == gridNodes[row, col] || neighbor == null)
                    continue;

                // Making things a bit random, build if...
                bool buildEdge = Random.Range(0, 10) < 5;
                if (!buildEdge)
                    continue;

                buildEdge = true;
                for (int k=0; k < neighbor.Edges.Count; k++) // need some optimization and changes
                {
                    if (neighbor.Edges[k].OtherNodeConnected(gridNodes[row, col]))
                    {
                        buildEdge = false;
                        break;
                    }
                }

                // Blocks building edges ontop of each other
                if (!buildEdge)
                    continue;

                // Build to 2nd node in position n2
                n2 = neighbors[i, j].transform.position;

                // Find center between nodes
                Vector3 centerPos = new Vector3(n1.x + n2.x, 0f, n1.z + n2.z) / 2;

                // Find angle
                float angle = 0f;
                if (row == neighbors[i, j].Cell[0])
                    angle = Mathf.Atan2(0, n2.x - n1.x) * Mathf.Rad2Deg;
                else
                    angle = Mathf.Atan2(gridSpace, n2.x - n1.x) * Mathf.Rad2Deg;

                // Initialize edge
                // Fix: South pointing directed egdes swapped nodes
                if (i == 2)
                    CreateEdge(neighbors[i, j], gridNodes[row, col], centerPos, angle);
                else
                    CreateEdge(gridNodes[row, col], neighbors[i, j], centerPos, angle);
            }
        }
    }

    // Goes through each node and tries to build edges from it (random chance for building*)
    private void PartialGrid(int row, int col)
    {
        Vector3 n1, n2;

        n1 = gridNodes[row, col].transform.position;
        List<GridNode> neighbors = GetAllNeighbors(gridNodes[row, col]);

        for (int i = 0; i < neighbors.Count; i++)
        {
            GridNode neighbor = neighbors[i];
            bool buildEdge = Random.Range(0, 10) < 5;

            if (neighbor != null && buildEdge)
                n2 = neighbors[i].transform.position;
            else
                continue;

            // Find center between nodes
            Vector3 centerPos = new Vector3(n1.x + n2.x, 0f, n1.z + n2.z) / 2;

            // Find angle
            float angle = 0f;
            if (row == neighbors[i].Cell[0])
                angle = Mathf.Atan2(0, n2.x - n1.x) * Mathf.Rad2Deg;
            else
                angle = Mathf.Atan2(gridSpace, n2.x - n1.x) * Mathf.Rad2Deg;

            // Initialize edge
            // Fix: South pointing directed egdes swapped nodes
            if (i == 2)
                CreateEdge(neighbors[i], gridNodes[row, col], centerPos, angle);
            else
                CreateEdge(gridNodes[row, col], neighbors[i], centerPos, angle);
        }
    }





    /* Returns a matrix of the neighboors of parameter node
     * 
     * |0   0   0
     * |  O O O
     * |0 O X O 0   Neighboors (X: node, O: depht=1, 0: depth=2)
     * |  O O O
     * |0   0   0
    */
    public GridNode[,] GetNeighbors(GridNode node, bool doubleDepth)
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
        GridNode[,] neighbors = new GridNode[rows, cols];

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
                if (IsValidNeighbor(z, x, cellZ, cellX, doubleDepth))
                {
                    neighbors[i, j] = gridNodes[cellZ, cellX];
                    //gridNodes[cellZ, cellX].CurrentColor = Color.red; // Debugging color
                }
                else
                    neighbors[i, j] = null; // ?
            }
        }
        //gridNodes[z, x].CurrentColor = Color.green; // Debugging color
        return neighbors;
    }

    // Checks whether node[z2, x2] is a neighboor of gridNodes[z1,x1]
    private bool IsValidNeighbor(int z1, int x1, int z2, int x2, bool doubleDepth)
    {
        int zDif = Mathf.Abs(z1 - z2);
        int xDif = Mathf.Abs(x1 - x2);
        if (z2 < 0 || x2 < 0 || z2 >= rows || x2 >= cols) // Inside grid
            return false;
        else if (gridNodes[z2, x2] == null) // exists
            return false;
        else
            return zDif % 2 == 0 && xDif % 2 == 0 || zDif <= 1 && xDif <= 1;  // double depth
    }

    // Not as intended (3*3 matrix)
    public List<GridNode> GetAllNeighbors(GridNode node)
    {
        List<GridNode> neighbors = new List<GridNode>();
        int z = node.Cell[0];
        int x = node.Cell[1];

        if (rows > 0)
        {
            for (int i = Mathf.Max(0, z - 1); i <= Mathf.Min(z + 1, rows); i++)
            {
                for (int j = Mathf.Max(0, x - 1); j <= Mathf.Min(x + 1, cols); j++)
                {
                    if (i >= 0 && i < rows && j >= 0 && j < cols) // needed?
                    {
                        if (i != z || j != x)
                            neighbors.Add(gridNodes[i, j]);
                    }
                }
            }
        }

        //for (int q = 0; q < neighbors.Count; q++)
        //{
        //    neighbors[q].CurrentColor = Color.red;
        //}
        return neighbors;
    }

    public List<GridNode> GetCompassNeighbors(GridNode node)
    {
        List<GridNode> neighbors = new List<GridNode>();
        int z = node.Cell[0], x = node.Cell[1];

        // West
        if (x - 1 >= 0)
            neighbors.Add(gridNodes[z, x - 1]);
        else
            neighbors.Add(null);

        // North
        if (z - 1 >= 0)
            neighbors.Add(gridNodes[z - 1, x]);
        else
            neighbors.Add(null);

        // South
        if (z + 1 < rows)
            neighbors.Add(gridNodes[z + 1, x]);
        else
            neighbors.Add(null);

        // West
        if (x + 1 < cols)
            neighbors.Add(gridNodes[z, x + 1]);
        else
            neighbors.Add(null);

        return neighbors;
    }

    // Return cell with a=row and b=column
    public override Node GetNode(int a, int b)
    {
        if (a >= 0 && a < rows && b >= 0 && b < cols)
            return gridNodes[a, b];
        Debug.LogError("Invalid node!");
        return null;
    }

    public override void ResetGraph()
    {
        for (int i=0; i < rows; i++)
        {
            for (int j=0; j < cols; j++)
            {
                gridNodes[i, j].ResetNode();
            }
        }
    }

    public override void DeleteGraph()
    {
        Node.NODE_ID = 0;
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                Destroy(gridNodes[i, j]);
            }
        }
    }

    protected override IEnumerator BacktrackShortestPaths(float seconds)
    {
        for (int z=rows-1; z >= 0; z--)
        {
            for (int x=cols-1; x >= 0; x--)
            {
                Node node = gridNodes[z, x];

                // Start backtracking from end node back to start node
                while (node != null)
                {
                    if (node.CurrentColor == null)
                        Debug.LogError("WTF!?!?");

                    // Change color of node
                    node.CurrentColor = UtilGraph.SHORTEST_PATH_COLOR;

                    // Change color of edge leading to previous node
                    Edge backtrackEdge = node.PrevEdge;

                    if (node.PrevEdge == null || node.PrevEdge.CurrentColor == UtilGraph.SHORTEST_PATH_COLOR)
                    {
                        node.CurrentColor = UtilGraph.SHORTEST_PATH_COLOR;
                        break;
                    }

                    backtrackEdge.CurrentColor = UtilGraph.SHORTEST_PATH_COLOR;

                    // Set "next" node
                    node = backtrackEdge.OtherNodeConnected(node);
                    yield return new WaitForSeconds(seconds);
                }
            }
        }
    }
}
