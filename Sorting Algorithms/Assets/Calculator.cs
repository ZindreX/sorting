using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Valve.VR.InteractionSystem;

public class Calculator : MonoBehaviour {

    public static readonly char NO_OP_CHOSEN = ' ';
    public static readonly Color STANDARD_BACKGROUND_COLOR = Color.black, CORRECT_INPUT_ANSWER_COLOR = Color.green, INCORRECT_INPUT_ANSWER_COLOR = Color.red;
    public static int BLANK = -1, INF = -2;
    public const string NORMAL_FUNCTION = "Normal function";
    public const string VALUE1_ACTION = "Value 1 action", VALUE2_ACTION = "Value 2 action", VALUE3_ACTION = "Value 3 action", OP_ACTION = "OP action", LESS_GREATER_ACTION = "Less greater action";

    private AudioSource audioSource;

    [SerializeField]
    private TextMeshPro displayText;

    [SerializeField]
    private Renderer displayBackground;

    /* Value represented by a list
     * 
     * List: [ 7, 3, 3, 1 ] -> 1*10^3 + 3*10^2 + 3*10^1 + 7*10^0 = 1337
     * 
     * 
     * 
    */

    private string task;
    private List<int> number1, number2; // Could just concacinate a string and parse to integer?
    private int value1, value2, result;
    private char op;
    private bool calculationInProcess, equalButtonClicked;
    private Stack<string> undoActions;

    // For graph
    public const string GRAPH_TASK = "Graph task";
    public static readonly string ANSWER_NOT_CHOSEN = "No answer";
    private List<int> number3;
    private int value3;
    private string newValueLessThanCurrent;



    [SerializeField]
    private Transform leftHand;
    private Camera playerCamera;
    private Rigidbody rb;

    private bool playerHoldingCalculator;


    private Vector3 startPos;
    private WaitForSeconds feedbackDuration = new WaitForSeconds(3f);

    private void Awake()
    {
        startPos = transform.position;
        playerCamera = FindObjectOfType<Player>().gameObject.GetComponentInChildren<Camera>();
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponentInParent<AudioSource>();
    }

    private void Update()
    {
        if (throwAble && transform.position.y < 0.1f)
            PlaceCalculator();
    }

    // Prepare for a new calculation
    public void InitCalculation(string task)
    {
        displayText.text = "Fill in the\n if-statement";
        this.task = task;

        number1 = new List<int>();
        number2 = new List<int>();

        value1 = BLANK;
        value2 = BLANK;
        result = BLANK;

        op = NO_OP_CHOSEN;


        if (task == GRAPH_TASK)
        {
            number3 = new List<int>();
            value3 = BLANK;
            newValueLessThanCurrent = ANSWER_NOT_CHOSEN;

        }

        calculationInProcess = true;
        equalButtonClicked = false;
        feedbackReceived = false;
        undoActions = new Stack<string>();
    }

    // ------------------------------------- Getters/Setters -------------------------------------

    private bool throwAble;
    public void PlayerHoldingCalculator(bool holding)
    {
        playerHoldingCalculator = holding;

        if (holding)
        {
            rb.useGravity = true;
            rb.constraints = RigidbodyConstraints.None;
        }
        else
        {
            if (!throwAble)
                rb.constraints = RigidbodyConstraints.FreezeAll;
        }


    }

    public int Value1
    {
        get { return value1; }
    }

    public int Value2
    {
        get { return value2; }
    }

    public int Result
    {
        get { return result; }
    }

    public bool CalculationInProcess
    {
        get { return calculationInProcess; }
    }

    public bool EqualButtonClicked
    {
        get { return equalButtonClicked; }
        set { equalButtonClicked = value; }
    }

    public string DisplayText
    {
        get { return displayText.text; }
    }

    // ------------------------------------- Methods -------------------------------------

    private int ValueOf(List<int> list)
    {
        int listLength = list.Count;
        if (listLength > 0)
        {
            int result = 0;

            for (int i = 0; i < listLength; i++)
            {
                if (list[i] == INF)
                {
                    result = INF;
                    break;
                }

                result += (int)Mathf.Pow(10, i) * list[i];
            }

            // For fun
            throwAble = (result == 1337);

            return result;
        }
        return BLANK;
    }

    private void UpdateDisplay(bool equalButtonClicked)
    {
        string text = "";

        // Check if value(s) and operator is inserted, display what have been inserted so far
        if (value1 != BLANK)
        {
            text += ConvertInf(value1);

            if (op != NO_OP_CHOSEN)
            {
                text += " " + op + " ";

                if (value2 != BLANK)
                {
                    text += ConvertInf(value2);

                    switch (task)
                    {
                        case NORMAL_FUNCTION: if (equalButtonClicked) text += " = " + result; break;
                        case GRAPH_TASK:

                            if (newValueLessThanCurrent != ANSWER_NOT_CHOSEN)
                            {
                                text += " " + newValueLessThanCurrent + " ";

                                if (value3 != BLANK)
                                    text += ConvertInf(value3);
                            }
                            break;
                    }
                }
            }
        }
        displayText.text = text;
    }

    private string ConvertInf(int value)
    {
        return (value == int.MaxValue) ? "Inf" : value.ToString();
    }

    // ------------------------------------- Button click -------------------------------------

    // Button 0 - 9 input
    public void ValueButtonClick(int value)
    {
        if (!calculationInProcess)
            return;

        audioSource.Play();

        if (value == INF)
            value = int.MaxValue;


        if (op == NO_OP_CHOSEN)
        {
            number1.Insert(0, value);
            value1 = ValueOf(number1);
            undoActions.Push(VALUE1_ACTION);
        }
        else if (newValueLessThanCurrent == ANSWER_NOT_CHOSEN)
        {
            number2.Insert(0, value);
            value2 = ValueOf(number2);
            undoActions.Push(VALUE2_ACTION);
        }
        else
        {
            number3.Insert(0, value);
            value3 = ValueOf(number3);
            undoActions.Push(VALUE3_ACTION);
        }

        UpdateDisplay(false);
    }

    // Called by one of the operator buttons on the calculator
    public void OperatorButtonClick(string op)
    {
        if (!calculationInProcess)
            return;

        audioSource.Play();

        // if user click operator button then set value 1 as zero
        if (number1.Count == 0)
        {
            number1.Add(0);
            value1 = ValueOf(number1);
            undoActions.Push(VALUE1_ACTION);
        }

        this.op = char.Parse(op);
        undoActions.Push(OP_ACTION);

        UpdateDisplay(false);
    }

    // Equal (=) button clicked
    public void EqualButtonClick()
    {
        if (!calculationInProcess)
            return;

        audioSource.Play();

        if (value1 != BLANK && value2 != BLANK)
        {
            switch (op)
            {
                case '+': result = value1 + value2; break;
                case '-': result = value1 - value2; break;
                case '*': result = value1 * value2; break;
                case '/': result = value1 / value2; break;
                case '%': result = value1 % value2; break;
                default: Debug.LogError("Unknown operator '" + op + "'."); break;
            }
            UpdateDisplay(true);
            equalButtonClicked = true;
        }
        else
            displayText.text = "Invalid values";
    }

    // Undo (TODO: improve)
    public void Undo()
    {
        if (!calculationInProcess)
            return;

        audioSource.Play();

        if (undoActions.Count > 0)
        {
            string prevAction = undoActions.Pop();

            switch (prevAction)
            {
                case VALUE1_ACTION:
                    number1.RemoveAt(0);
                    value1 = ValueOf(number1);
                    break;

                case VALUE2_ACTION:
                    number2.RemoveAt(0);
                    value2 = ValueOf(number2);
                    break;

                case OP_ACTION:
                    op = NO_OP_CHOSEN;
                    break;

                case VALUE3_ACTION:
                    number3.RemoveAt(0);
                    value3 = ValueOf(number3);
                    break;

                case LESS_GREATER_ACTION:
                    newValueLessThanCurrent = ANSWER_NOT_CHOSEN;
                    break;
            }
            UpdateDisplay(false);
        }

    }

    public void GreaterOrLessAnswerButtonClick(string answer)
    {
        if (!calculationInProcess)
            return;

        audioSource.Play();

        newValueLessThanCurrent = answer;
        undoActions.Push(LESS_GREATER_ACTION);

        UpdateDisplay(false);

    }


    // ------------------------------------- Graph related -------------------------------------

    private bool feedbackReceived;    
    public bool FeedbackReceived
    {
        get { return feedbackReceived; }
        set { feedbackReceived = value; }
    }

    public void PlaceCalculator()
    {
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeAll;

        // Place infront of player
        transform.position = playerCamera.transform.position + playerCamera.transform.forward * 0.4f;
        
        // Rotate towards the player
        transform.LookAt(2 * transform.position - playerCamera.transform.position);
    }

    // Checks whether the input is correct
    public bool ControlUserInput(int correctNode1Dist, int correctEdgeCost, int correctNode2Dist)
    {
        string feedback = "";

        if (correctNode1Dist == value1 && correctEdgeCost == value2)
        {
            int newDist = correctNode1Dist + correctEdgeCost;

            if (newDist < correctNode2Dist && newValueLessThanCurrent == "<" || newDist > correctNode2Dist && newValueLessThanCurrent == ">")
            {
                if (correctNode2Dist == value3)
                {
                    return Feedback(true, feedback);
                }
                else
                {
                    feedback = "right hand side error";
                    return Feedback(false, feedback);
                }
            }
            else
            {
                feedback += "krokodille kjeft error";
                return Feedback(false, feedback);
            }
        }

        if (correctNode1Dist != value1)
            feedback += "> w node dist error\n";

        if (op != '+')
            feedback += "operand error";

        if (correctEdgeCost != value2)
            feedback += "> edge cost error\n";

        return Feedback(false, feedback);
    }

    public bool Feedback(bool correctAnswer, string feedback)
    {
        StartCoroutine(InputFeedback(correctAnswer));
        if (correctAnswer)
            return true;

        displayText.text = feedback;
        return false;
    }

    // Gives feedback whether the calculation exercise was performed correctly
    private IEnumerator InputFeedback(bool correctInput)
    {
        if (correctInput)
            displayBackground.material.color = CORRECT_INPUT_ANSWER_COLOR;
        else
            displayBackground.material.color = INCORRECT_INPUT_ANSWER_COLOR;

        yield return feedbackDuration;
        displayBackground.material.color = STANDARD_BACKGROUND_COLOR;

        if (correctInput)
        {
            calculationInProcess = false;
            transform.position = startPos;
        }
        else
            InitCalculation(GRAPH_TASK);
    }

    public void ResetCalculator()
    {
        rb.useGravity = true;
        transform.position = startPos;
    }


}
