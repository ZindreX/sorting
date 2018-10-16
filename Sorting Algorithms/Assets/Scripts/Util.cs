using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* -------------------------------------- Utility methods & constants --------------------------------------
 * Generalized methods and such...
 * 
 * ---------------------------------------------------------------------------------------------------------
*/

public class Util : MonoBehaviour {
    // Rules (numbers)
    public static readonly int MAX_NUMBER_OF_ELEMENTS = 8, MAX_VALUE = 100, NUMBER_OF_RULE_TYPES = 2;

    // Instructions
    public static readonly int NO_DESTINATION = -1, INIT_STATE = -2, NO_VALUE = -3;

    // Instructions (strings)
    public const string INIT_INSTRUCTION = "Initialization", PIVOT_START_INST = "Pivot start", PIVOT_END_INST = "Pivot end";
    public const string COMPARE_START_INST = "Compare start", COMPARE_END_INST = "Compare end", SWITCH_INST = "Switching", EXECUTED_INST = "Executed";
    public const string MOVE_TO_BUCKET_INST = "Move to bucket", PHASING_INST = "Phasing", MOVE_BACK_INST = "Move back";

    // Checking instruction (strings)
    public const string INIT_OK = "Init ok", INIT_ERROR = "Init error", MOVE_INTERMEDIATE = "Move intermediate";
    public const string CORRECT_HOLDER = "Correct holder", WRONG_HOLDER = "Wrong holder", CANNOT_VALIDATE_ERROR = "Cannot validate error";

    // Rules (cases)
    public const string WORST_CASE = "Worst case", BEST_CASE = "Best case", DUPLICATES = "Duplicates";
    public const string TUTORIAL = "Tutorial", USER_TEST = "User test", HELP_ENABLED = "Help enabled";

    // Rooms
    public const string MAIN_MENU = "Main menu", TUTORIAL_ROOM = "Tutorial room", VR_TEST_ROOM = "VR test room";

    // Buttons
    public const string NUMBER_BUTTONS = "Number buttons", RULE_BUTTONS = "Rule buttons", ON_OR_OFF = "On or off", PORTAL = "Portal", QUIT = "Quit";

    // Algorithms
    public const string BUBBLE_SORT = "Bubble sort", INSERTION_SORT = "Insertion sort", MERGE_SORT = "Merge sort", QUICK_SORT = "Quick sort";
    public const string BUCKET_SORT = "Bucket sort";

    // Object types
    public static readonly string PLAYER_TAG = "Player", SORTING_ELEMENT_TAG = "SortingElement", HOLDER_TAG = "Holder", BUCKET_TAG = "Bucket";
    
    // Other switch cases
    public const string INIT = "Init", UPDATE_BLACKBOARD = "Update blackboard";

    // Colors used
    public static Color PIVOT_COLOR = Color.blue, COMPARE_COLOR = Color.blue, SORTED_COLOR = Color.green, ERROR_COLOR = Color.red;
    public static Color STANDARD_COLOR = Color.black, MOVING_WRONG = Color.yellow, TEST_COLOR = Color.cyan, HIGHLIGHT_COLOR = Color.magenta;
    public static Color BLACKBOARD_COLOR = Color.white;



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
        for (int x=0; x < list.Length; x++)
        {
            positions[x] = list[x].transform.position + new Vector3(0f, 1f, 0f);
        }
        return positions;
    }

    public static string ModifyPluralString(string singular, int number)
    {
        return (number > 1) ? singular + "s" : singular;
    }

    public static string TranslateNextHolder(int nextHolderID)
    {
        return (nextHolderID == NO_DESTINATION) ? "N/A" : nextHolderID.ToString();
    }

    public static string ConvertSceneBuildIndexToName(int sceneBuildIndex)
    {
        switch (sceneBuildIndex)
        {
            case 0: return MAIN_MENU;
            case 1: return TUTORIAL_ROOM;
            case 2: return BUBBLE_SORT;
            case 3: return INSERTION_SORT;
            case 4: return MERGE_SORT;
            case 5: return QUICK_SORT;
            case 6: return BUCKET_SORT;
            case 7: return VR_TEST_ROOM;
            default: return "X";

        }
    }

}
