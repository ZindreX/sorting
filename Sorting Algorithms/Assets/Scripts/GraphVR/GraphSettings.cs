using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphSettings : SettingsBase {

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

    [SerializeField]
    private GraphTaskEditor graphTaskEditor;
    private enum GraphTaskEditor { Traverse, ShortestPath }

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
    [Range(1, 5)]
    private int gridRows;

    [SerializeField]
    [Range(1, 5)]
    private int gridColumns;

    [SerializeField]
    [Range(1, 2)]
    private int gridSpace;


    // *** Tree graph ***
    [Space(2)]
    [Header("Tree graph settings")]
    [SerializeField]
    [Range(0, 5)]
    private int treeDepth;

    [SerializeField]
    [Range(2, 3)]
    private int nTree;

    [SerializeField]
    [Range(1, 2)]
    private int levelDepthLength;

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
    private bool shortestPathOneToAll, selectStartEndNodes;

    // Rolling random numbers: Random.Range(ROLL_START, ROLL_END) < 'INSERT_NAME'_CHANCE
    [Header("RNG stuff")]
    [SerializeField]
    [Range(0, 10)]
    private int symmetricEdgeChance;

    [SerializeField]
    [Range(0, 10)]
    private int partialBuildTreeChildChance;

    [SerializeField]
    [Range(0, 10)]
    private int buildEdgeChance;

    private string graphTask, graphStructure, edgeType, edgeBuildMode;

    private void Start()
    {
        // Debugging editor (fast edit settings)
        if (useDebuggingSettings)
            GetSettingsFromEditor();
        else
        {
            // Init settings
            Algorithm = Util.BFS;
            TeachingMode = Util.DEMO;
            GraphTask = UtilGraph.TRAVERSE;
            AlgorithmSpeedLevel = 0;
            Difficulty = 0;
            ShortestPathOneToAll = true;
            VisitLeftFirst = true;
            SelectStartEndNodes = false;

            // Graph
            GraphStructure = UtilGraph.GRID_GRAPH;
            EdgeType = UtilGraph.UNDIRECTED_EDGE;
            EdgeBuildMode = UtilGraph.FULL_EDGES_NO_CROSSING;

            if (graphStructure == UtilGraph.GRID_GRAPH)
            {
                gridRows = 5;
                gridColumns = 5;
                gridSpace = 4;
            }
            else if (graphStructure == UtilGraph.TREE_GRAPH)
            {
                treeDepth = 2;
                nTree = 2;
                levelDepthLength = 4;
            }
        }
        tooltips.text = "";

        InitButtons();
    }

    protected override void GetSettingsFromEditor()
    {
        base.GetSettingsFromEditor();

        switch ((int)algorithmEditor)
        {
            case 0: Algorithm = Util.BFS; break;
            case 1: Algorithm = Util.DFS; break;
            case 2: Algorithm = Util.DFS_RECURSIVE; break;
            case 3: Algorithm = Util.DIJKSTRA; break;
        }

        switch ((int)graphTaskEditor)
        {
            case 0: GraphTask = UtilGraph.TRAVERSE; break;
            case 1: GraphTask = UtilGraph.SHORTEST_PATH; break;
        }

        // Graph
        switch ((int)graphStructureEditor)
        {
            case 0: GraphStructure = UtilGraph.GRID_GRAPH; break;
            case 1: GraphStructure = UtilGraph.TREE_GRAPH; break;
            case 2: GraphStructure = UtilGraph.RANDOM_GRAPH; break;
        }

        switch ((int)edgeTypeEditor)
        {
            case 0: EdgeType = UtilGraph.UNDIRECTED_EDGE; break;
            case 1: EdgeType = UtilGraph.DIRECED_EDGE; break;
        }

        switch ((int)edgeModeEditor)
        {
            case 0: EdgeBuildMode = UtilGraph.FULL_EDGES; break;
            case 1: EdgeBuildMode = UtilGraph.FULL_EDGES_NO_CROSSING; break;
            case 2: EdgeBuildMode = UtilGraph.PARTIAL_EDGES; break;
            case 3: EdgeBuildMode = UtilGraph.PARTIAL_EDGES_NO_CROSSING; break;
        }

        // Grid settings
        if (gridRows == 6)
            gridRows += 25;

        if (gridColumns == 6)
            gridColumns += 25;

        gridSpace *= 2;

        // Tree settings
        levelDepthLength *= 2;

        // Extra
        ShortestPathOneToAll = shortestPathOneToAll;
        VisitLeftFirst = visitLeftFirst;
    }

    public override void UpdateInteraction(string sectionID, string itemID, string itemDescription)
    {
        // Fill information on the "display" on the settings menu about the button just clicked
        FillTooltips(itemDescription);

        switch (sectionID)
        {
            case Util.ALGORITHM:
                Algorithm = itemID;

                // Algorithm changes graph task (not fully covered***)
                if (itemID == Util.DIJKSTRA)
                {
                    GraphTask = UtilGraph.SHORTEST_PATH;
                    InitButtonState(UtilGraph.GRAPH_TASK, graphTask);
                }
                else
                {
                    GraphTask = UtilGraph.TRAVERSE;
                    InitButtonState(UtilGraph.GRAPH_TASK, graphTask);
                }
                break;

            case UtilGraph.GRAPH_STRUCTURE: GraphStructure = itemID; break;
            case UtilGraph.GRAPH_TASK:
                if (itemID == UtilGraph.SHORTEST_PATH && algorithm == Util.DIJKSTRA || itemID == UtilGraph.TRAVERSE && (algorithm == Util.BFS || algorithm == Util.DFS))
                {
                    GraphTask = itemID;
                }
                else
                {
                    FillTooltips("Not implemented yet.");
                    if (itemID == UtilGraph.SHORTEST_PATH)
                        InitButtonState(sectionID, UtilGraph.TRAVERSE);
                    else
                        InitButtonState(sectionID, UtilGraph.SHORTEST_PATH);
                }

                break;

            case UtilGraph.EDGE_TYPE: EdgeType = itemID; break;
            case UtilGraph.EDGE_BUILD_MODE: EdgeBuildMode = itemID; break;
            case UtilGraph.SHORTEST_PATH_ONE_TO_ALL: ShortestPathOneToAll = Util.ConvertStringToBool(itemDescription); break;
            case UtilGraph.VISIT_LEFT_FIRST: VisitLeftFirst = Util.ConvertStringToBool(itemDescription); break;
            case Util.OPTIONAL:
                switch (itemID)
                {
                    case UtilGraph.SELECT_NODE: SelectStartEndNodes = Util.ConvertStringToBool(itemDescription); break;
                    default: Debug.LogError("No optional choice case for '" + itemID + "'."); break;
                }
                break;

            case UtilGraph.GRID_SUB:
                switch (itemID)
                {
                    case UtilGraph.ROWS:
                        switch (itemDescription)
                        {
                            case Util.PLUS: ChangeGridRows(true); break;
                            case Util.MINUS: ChangeGridRows(false); break;
                        }
                        break;

                    case UtilGraph.COLUMNS:
                        switch (itemDescription)
                        {
                            case Util.PLUS: ChangeGridColumns(true); break;
                            case Util.MINUS: ChangeGridColumns(false); break;
                        }
                        break;
                }
                break;

            case UtilGraph.TREE_SUB:
                switch (itemID)
                {
                    case UtilGraph.TREE_DEPTH:
                        switch (itemDescription)
                        {
                            case Util.PLUS: ChangeTreeDepth(true); break;
                            case Util.MINUS: ChangeTreeDepth(false); break;
                        }
                        break;

                    case UtilGraph.N_TREE:
                        switch (itemDescription)
                        {
                            case Util.PLUS: ChangeNTree(true); break;
                            case Util.MINUS: ChangeNTree(false); break;
                        }
                        break;
                }
                break;

            default: base.UpdateInteraction(sectionID, itemID, itemDescription); break;
        }

    }

    protected override void InitButtons()
    {
        InitButtonState(Util.ALGORITHM, algorithm);
        InitButtonState(Util.TEACHING_MODE, teachingMode);
        InitButtonState(Util.DIFFICULTY, Util.DIFFICULTY, difficulty);
        InitButtonState(Util.DEMO_SPEED, Util.DEMO_SPEED, algSpeed);
        InitButtonState(UtilGraph.GRAPH_TASK, graphTask);
        InitButtonState(UtilGraph.GRAPH_STRUCTURE, graphStructure);
        InitButtonState(UtilGraph.EDGE_TYPE, edgeType);
        InitButtonState(UtilGraph.EDGE_BUILD_MODE, edgeBuildMode);
        InitButtonState(UtilGraph.SHORTEST_PATH_SUB_SECTION, UtilGraph.SHORTEST_PATH_ONE_TO_ALL, shortestPathOneToAll);
        InitButtonState(UtilGraph.TRAVERSE_SUB_SECTION, UtilGraph.VISIT_LEFT_FIRST, visitLeftFirst);
        InitButtonState(Util.OPTIONAL, UtilGraph.SELECT_NODE, selectStartEndNodes);
    }

    protected override MainManager MainManager
    {
        get { return graphMain; }
        set { graphMain = (GraphMain)value; }
    }

    public string GraphTask
    {
        get { return graphTask; }
        set { graphTask = value; }
    }

    public string GraphStructure
    {
        get { return graphStructure; }
        set { graphStructure = value; }
    }

    public string EdgeType
    {
        get { return edgeType; }
        set { edgeType = value; }
    }

    public string EdgeBuildMode
    {
        get { return edgeBuildMode; }
        set { edgeBuildMode = value; }
    }

    public void ChangeGridRows(bool increment)
    {
        if (increment)
        {
            if (gridRows < UtilGraph.MAX_ROWS)
                gridRows++;
            else
            {
                FillTooltips("Max rows: " + UtilGraph.MAX_ROWS);
                return;
            }
        }
        else
        {
            if (gridRows > 1)
                gridRows--;
            else
            {
                FillTooltips("Min rows: 1");
                return;
            }

        }
        FillTooltips("#Rows: " + gridRows);
    }

    public void ChangeGridColumns(bool increment)
    {
        if (increment)
        {
            if (gridColumns < UtilGraph.MAX_COLUMNS)
                gridColumns++;
            else
            {
                FillTooltips("Max columns: " + UtilGraph.MAX_COLUMNS);
                return;
            }
        }
        else
        {
            if (gridColumns > 1)
                gridColumns--;
            else
            {
                FillTooltips("Min columns: 1");
                return;
            }

        }
        FillTooltips("#Columns: " + gridColumns);
    }

    public void ChangeTreeDepth(bool increment)
    {
        if (increment)
        {
            if (treeDepth < UtilGraph.MAX_TREE_DEPTH)
                treeDepth++;
            else
            {
                FillTooltips("Max tree depth: " + UtilGraph.MAX_TREE_DEPTH);
                return;
            }
        }
        else
        {
            if (treeDepth > 0)
                treeDepth--;
            else
            {
                FillTooltips("Min tree depth: 0");
                return;
            }

        }
        FillTooltips("#Tree depth: " + treeDepth);
    }

    public void ChangeNTree(bool increment)
    {
        if (increment)
        {
            if (nTree < UtilGraph.MAX_N_TREE)
                nTree++;
            else
            {
                FillTooltips("Max n-tree: " + UtilGraph.MAX_N_TREE);
                return;
            }
        }
        else
        {
            if (nTree > 2)
                nTree--;
            else
            {
                FillTooltips("Min n-tree: 2");
                return;
            }

        }
        FillTooltips("n-tree: " + treeDepth);
    }

    public int[] StartNode()
    {
        return new int[2] { x1, z1 };
    }

    public int[] EndNode()
    {
        return new int[2] { x2, z2 };
    }


    public bool SelectStartEndNodes
    {
        get { return selectStartEndNodes; }
        set { selectStartEndNodes = value; }
    }

    public void SetSelectNodes()
    {
        selectStartEndNodes = !selectStartEndNodes;
    }

    public bool ShortestPathOneToAll
    {
        get { return shortestPathOneToAll; }
        set { shortestPathOneToAll = value; }
    }
    public bool VisitLeftFirst
    {
        get { return visitLeftFirst; }
        set { visitLeftFirst = value; }
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
