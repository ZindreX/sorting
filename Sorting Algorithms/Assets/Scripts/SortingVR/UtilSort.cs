﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* -------------------------------------- Utility methods & constants --------------------------------------
 * Generalized methods and such...
 * 
 * ---------------------------------------------------------------------------------------------------------
*/

public class UtilSort : Util {

    // ----------------------------------------- Support until lvl ----------------------------------------- 
    public static readonly int HOLDER_FEEDBACK_MAX_DIFFICULTY = INTERMEDIATE;


    // ******************************************** Settings ********************************************
    // Rules (cases)
    public const string NONE = "None", WORST_CASE = "Worst case", BEST_CASE = "Best case", DUPLICATES = "Duplicates", HELP_ENABLED = "Help enabled";

    // Rules (numbers)
    public static readonly int MAX_NUMBER_OF_ELEMENTS = 8, MAX_NUMBER_OF_BUCKETS = 10;

    // ******************************************** Instructions ******************************************** 
    // Instructions
    public static readonly int NO_DESTINATION = -1, INIT_STATE = -2;


    // Instructions (strings)
    public const string PIVOT_START_INST = "Pivot start", PIVOT_END_INST = "Pivot end";
    public const string COMPARE_START_INST = "Compare start", COMPARE_END_INST = "Compare end", SWITCH_INST = "Switching";

    // Loops
    public const string FIRST_LOOP = "First loop", END_LOOP_INST = "End of loop";
    public const string UPDATE_LOOP_INST = "Update loop";
    public const int OUTER_LOOP = -1, INNER_LOOP = -2; // Help out, might change later

    // Other instructions
    public const string SET_SORTED_INST = "Set sorted";

    // Bucket sort
    public const string CREATE_BUCKETS_INST = "Create buckets", BUCKET_INDEX_INST = "Bucket index", MOVE_TO_BUCKET_INST = "Move to bucket", PHASING_INST = "Phasing", MOVE_BACK_INST = "Move back", DISPLAY_ELEMENT = "Display element";

    // Checking instruction (strings)
    public const string MOVE_INTERMEDIATE = "Move intermediate";
    public const string CORRECT_HOLDER = "Correct holder", WRONG_HOLDER = "Wrong holder", CANNOT_VALIDATE_ERROR = "Cannot validate error";
    public const string CORRECT_BUCKET = "Correct bucket", WRONG_BUCKET = "Wrong bucket";


    // ******************************************** Collision/tags ********************************************
    // Object types
    public static readonly string PLAYER_TAG = "Player", SORTING_ELEMENT_TAG = "SortingElement", HOLDER_TAG = "Holder", BUCKET_TAG = "Bucket";


    // ******************************************** Visualization ********************************************
    // Colors used
    public static Color PIVOT_COLOR = Color.cyan, COMPARE_COLOR = Color.blue, SORTED_COLOR = Color.green, ERROR_COLOR = Color.red;
    public static Color MOVING_WRONG = Color.yellow, TEST_COLOR = Color.cyan;

    // The distance above a holder (when teleporting a element)
    public static Vector3 ABOVE_HOLDER_VR = new Vector3(0f, 0.06f, 0f), ABOVE_BUCKET_VR = new Vector3(0f, 1.0f, 0f);
    public static float ELEMENT_STACK_VR = 3f;

    // Spacing
    public static float SPACE_BETWEEN_HOLDERS = 0.2f, SPACE_BETWEEN_BUCKETS = 0.5f, SPACE_BETWEEN_CODE_LINES = 0.2f;

    // Timing
    public static float COLOR_CHANGE_TIMER = 1.05f;


    // ---------------------------------------------- Interface ----------------------------------------------

    // Buttons
    public const string NUMBER_OF_ELEMENTS = "Number of elements", SORTING_CASE = "Sorting case", NUMBER_OF_BUCKETS = "Number of buckets";


    // ******************************************** Outdated stuff? ********************************************
    // Buttons
    public const string NUMBER_BUTTONS = "Number buttons", RULE_BUTTONS = "Rule buttons", ON_OR_OFF = "On or off", PORTAL = "Portal", PORTAL_OBJECT = "Portal object", QUIT = "Quit";

    // Other switch cases
    public const string INIT = "Init", UPDATE_BLACKBOARD = "Update blackboard";


    // Displays
    public const string SORT_TABLE_TEXT = "Sort table text", LEFT_BLACKBOARD = "Left blackboard", RIGHT_BLACKBOARD = "Right blackboard", SETTINGS_MENU_TEXT = "Settings menu text";
    public const char BLACKBOARD_SPLIT = ':';

    public static string TranslateNextHolder(int nextHolderID)
    {
        return (nextHolderID == NO_DESTINATION) ? "N/A" : nextHolderID.ToString();
    }

    public static void IndicateElement(GameObject obj)
    {
        if (obj != null)
            obj.transform.position += ABOVE_HOLDER_VR;
    }

    public static string TranslateInstructionForExamination(string instruction)
    {
        switch (instruction)
        {
            case INIT_INSTRUCTION: return "Wrong non-sorted element moved";
            case PIVOT_START_INST: return "Wrong pivot chosen";
            case PIVOT_END_INST: return "Pivot placed wrong";
            case COMPARE_START_INST: return "Comparison";
            case COMPARE_END_INST: return "Moved sorted element to wrong holder";
            case SWITCH_INST: return "Wrong element moved";
            default: return instruction;
        }
    }
}
