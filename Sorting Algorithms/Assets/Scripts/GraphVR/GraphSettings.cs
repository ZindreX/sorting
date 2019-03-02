using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphSettings : MonoBehaviour, ISettings {

    public GameObject nodePrefab, undirectedEdgePrefab, directedEdgePrefab, symmetricDirectedEdgePrefab;

    [SerializeField]
    private GameObject graphAlgorithmObj;

    [Header("Overall settings")]
    [SerializeField]
    private TeachingModeEditor teachingModeEditor;
    private enum TeachingModeEditor { Demo, UserTest }

    [SerializeField]
    private GraphStructureEditor graphStructureEditor;
    private enum GraphStructureEditor { Grid, Tree, Random }

    [SerializeField]
    private UseAlgorithmEditor useAlgorithmEditor;
    private enum UseAlgorithmEditor { BFS, DFS, DFSRecursive, Dijkstra }

    [SerializeField]
    private AlgorithmSpeedEditor algorithmSpeedEditor;
    private enum AlgorithmSpeedEditor { Slow, Normal, Fast, Test }

    [Space(2)]
    [Header("Edge settings")]
    [SerializeField]
    private EdgeTypeEditor edgeTypeEditor;
    private enum EdgeTypeEditor { Undirected, Directed }

    [SerializeField]
    private EdgeModeEditor edgeModeEditor;
    private enum EdgeModeEditor { Full, FullNoCrossing, Partial, PartialNoCrossing }


    // *** Grid graph ***
    [Space(2)]
    [Header("Grid graph settings")]
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
    [Space(2)]
    [Header("Tree graph settings")]
    [SerializeField]
    private TreeDepth treeDepthEditor;
    private enum TreeDepth { Zero, One, Two, Three, Four, Five }

    [SerializeField]
    private NTree nTreeEditor;
    private enum NTree { Binary, Ternary }

    [SerializeField]
    private LevelDepthLengthEditor levelDepthLengthEditor;
    private enum LevelDepthLengthEditor { Two, Four }

    [SerializeField]
    private bool visitLeftFirst;


    // *** Start-/End nodes ***
    [Space(2)]
    [Header("Start node")]
    [SerializeField]
    private int x1;
    [SerializeField]
    private int z1;

    [Header("End node (Shortest path)")]
    [SerializeField]
    private int x2;
    [SerializeField]
    private int z2;

    [SerializeField]
    private bool shortestPathOneToAll;

    // Rolling random numbers: Random.Range(ROLL_START, ROLL_END) < 'INSERT_NAME'_CHANCE
    [Header("RNG stuff")]
    [SerializeField]
    private int symmetricEdgeChance = 10;
    [SerializeField]
    private int partialBuildTreeChildChance = 4;
    [SerializeField]
    private int buildEdgeChance = 1;

    private float algorithmSpeed;
    private string teachingMode, graphStructure, edgeType, edgeMode, useAlgorithm;
    private int gridRows, gridColumns, gridSpace;
    private int treeDepth, nTree, levelDepthLength;

    public void PrepareSettings()
    {
        switch ((int)teachingModeEditor)
        {
            case 0: teachingMode = Util.DEMO; break;
            case 1: teachingMode = Util.USER_TEST; break;
        }

        switch ((int)graphStructureEditor)
        {
            case 0: graphStructure = UtilGraph.GRID_GRAPH; break;
            case 1: graphStructure = UtilGraph.TREE_GRAPH; break;
            case 2: graphStructure = UtilGraph.RANDOM_GRAPH; break;
        }

        switch ((int)edgeTypeEditor)
        {
            case 0: edgeType = UtilGraph.UNDIRECTED_EDGE; break;
            case 1: edgeType = UtilGraph.DIRECED_EDGE; break;
        }

        switch ((int)edgeModeEditor)
        {
            case 0: edgeMode = UtilGraph.FULL_EDGES; break;
            case 1: edgeMode = UtilGraph.FULL_EDGES_NO_CROSSING; break;
            case 2: edgeMode = UtilGraph.PARTIAL_EDGES; break;
            case 3: edgeMode = UtilGraph.PARTIAL_EDGES_NO_CROSSING; break;
        }

        switch ((int)useAlgorithmEditor)
        {
            case 0: useAlgorithm = Util.BFS; break;
            case 1: useAlgorithm = Util.DFS; break;
            case 2: useAlgorithm = Util.DFS_RECURSIVE; break;
            case 3: useAlgorithm = Util.DIJKSTRA; break;
        }

        switch ((int)algorithmSpeedEditor)
        {
            case 0: algorithmSpeed = 3f; break;
            case 1: algorithmSpeed = 1f; break;
            case 2: algorithmSpeed = 0.25f; break;
            case 4: algorithmSpeed = 0f; break;
        }

        if (graphStructure.Equals(UtilGraph.GRID_GRAPH))
        {
            gridRows = (int)gridRowsEditor + 1;
            gridColumns = (int)gridColumnsEditor + 1;
            gridSpace = ((int)gridSpaceEditor + 1) * 4;

            if ((int)gridRowsEditor == 5)
                gridRows += 25;
            if ((int)gridColumnsEditor == 5)
                gridColumns += 25;
        }
        else if (graphStructure.Equals(UtilGraph.TREE_GRAPH))
        {
            treeDepth = (int)treeDepthEditor;
            nTree = ((int)nTreeEditor + 2);
            levelDepthLength = ((int)levelDepthLengthEditor + 2) + (int)levelDepthLengthEditor * 2;
        }
    }

    public string TeachingMode
    {
        get { return teachingMode; }
    }

    public string Graphstructure
    {
        get { return graphStructure; }
    }

    public string EdgeType
    {
        get { return edgeType; }
    }

    public string EdgeMode
    {
        get { return edgeMode; }
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

    public bool ShortestPathOneToAll
    {
        get { return shortestPathOneToAll; }
    }

    public bool VisitLeftFirst
    {
        get { return visitLeftFirst; }
    }

    public bool IsDemo()
    {
        return teachingMode == Util.DEMO;
    }

    public bool IsUserTest()
    {
        return teachingMode == Util.USER_TEST;
    }

    public int[] GraphSetup()
    {
        switch (graphStructure)
        {
            case UtilGraph.GRID_GRAPH: return new int[3] { gridRows, gridColumns, gridSpace };
            case UtilGraph.TREE_GRAPH: return new int[3] { treeDepth, nTree, levelDepthLength };
            case UtilGraph.RANDOM_GRAPH: return new int[3] { 5, 6, 2 };
            default: Debug.LogError("Couldn't setup graph! Unknown graph structure: '" + graphStructure + "'."); return null;
        }
    }

    public Dictionary<string, int> RNGDict()
    {
        Dictionary<string, int> rngDict = new Dictionary<string, int>();
        rngDict.Add(UtilGraph.SYMMETRIC_EDGE_CHANCE, symmetricEdgeChance);
        rngDict.Add(UtilGraph.PARTIAL_BUILD_TREE_CHILD_CHANCE, partialBuildTreeChildChance);
        rngDict.Add(UtilGraph.BUILD_EDGE_CHANCE, buildEdgeChance);
        return rngDict;
    }

}
