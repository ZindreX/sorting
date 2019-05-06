using System;
using System.Collections.Generic;
using UnityEngine;

public class UtilGraph : Util {

    // ----------------------------------------- Graph limitations ----------------------------------------- 
    public static readonly int MAX_ROWS = 5, MAX_COLUMNS = 5;
    public static readonly int MAX_TREE_DEPTH = 3, MAX_N_TREE = 3;
    public static readonly int MAX_RANDOM_NODES = 5, MAX_RANDOM_EDGES = 20;
    public static readonly int GRAPH_MIN_X = -8, GRAPH_MAX_X = 8, GRAPH_MIN_Z = 0, GRAPH_MAX_Z = 16, EDGE_MAX_WEIGHT = 101;
    public static readonly string SYMMETRIC_EDGE_CHANCE = "Symmetric edge chance", PARTIAL_BUILD_TREE_CHILD_CHANCE = "Partial build tree child chance", BUILD_EDGE_CHANCE = "Build edge chance";

    // ----------------------------------------- Support until lvl ----------------------------------------- 
    public static readonly int LIST_VISUAL_MAX_DIFFICULTY = ADVANCED;


    // ----------------------------------------- GUI ----------------------------------------- 
    // Graph task
    public const string GRAPH_TASK = "Graph task", TRAVERSE = "Traverse", SHORTEST_PATH = "Shortest path";
    public const string TRAVERSE_SUB_SECTION = "Traverse sub section", SHORTEST_PATH_SUB_SECTION = "Shortest path sub section";
    public const string GRAPH_STRUCTURE = "Graph structure", EDGE_TYPE = "Edge type", EDGE_BUILD_MODE = "Edge mode";
    public const string GRID_SUB = "Grid sub", ROWS = "Rows", COLUMNS = "Columns";
    public const string TREE_SUB = "Tree sub", TREE_DEPTH = "Tree depth", N_TREE = "N tree";
    public const string RANDOM_SUB = "Random sub", RANDOM_NODES = "Random nodes", RANDOM_EDGES = "Random edges", RANDOM_SPACE = "Random space";
    public const string SHORTEST_PATH_OPTIONAL = "Shortest path optional";

    public const string SELECT_NODE = "Select node", NO_TASK = "No task";

    // ----------------------------------------- Animation -----------------------------------------

    public static readonly string NODE_VISIT_ANIMATION = "NodeVisit", NODE_TRAVERSE_ANIMATION = "NodeTraverse", NODE_ERROR_ANIMATION = "NodeError";


    // Rules
    public const string SHORTEST_PATH_ONE_TO_ALL = "Shortest path one to all"; // VISIT_LEFT_FIRST = "Visit left first";

    // Graph structures
    public const string GRID_GRAPH = "Grid", TREE_GRAPH = "Tree", RANDOM_GRAPH = "Random", UNDIRECTED_EDGE = "Undirected", DIRECED_EDGE = "Directed", SYMMETRIC_EDGE = "Symmetric";
    public const string FULL_EDGES = "Full", FULL_EDGES_NO_CROSSING = "Full no cross", PARTIAL_EDGES = "Partial", PARTIAL_EDGES_NO_CROSSING = "Partial no cross";

    // Node types
    public const string GRID_NODE = "Grid node", TREE_NODE = "Tree node", RANDOM_NODE = "Random node";

    // Compass
    public const string NORTH = "North", NORTH_WEST = "North west", NORTH_EAST = "North east", SOUTH = "South", SOUTH_WEST = "South west", SOUTH_EAST = "South east";
    public const string WEST = "West", EAST = "East";

    // Graph objects
    public static string NODE_TAG = "Node", EDGE_TAG = "Edge";

    // 
    public static int INF = int.MaxValue, NO_COST = -1, INIT_NODE_DIST = -2;

    // Pseudo code stuff
    public static readonly string START_NODE = "Start node", LENGTH_OF_LIST = "Length of list", NODE = "vertice", NEIGHBOR = "neighbor";

    // Colors
    public static Color TRAVERSE_COLOR = Color.blue, VISITED_COLOR = Color.yellow, TRAVERSED_COLOR = Color.green, SHORTEST_PATH_COLOR = Color.magenta;
    public static Color DIST_UPDATE_COLOR = Color.cyan, STANDARD_COST_TEXT_COLOR = Color.white;

    // *** Instructions ***
    // Validation
    public const string NODE_VISITED = "Node visited", NODE_TRAVERSED = "Node traversed", NODE_BACKTRACKED = "Node backtracked", NODE_ERROR = "Node error", NODE_OK = "Node ok";

    // Common graph instructions
    public const string EMPTY_LIST_CONTAINER = "Empty list inst", WHILE_LIST_NOT_EMPTY_INST = "While list not empty inst", FOR_ALL_NEIGHBORS_INST = "For all neighbors inst";
    public const string IF_NOT_VISITED_INST = "If not visited inst", END_IF_INST = "End if inst", END_FOR_LOOP_INST = "End for loop inst", END_WHILE_INST = "End while inst";

    // BFS
    public const string ENQUEUE_NODE_INST = "Enqueue node inst", DEQUEUE_NODE_INST = "Dequeue node inst";

    // DFS
    public const string PUSH_INST = "Push node inst", POP_INST = "Pop node inst";

    // Dijkstra
    public const string SET_ALL_NODES_TO_INFINITY = "Set all nodes to infinity", SET_START_NODE_DIST_TO_ZERO = "Set start node dist to zero", PRIORITY_REMOVE_NODE = "Priority remove node", VISIT_CONNECTED_NODE = "Visit connected node";
    public const string IF_DIST_PLUS_EDGE_COST_LESS_THAN = "If dist plus edge cost less than", UPDATE_CONNECTED_NODE_DIST = "Update connected node dist", UPDATE_CONNECTED_NODE_PREV_EDGE = "Update connected node prev edge";
    public const string UPDATE_LIST_VISUAL_VALUE_AND_POSITION = "Update list visual value and position", END_NODE_FOUND = "End node found", NO_PATH_FOUND = "No path found";

    // Backtracking
    public const string MARK_END_NODE = "Mark end node", BACKTRACK = "Backtrack";



    // ----------------------------------------- List visual -----------------------------------------

    public const string LIST_VISUAL = "List visual";
    public const string ADD_NODE = "Add node", PRIORITY_ADD_NODE = "Priority add node", REMOVE_CURRENT_NODE = "Remove current node";
    public const string DESTROY_CURRENT_NODE = "Destroy current node", HAS_NODE_REPRESENTATION = "Has node representation";
    public const string PREPARE_BACKTRACKING = "Prepare backtracking", BACKTRACK_REMOVE_CURRENT_NODE = "Backtrack remove current node";


    // Spacing
    public static float SPACE_BETWEEN_CODE_LINES = 0.6f;

    // Height
    public static Vector3 increaseHeightOfText = new Vector3(0f, 0.5f, 0f);


    // Converts cost into 'Inf' if cost is infinity, otherwise return input
    public static string ConvertDist(int value)
    {
        if (value == INF)
            return "Inf";
        else if (value == INIT_NODE_DIST)
            return "";
        return value.ToString();
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

}

