using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GraphSettings : TeachingSettings {

    [SerializeField]
    private GameObject graphAlgorithmObj;

    [SerializeField]
    private GraphMain graphMain;

    [Space(5)]
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

    [SerializeField]
    [Range(-100, 100)]
    private int minEdgeCost;

    [SerializeField]
    [Range(-100, 100)]
    private int maxEdgeCost;

    // Rolling random numbers: Random.Range(ROLL_START, ROLL_END) < 'INSERT_NAME'_CHANCE
    [Space(2)]
    [Header("RNG stuff (Editor only)")]
    [SerializeField]
    [Range(0, 10)]
    private int symmetricEdgeChance;

    [SerializeField]
    [Range(0, 10)]
    private int partialBuildTreeChildChance;

    [SerializeField]
    [Range(0, 10)]
    private int buildEdgeChance;

    // *** Grid graph ***
    [Space(5)]
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

    // *** Random graph ***
    [Header("Random graph settings")]
    [SerializeField]
    [Range(2, 5)]
    private int randomNodes;

    [SerializeField]
    [Range(1, 20)]
    private int randomEdges;

    [SerializeField]
    [Range(2, 5)]
    private int minRandomSpace;

    [Space(5)]
    [Header("Shortest path")]
    [SerializeField]
    private bool shortestPathOneToAll;

    [Space(5)]
    [Header("Optional stuff")]
    [SerializeField]
    private bool selectStartEndNodes;


    private string graphTask, graphStructure, edgeType, edgeBuildMode;

    [SerializeField]
    private TextMeshPro variable1Text, variable2Text;

    protected override void Start()
    {
        // Debugging editor (fast edit settings)
        if (useDebuggingSettings)
            GetSettingsFromEditor();
        else
        {
            base.Start();

            // Init settings
            Algorithm = Util.BFS;
            GraphTask = UtilGraph.TRAVERSE;
            ShortestPathOneToAll = shortestPathOneToAll;
            SelectStartEndNodes = selectStartEndNodes;

            // Graph
            GraphStructure = UtilGraph.GRID_GRAPH;
            EdgeType = UtilGraph.UNDIRECTED_EDGE;
            EdgeBuildMode = UtilGraph.FULL_EDGES_NO_CROSSING;

            switch (graphStructure)
            {
                case UtilGraph.GRID_GRAPH:
                    gridRows = 5;
                    gridColumns = 5;
                    gridSpace = 4;
                    break;

                case UtilGraph.TREE_GRAPH:
                    treeDepth = 2;
                    nTree = 2;
                    levelDepthLength = 4;
                    break;

                case UtilGraph.RANDOM_GRAPH:
                    randomNodes = 5;
                    randomEdges = 6;
                    minRandomSpace = 4;
                    break;
            }
        }

        // Update variable texts in the settings menu
        UpdateGraphSubVariables();
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
        SelectStartEndNodes = selectStartEndNodes;
        ShortestPathOneToAll = shortestPathOneToAll;
    }

    public override void UpdateInteraction(string sectionID, string itemID, string itemDescription)
    {
        // Fill information on the "display" on the settings menu about the button just clicked
        FillTooltips(itemDescription, false);

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

            case UtilGraph.GRAPH_STRUCTURE:
                GraphStructure = itemID;
                UpdateGraphSubVariables();
                break;

            case UtilGraph.GRAPH_TASK:
                if (itemID == UtilGraph.SHORTEST_PATH && algorithm == Util.DIJKSTRA || itemID == UtilGraph.TRAVERSE && (algorithm == Util.BFS || algorithm == Util.DFS))
                {
                    GraphTask = itemID;
                }
                else
                {
                    FillTooltips("Not implemented yet.", false);
                    if (itemID == UtilGraph.SHORTEST_PATH)
                        InitButtonState(sectionID, UtilGraph.TRAVERSE);
                    else
                        InitButtonState(sectionID, UtilGraph.SHORTEST_PATH);
                }

                break;

            case UtilGraph.EDGE_TYPE: EdgeType = itemID; break;
            case UtilGraph.EDGE_BUILD_MODE: EdgeBuildMode = itemID; break;
            case Util.OPTIONAL:
                switch (itemID)
                {
                    case UtilGraph.SELECT_NODE:
                        SelectStartEndNodes = Util.ConvertStringToBool(itemDescription);
                        if (selectStartEndNodes)
                            FillTooltips("Manually choose node(s).", false);
                        else
                            FillTooltips("Node(s) chosen in editor. Not recommended.", false);
                        break;

                    default: base.UpdateInteraction(sectionID, itemID, itemDescription); break;
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

            case UtilGraph.RANDOM_SUB:
                switch (itemID)
                {
                    case UtilGraph.RANDOM_NODES:
                        switch (itemDescription)
                        {
                            case Util.PLUS: ChangeNumberOfRandomNodes(true); break;
                            case Util.MINUS: ChangeNumberOfRandomNodes(false); break;
                        }
                        break;

                    case UtilGraph.RANDOM_EDGES:
                        switch (itemDescription)
                        {
                            case Util.PLUS: ChangeNumberofRandomEdges(true); break;
                            case Util.MINUS: ChangeNumberofRandomEdges(false); break;
                        }
                        break;
                }
                break;

            case UtilGraph.SHORTEST_PATH_OPTIONAL:
                bool shortestPathOneToAllActive = Util.ConvertStringToBool(itemDescription);
                shortestPathOneToAll = shortestPathOneToAllActive;

                if (shortestPathOneToAllActive)
                    FillTooltips("Demo only: displays the shortest path from the start node to all the other nodes.", false);
                else
                    FillTooltips("Displays the shortest path from the start- to end node.", false);
                break;

            default: base.UpdateInteraction(sectionID, itemID, itemDescription); break;
        }

    }

    protected override void InitButtons()
    {
        base.InitButtons();

        InitButtonState(Util.ALGORITHM, algorithm);
        InitButtonState(UtilGraph.GRAPH_TASK, graphTask);
        InitButtonState(UtilGraph.GRAPH_STRUCTURE, graphStructure);
        InitButtonState(UtilGraph.EDGE_TYPE, edgeType);
        InitButtonState(UtilGraph.EDGE_BUILD_MODE, UtilGraph.EDGE_BUILD_MODE, (int)edgeModeEditor); // TODO fix variable
        InitButtonState(Util.OPTIONAL, UtilGraph.SELECT_NODE, selectStartEndNodes);
        InitButtonState(UtilGraph.SHORTEST_PATH_OPTIONAL, UtilGraph.SHORTEST_PATH_ONE_TO_ALL, shortestPathOneToAll);
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

    public int MinEdgeCost
    {
        get { return minEdgeCost; }
    }

    public int MaxEdgeCost
    {
        get { return maxEdgeCost; }
    }

    private void UpdateGraphSubVariables()
    {
        switch (graphStructure)
        {
            case UtilGraph.GRID_GRAPH:
                variable1Text.text = gridRows.ToString();
                variable2Text.text = gridColumns.ToString();
                break;

            case UtilGraph.TREE_GRAPH:
                variable1Text.text = treeDepth.ToString();
                variable2Text.text = nTree.ToString();
                break;

            case UtilGraph.RANDOM_GRAPH:
                variable1Text.text = randomNodes.ToString();
                variable2Text.text = randomEdges.ToString();
                break;
        }
    }


    #region Grid sub
    public void ChangeGridRows(bool increment)
    {
        if (increment)
        {
            if (gridRows < UtilGraph.MAX_ROWS)
                gridRows++;
            else
            {
                FillTooltips("Max rows: " + UtilGraph.MAX_ROWS, false);
                return;
            }
        }
        else
        {
            if (gridRows > 1)
                gridRows--;
            else
            {
                FillTooltips("Min rows: 1", false);
                return;
            }

        }
        //FillTooltips("#Rows: " + gridRows, false);
        variable1Text.text = gridRows.ToString();
    }

    public void ChangeGridColumns(bool increment)
    {
        if (increment)
        {
            if (gridColumns < UtilGraph.MAX_COLUMNS)
                gridColumns++;
            else
            {
                FillTooltips("Max columns: " + UtilGraph.MAX_COLUMNS, false);
                return;
            }
        }
        else
        {
            if (gridColumns > 1)
                gridColumns--;
            else
            {
                FillTooltips("Min columns: 1", false);
                return;
            }

        }
        //FillTooltips("#Columns: " + gridColumns, false);
        variable2Text.text = gridColumns.ToString();
    }
    #endregion

    #region Tree sub
    public void ChangeTreeDepth(bool increment)
    {
        if (increment)
        {
            if (treeDepth < UtilGraph.MAX_TREE_DEPTH)
                treeDepth++;
            else
            {
                FillTooltips("Max tree depth: " + UtilGraph.MAX_TREE_DEPTH, false);
                return;
            }
        }
        else
        {
            if (treeDepth > 0)
                treeDepth--;
            else
            {
                FillTooltips("Min tree depth: 0", false);
                return;
            }

        }
        //FillTooltips("#Tree depth: " + treeDepth, false);
        variable1Text.text = treeDepth.ToString();
    }

    public void ChangeNTree(bool increment)
    {
        if (increment)
        {
            if (nTree < UtilGraph.MAX_N_TREE)
                nTree++;
            else
            {
                FillTooltips("Max n-tree: " + UtilGraph.MAX_N_TREE, false);
                return;
            }
        }
        else
        {
            if (nTree > 2)
                nTree--;
            else
            {
                FillTooltips("Min n-tree: 2", false);
                return;
            }

        }
        //FillTooltips("n-tree: " + treeDepth, false);
        variable2Text.text = nTree.ToString();
    }
    #endregion

    #region Random sub
    public void ChangeNumberOfRandomNodes(bool increment)
    {
        if (increment)
        {
            if (randomNodes < UtilGraph.MAX_RANDOM_NODES)
                randomNodes++;
            else
            {
                FillTooltips("Max #nodes: " + UtilGraph.MAX_RANDOM_NODES, false);
                return;
            }
        }
        else
        {
            if (randomNodes > 2)
                randomNodes--;
            else
            {
                FillTooltips("Min #nodes: 2", false);
                return;
            }

        }
        //FillTooltips("#Nodes: " + randomNodes, false);
        variable1Text.text = randomNodes.ToString();
    }

    public void ChangeNumberofRandomEdges(bool increment)
    {
        if (increment)
        {
            if (randomEdges < UtilGraph.MAX_RANDOM_EDGES)
                randomEdges++;
            else
            {
                FillTooltips("Max #edges: " + UtilGraph.MAX_RANDOM_EDGES, false);
                return;
            }
        }
        else
        {
            if (randomEdges > 1)
                randomEdges--;
            else
            {
                FillTooltips("Min #edges: 2", false);
                return;
            }

        }
        //FillTooltips("#Edges: " + randomEdges, false);
        variable2Text.text = randomEdges.ToString();
    }
    #endregion

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

    public int[] GraphSetup()
    {
        switch (graphStructure)
        {
            case UtilGraph.GRID_GRAPH: return new int[3] { gridRows, gridColumns, gridSpace };
            case UtilGraph.TREE_GRAPH: return new int[3] { treeDepth, nTree, levelDepthLength };
            case UtilGraph.RANDOM_GRAPH: return new int[3] { randomNodes, randomEdges, minRandomSpace }; //{ 5, 6, 2 };
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
