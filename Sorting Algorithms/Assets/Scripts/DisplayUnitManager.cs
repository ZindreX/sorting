using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayUnitManager : MonoBehaviour {

    [SerializeField]
    private GameObject leftBlackboard, centerBlackboard, rightBlackboard;

    private Algorithm algorithm;

    public Algorithm Algorithm
    {
        get { return algorithm; }
        set { algorithm = value; }
    }

    public PseudoCodeViewer PseudoCodeViewerFixed
    {
        get { return leftBlackboard.GetComponent<PseudoCodeViewer>(); }
    }

    // Algorithm will control this blackboard
    public PseudoCodeViewer PseudoCodeViewer
    {
        get { return centerBlackboard.GetComponent<PseudoCodeViewer>(); }
    }

    public Blackboard BlackBoard
    {
        get { return rightBlackboard.GetComponent<Blackboard>(); }
    }

    public void ResetDisplays()
    {
        leftBlackboard.GetComponent<PseudoCodeViewer>().EmptyContent();
        centerBlackboard.GetComponent<PseudoCodeViewer>().EmptyContent();
        rightBlackboard.GetComponent<Blackboard>().EmptyContent();
    }


}
