using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackboard : MonoBehaviour {

    /* -------------------------------------------- Blackboard --------------------------------------------
     * 
     * 
    */

    private static readonly int TITLE_LABEL = 0, STATUS_LABEL = 1, SCORE_LABEL = 2, SPLIT_LABEL = 3;
    private static readonly int STATUS_TEXT = 0, SCORE_TEXT = 1;


    [SerializeField]
    private TextMesh[] labels, texts;

    [SerializeField]
    private Font font;

    private Color standardColor = Color.white;

    void Awake()
    {
        //font = Resources.Load("Roboto-bold") as Font;
    }

    public Color StandardColor
    {
        get { return standardColor; }
    }

    public void SetTitleLabel(string title)
    {
        labels[TITLE_LABEL].text = title;
    }

    public void SetResultText(string result)
    {
        texts[STATUS_TEXT].text = result;
    }

    public void SetScoreText(string score)
    {
        texts[SCORE_TEXT].text = score;
    }

    // Sets up the blackboard before a Tutorial or User Test
    public void InitializeBlackboard(bool tutorial)
    {
        if (tutorial)
        {
            // Status label
            labels[STATUS_LABEL].transform.position = gameObject.transform.position + new Vector3(0f, -1.63f, 0f);
            // Status text
            texts[STATUS_TEXT].transform.position = gameObject.transform.position + new Vector3(0f, -3.4f, 0f);
            // Split label
            labels[SPLIT_LABEL].GetComponentInChildren<MeshRenderer>().enabled = false;
            // Score label
            labels[SCORE_LABEL].GetComponentInChildren<MeshRenderer>().enabled = false;
            // Score text
            texts[SCORE_TEXT].GetComponentInChildren<MeshRenderer>().enabled = false;
        }
        else
        {
            // Result label
            labels[STATUS_LABEL].transform.position = gameObject.transform.position + new Vector3(-2.69f, -1.63f, 0f);
            // Result text
            texts[STATUS_TEXT].transform.position = gameObject.transform.position + new Vector3(-2.69f, -3.4f, 0f);
            // Score label
            labels[SCORE_LABEL].transform.position = gameObject.transform.position + new Vector3(2.69f, -1.63f, 0f);
            // Score text
            texts[SCORE_TEXT].transform.position = gameObject.transform.position + new Vector3(2.69f, -3.4f, 0f);
        }
    }


    private Dictionary<int, TextMesh> pseudoCodeParts;
    public void CreatePseudoCodeSetup(Dictionary<int, string> pseudoCode, Vector3 startPos)
    {
        for (int x = 0; x < labels.Length; x++)
        {
            labels[x].text = "";
        }
        foreach (TextMesh text in texts)
        {
            text.text = "";
        }

        pseudoCodeParts = new Dictionary<int, TextMesh>();
        for (int x=0; x < pseudoCode.Count; x++)
        {
            GameObject temp = new GameObject("PseudoCodePart " + x);
            temp.transform.position = startPos + new Vector3(0f, -x, 0f);
            TextMesh textMesh = temp.AddComponent<TextMesh>();
            //textMesh.font = font;
            temp.transform.localScale += new Vector3(-0.5f, -0.5f, -0.5f);

            textMesh.text = pseudoCode[x];
            pseudoCodeParts.Add(x, textMesh);
        }
    }

    public void HighLightPseudoCodePart(int part, Color color)
    {
        pseudoCodeParts[part].color = color;
    }
}
