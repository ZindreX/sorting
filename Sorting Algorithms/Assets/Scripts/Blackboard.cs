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

    void Awake()
    {
        //font = Resources.Load("Roboto-bold") as Font;
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
    public void InitializeBlackboard(string teachingMode)
    {
        switch (teachingMode)
        {
            case Util.TUTORIAL:
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
                break;

            case Util.TUTORIAL_STEP:
                break;

            case Util.USER_TEST:
                // Result label
                labels[STATUS_LABEL].transform.position = gameObject.transform.position + new Vector3(-2.69f, -1.63f, 0f);
                // Result text
                texts[STATUS_TEXT].transform.position = gameObject.transform.position + new Vector3(-2.69f, -3.4f, 0f);
                // Score label
                labels[SCORE_LABEL].transform.position = gameObject.transform.position + new Vector3(2.69f, -1.63f, 0f);
                // Score text
                texts[SCORE_TEXT].transform.position = gameObject.transform.position + new Vector3(2.69f, -3.4f, 0f);
                break;
        }
    }
}
