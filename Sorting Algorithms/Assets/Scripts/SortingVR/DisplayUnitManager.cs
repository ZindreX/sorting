using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayUnitManager : MonoBehaviour {

    [SerializeField]
    private GameObject leftBlackboard, centerBlackboard, rightBlackboard;

    private SortAlgorithm algorithm;

    public SortAlgorithm Algorithm
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

    public void SetAlgorithmForPseudo(SortAlgorithm algorithm)
    {
        PseudoCodeViewer.InitPseudoCodeViewer(algorithm, UtilSort.SPACE_BETWEEN_CODE_LINES);
        //PseudoCodeViewerFixed.SetAlgorithm(algorithm);
    }


    public void ResetDisplays()
    {
        //leftBlackboard.GetComponent<PseudoCodeViewer>().EmptyContent();
        centerBlackboard.GetComponent<PseudoCodeViewer>().EmptyContent();
        rightBlackboard.GetComponent<Blackboard>().EmptyContent();
    }


}
