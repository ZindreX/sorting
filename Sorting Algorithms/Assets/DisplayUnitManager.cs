using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayUnitManager : MonoBehaviour {

    [SerializeField]
    private GameObject LeftBlackboard, centerBlackboard, rightBlackboard;

    private Algorithm algorithm;

    public Algorithm Algorithm
    {
        get { return algorithm; }
        set { algorithm = value; }
    }

    public PseudoCodeViewer PseudoCodeViewerFixed
    {
        get { return LeftBlackboard.GetComponent<PseudoCodeViewer>(); }
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


}
