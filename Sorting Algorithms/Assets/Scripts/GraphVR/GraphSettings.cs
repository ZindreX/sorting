using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphSettings : SettingsBase {

    [Space(5)]
    [Header("Graph prefabs")]
    public GameObject nodePrefab;
    public GameObject undirectedEdgePrefab;
    public GameObject directedEdgePrefab;
    public GameObject symmetricDirectedEdgePrefab;

    [SerializeField]
    private GameObject graphAlgorithmObj;

    [SerializeField]
    private GraphMain graphMain;

    [Space(2)]
    [Header("Graph settings")]
    [SerializeField]
    private GraphStructureEditor graphStructureEditor;
    private enum GraphStructureEditor { Grid, Tree, Random }

    [SerializeField]
    private AlgorithmEditor algorithmEditor;
    private enum AlgorithmEditor { BFS, DFS, DFSRecursive, Dijkstra }

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

    [SerializeField]
    private bool initGrid;
    private string graphStructure, edgeType, edgeMode;
    private int gridRows, gridColumns, gridSpace;
    private int treeDepth, nTree, levelDepthLength;

    private void Start()
    {
        // Debugging editor (fast edit settings)
        if (useDebuggingSettings)
            PrepareSettings();
        else
        {
            // Init settings
            Algorithm = Util.BFS;
            TeachingMode = Util.DEMO;
            AlgorithmSpeed = 1;
            Difficulty = 1;

            // Graph
            if (initGrid)
            {
                Graphstructure = UtilGraph.GRID_GRAPH;
                EdgeType = UtilGraph.UNDIRECTED_EDGE;
                EdgeMode = UtilGraph.FULL_EDGES_NO_CROSSING;
                gridRows = 5;
                gridColumns = 5;
                gridSpace = 4;
            }
            else
            {
                treeDepth = 2;
                nTree = 2;
                levelDepthLength = 4;
            }
        }
        tooltips.text = "";
    }

    public override void PrepareSettings()
    {
        switch ((int)teachingModeEditor)
        {
            case 0: TeachingMode = Util.DEMO; break;
            case 1: TeachingMode = Util.USER_TEST; break;
        }

        switch ((int)algorithmEditor)
        {
            case 0: Algorithm = Util.BFS; break;
            case 1: Algorithm = Util.DFS; break;
            case 2: Algorithm = Util.DFS_RECURSIVE; break;
            case 3: Algorithm = Util.DIJKSTRA; break;
        }

        switch ((int)algorithmSpeedEditor)
        {
            case 0: AlgorithmSpeed = 3f; break;
            case 1: AlgorithmSpeed = 1f; break;
            case 2: AlgorithmSpeed = 0.25f; break;
            case 4: AlgorithmSpeed = 0f; break;
        }

        // Graph
        switch ((int)graphStructureEditor)
        {
            case 0: Graphstructure = UtilGraph.GRID_GRAPH; break;
            case 1: Graphstructure = UtilGraph.TREE_GRAPH; break;
            case 2: Graphstructure = UtilGraph.RANDOM_GRAPH; break;
        }

        switch ((int)edgeTypeEditor)
        {
            case 0: EdgeType = UtilGraph.UNDIRECTED_EDGE; break;
            case 1: EdgeType = UtilGraph.DIRECED_EDGE; break;
        }

        switch ((int)edgeModeEditor)
        {
            case 0: EdgeMode = UtilGraph.FULL_EDGES; break;
            case 1: EdgeMode = UtilGraph.FULL_EDGES_NO_CROSSING; break;
            case 2: EdgeMode = UtilGraph.PARTIAL_EDGES; break;
            case 3: EdgeMode = UtilGraph.PARTIAL_EDGES_NO_CROSSING; break;
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

        Debug.Log("Teachingmode: " + teachingMode + ", algorithm: " + algorithm + ", graph: " + graphStructure);
    }

    protected override MainManager MainManager
    {
        get { return graphMain; }
        set { graphMain = (GraphMain)value; }
    }

    public string Graphstructure
    {
        get { return graphStructure; }
        set { graphStructure = value; FillTooltips("Graph structure: " + value); SetActiveButton(""); }
    }

    public string EdgeType
    {
        get { return edgeType; }
        set { edgeType = value; FillTooltips("Edge type: " + value); SetActiveButton(""); }
    }

    public string EdgeMode
    {
        get { return edgeMode; }
        set { edgeMode = value; FillTooltips("Edge mode: " + value); SetActiveButton(""); }
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
        set { shortestPathOneToAll = value; FillTooltips("Shortest path one to all: " + value); SetActiveButton(""); }
    }

    public bool VisitLeftFirst
    {
        get { return visitLeftFirst; }
        set { visitLeftFirst = value; FillTooltips("Visit left first: " + value); SetActiveButton(""); }
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
