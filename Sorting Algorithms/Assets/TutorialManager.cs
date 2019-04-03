using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour {

    [SerializeField]
    private TutorialArea startArea;

    private TutorialArea currentArea;

    private void Awake()
    {
        PlayerInArea(startArea);
        CurrentAreaCleared();
    }

    public void PlayerInArea(TutorialArea area)
    {
        currentArea = area;
        CurrentAreaCleared();
    }

    public void CurrentAreaCleared()
    {
        TutorialArea nextArea = currentArea.NextArea;
        if (nextArea != null)
        {
            transform.position = currentArea.NextArea.transform.position;
        }
    }


}
