using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util : MonoBehaviour {

    // Buttons
    public const string ALGORITHM = "Algorithm", TEACHING_MODE = "Teaching mode", DIFFICULTY = "Difficulty", SLOW_SPEED = "Slow", NORMAL_SPEED = "Normal", FAST_SPEED = "Fast", TEST_SPEED = "Test speed";
    public const string ALGORITHM_SPEED = "Algorithm speed";

    // Teaching modes
    public const string DEMO = "Demo", STEP_BY_STEP = "Step by step", USER_TEST = "User test";

    // Algorithms
    public const string SORTING_ALGORITHMS = "Sorting algorithms", GRAPH_ALGORITHMS = "Graph algorithms";
        
    // Sorting Algorithms
    public const string BUBBLE_SORT = "Bubble sort", INSERTION_SORT = "Insertion sort", MERGE_SORT = "Merge sort", QUICK_SORT = "Quick sort";
    public const string BUCKET_SORT = "Bucket sort";

    // Graph Algorithms
    public const string BFS = "Breadth-First Search", DFS = "Depth-First Search", DFS_RECURSIVE = "DFS recursive", DIJKSTRA = "Dijkstra";
    //public const string TREE_PRE_ORDER_TRAVERSAL = "Pre order", TREE_IN_ORDER_TRAVERSAL = "In order", TREE_POST_ORDER_TRAVERSAL = "Post order";
    public const string QUEUE = "Queue", STACK = "Stack", PRIORITY_LIST = "Priority";

    // Difficulty
    public const string BEGINNER_STR = "Beginner", INTERMEDIATE_STR = "Intermediate", ADVANCED_STR = "Advanced", EXAMINATION_STR = "Examination";
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

    // Roll for
    public static readonly int ROLL_MIN = 0, ROLL_MAX = 10;


    /* Creates a list of objects
     * - Puts them ontop of another object if positions are provided 
    */
    public static GameObject[] CreateObjects(GameObject prefab, int numberOfElements, Vector3[] pos, float spreadDist, SortMain superElement)
    {
        GameObject[] objects = new GameObject[numberOfElements];
        GameObject element;

        for (int x = 0; x < numberOfElements; x++)
        {
            if (pos.Length == 1)
                element = Instantiate(prefab, pos[0] + new Vector3(x * spreadDist, 0f, 0f), Quaternion.identity);
            else
                element = Instantiate(prefab, pos[x], Quaternion.identity);

            element.GetComponent<ISortSubElement>().SuperElement = superElement;
            objects[x] = element;
        }
        return objects;
    }

    // Destroys all objects in a list
    public static void DestroyObjects(GameObject[] objects)
    {
        Debug.Log("Destroying objects: " + objects.Length);
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

    public static bool RollRandom(int below)
    {
        return Random.Range(ROLL_MIN, ROLL_MAX) < below;
    }

    public static string ConvertDifficulty(int difficulty)
    {
        switch (difficulty)
        {
            case 1: return BEGINNER_STR;
            case 2: return INTERMEDIATE_STR;
            case 3: return ADVANCED_STR;
            case 4: return EXAMINATION_STR;
            default: return "'" + difficulty + "' not found";
        }
    }

    public static string ConvertAlgorithmSpeed(float value)
    {
        if (value == 2f)
            return SLOW_SPEED;
        else if (value == 1f)
            return NORMAL_SPEED;
        else if (value == 0.5f)
            return FAST_SPEED;
        else
            return TEST_SPEED;
    }

    public static void HideObject(GameObject obj, bool visible)
    {
        Component[] visibleParts = obj.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer part in visibleParts)
        {
            part.enabled = visible;

            //Vector3 pos = part.transform.position;
            //float yPos = pos.y + 10f;
            //if (!visible)
            //    yPos *= -1;

            //part.transform.position += new Vector3(pos.x, yPos, pos.z);

        }
    }
}
