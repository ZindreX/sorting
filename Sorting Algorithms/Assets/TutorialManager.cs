using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class TutorialManager : MonoBehaviour {

    [SerializeField]
    private TutorialArea startArea;

    private TutorialArea currentArea;

    private void Awake()
    {
        PlayerInArea(startArea);
        MoveArrow();
    }

    private void Update()
    {
        if (currentArea != null)
        {
            switch (currentArea.AreaName)
            {
                case "Graph":
                    break;

            }

        }
    }

    public void PlayerInArea(TutorialArea area)
    {
        currentArea = area;
        MoveArrow();
    }

    public void MoveArrow()
    {
        TutorialArea nextArea = currentArea.NextArea;
        if (nextArea != null && !currentArea.Cleared)
        {
            transform.position = currentArea.NextArea.transform.position;
        }
        //else if (nextArea == null)
        //    Destroy(gameObject);
    }


}
