using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilGraph : MonoBehaviour {

    public static readonly int GRAPH_MIN_X = -8, GRAPH_MAX_X = 8, GRAPH_MIN_Z = 0, GRAPH_MAX_Z = 16;

    // Graph structures
    public const string GRID = "Grid", TREE = "Tree";

    // Algorithms
    public const string BFS = "Breadth-First Search", DFS = "Depth-First Search", DIJKSTRA = "Dijkstra";

    // 
    public static int INF = int.MaxValue;


    // Colors
    public static Color STANDARD_COLOR = Color.black, TRAVERSE_COLOR = Color.red, MARKED = Color.yellow, TRAVERSED_COLOR = Color.green;



    public static string ConvertIfInf(string costText)
    {
        int cost;
        bool isNumberic = int.TryParse(costText, out cost);
        return (cost == INF) ? "Inf" : costText;
    }


    public static int ConvertCostToInt(string costText)
    {
        int cost;
        bool isNumberic = int.TryParse(costText, out cost); // C# 7:         var isNumeric = int.TryParse("123", out int n);

        return (isNumberic) ? cost : INF;

    }

}
