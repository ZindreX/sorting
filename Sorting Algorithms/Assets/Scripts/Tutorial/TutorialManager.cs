using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class TutorialManager : MonoBehaviour {

    [SerializeField]
    private TutorialArea startArea;

    private TutorialArea playerCurrentlyInArea, arrowInArea;

    private void Awake()
    {
        PlayerInArea(startArea);
    }

    private void Update()
    {
        if (arrowInArea == playerCurrentlyInArea)
            ArrowVisible(false);
    }

    public void PlayerInArea(TutorialArea area)
    {
        Debug.Log("Report from area: " + area);
        playerCurrentlyInArea = area;
        MoveArrow();
    }

    private void ArrowVisible(bool visible)
    {
        MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer mesh in meshRenderers)
        {
            mesh.enabled = visible;
        }
    }

    public void MoveArrow()
    {
        TutorialArea nextArea = playerCurrentlyInArea.NextArea;

        if (nextArea != null)
        {
            ArrowVisible(true);
            transform.position = nextArea.transform.position;
            arrowInArea = nextArea;
            Debug.Log("Moving arrow to area: " + nextArea.AreaName);
        }
    }




}
