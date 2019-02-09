using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilGraph : MonoBehaviour {

    public static readonly int GRAPH_MIN_X = -8, GRAPH_MAX_X = 8, GRAPH_MIN_Z = 0, GRAPH_MAX_Z = 16;

    public const string GRID = "Grid", TREE = "Tree";


    public static Color STANDARD_COLOR = Color.black, TRAVERSE_COLOR = Color.red, MARKED = Color.yellow, TRAVERSED_COLOR = Color.green;

}
