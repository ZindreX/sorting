using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilGraph : Util {

    public static readonly int GRAPH_MIN_X = -8, GRAPH_MAX_X = 8, GRAPH_MIN_Z = 0, GRAPH_MAX_Z = 16, EDGE_MAX_WEIGHT = 101;

    // Graph structures
    public const string GRID = "Grid", TREE = "Tree", RANDOM_GRAPH = "Random graph";

    // Graph objects
    public static string NODE = "Node", EDGE = "Edge";

    // 
    public static int INF = int.MaxValue;


    // Colors
    public static Color TRAVERSE_COLOR = Color.red, MARKED = Color.yellow, TRAVERSED_COLOR = Color.green;



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

    public static float DistanceBetweenNodes(Transform n1, Transform n2)
    {
        return Mathf.Sqrt(Mathf.Pow(n1.position.x - n2.position.x, 2) + Mathf.Pow(n1.position.z - n2.position.z, 2));
    }

}
