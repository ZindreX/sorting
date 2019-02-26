using System;
using System.Collections.Generic;
using UnityEngine;

public class UtilGraph : Util {

    public static readonly int GRAPH_MIN_X = -8, GRAPH_MAX_X = 8, GRAPH_MIN_Z = 0, GRAPH_MAX_Z = 16, EDGE_MAX_WEIGHT = 101;

    // Rolling random numbers: Random.Range(ROLL_START, ROLL_END) < 'INSERT_NAME'_CHANCE
    public static readonly int ROLL_MIN = 0, ROLL_MAX = 10, SYMMETRIC_EDGE_CHANCE = 10, PARTIAL_BUILD_TREE_CHILD_CHANCE = 4, BUILD_EDGE_CHANCE = 3;

    // Graph structures
    public const string GRID_GRAPH = "Grid", TREE_GRAPH = "Tree", RANDOM_GRAPH = "Random graph", UNDIRECTED_EDGE = "Undirected edge", DIRECED_EDGE = "Directed edge", SYMMETRIC_EDGE = "Symmetric edge";
    public const string FULL_EDGES = "Full edges", FULL_EDGES_NO_CROSSING = "Full edges no crossing", PARTIAL_EDGES = "Partial edges", PARTIAL_EDGES_NO_CROSSING = "Partial edges no crossing ";
    public const string MINIMUM_EDGES = "Minimum edges";

    // Node types
    public const string GRID_NODE = "Grid node", TREE_NODE = "Tree node", RANDOM_NODE = "Random node";

    // Compass
    public const string NORTH = "North", NORTH_WEST = "North west", NORTH_EAST = "North east", SOUTH = "South", SOUTH_WEST = "South west", SOUTH_EAST = "South east";
    public const string WEST = "West", EAST = "East";

    // Graph objects
    public static string NODE_TAG = "Node", EDGE_TAG = "Edge";

    // 
    public static int INF = int.MaxValue, NO_COST = -1;

    // Pseudo code stuff
    public static readonly string START_NODE = "Start node", LENGTH_OF_LIST = "Length of list", NODE = "vertice", NEIGHBOR = "neighbor";

    // Colors
    public static Color TRAVERSE_COLOR = Color.red, VISITED_COLOR = Color.yellow, TRAVERSED_COLOR = Color.green, SHORTEST_PATH_COLOR = Color.magenta;
    public static Color CURRENT_NODE_COLOR = Color.red, DIST_UPDATE_COLOR = Color.cyan;

    // *** Instructions ***
    // BFS
    public const string EMPTY_QUEUE_INST = "Empty queue inst", ENQUEUE_NODE_INST = "Enqueue node inst", MARK_VISITED_INST = "Mark visited inst";
    public const string WHILE_LIST_NOT_EMPTY_INST = "While list not empty inst", DEQUEUE_NODE_INST = "Dequeue node inst", FOR_ALL_NEIGHBORS_INST = "For all neighbors inst";
    public const string IF_NOT_VISITED_INST = "If not visited inst", END_IF_INST = "End if inst", END_FOR_LOOP_INST = "End for loop inst", END_WHILE_INST = "End while inst";


    // Spacing
    public static float SPACE_BETWEEN_CODE_LINES = 0.6f;


    // Converts cost into 'Inf' if cost is infinity, otherwise return input
    public static string ConvertIfInf(int value)
    {
        return (value == INF) ? "Inf" : value.ToString();
    }

    // Converts cost from string to int
    public static int ConvertCostToInt(string costText)
    {
        int cost;
        bool isNumberic = int.TryParse(costText, out cost); // C# 7:         var isNumeric = int.TryParse("123", out int n);

        return (isNumberic) ? cost : INF;
    }

    public static char ConvertIDToAlphabet(int nr)
    {
        return Convert.ToChar(nr + 65);
    }

    public static float DistanceBetweenNodes(Transform n1, Transform n2)
    {
        return Mathf.Sqrt(Mathf.Pow(n1.position.x - n2.position.x, 2) + Mathf.Pow(n1.position.z - n2.position.z, 2));
    }

    public static string GetCompassDirection(int i, int j)
    {
        switch (i)
        {
            case 0:
                switch (j)
                {
                    case 0: return NORTH_WEST;
                    case 1: return NORTH;
                    case 2: return NORTH_EAST;
                }
                break;

            case 1:
                switch (j)
                {
                    case 0: return WEST;
                    case 1: return null;
                    case 2: return EAST;
                }
                break;

            case 2:
                switch (j)
                {
                    case 0: return SOUTH_WEST;
                    case 1: return SOUTH;
                    case 2: return SOUTH_EAST;
                }
                break;
        }
        return null;
    }

    //string compassDir = UtilGraph.GetCompassDirection(i, j);
    //switch (compassDir)
    //{
    //    case UtilGraph.NORTH: case UtilGraph.SOUTH:
    //        angle = Mathf.Atan2(0, n2.x - n1.x) * Mathf.Rad2Deg;
    //        break;

    //    case UtilGraph.NORTH_WEST: case UtilGraph.NORTH_EAST: case UtilGraph.EAST: case UtilGraph.WEST:
    //        angle = Mathf.Atan2(gridSpace, n2.x - n1.x) * Mathf.Rad2Deg;
    //        break;

    //    case UtilGraph.SOUTH_WEST: case UtilGraph.SOUTH_EAST:
    //        angle = Mathf.Atan2(gridSpace, n2.x - n1.x) * Mathf.Rad2Deg;
    //        break;
    //    default: Debug.LogError("Compass direction unknown!"); break;
    //}
}

