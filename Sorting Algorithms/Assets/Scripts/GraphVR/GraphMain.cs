using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GraphManager))]
[RequireComponent(typeof(UserTestManager))]
public class GraphMain : MainManager {

    [Header("Graph prefabs")]
    public GameObject nodePrefab;
    public GameObject undirectedEdgePrefab;
    public GameObject directedEdgePrefab;
    public GameObject symmetricDirectedEdgePrefab;

    [Space(5)]
    [Header("Environment objects")]
    [SerializeField]
    private GameObject settingsObj;
    [SerializeField]
    private GameObject startPillar;

    [Space(5)]
    [Header("Other objects")]
    [SerializeField]
    private GraphSettings graphSettings;

    [SerializeField]
    private GameObject graphAlgorithmObj;


    private GraphAlgorithm graphAlgorithm;
    private GraphManager graphManager;
    private UserTestManager userTestManager;
    private PositionManager posManager;


    [Space(5)]
    [Header("Support")]

    [SerializeField]
    private PseudoCodeViewer pseudoCodeViewer;

    [SerializeField]
    private ListVisual listVisual;

    private bool backtracking = false, userTestGoalActive = false;



    protected virtual void Awake()
    {
        // >>> Basic components
        userTestManager = GetComponent(typeof(UserTestManager)) as UserTestManager;
        posManager = FindObjectOfType<PositionManager>();
    }

    public GraphSettings GraphSettings
    {
        get { return graphSettings; }
    }

    public GraphManager GraphManager
    {
        get { return graphManager; }
    }

    protected override void DemoUpdate()
    {

    }

    protected override void StepByStepUpdate()
    {

    }

    protected override void UserTestUpdate()
    {
        // If a node is set to be visited or traversed, wait until it has been achieved
        if (userTestGoalActive && !posManager.PlayerWithinGoalPosition)
            return;
        else
            userTestGoalActive = false;

        // First check if user test setup is complete
        if (userTestManager.HasInstructions() && !beginnerWait)
        {
            if (userTestManager.ReadyForNext == userTestManager.UserActionToProceed)
            {
                // Reset counter
                userTestManager.ReadyForNext = 0;

                // Checking if all sorting elements are sorted
                if (!userTestManager.HasInstructions()) // && elementManager.AllSorted())
                {
                    graphAlgorithm.IsTaskCompleted = true;
                    // ?
                }
                else
                {
                    // Still some elements not sorted, so go on to next round
                    bool hasInstruction = userTestManager.IncrementToNextInstruction();

                    // Hot fix - solve in some other way?
                    if (hasInstruction)
                        userTestManager.ReadyForNext += PrepareNextInstruction(userTestManager.GetInstruction());
                    //else if (elementManager.AllSorted())
                    //    StartCoroutine(FinishUserTest());

                }
            }
            //displayUnitManager.BlackBoard.ChangeText(displayUnitManager.BlackBoard.TextIndex, userTestManager.FillInBlackboard());
        }
    }

    protected override void TaskCompletedFinishOff()
    {
        if (!backtracking && graphAlgorithm is IShortestPath)
        {
            Debug.Log("Starting backtracking");
            if (graphAlgorithm.ShortestPathOneToAll)
                StartCoroutine(graphManager.BacktrackShortestPathsAll(graphAlgorithm.DemoStepDuration));
            else
                StartCoroutine(graphManager.BacktrackShortestPath(graphManager.EndNode, graphAlgorithm.DemoStepDuration));
            backtracking = true;
        }
    }


    public override TeachingAlgorithm GetTeachingAlgorithm()
    {
        return graphAlgorithm;
    }

    public override SettingsBase Settings
    {
        get { return graphSettings; }
    }

    // Keeps only one graph structure active: Activate/deactivate components (Grid / Tree / Random) - working?
    private GraphManager ActivateDeactivateGraphComponents(string graphStructure)
    {
        switch (graphStructure)
        {
            case UtilGraph.GRID_GRAPH:
                GetComponent<GridManager>().enabled = true;
                GetComponent<TreeManager>().enabled = false;
                GetComponent<RandomGraphManager>().enabled = false;
                return GetComponent<GridManager>();

            case UtilGraph.TREE_GRAPH:
                GetComponent<TreeManager>().enabled = true;
                GetComponent<GridManager>().enabled = false;
                GetComponent<RandomGraphManager>().enabled = false;
                return GetComponent<TreeManager>();

            case UtilGraph.RANDOM_GRAPH:
                GetComponent<RandomGraphManager>().enabled = true;
                GetComponent<TreeManager>().enabled = false;
                GetComponent<GridManager>().enabled = false;
                return GetComponent<RandomGraphManager>();

            default: Debug.Log("Graph structure '" + graphStructure + "' not found."); break;
        }
        return null;
    }

    protected override TeachingAlgorithm GrabAlgorithmFromObj()
    {
        switch (algorithmName)
        {
            case Util.BFS:
                //graphAlgorithmObj.GetComponent<BFS>().enabled = true;
                //graphAlgorithmObj.GetComponent<DFS>().enabled = false;
                //graphAlgorithmObj.GetComponent<Dijkstra>().enabled = false;
                return graphAlgorithmObj.GetComponent<BFS>();

            case Util.DFS:
            case Util.DFS_RECURSIVE:
                //graphAlgorithmObj.GetComponent<DFS>().enabled = true;
                //graphAlgorithmObj.GetComponent<BFS>().enabled = false;
                //graphAlgorithmObj.GetComponent<Dijkstra>().enabled = false;
                return graphAlgorithmObj.GetComponent<DFS>();

            case Util.DIJKSTRA:
                //graphAlgorithmObj.GetComponent<Dijkstra>().enabled = true;
                //graphAlgorithmObj.GetComponent<BFS>().enabled = false;
                //graphAlgorithmObj.GetComponent<DFS>().enabled = false;
                return graphAlgorithmObj.GetComponent<Dijkstra>();

            default: return null;
        }
    }

    // Test
    private bool playerChooseStartNode = false;
    public bool PlayerChooseStartNode
    {
        get { return playerChooseStartNode; }
        set { playerChooseStartNode = value; }
    }

    private bool playerChooseEndNode = false;
    public bool PlayerChooseEndNode
    {
        get { return playerChooseEndNode; }
        set { playerChooseEndNode = value; }
    }

    public override void InstantiateSetup()
    {
        // >>> Grab variable data from settings
        algorithmName = graphSettings.Algorithm;
        int difficulty = graphSettings.Difficulty;
        string graphTask = graphSettings.GraphTask; //..
        float algorithmSpeed = graphSettings.AlgorithmSpeed;
        string graphStructure = graphSettings.GraphStructure;
        string edgeType = graphSettings.EdgeType;
        string edgeBuildMode = graphSettings.EdgeBuildMode;
        int[] graphSetup = graphSettings.GraphSetup();

        // Extra
        bool shortestPathOneToAll = graphSettings.ShortestPathOneToAll;
        bool visitLeftFirst = graphSettings.VisitLeftFirst;

        // >>> Init Algorithm
        graphAlgorithm = (GraphAlgorithm)GrabAlgorithmFromObj();
        graphAlgorithm.InitGraphAlgorithm(this, graphStructure, algorithmSpeed);

        // Extra settings
        graphAlgorithm.ShortestPathOneToAll = shortestPathOneToAll;
        graphAlgorithm.VisitLeftFirst = visitLeftFirst;

        // >>> Init position manager
        bool isShortestPath = graphTask == UtilGraph.SHORTEST_PATH;
        posManager.InitPositionManager(isShortestPath);


        // >>> Extra learning material
        // Init list visual
        string listType = graphAlgorithm.GetListType();
        listVisual.InitListVisual(listType, algorithmSpeed);

        // Pseudocode
        graphAlgorithm.PseudoCodeViewer = pseudoCodeViewer;
        pseudoCodeViewer.PseudoCodeSetup();


        // >>> Init Graph manager
        graphManager = ActivateDeactivateGraphComponents(graphSettings.GraphStructure);
        graphManager.InitGraphManager(algorithmName, graphStructure, edgeType, isShortestPath, graphSettings.RNGDict());

        // Graph setup (rows/colums - tree depth/nTree - random?)
        graphManager.InitGraph(graphSetup);

        // Create graph based on init variables
        graphManager.CreateGraph(edgeBuildMode);

        // Prepare difficulty level related stuff for user test (copied from sort)
        if (graphSettings.TeachingMode == Util.USER_TEST)
        {
            if (difficulty <= Util.INTERMEDIATE)
            {
                //displayUnitManager.PseudoCodeViewer.PseudoCodeSetup();
            }
            else if (difficulty == Util.ADVANCED)
            {
                //isplayUnitManager.PseudoCodeViewer.PseudoCodeSetup();
                // Ideas for left/center blackboard?
            }
            else
            {
                // Ideas for left/center? (Examination)
            }
        }
        else
        {
            //displayUnitManager.PseudoCodeViewer.PseudoCodeSetup();
        }



        // Hide menu
        StartCoroutine(ActivateTaskObjects(true));

        if (graphSettings.SelectStartEndNodes)
        {
            Debug.Log("Selecting nodes");
            playerChooseStartNode = true;
            if (graphAlgorithm is IShortestPath)
                playerChooseEndNode = true;
        }
        else
            StartCoroutine(SetAutomaticallyImportantNodes());
    }

    public IEnumerator SetAutomaticallyImportantNodes()
    {
        yield return loading;

        // Set starting nodes
        int[] startCell = graphSettings.StartNode();
        int[] endCell = graphSettings.EndNode();
        graphManager.StartNode = graphManager.GetNode(startCell[0], startCell[1]);
        graphManager.EndNode = graphManager.GetNode(endCell[0], endCell[1]);
    }

    protected override IEnumerator ActivateTaskObjects(bool active)
    {
        graphSettings.FillTooltips("Loading setup...");
        yield return loading;

        // Settings menu
        graphSettings.SetSettingsActive(!active);

        startPillar.SetActive(active);

        yield return loading;
    }


    public override void DestroyAndReset()
    {
        algorithmStarted = false;

        // Stop ongoing actions
        UserStoppedAlgorithm = true;

        // Delete list visual
        listVisual.DestroyAndReset();

        // Delete graph
        graphManager.DeleteGraph();

        // Reset algorithm
        graphAlgorithm.ResetSetup();

        StartCoroutine(ActivateTaskObjects(false));
    }


    public override void PerformAlgorithmDemo()
    {
        Debug.Log("Performing " + algorithmName + " " + graphSettings.GraphTask + " demo.");
        switch (graphSettings.GraphTask)
        {
            case UtilGraph.TRAVERSE: StartCoroutine(((ITraverse)graphAlgorithm).TraverseDemo(graphManager.StartNode)); break;
            case UtilGraph.SHORTEST_PATH: StartCoroutine(((IShortestPath)graphAlgorithm).ShortestPathDemo(graphManager.StartNode, graphManager.EndNode)); break;
        }

        //switch (graphSettings.Algorithm)
        //{
        //    case Util.BFS: StartCoroutine(((BFS)graphAlgorithm).Demo(graphManager.StartNode)); break;

        //    case Util.DFS:
        //        StartCoroutine(((DFS)graphAlgorithm).Demo(graphManager.StartNode));
        //        break;

        //    case Util.DFS_RECURSIVE:
        //        //algorithm.ListVisual.AddListObject(GetNode(startNode[0], startNode[1]).NodeAlphaID);
        //        StartCoroutine(((DFS)graphAlgorithm).DemoRecursive(graphManager.StartNode));
        //        break;

        //    case Util.DIJKSTRA:
        //        StartCoroutine(((Dijkstra)graphAlgorithm).Demo(graphManager.StartNode, );
        //        //StartCoroutine(((Dijkstra)graphAlgorithm).DemoNoPseudocode(startNode, endNode));
        //        break;

        //    default: Debug.LogError("'" + algorithmName + "' unknown."); break;
        //}
    }

    public override void PerformAlgorithmStepByStep()
    {
        throw new System.NotImplementedException();
    }

    public override void PerformAlgorithmUserTest()
    {
        Debug.Log("Starting " + algorithmName + " " + graphSettings.GraphTask + " user test.");

        // Getting instructions for this sample of sorting elements
        Dictionary<int, InstructionBase> instructions = null;

        switch (graphSettings.GraphTask)
        {
            case UtilGraph.TRAVERSE: instructions = ((ITraverse)graphAlgorithm).TraverseUserTestInstructions(graphManager.StartNode); break;
            case UtilGraph.SHORTEST_PATH: instructions = ((IShortestPath)graphAlgorithm).ShortestPathUserTestInstructions(graphManager.StartNode, graphManager.EndNode); break;
            default: Debug.LogError("Graph task '" + graphSettings.GraphTask + "' invalid."); break;
        }

        if (instructions == null)
            return;

        // Initialize user test
        int movesNeeded = 1;
        userTestManager.InitUserTest(instructions, movesNeeded, FindNumberOfUserAction(instructions));

        // Set start time
        userTestManager.SetStartTime();

        if (graphSettings.Algorithm == Util.BFS)
        {
            //string result = "";
            foreach (KeyValuePair<int, InstructionBase> entry in instructions)
            {
                int instNr = entry.Key;
                InstructionBase inst = entry.Value;
                Debug.Log(inst.DebugInfo());
            }
        }

        graphManager.ResetGraph();
        userTestReady = true;
        //Debug.Log("Ready for user test!");
    }

    public int PrepareNextInstruction(InstructionBase instruction)
    {
        Debug.Log("Preparing next instruction: " + instruction.Instruction);
        Node node = null;
        bool gotNode = !graphAlgorithm.SkipDict[Util.SKIP_NO_ELEMENT].Contains(instruction.Instruction);
        bool noDestination = graphAlgorithm.SkipDict[Util.SKIP_NO_DESTINATION].Contains(instruction.Instruction);

        if (gotNode)
        {
            TraverseInstruction traverseInstruction = (TraverseInstruction)instruction;

            // Get the Sorting element
            node = traverseInstruction.Node;

            // Hands out the next instruction
            node.Instruction = traverseInstruction;

            // Set goal
            posManager.CurrentGoal = node;

            // Give this sorting element permission to give feedback to progress to next intstruction
            if (instruction.Instruction == UtilGraph.ENQUEUE_NODE_INST || instruction.Instruction == UtilGraph.DEQUEUE_NODE_INST)
                node.NextMove = true;

            if (instruction.Instruction == UtilGraph.PUSH_INST || instruction.Instruction == UtilGraph.POP_INST)
                node.NextMove = true;
        }

        // Display help on blackboard
        if (graphSettings.Difficulty <= Util.BEGINNER)
        {
            BeginnerWait = true;
            StartCoroutine(graphAlgorithm.UserTestHighlightPseudoCode(instruction, gotNode && !noDestination));

            // Node representation (destroy previous node)
            switch (instruction.Instruction)
            {
                case UtilGraph.DEQUEUE_NODE_INST:
                case UtilGraph.POP_INST:
                case UtilGraph.END_WHILE_INST:
                    UpdateListVisual(UtilGraph.DESTROY_CURRENT_NODE, null, Util.NO_VALUE); break;

            }
        }

        //Debug.Log("Got node: " + gotNode + ", no destination: " + noDestination);
        if (gotNode && !noDestination)
            return 0;
        //Debug.Log("Nothing to do for player, get another instruction");
        return 1;
    }


    // *** List visual / Node representation ***

    public void UpdateListVisual(string action, Node node, int index)
    {
        if (graphSettings.Difficulty == Util.BEGINNER)
        {
            switch (action)
            {
                case UtilGraph.ADD_NODE: listVisual.AddListObject(node); break;
                case UtilGraph.PRIORITY_ADD_NODE: listVisual.PriorityAdd(node, index); break;
                case UtilGraph.REMOVE_CURRENT_NODE: listVisual.RemoveCurrentNode(); break;
                case UtilGraph.DESTROY_CURRENT_NODE: listVisual.DestroyCurrentNode(); break;
            }
        }
    }

    public bool CheckListVisual(string action, Node node)
    {
        if (graphSettings.Difficulty == Util.BEGINNER)
        {
            switch (action)
            {
                case UtilGraph.HAS_NODE_REPRESENTATION: return listVisual.HasNodeRepresentation(node);
                default: Debug.LogError("Add case!"); return false;
            }
        }
        Debug.LogError("False false!");
        return true; // returning true leads to UpdateListVisual which will be blocked anyways
    }

    public ListVisual ListVisual
    {
        get { return listVisual; }
    }



}
