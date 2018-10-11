using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionDevice : MonoBehaviour {

    [SerializeField]
    private GameObject[] alternativeButtons;

    [SerializeField]
    private GameObject[] choiceButtons;

    // Algorithm settings
    private int numberOfElements = 8;
    private Dictionary<string, bool> options;


    void Awake()
    {
        options = CreateOptions();
        
    }

    private Dictionary<string, bool> CreateOptions()
    {
        Dictionary<string, bool> options = new Dictionary<string, bool>();
        options.Add("Tutorial", true);
        options.Add("Help", false);
        options.Add("Duplicates", true);
        options.Add("Case", false); // Best- / worst case
        options.Add(Util.BEST_CASE, false);
        options.Add(Util.WORST_CASE, false);
        return options;
    }


    private void SetTextAlternativeButtons(string buttonSet)
    {
        switch (buttonSet)
        {
            case Util.NUMBER_BUTTONS:
                for (int x=0; x < alternativeButtons.Length; x++)
                {
                    alternativeButtons[x].GetComponentInChildren<TextMesh>().text = x.ToString();
                }
                break;
            case Util.RULE_BUTTONS: break;

        }
    }

    private void SetColorAlternativeButtons(string buttonMode)
    {
        switch (buttonMode)
        {
            case Util.ON_OR_OFF:

                break;
            case Util.PORTAL:
                for (int x=0; x < alternativeButtons.Length; x++)
                {
                    alternativeButtons[x].GetComponentInChildren<TextMesh>().text = Util.ConvertSceneBuildIndexToName(x);
                }
                break;
        }
    }


}
