using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : GraphManager {

    public readonly int MAX_ROWS = 5, MAX_COLUMNS = 5;

    private int rows, cols, gridSpace;

    private GridNode[,] gridNodes;

    // Finds the prefab, but null when Instantiate
    //protected virtual void Awake()
    //{
    //    Debug.Log("Fixing prefabs!");
    //    nodePrefab = (GameObject)Resources.Load("Assets/Prefabs/Graph/Node", typeof(GameObject));
    //    undirectedEdgePrefab = (GameObject)Resources.Load("Assets/Prefabs/Graph/UndirectedEdge", typeof(GameObject));
    //    directedEdgePrefab = (GameObject)Resources.Load("Assets/Prefabs/Graph/DirectedEdge", typeof(GameObject));
    //    symmetricDirectedEdgePrefab = (GameObject)Resources.Load("Assets/Prefabs/Graph/SymmetricDirectedEdge", typeof(GameObject));
    //    Debug.Log("Node prefab loaded: " + nodePrefab != null);
    //}

    public override void InitGraph(int[] graphStructure)
    {        
        // Init graph strucutre values
        rows = graphStructure[0];
        cols = graphStructure[1];
        gridSpace = graphStructure[2];
    }

    public override int GetMaxNumberOfNodes()
    {
        return rows * cols;
    }

    public GridNode[,] GridNodes
    {
        get { return gridNodes; }
    }

    public override void CreateNodes(string mode)
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
                node.GetComponent<GridNode>().InitGridNode(algorithmName, new int[2] { z, x });
                node.transform.parent = nodeContainerObject.transform;
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

    public override void CreateEdges(string mode)
    {
        for (int row=0; row < rows; row++)
        {
            for (int col=0; col < cols; col++)
            {
                GridNode currentNode = gridNodes[row, col];
                switch (mode)
                {
                    case UtilGraph.FULL_EDGES: BuildEdges(currentNode, false, Util.ROLL_MAX); break;
                    case UtilGraph.FULL_EDGES_NO_CROSSING: BuildEdgesNoCrossing(currentNode, row, col, Util.ROLL_MAX); break;
                    case UtilGraph.PARTIAL_EDGES: BuildEdges(currentNode, false, LookUpRNGDict(UtilGraph.BUILD_EDGE_CHANCE)); break;
                    case UtilGraph.PARTIAL_EDGES_NO_CROSSING: BuildEdgesNoCrossing(currentNode, row, col, Util.ROLL_MAX); break;
                    default: Debug.LogError("'" + mode + "' mode not implemented!"); break;
                }
            }
        }
    }

    // Builds efficiently a full grid w/o any crossing edges
    private void BuildEdgesNoCrossing(GridNode currentNode, int row, int col, int chance)
    {
        bool useCompassFormation = row % 2 == 0 && col % 2 == 0;
        bool useStartFormation = row % 2 != 0 && col % 2 != 0;

        // Build edges from this node if valid node for compass/star formation
        if (useCompassFormation || useStartFormation)
        {
            Vector3 n1 = currentNode.transform.position, n2;
            List<GridNode> neighbors = null;

            if (useCompassFormation)
                neighbors = GetCompassNeighbors(currentNode);
            else if (useStartFormation)
                neighbors = GetStarNeighbors(currentNode);

            for (int i = 0; i < neighbors.Count; i++)
            {
                if (Util.RollRandom(Util.ROLL_MAX - chance))
                    continue;

                GridNode neighbor = neighbors[i];
                if (neighbor != null)
                    n2 = neighbors[i].transform.position;
                else
                    continue;

                // Find center between nodes
                Vector3 centerPos = new Vector3(n1.x + n2.x, 0f, n1.z + n2.z) / 2;

                // Find angle
                float angle = Mathf.Atan2(n2.z - n1.z, n2.x - n1.x) * Mathf.Rad2Deg;

                // Initialize edge
                CreateEdge(currentNode, neighbor, centerPos, -angle);
            }
        }
    }

    // Tries to build edges from the input node (total 8 nodes, see GetNeighbors method)
    private void BuildEdges(GridNode currentNode, bool doubleDepth, int chance)
    {
        Vector3 n1 = currentNode.transform.position, n2;
        GridNode[,] neighbors = GetNeighbors(currentNode, doubleDepth);

        for (int i=0; i < neighbors.GetLength(0); i++)
        {
            for (int j=0; j < neighbors.GetLength(1); j++)
            {
                GridNode neighbor = neighbors[i, j];

                // 2nd node cant be the same as the 1st
                if (neighbor == currentNode || neighbor == null)
                    continue;

                // Making things a bit random, build if...
                bool buildEdge = true;
                if (neighbor.IsStartNode || neighbor.IsEndNode) //row == START_Z && col == START_X || row == END_Z && col == END_X)
                    buildEdge = true;
                else
                    buildEdge = Util.RollRandom(chance);

                if (!buildEdge)
                    continue;

                buildEdge = true;

                if (neighbor.IsAlreadyNeighbor(currentNode))
                    buildEdge = false;

                // Blocks building edges ontop of each other
                if (!buildEdge)
                    continue;

                // Build to 2nd node in position n2
                n2 = neighbors[i, j].transform.position;

                // Find center between nodes
                Vector3 centerPos = new Vector3(n1.x + n2.x, 0f, n1.z + n2.z) / 2;

                // Find angle
                float angle = Mathf.Atan2(n2.z - n1.z, n2.x - n1.x) * Mathf.Rad2Deg;

                // Initialize edge
                CreateEdge(currentNode, neighbor, centerPos, -angle);
            }
        }
        // Destroy nodes with no connected neighbors
        //if (currentNode.NumberOfNeighbors() == 0)
        //{
        //    Destroy(currentNode.gameObject);
        //    gridNodes[row, col] = null;
        //}
    }

    private void CheckAndDestroyUnreachableNodes()
    {

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
    public List<GridNode> GetStarNeighbors(GridNode node)
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
            return gridNodes[b, a];
        Debug.LogError("Invalid node! : a = " + a + ", b = " + b);
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

        base.ResetGraph();
    }

    public override void DeleteGraph()
    {
        Node.NODE_ID = 0;
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                Destroy(gridNodes[i, j].gameObject);
            }
        }

        gridNodes = null;

        // Delete edges
        base.DeleteGraph();
    }

    public override void SetAllNodesToInf()
    {
        for (int i=0; i < rows; i++)
        {
            for (int j=0; j < cols; j++)
            {
                GridNode gridNode = gridNodes[i, j];
                if (gridNode != null)
                    gridNode.Dist = UtilGraph.INF;
            }
        }
    }

    public override IEnumerator BacktrackShortestPathsAll(WaitForSeconds demoStepDuration)
    {
        for (int z=rows-1; z >= 0; z--)
        {
            for (int x=cols-1; x >= 0; x--)
            {
                Node node = gridNodes[z, x];

                // Start backtracking from end node back to start node
                while (node != null)
                {
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
                    yield return demoStepDuration;
                }
            }
        }
    }
}
