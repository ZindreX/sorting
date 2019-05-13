using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util : MonoBehaviour {

    // Support until lvl
    public static readonly int PSEUDO_CODE_MAX_DIFFICULTY = ADVANCED;
    public static readonly int PSEUDO_CODE_HIGHTLIGHT_MAX_DIFFICULTY = BEGINNER;
    public static readonly int SOUND_EFFECT_MAX_DIFFICULTY = EXAMINATION; // Not implemented/decided yet

    // ----------------------------------------- GUI ----------------------------------------- 
    // Buttons
    public const string ONE_ACTIVE_BUTTON = "One active button", TOGGLE_BUTTON = "Toggle button", MULTI_STATE_BUTTON = "Multi state button", STATIC_BUTTON = "Static button";
    public const string START_BUTTON = "Start button";

    // Sections
    public const string ALGORITHM = "Algorithm", TEACHING_MODE = "Teaching mode", DIFFICULTY = "Difficulty", SLOW_STR = "Slow", NORMAL_STR = "Normal", FAST_STR = "Fast", S_FAST_STR = "SFast";
    public const string DEMO_SPEED = "Demo speed";
    public const string PLUS = "Plus", MINUS = "Minus", ON = "ON", OFF = "OFF", START = "Start", READY = "Ready", OPTIONAL = "Optional";
    public const string PSEUDOCODE_STEP = "Pseudocode step", PSEUDOCODE_LINE_NR = "Pseudocode line nr", SCORE = "Score";

    // ----------------------------------------- ... ----------------------------------------- 

    // Teaching modes
    public const string DEMO = "Demo", STEP_BY_STEP = "Step by step", USER_TEST = "User test";

    // Algorithms
    public const string SORTING_ALGORITHMS = "Sorting algorithms", GRAPH_ALGORITHMS = "Graph algorithms";
        
    // Sorting Algorithms
    public const string BUBBLE_SORT = "Bubble sort", INSERTION_SORT = "Insertion sort", MERGE_SORT = "Merge sort", QUICK_SORT = "Quick sort";
    public const string BUCKET_SORT = "Bucket sort";

    // Graph Algorithms
    public const string BFS = "BFS", DFS = "DFS", DFS_RECURSIVE = "DFS recursive", DIJKSTRA = "Dijkstra";
    //public const string TREE_PRE_ORDER_TRAVERSAL = "Pre order", TREE_IN_ORDER_TRAVERSAL = "In order", TREE_POST_ORDER_TRAVERSAL = "Post order";
    public const string QUEUE = "Queue", STACK = "Stack", PRIORITY_LIST = "Priority";

    // Difficulty
    public const string BEGINNER_STR = "Beginner", INTERMEDIATE_STR = "Intermediate", ADVANCED_STR = "Advanced", EXAMINATION_STR = "Examination";
    public const int BEGINNER = 0, INTERMEDIATE = 1, ADVANCED = 2, EXAMINATION = 3; // enum?
    public const int SLOW = 0, NORMAL = 1, FAST = 2, SFAST = 3;

    // Rooms
    public const string START_ROOM = "Start room", MAIN_MENU = "Main menu", TUTORIAL_ROOM = "Tutorial room", VR_TEST_ROOM = "VR test room";
    public const string TUTORIAL = "Tutorial";

    // Colors
    public static Color STANDARD_COLOR = Color.black, BLACKBOARD_TEXT_COLOR = Color.white;
    public static Color HIGHLIGHT_USER_ACTION = Color.green, HIGHLIGHT_STANDARD_COLOR = Color.magenta, HIGHLIGHT_CONDITION_FULFILLED = Color.blue, HIGHLIGHT_CONDITION_NOT_FULFILLED = Color.red;


    // Instruction values (todo: update Utilsort)
    public static readonly int NO_VALUE = -3, NO_INDEX_VALUE = -4, NO_INSTRUCTION_NR = -5;
    public const string INIT_OK = "Init ok", INIT_ERROR = "Init error";

    public const string FIRST_INSTRUCTION = "First instruction", FINAL_INSTRUCTION = "Final instruction";
    public const string INIT_INSTRUCTION = "Init instruction", EXECUTED_INST = "Executed", NOT_EXECUTED = "Not executed";

    // Variables
    public const string INCREMENT_VAR_I = "Increment variable i";
    public const string SET_VAR_J = "Set variable j", UPDATE_VAR_J = "Update variable J";
    public const string SET_VAR_K = "Set variable K", UPDATE_VAR_K = "Update variable K";

    // Controllers
    public const string INCREMENT = "Incremenet", DECREMENT = "Decrement";

    //
    public const string ALGORITHM_MANAGER = "Algorithm manager";


    public const string SPLIT_INST = "::", INVALID_PSEUDO_CODE_LINE = "X";
    public static readonly char PSEUDO_SPLIT_LINE_ID = '§';

    // Skip words
    public static readonly string SKIP_NO_ELEMENT = "Skip no element", SKIP_NO_DESTINATION = "Skip no destination";

    // Roll for
    public static readonly int ROLL_MIN = 0, ROLL_MAX = 10;


    // ----- Area stuff -----
    public const string INIT_AREA = "Init area", LOAD_SCENE = "Load scene", HEAD_COLLIDER = "HeadCollider", FLOOR_TAG = "Floor";

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

    public static bool ConvertStringToBool(string str)
    {
        return str == ON;
    }

    public static void ResetRotation(GameObject obj)
    {
        obj.transform.rotation = Quaternion.identity;
    }

    public static bool RollRandom(int below)
    {
        return Random.Range(ROLL_MIN, ROLL_MAX) < below;
    }

    public static Dictionary<int, string> difficultyConverterDict = new Dictionary<int, string>() { { BEGINNER, BEGINNER_STR }, { INTERMEDIATE, INTERMEDIATE_STR }, { ADVANCED, ADVANCED_STR }, { EXAMINATION, EXAMINATION_STR } };
    //public static string ConvertDifficulty(int difficulty)
    //{
    //    switch (difficulty)
    //    {
    //        case 0: return BEGINNER_STR;
    //        case 1: return INTERMEDIATE_STR;
    //        case 2: return ADVANCED_STR;
    //        case 3: return EXAMINATION_STR;
    //        default: return "'" + difficulty + "' not found";
    //    }
    //}

    public static Dictionary<int, string> algorithSpeedConverterDict = new Dictionary<int, string>() { { SLOW, SLOW_STR }, { NORMAL, NORMAL_STR }, { FAST, FAST_STR }, { SFAST, S_FAST_STR } };
    //public static string ConvertAlgorithmSpeed(int speed)
    //{
    //    switch (speed)
    //    {
    //        case 0: return SLOW_STR;
    //        case 1: return NORMAL_STR;
    //        case 2: return FAST_STR;
    //        default: return "'" + speed + "' not found";
    //    }
    //}

    public static Vector3 moveUnderGround = new Vector3(0f, 10f, 0f);

    public static void HideObject(GameObject obj, bool visible)
    {
        Vector3 pos = obj.transform.position;
        if (visible && obj.transform.position.y < 0)
            obj.transform.position += moveUnderGround;
        else if (obj.transform.position.y > 0)
            obj.transform.position -= moveUnderGround;
    }

    //public static void HideObject(GameObject obj, bool visible, bool moveDown)
    //{
    //    if (moveDown)
    //    {
    //        Vector3 pos = obj.transform.position;
    //        if (visible && obj.transform.position.y < 0)
    //            obj.transform.position += moveUnderGround;
    //        else if (obj.transform.position.y > 0)
    //            obj.transform.position -= moveUnderGround;
    //    }

    //    //Component[] visibleParts = obj.GetComponentsInChildren<MeshRenderer>();
    //    //foreach (MeshRenderer part in visibleParts)
    //    //{
    //    //    part.enabled = visible;
    //    //}
    //}


    public static bool ObjectWithinBounderies(Vector3 objectPos, Vector3 bounderiesObject, float within)
    {
        float objectRelativeToBounderiesObjectX = Mathf.Abs(objectPos.x - bounderiesObject.x);
        float objectRelativeToBounderiesObjectZ = Mathf.Abs(objectPos.z - bounderiesObject.z);

        return objectRelativeToBounderiesObjectX < within && objectRelativeToBounderiesObjectZ < within;
    }
}
