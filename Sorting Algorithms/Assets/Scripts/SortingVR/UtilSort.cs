using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* -------------------------------------- Utility methods & constants --------------------------------------
 * Generalized methods and such...
 * 
 * ---------------------------------------------------------------------------------------------------------
*/

public class UtilSort : Util {

    // ******************************************** Settings ********************************************
    // Rules (cases)
    public const string NONE = "None", WORST_CASE = "Worst case", BEST_CASE = "Best case", DUPLICATES = "Duplicates", HELP_ENABLED = "Help enabled";

    // Rules (numbers)
    public static readonly int MAX_NUMBER_OF_ELEMENTS = 8, MAX_VALUE = 100;

    // ******************************************** Instructions ******************************************** 
    // Instructions
    public static readonly int NO_DESTINATION = -1, INIT_STATE = -2, NO_VALUE = -3;

    // Help out, might change later
    public const int OUTER_LOOP = -1, INNER_LOOP = -2;

    // Instructions (strings)
    public const string INIT_INSTRUCTION = "Init instruction", PIVOT_START_INST = "Pivot start", PIVOT_END_INST = "Pivot end";
    public const string COMPARE_START_INST = "Compare start", COMPARE_END_INST = "Compare end", SWITCH_INST = "Switching", EXECUTED_INST = "Executed", NOT_EXECUTED = "Not executed";
    public const string CREATE_BUCKETS_INST = "Create buckets", BUCKET_INDEX_INST = "Bucket index", MOVE_TO_BUCKET_INST = "Move to bucket", PHASING_INST = "Phasing", MOVE_BACK_INST = "Move back";
    public const string FIRST_INSTRUCTION = "First instruction", FINAL_INSTRUCTION = "Final instruction", UPDATE_LOOP_INST = "Update loop", DISPLAY_ELEMENT = "Display element";
    public const string END_LOOP_INST = "End of loop", SET_SORTED_INST = "Set sorted";
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
    public static readonly string PLAYER_TAG = "Player", SORTING_ELEMENT_TAG = "SortingElement", HOLDER_TAG = "Holder", BUCKET_TAG = "Bucket", TOOLTIPS_ELEMENT = "Tooltips element";


    // ******************************************** Visualization ********************************************
    // Colors used
    public static Color PIVOT_COLOR = Color.cyan, COMPARE_COLOR = Color.blue, SORTED_COLOR = Color.green, ERROR_COLOR = Color.red;
    public static Color MOVING_WRONG = Color.yellow, TEST_COLOR = Color.cyan;

    // The distance above a holder (when teleporting a element)
    public static Vector3 ABOVE_HOLDER_VR = new Vector3(0f, 0.06f, 0f), ABOVE_BUCKET_VR = new Vector3(0f, 1.0f, 0f);

    // Spacing
    public static float SPACE_BETWEEN_HOLDERS = 0.2f, SPACE_BETWEEN_BUCKETS = 0.5f, SPACE_BETWEEN_CODE_LINES = 0.25f;

    // Timing
    public static float COLOR_CHANGE_TIMER = 0.25f;


    // ******************************************** Outdated stuff? ********************************************
    // Buttons
    public const string NUMBER_BUTTONS = "Number buttons", RULE_BUTTONS = "Rule buttons", ON_OR_OFF = "On or off", PORTAL = "Portal", PORTAL_OBJECT = "Portal object", QUIT = "Quit";

    // Other switch cases
    public const string INIT = "Init", UPDATE_BLACKBOARD = "Update blackboard";

    public static List<string> skipAbleInstructions = new List<string>() { FIRST_INSTRUCTION, FINAL_INSTRUCTION, COMPARE_START_INST, COMPARE_END_INST, UPDATE_LOOP_INST };

    public static string TranslateNextHolder(int nextHolderID)
    {
        return (nextHolderID == NO_DESTINATION) ? "N/A" : nextHolderID.ToString();
    }

    public static void IndicateElement(GameObject obj)
    {
        obj.transform.position += ABOVE_HOLDER_VR;
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
}
