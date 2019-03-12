using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Calculator : MonoBehaviour {

    public static readonly char NO_OP_CHOSEN = ' ';
    public static int BLANK = -1;

    [SerializeField]
    private TextMeshPro displayText;

    private List<int> number1, number2; // Could just concacinate a string and parse to integer?
    private int value1, value2, result;
    private char op;

    private void Awake()
    {
        InitCalculation();
    }

    public void InitCalculation()
    {
        Debug.Log("Calculator initialized");
        number1 = new List<int>();
        number2 = new List<int>();
        op = NO_OP_CHOSEN;
        value1 = BLANK;
        value2 = BLANK;
        result = BLANK;
    }

    public void ValueButtonClick(int value)
    {
        Debug.Log("Button clicked: " + value);
        if (op != NO_OP_CHOSEN)
            number1.Insert(0, value);
        else
            number2.Insert(0, value);

        UpdateDisplay(true);
    }

    // Called by one of the operator buttons on the calculator
    public void OperatorButtonClick(string op)
    {
        Debug.Log("Operator clicked: " + op);
        this.op = char.Parse(op);
        UpdateDisplay(true);
    }

    private void UpdateDisplay(bool fix)
    {
        if (fix)
            MakeValues();

        displayText.text = BlankIfNoValue(value1) + "\n" + op + "        " + BlankIfNoValue(value2) + "\n=" + BlankIfNoValue(result);
    }

    private string BlankIfNoValue(int value)
    {
        return value == BLANK ? "" : value.ToString();
    }

    private void MakeValues()
    {
        value1 = ValueOf(number1);
        value2 = ValueOf(number2);
    }

    private int ValueOf(List<int> list)
    {
        if (list != null)
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
        MakeValues();
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

            UpdateDisplay(false);
        }
        else
            displayText.text = "Invalid values";
    }



    public void Undo()
    {

    }

}
