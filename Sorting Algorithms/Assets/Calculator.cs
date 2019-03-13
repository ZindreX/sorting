using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Valve.VR.InteractionSystem;

public class Calculator : MonoBehaviour {

    public static readonly char NO_OP_CHOSEN = ' ';
    public static readonly Color STANDARD_BACKGROUND_COLOR = Color.black, CORRECT_INPUT_ANSWER_COLOR = Color.green, INCORRECT_INPUT_ANSWER_COLOR = Color.red;
    public static int BLANK = -1;

    [SerializeField]
    private TextMeshPro displayText;

    [SerializeField]
    private Renderer displayBackground;

    private List<int> number1, number2; // Could just concacinate a string and parse to integer?
    private int value1, value2, result;
    private char op;
    private bool calculationInProcess, equalButtonClicked;

    private Camera playerCamera;

    private Vector3 startPos;
    private WaitForSeconds feedbackDuration = new WaitForSeconds(3f);

    private void Awake()
    {
        startPos = transform.position;
        playerCamera = FindObjectOfType<Player>().gameObject.GetComponentInChildren<Camera>();
    }

    private void Update()
    {
        if (calculationInProcess)
        {
            transform.position = playerCamera.transform.position + new Vector3(0f, -0.25f, 0.4f);
            transform.LookAt(2 * transform.position - playerCamera.transform.position);
        }
    }

    public void InitCalculation()
    {
        number1 = new List<int>();
        number2 = new List<int>();
        op = NO_OP_CHOSEN;
        value1 = BLANK;
        value2 = BLANK;
        result = BLANK;
        calculationInProcess = true;
        equalButtonClicked = false;
        UpdateDisplay(false);
    }

    public void ValueButtonClick(int value)
    {
        if (op == NO_OP_CHOSEN)
        {
            number1.Insert(0, value);
            value1 = ValueOf(number1);
        }
        else
        {
            number2.Insert(0, value);
            value2 = ValueOf(number2);
        }

        UpdateDisplay(false);
    }

    // Called by one of the operator buttons on the calculator
    public void OperatorButtonClick(string op)
    {
        // if user click operator button then set value 1 as zero
        if (number1.Count == 0)
            number1.Add(0);

        this.op = char.Parse(op);
        UpdateDisplay(false);
    }

    private void UpdateDisplay(bool equalButtonClicked)
    {
        string text = "";

        // Check if value(s) and operator is inserted, display what have been inserted so far
        if (value1 != BLANK)
        {
            text += value1;

            if (op != NO_OP_CHOSEN)
            {
                text += op;

                if (value2 != BLANK)
                {
                    text += value2;

                    if (equalButtonClicked)
                        text += " = " + result;
                }
            }
        }

        displayText.text = text;
    }

    private int ValueOf(List<int> list)
    {
        if (list.Count > 0)
        {
            int listLength = list.Count;
            int result = 0;

            for (int i=0; i < listLength; i++)
            {
                result += (int)Mathf.Pow(10, i) * list[i];
            }
            return result;
        }
        return BLANK;
    } 

    public void EqualButtonClick()
    {
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


    public void Undo()
    {
        InitCalculation();
        UpdateDisplay(false);
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

    public bool ControlUserInput(int correctValue1, int correctValue2)
    {
        if (correctValue1 == value1 && correctValue2 == value2)
        {
            StartCoroutine(CalculationFeedback(true));
            return true;
        }
        StartCoroutine(CalculationFeedback(false));
        return false;
    }

    private IEnumerator CalculationFeedback(bool correctInput)
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
            InitCalculation();
        
    }

}
