using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util : MonoBehaviour {

    // Teaching modes
    public const string DEMO = "Demo", STEP_BY_STEP = "Step by step", USER_TEST = "User test";

    // Sorting Algorithms
    public const string BUBBLE_SORT = "Bubble sort", INSERTION_SORT = "Insertion sort", MERGE_SORT = "Merge sort", QUICK_SORT = "Quick sort";
    public const string BUCKET_SORT = "Bucket sort";

    // Graph Algorithms
    public const string BFS = "Breadth-First Search", DFS = "Depth-First Search", DIJKSTRA = "Dijkstra";

    // Difficulty
    //public const string BEGINNER = "Beginner", INTERMEDIATE = "Intermediate", ADVANCED = "Advanced", EXAMINATION = "Examination";
    public const int BEGINNER = 1, INTERMEDIATE = 2, ADVANCED = 3, EXAMINATION = 4;

    // Rooms
    public const string START_ROOM = "Start room", MAIN_MENU = "Main menu", TUTORIAL_ROOM = "Tutorial room", VR_TEST_ROOM = "VR test room";
    public const string TUTORIAL = "Tutorial";

    // Colors
    public static Color STANDARD_COLOR = Color.black;


    public static string ConvertSceneBuildIndexToName(int sceneBuildIndex)
    {
        switch (sceneBuildIndex)
        {
            case 0: return START_ROOM;
            case 1: return TUTORIAL_ROOM;
            case 2: return MAIN_MENU;
            case 3: return BUBBLE_SORT;
            case 4: return INSERTION_SORT;
            case 5: return BUCKET_SORT;
            default: return "X";

        }
    }
}
