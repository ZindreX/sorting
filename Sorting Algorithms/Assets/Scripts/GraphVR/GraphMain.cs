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

    [SerializeField]
    private Calculator calculator;

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

    // Backtracking shortest path, userTestGoalActive: move to node, usingCalculator: calculation in process to progress
    private bool backtracking = false, userTestGoalActive = false, usingCalculator = false;



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
            // If a calculation is required to progress in the algorithm
            if (usingCalculator)
            {
                // Check if it's in process and whether the equal button has been clicked
                if (calculator.CalculationInProcess && calculator.EqualButtonClicked)
                {
                    // When equal buttons has been clicked, check  whether the input data is correct
                    int nodeDist = ((ShortestPathInstruction)userTestManager.GetInstruction()).CurrentNode.Dist;
                    int edgeCost = ((ShortestPathInstruction)userTestManager.GetInstruction()).CurrentEdge.Cost;
                    bool correctUserInput = calculator.ControlUserInput(nodeDist, edgeCost);

                    // If input data was correct, then progress to next instruction (add only one)
                    if (correctUserInput && userTestManager.ReadyForNext != userTestManager.UserActionToProceed)
                        userTestManager.ReadyForNext += 1;
                }
                else if (!calculator.CalculationInProcess)
                {
                    // Feedback is given on the calculator, so when calculationInProcess = false (calculation completed), we continue
                    usingCalculator = false;
                }
            }

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

        backtracking = false;

        userTestGoalActive = false;

        usingCalculator = false;

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

        Debug.Log("Number of instructions: " + instructions.Count);

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
        string inst = instruction.Instruction;
        int difficulty = graphSettings.Difficulty;
        bool gotNode = !graphAlgorithm.SkipDict[Util.SKIP_NO_ELEMENT].Contains(inst);
        bool noDestination = graphAlgorithm.SkipDict[Util.SKIP_NO_DESTINATION].Contains(inst);

        Debug.Log("Preparing next instruction: " + inst);

        // List visual Update
        if (instruction is ListVisualInstruction)
        {
            Debug.Log("List visual instruction");
            BeginnerWait = true;
            ListVisualInstruction listVisualInst = (ListVisualInstruction)instruction;
            switch (inst)
            {
                case UtilGraph.ADD_NODE: listVisual.AddListObject(listVisualInst.Node); break;
                case UtilGraph.PRIORITY_ADD_NODE: listVisual.PriorityAdd(listVisualInst.Node, listVisualInst.Index); break;
                case UtilGraph.REMOVE_CURRENT_NODE: listVisual.RemoveCurrentNode(); break;
                case UtilGraph.DESTROY_CURRENT_NODE: listVisual.DestroyCurrentNode(); break;
                case UtilGraph.HAS_NODE_REPRESENTATION:
                    Node node = listVisualInst.Node;
                    int index = listVisualInst.Index;
                    bool hasNodeRep = listVisual.HasNodeRepresentation(node);
                    string subInstruction = listVisualInst.GetCase(hasNodeRep);

                    if (!hasNodeRep)
                        PrepareNextInstruction(new ListVisualInstruction(subInstruction, instruction.INSTRUCION_NR, node, index));
                    else
                        StartCoroutine(listVisual.UpdateValueAndPositionOf(node, index));

                    break;
                default: Debug.LogError("List visual instruction '" + instruction.Instruction + "' invalid."); break;
            }
            BeginnerWait = false; // ???

            return 1;
        }
        else
        {
            Node node = null;

            if (gotNode)
            {
                if (instruction is TraverseInstruction)
                {
                    TraverseInstruction traverseInstruction = (TraverseInstruction)instruction;

                    // Get the Sorting element
                    node = traverseInstruction.Node;

                    // Hands out the next instruction
                    node.Instruction = traverseInstruction;

                    // Set goal
                    posManager.CurrentGoal = node;

                    // Give this sorting element permission to give feedback to progress to next intstruction
                    node.NextMove = NextIsUserMove(inst);
                }
                else if (instruction is ShortestPathInstruction)
                {
                    ShortestPathInstruction shortestPathInst = (ShortestPathInstruction)instruction;

                    // Get the Sorting element
                    if (shortestPathInst.CurrentNode != null)
                        node = shortestPathInst.CurrentNode;
                    else
                        node = shortestPathInst.ConnectedNode;

                    // Hands out the next instruction
                    node.Instruction = shortestPathInst;

                    // Set goal
                    posManager.CurrentGoal = node;

                    // Give this sorting element permission to give feedback to progress to next intstruction
                    node.NextMove = NextIsUserMove(inst);
                }
            }

            // Display help on blackboard
            if (difficulty <= Util.BEGINNER)
            {
                BeginnerWait = true;
                StartCoroutine(graphAlgorithm.UserTestHighlightPseudoCode(instruction, gotNode));// && !noDestination));
            }


            switch (inst)
            {
                case UtilGraph.SET_ALL_NODES_TO_INFINITY: graphManager.SetAllNodesToInf(); break;
                case UtilGraph.SET_START_NODE_DIST_TO_ZERO: graphManager.StartNode.Dist = 0; break;
                case UtilGraph.IF_DIST_PLUS_EDGE_COST_LESS_THAN:
                    usingCalculator = true;
                    calculator.InitCalculation();
                    break;
            }

            Debug.Log("Got node: " + gotNode + ", no destination: " + noDestination);
            if (gotNode && !noDestination)
                return 0;
            Debug.Log("Nothing to do for player, get another instruction");
            return 1;
        }

    }

    private bool NextIsUserMove(string inst)
    {
        switch (algorithmName)
        {
            case Util.BFS: return inst == UtilGraph.ENQUEUE_NODE_INST || inst == UtilGraph.DEQUEUE_NODE_INST;
            case Util.DFS: return inst == UtilGraph.PUSH_INST || inst == UtilGraph.POP_INST;
            case Util.DIJKSTRA: return inst == UtilGraph.ADD_NODE || inst == UtilGraph.PRIORITY_REMOVE_NODE || inst == UtilGraph.VISIT_CONNECTED_NODE || inst == UtilGraph.IF_DIST_PLUS_EDGE_COST_LESS_THAN;
            default: Debug.LogError("Next move rules not defined for algorithm: " + algorithmName + "'."); return false;
        }
    }

    // *** List visual / Node representation ***

    // Used by demo to update list visual (node/list representation)
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
