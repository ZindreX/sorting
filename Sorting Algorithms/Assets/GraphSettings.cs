using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphSettings : MonoBehaviour {

    [SerializeField]
    private GraphStructureEditor graphStructureEditor;
    private enum GraphStructureEditor { Grid, Tree }

    [SerializeField]
    private UseAlgorithmEditor useAlgorithmEditor;
    private enum UseAlgorithmEditor { BFS, DFS, Dijkstra }

    [SerializeField]
    private AlgorithmSpeedEditor algorithmSpeedEditor;
    private enum AlgorithmSpeedEditor { Slow, Normal, Fast, Test }


    // *** Grid graph ***
    [SerializeField]
    private GridRowsEditor gridRowsEditor;
    private enum GridRowsEditor { One, Two, Three, Four, Five, Test }

    [SerializeField]
    private GridColumnsEditor gridColumnsEditor;
    private enum GridColumnsEditor { One, Two, Three, Four, Five, Test }

    [SerializeField]
    private GridSpaceEditor gridSpaceEditor;
    private enum GridSpaceEditor { Four, Eight }


    // *** Tree graph ***
    [SerializeField]
    private TreeDepth treeDepthEditor;
    private enum TreeDepth { Zero, One, Two, Three, Four, Five }

    [SerializeField]
    private NTree nTreeEditor;
    private enum NTree { Binary, Ternary }

    [SerializeField]
    private NodeSpaceX nodeSpaceXEditor;
    private enum NodeSpaceX { Two, Four }

    [SerializeField]
    private NodeSpaceZ nodeSpaceZEditor;
    private enum NodeSpaceZ { Two, Four }

    [SerializeField]
    private bool visitLeftFirst;


    // *** Start-/End nodes ***
    [SerializeField]
    private int x1, z1, x2, z2;

    private float algorithmSpeed;
    private string graphStructure, useAlgorithm;
    private int gridRows, gridColumns, gridSpace;
    private int treeDepth, nTree, nodeSpaceX, nodeSpaceZ;


    public void PrepareSettings()
    {
        switch ((int)graphStructureEditor)
        {
            case 0: graphStructure = UtilGraph.GRID; break;
            case 1: graphStructure = UtilGraph.TREE; break;
        }

        switch ((int)useAlgorithmEditor)
        {
            case 0: useAlgorithm = UtilGraph.BFS; break;
            case 1: useAlgorithm = UtilGraph.DFS; break;
            case 2: useAlgorithm = UtilGraph.DIJKSTRA; break;
        }

        switch ((int)algorithmSpeedEditor)
        {
            case 0: algorithmSpeed = 3f; break;
            case 1: algorithmSpeed = 1f; break;
            case 2: algorithmSpeed = 0.5f; break;
            case 4: algorithmSpeed = 0f; break;
        }

        if (graphStructure.Equals(UtilGraph.GRID))
        {
            gridRows = (int)gridRowsEditor + 1;
            gridColumns = (int)gridColumnsEditor + 1;
            gridSpace = ((int)gridSpaceEditor + 1) * 4;

            if ((int)gridRowsEditor == 5)
                gridRows += 25;
            if ((int)gridColumnsEditor == 5)
                gridColumns += 25;
        }
        else if (graphStructure.Equals(UtilGraph.TREE))
        {
            treeDepth = (int)treeDepthEditor;
            nTree = ((int)nTreeEditor + 2) + (int)nTreeEditor;
            nodeSpaceX = ((int)nodeSpaceXEditor + 2) + (int)nodeSpaceXEditor * 2;
            nodeSpaceZ = ((int)nodeSpaceZEditor + 2) + (int)nodeSpaceZEditor * 2;
        }
    }

    public string Graphstructure
    {
        get { return graphStructure; }
    }

    public string UseAlgorithm
    {
        get { return useAlgorithm; }
    }

    public float AlgorithmSpeed
    {
        get { return algorithmSpeed; }
    }

    public int[] StartNode()
    {
        return new int[2] { x1, z1 };
    }

    public int[] EndNode()
    {
        return new int[2] { x2, z2 };
    }

    public bool VisitLeftFirst
    {
        get { return visitLeftFirst; }
    }

    public int[] GraphSetup()
    {
        switch (graphStructure)
        {
            case UtilGraph.GRID: return new int[3] { gridRows, gridColumns, gridSpace };
            case UtilGraph.TREE: return new int[4] { treeDepth, nTree, nodeSpaceX, nodeSpaceZ };
            default: Debug.LogError("Couldn't setup graph! Unknown graph structure: '" + graphStructure + "'."); return null;
        }
    }
}
