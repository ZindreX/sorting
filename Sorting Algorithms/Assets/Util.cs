﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util : MonoBehaviour {

    // Teaching modes
    public const string DEMO = "Demo", STEP_BY_STEP = "Step by step", USER_TEST = "User test";

    // Sorting Algorithms
    public const string BUBBLE_SORT = "Bubble sort", INSERTION_SORT = "Insertion sort", MERGE_SORT = "Merge sort", QUICK_SORT = "Quick sort";
    public const string BUCKET_SORT = "Bucket sort";

    // Graph Algorithms
    public const string BFS = "Breadth-First Search", DFS = "Depth-First Search", DFS_RECURSIVE = "DFS recursive", DIJKSTRA = "Dijkstra";
    //public const string TREE_PRE_ORDER_TRAVERSAL = "Pre order", TREE_IN_ORDER_TRAVERSAL = "In order", TREE_POST_ORDER_TRAVERSAL = "Post order";
    public const string QUEUE = "Queue", STACK = "Stack", PRIORITY_LIST = "Priority";

    // Difficulty
    //public const string BEGINNER = "Beginner", INTERMEDIATE = "Intermediate", ADVANCED = "Advanced", EXAMINATION = "Examination";
    public const int BEGINNER = 1, INTERMEDIATE = 2, ADVANCED = 3, EXAMINATION = 4;

    // Rooms
    public const string START_ROOM = "Start room", MAIN_MENU = "Main menu", TUTORIAL_ROOM = "Tutorial room", VR_TEST_ROOM = "VR test room";
    public const string TUTORIAL = "Tutorial";

    // Colors
    public static Color STANDARD_COLOR = Color.black, BLACKBOARD_TEXT_COLOR = Color.white;
    public static Color HIGHLIGHT_MOVE_COLOR = Color.green, HIGHLIGHT_COLOR = Color.magenta;


    // Instruction values (todo: update Utilsort)
    public static readonly int NO_VALUE = -3, NO_INDEX_VALUE = -4;

    public const string SPLIT_INST = "::";
    public static readonly char PSEUDO_SPLIT_LINE_ID = ':';

    // Skip words
    public static readonly string SKIP_NO_ELEMENT = "Skip no element", SKIP_NO_DESTINATION = "Skip no destination";


    /* Creates a list of objects
     * - Puts them ontop of another object if positions are provided 
    */
    public static GameObject[] CreateObjects(GameObject prefab, int numberOfElements, Vector3[] pos, float spreadDist, GameObject parent)
    {
        GameObject[] objects = new GameObject[numberOfElements];
        GameObject element;

        for (int x = 0; x < numberOfElements; x++)
        {
            if (pos.Length == 1)
                element = Instantiate(prefab, pos[0] + new Vector3(x * spreadDist, 0f, 0f), Quaternion.identity);
            else
                element = Instantiate(prefab, pos[x], Quaternion.identity);

            element.GetComponent<IChild>().Parent = parent;
            objects[x] = element;
        }
        return objects;
    }

    // Destroys all objects in a list
    public static void DestroyObjects(GameObject[] objects)
    {
        if (objects != null)
        {
            for (int i = 0; i < objects.Length; i++)
            {
                Destroy(objects[i]);
            }
        }
    }

    // Gather positions of one type of GameObject, such that other objects can be put directly above
    public static Vector3[] GatherPositionsOfListElements(GameObject[] list)
    {
        Vector3[] positions = new Vector3[list.Length];
        for (int x = 0; x < list.Length; x++)
        {
            positions[x] = list[x].transform.position + new Vector3(0f, 1f, 0f);
        }
        return positions;
    }

    public static string ModifyPluralString(string singular, int number)
    {
        return (number > 1) ? singular + "s" : singular;
    }

    public static string EnabledToString(bool enabled)

    {
        return enabled ? "Enabled" : "Disabled";
    }

    public static void ResetRotation(GameObject obj)
    {
        obj.transform.rotation = Quaternion.identity;
    }

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
