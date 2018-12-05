using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* -------------------------------------- Utility methods & constants --------------------------------------
 * Generalized methods and such...
 * 
 * ---------------------------------------------------------------------------------------------------------
*/

public class Util : MonoBehaviour {

    // ******************************************** Settings ********************************************
    // Algorithms
    public const string BUBBLE_SORT = "Bubble sort", INSERTION_SORT = "Insertion sort", MERGE_SORT = "Merge sort", QUICK_SORT = "Quick sort";
    public const string BUCKET_SORT = "Bucket sort";

    // Teaching modes
    public const string TUTORIAL = "Tutorial", STEP_BY_STEP = "Step by step", USER_TEST = "User test";

    // Difficulty
    //public const string BEGINNER = "Beginner", INTERMEDIATE = "Intermediate", ADVANCED = "Advanced", EXAMINATION = "Examination";
    public const int BEGINNER = 1, INTERMEDIATE = 2, ADVANCED = 3, EXAMINATION = 4;

    // Rules (cases)
    public const string NONE = "None", WORST_CASE = "Worst case", BEST_CASE = "Best case", DUPLICATES = "Duplicates", HELP_ENABLED = "Help enabled";

    // Rules (numbers)
    public static readonly int MAX_NUMBER_OF_ELEMENTS = 8, MAX_VALUE = 100;


    // ******************************************** Instructions ******************************************** 
    // Instructions
    public static readonly int NO_DESTINATION = -1, INIT_STATE = -2, NO_VALUE = -3;

    // Help out, might change later
    public const int LOOP_ONE = -1, LOOP_TWO = -2;

    // Instructions (strings)
    public const string INIT_INSTRUCTION = "Init instruction", PIVOT_START_INST = "Pivot start", PIVOT_END_INST = "Pivot end";
    public const string COMPARE_START_INST = "Compare start", COMPARE_END_INST = "Compare end", SWITCH_INST = "Switching", EXECUTED_INST = "Executed", NOT_EXECUTED = "Not executed";
    public const string CREATE_BUCKETS_INST = "Create buckets", BUCKET_INDEX_INST = "Bucket index", MOVE_TO_BUCKET_INST = "Move to bucket", PHASING_INST = "Phasing", MOVE_BACK_INST = "Move back";
    public const string FIRST_INSTRUCTION = "First instruction", FINAL_INSTRUCTION = "Final instruction", UPDATE_LOOP_INST = "Update loop", DISPLAY_ELEMENT = "Display element";
    public const string END_LOOP_INST = "End of loop";
    public const string INCREMENT_VAR_I = "Increment variable i", SET_VAR_J = "Set variable j", UPDATE_VAR_J = "Update variable J", FIRST_LOOP = "First loop";

    // Other instructions
    public const string INCREMENT = "Incremenet", DECREMENT = "Decrement";

    // Skip words
    public static readonly string SKIP_NO_ELEMENT = "Skip no element", SKIP_NO_DESTINATION = "Skip no destination";

    // Checking instruction (strings)
    public const string INIT_OK = "Init ok", INIT_ERROR = "Init error", MOVE_INTERMEDIATE = "Move intermediate";
    public const string CORRECT_HOLDER = "Correct holder", WRONG_HOLDER = "Wrong holder", CANNOT_VALIDATE_ERROR = "Cannot validate error";


    // ******************************************** Collision/tags ********************************************
    // Object types
    public static readonly string PLAYER_TAG = "Player", SORTING_ELEMENT_TAG = "SortingElement", HOLDER_TAG = "Holder", BUCKET_TAG = "Bucket";


    // ******************************************** Visualization ********************************************
    // Colors used
    public static Color PIVOT_COLOR = Color.blue, COMPARE_COLOR = Color.blue, SORTED_COLOR = Color.green, ERROR_COLOR = Color.red;
    public static Color STANDARD_COLOR = Color.black, MOVING_WRONG = Color.yellow, TEST_COLOR = Color.cyan, HIGHLIGHT_COLOR = Color.magenta;
    public static Color BLACKBOARD_TEXT_COLOR = Color.white, HIGHLIGHT_MOVE_COLOR = Color.green;

    // The distance above a holder (when teleporting a element)
    public static Vector3 ABOVE_HOLDER_VR = new Vector3(0f, 0.06f, 0f), ABOVE_BUCKET_VR = new Vector3(0f, 1.0f, 0f);

    // Spacing
    public static float SPACE_BETWEEN_HOLDERS = 0.2f, SPACE_BETWEEN_BUCKETS = 0.5f, SPACE_BETWEEN_CODE_LINES = 0.25f;

    // Timing
    public static float COLOR_CHANGE_TIMER = 0.25f;


    // ******************************************** Outdated stuff? ********************************************
    // Rooms
    public const string MAIN_MENU = "Main menu", TUTORIAL_ROOM = "Tutorial room", VR_TEST_ROOM = "VR test room";

    // Buttons
    public const string NUMBER_BUTTONS = "Number buttons", RULE_BUTTONS = "Rule buttons", ON_OR_OFF = "On or off", PORTAL = "Portal", QUIT = "Quit";

    // Other switch cases
    public const string INIT = "Init", UPDATE_BLACKBOARD = "Update blackboard";

    public static List<string> skipAbleInstructions = new List<string>() { FIRST_INSTRUCTION, FINAL_INSTRUCTION, COMPARE_START_INST, COMPARE_END_INST, UPDATE_LOOP_INST };


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

    public static string EnabledToString(bool enabled)
    {
        return enabled ? "Enabled" : "Disabled";
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

    public static string TranslateInstructionForExamination(string instruction)
    {
        switch (instruction)
        {
            case INIT_INSTRUCTION: return "Wrong non-sorted element moved";
            case PIVOT_START_INST: return "Wrong pivot chosen";
            case PIVOT_END_INST: return "Pivot placed wrong";
            //case COMPARE_START_INST: return "";
            case COMPARE_END_INST: return "Moved sorted element to wrong holder";
            case SWITCH_INST: return "Wrong element moved";
            default: return "Examination error explanation not added (" + instruction + ")";
        }
    }

    public static void IndicateElement(GameObject obj)
    {
        obj.transform.position += ABOVE_HOLDER_VR;
    }

    public static void ResetRotation(GameObject obj)
    {
        obj.transform.rotation = Quaternion.identity;
    }
}
