using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GraphMain : MainManager {

    [Space(10)]
    [Header("Graph main")]
    [Space(2)]
    [Header("Graph prefabs")]
    public GameObject nodePrefab;
    public GameObject undirectedEdgePrefab;
    public GameObject directedEdgePrefab;
    public GameObject symmetricDirectedEdgePrefab;

    [Space(5)]
    [Header("Containers")]
    [SerializeField]
    public GameObject nodeContainerObject, edgeContainerObject;

    [Space(5)]
    [Header("Environment objects")]
    [SerializeField]
    private GameObject settingsObj;

    [Space(5)]
    [Header("Other objects")]
    private GraphSettings graphSettings;
    private StartPillar startPillar;

    [SerializeField]
    private GameObject graphAlgorithmObj;

    [SerializeField]
    private Calculator calculator;

    [SerializeField]
    private Pointer pointer;

    private GraphAlgorithm graphAlgorithm;
    private GraphManager graphManager;
    private PositionManager posManager;

    // Moveable objects (blocking pseudocode)
    private List<IMoveAble> moveAbleObjects;

    [Space(5)]
    [Header("Support")]

    [SerializeField]
    private PseudoCodeViewer pseudoCodeViewer;

    [SerializeField]
    private Blackboard blackboard;

    [SerializeField]
    private ListVisual listVisual;

    // Backtracking shortest path, userTestGoalActive: move to node, usingCalculator: calculation in process to progress
    private bool backtracking = false, userTestGoalActive = false, usingCalculator = false;

    // Used by Pointer when selecting start-/end node(s)
    private int chosenNodes, numberOfNodesToChoose;

    private Vector3 pseudocodeChangeSizeStartPos = new Vector3(0f, 0f, 0f);

    // Update list visual after user has performed correct action
    private ListVisualInstruction updateListVisualInstruction;

    [Header("Debugging")]
    [SerializeField]
    private bool autoCalculation; // Not working anymore

    protected override void Awake()
    {
        base.Awake();

        // >>> Basic components
        graphSettings = settingsObj.GetComponent<GraphSettings>();

        demoManager = GetComponentInChildren<DemoManager>();
        userTestManager = GetComponentInChildren<UserTestManager>();

        posManager = FindObjectOfType<PositionManager>();

        startPillar = FindObjectOfType<StartPillar>();

        // Moveable
        moveAbleObjects = new List<IMoveAble>();
        var components = FindObjectsOfType<MonoBehaviour>().OfType<IMoveAble>();
        foreach (IMoveAble obj in components)
        {
            moveAbleObjects.Add(obj);
        }
    }

    //  --------------------------------------- Getters ---------------------------------------

    public GraphManager GraphManager
    {
        get { return graphManager; }
    }

    public override TeachingAlgorithm GetTeachingAlgorithm()
    {
        return graphAlgorithm;
    }

    public override TeachingSettings Settings
    {
        get { return graphSettings; }
    }

    public int ChosenNodes
    {
        get { return chosenNodes; }
        set { chosenNodes = value; }
    }

    public int NumberOfNodesToChoose
    {
        get { return numberOfNodesToChoose; }
    }

    //  --------------------------------------- Initialization methods ---------------------------------------

    protected override void PerformCheckList(string check)
    {
        switch (check)
        {
            case START_UP_CHECK:
                // Check until all node(s) are chosen by the player (traverse: start node // shortest path: start + end nodes)
                if (chosenNodes == numberOfNodesToChoose)
                {
                    startPillar.SetButtonActive(true);
                    startPillar.SetDisplayText("Click start to play");
                    checkListModeActive = false;
                }
                break;

            case WAIT_FOR_SUPPORT:
                break;

            case SHUT_DOWN_CHECK:
                // Get feedback from sub units whenever they are ready to shut down
                // if nodes etc. ready -> destroy

                bool readyForDestroy = true;
                foreach (KeyValuePair<string, bool> entry in safeStopChecklist)
                {
                    string key = entry.Key;
                    if (!safeStopChecklist[key])
                    {
                        readyForDestroy = false;
                        break;
                    }
                }

                // If no background process, start resetting and destroying game objects
                ShutdownProcess(readyForDestroy);
                break;

            default: Debug.Log(">>>>>>>>> Unknown check '" + check + "'."); break;
        }
    }

    public override void InstantiateSafeStart()
    {
        // First init common stuff
        base.InstantiateSafeStart();

        // >>> Grab variable data from settings
        string teachingMode = graphSettings.TeachingMode;
        int difficulty = graphSettings.Difficulty;
        float algorithmSpeed = graphSettings.AlgorithmSpeed;


        string graphTask = graphSettings.GraphTask; //..
        string graphStructure = graphSettings.GraphStructure;
        string edgeType = graphSettings.EdgeType;
        string edgeBuildMode = graphSettings.EdgeBuildMode;
        int[] graphSetup = graphSettings.GraphSetup();

        // Extra
        bool shortestPathOneToAll = graphSettings.ShortestPathOneToAll;

        // Only works for Demo as for now,
        if (shortestPathOneToAll && graphSettings.IsUserTest())
            shortestPathOneToAll = false;

        // >>> Init Algorithm
        graphAlgorithm = (GraphAlgorithm)GrabAlgorithmFromObj();
        graphAlgorithm.InitGraphAlgorithm(this, graphStructure, algorithmSpeed, shortestPathOneToAll);

        // >>> Init Graph manager
        bool isShortestPath = graphTask == UtilGraph.SHORTEST_PATH;
        graphManager = ActivateDeactivateGraphComponents(graphSettings.GraphStructure);
        graphManager.InitGraphManager(algorithmName, graphStructure, edgeType, isShortestPath, graphSettings.RNGDict(), listVisual);

        // Graph setup (rows/colums - tree depth/nTree - random?)
        graphManager.InitGraph(graphSetup);

        // Create graph based on init variables
        graphManager.CreateGraph(edgeBuildMode);

        // >>> Init position manager
        posManager.InitPositionManager(isShortestPath);

        // >>> Support
        blackboard.ChangeText(0, algorithmName);

        bool includeLineNr = Settings.PseudocodeLineNr;
        bool inDetailStep = Settings.PseudocodeStep;

        // Prepare difficulty level related stuff for user test (copied from sort)
        pseudoCodeViewer.InitPseudoCodeViewer(graphAlgorithm, includeLineNr, inDetailStep);
        if (graphSettings.TeachingMode == Util.USER_TEST)
        {
            if (difficulty <= Util.PSEUDO_CODE_MAX_DIFFICULTY)
            {
                // Pseudocode
                pseudoCodeViewer.PseudoCodeSetup();
                // Fix pseudocode for graph
                pseudoCodeViewer.ChangeSizeOfPseudocode(pseudocodeChangeSizeStartPos);
            }

            if (difficulty <= UtilGraph.LIST_VISUAL_MAX_DIFFICULTY)
            {
                // List visual
                string listType = graphAlgorithm.GetListType();
                listVisual.InitListVisual(listType, algorithmSpeed);
                //AddToCheckList(UtilGraph.LIST_VISUAL);
            }
        }
        else
        {
            // >>> Demo
            // Pseudocode
            pseudoCodeViewer.PseudoCodeSetup();

            // Fix pseudocode for graph
            pseudoCodeViewer.ChangeSizeOfPseudocode(pseudocodeChangeSizeStartPos);

            // List visual
            listVisual.InitListVisual(graphAlgorithm.GetListType(), algorithmSpeed);
        }

        // Hide menu
        StartCoroutine(ActivateTaskObjects(true));

        // Init start pillar
        bool selectNodes = graphSettings.SelectStartEndNodes;
        startPillar.InitStartPillar(teachingMode, selectNodes, isShortestPath);

        // If user want to select nodes, then start button will become inactive until node(s) have been chosen
        if (selectNodes)
        {
            // Start node
            chosenNodes = 0;
            numberOfNodesToChoose = 1;

            // End node
            if (isShortestPath)
                numberOfNodesToChoose++;

            // Init pointer with start task
            pointer.InitPointer(UtilGraph.SELECT_NODE, numberOfNodesToChoose);

            // Start check list
            activeChecklist = START_UP_CHECK;
            checkListModeActive = true;
        }
        else
            StartCoroutine(SetAutomaticallyImportantNodes(isShortestPath));
    }

    // Start-/end node(s) set automatically (settings in editor)
    public IEnumerator SetAutomaticallyImportantNodes(bool isShortestPath)
    {
        yield return loading;

        // Set start (and end) node(s)
        graphManager.SetNodes(isShortestPath);

        checkListModeActive = false;
    }

    // Makes the settings menu and start pillar visible/invisible
    protected override IEnumerator ActivateTaskObjects(bool active)
    {
        graphSettings.FillTooltips("Loading setup...", false);
        yield return loading;

        // Settings menu
        graphSettings.ActiveInScene(!active);

        startPillar.ActiveInScene(active);

        yield return loading;
        graphSettings.FillTooltips("", false);

        pointer.AllowShooting = active;
    }

    //  --------------------------------------- Reset methods ---------------------------------------
    public override void DestroyAndReset()
    {
        base.DestroyAndReset();

        chosenNodes = 0;
        numberOfNodesToChoose = 0;

        // Variable reset
        backtracking = false;
        userTestGoalActive = false;
        usingCalculator = false;

        // Delete list visual
        listVisual.DestroyAndReset();
        updateListVisualInstruction = null;

        // Delete graph
        graphManager.DeleteGraph();

        // Reset algorithm
        graphAlgorithm.ResetSetup();

        // Pseudocode
        pseudoCodeViewer.DestroyContent();

        // Pointer
        pointer.ResetPointer();

        StartCoroutine(ActivateTaskObjects(false));

        calculator.ResetDevice();
        demoDevice.ResetDevice();
    }

    /* --------------------------------------- Demo ---------------------------------------
     * - Gives a visual presentation of <graph algorithm>
     * - Watch and learn while the graph changes based on the algorithm
    */
    public override void PerformAlgorithmDemo()
    {
        Debug.Log("Performing " + algorithmName + " " + graphSettings.GraphTask + " demo.");
        if (true)
        {
            // Getting instructions for this sample of sorting elements
            Dictionary<int, InstructionBase> instructions = null;

            switch (graphSettings.GraphTask)
            {
                case UtilGraph.TRAVERSE:
                    if (algorithmName == Util.DFS_RECURSIVE)
                        instructions = ((DFS)graphAlgorithm).RecursiveInstructions(graphManager.StartNode);
                    else
                        instructions = ((ITraverse)graphAlgorithm).TraverseUserTestInstructions(graphManager.StartNode);
                    break;

                case UtilGraph.SHORTEST_PATH: instructions = ((IShortestPath)graphAlgorithm).ShortestPathUserTestInstructions(graphManager.StartNode, graphManager.EndNode); break;
                default: Debug.LogError("Graph task '" + graphSettings.GraphTask + "' invalid."); break;
            }

            if (instructions == null)
                return;

            Debug.Log("Number of instructions: " + instructions.Count);

            demoManager.InitDemo(instructions);

            graphManager.ResetGraph();

            newDemoImplemented = true;
        }
        else
        {
            switch (graphSettings.GraphTask)
            {
                case UtilGraph.TRAVERSE: StartCoroutine(((ITraverse)graphAlgorithm).TraverseDemo(graphManager.StartNode)); break;
                case UtilGraph.SHORTEST_PATH: StartCoroutine(((IShortestPath)graphAlgorithm).ShortestPathDemo(graphManager.StartNode, graphManager.EndNode)); break;
            }
        }
    }

    /* --------------------------------------- User Test ---------------------------------------
     * - Gives a visual presentation of elements used in <graph algorithm>
     * - Player needs to do several tasks to complete (point and shoot at nodes, teleport to nodes)
    */
    public override void PerformAlgorithmUserTest()
    {
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
        userTestManager.InitUserTest(instructions, 1, FindNumberOfUserAction(instructions), Settings.Difficulty);

        graphManager.ResetGraph();
    }

    protected override void UserTestUpdate()
    {
        // If a node is set to be visited or traversed, wait until it has been achieved
        if (userTestGoalActive && !posManager.PlayerWithinGoalPosition)
            return;
        else
            userTestGoalActive = false;

        // First check if user test setup is complete
        if (userTestManager.HasInstructions())
        {
            // If a calculation is required to progress in the algorithm
            if (algorithmName == Util.DIJKSTRA && usingCalculator)
            {
                // Check if it's in process and whether the equal button has been clicked
                if (calculator.CalculationInProcess && calculator.EqualButtonClicked)
                {
                    InstructionBase inst = userTestManager.GetInstruction();
                    ShortestPathInstruction spInst = null;
                    if (inst != null && inst.Instruction == UtilGraph.IF_DIST_PLUS_EDGE_COST_LESS_THAN)
                        spInst = ((ShortestPathInstruction)userTestManager.GetInstruction());
                    else
                        return;

                    // When equal buttons has been clicked, check  whether the input data is correct
                    int node1Dist = spInst.CurrentNode.Dist;
                    int edgeCost = spInst.CurrentEdge.Cost;
                    int node2Dist = spInst.ConnectedNode.Dist;

                    bool correctUserInput = calculator.ControlUserInput(node1Dist, edgeCost, node2Dist);

                    // If input data was correct, then progress to next instruction (add only one)
                    if (correctUserInput && userTestManager.ReadyForNext != userTestManager.UserActionToProceed)
                    {
                        userTestManager.IncrementTotalCorrect();
                        userTestManager.ReadyForNext += 1;
                        
                        ((Dijkstra)graphAlgorithm).IfStatementContent = calculator.IfStatementContent();
                        pseudoCodeViewer.SetCodeLine(((Dijkstra)graphAlgorithm).CollectLine(9), Util.BLACKBOARD_TEXT_COLOR);
                        calculator.FeedbackReceived = true;
                    }
                    else if (!calculator.FeedbackReceived)
                    {
                        userTestManager.Mistake();
                        calculator.FeedbackReceived = true;
                    }
                }
                else if (!calculator.CalculationInProcess)
                {
                    // Feedback is given on the calculator, so when calculationInProcess = false (calculation completed), we continue
                    usingCalculator = false;
                    pointer.AllowShooting = true;
                }
            }

            // Continue when user has finished sub task, or else whenever ready
            if (userTestManager.ReadyForNext == userTestManager.UserActionToProceed)
            {
                if (graphSettings.Difficulty <= UtilGraph.LIST_VISUAL_MAX_DIFFICULTY && updateListVisualInstruction != null)
                {
                    listVisual.ExecuteInstruction(updateListVisualInstruction, true);
                    updateListVisualInstruction = null;
                }

                // Reset counter
                userTestManager.ReadyForNext = 0;

                // Check if we still have any instructions
                if (!userTestManager.HasInstructions()) // ???
                {
                    graphAlgorithm.IsTaskCompleted = true;
                }
                else
                {
                    // Still got instructions?
                    bool hasInstruction = userTestManager.IncrementToNextInstruction();

                    // Hot fix - solve in some other way?
                    if (hasInstruction)
                        userTestManager.ReadyForNext += PrepareNextInstruction(userTestManager.GetInstruction());
                }
            }

            if (Settings.UserTestScore)
                blackboard.ChangeText(1, userTestManager.FillInBlackboard());
        }
    }

    // User test
    public int PrepareNextInstruction(InstructionBase instruction)
    {
        string inst = instruction.Instruction;
        Debug.Log(">>> " + instruction.DebugInfo());

        // First check whether the instruction contains a node and/or destination
        bool gotNode = !graphAlgorithm.SkipDict[Util.SKIP_NO_ELEMENT].Contains(inst);
        bool noDestination = graphAlgorithm.SkipDict[Util.SKIP_NO_DESTINATION].Contains(inst);

        // List visual Update
        if (instruction is ListVisualInstruction)
        {
            // Only provide list visual support until <lvl>
            if (graphSettings.Difficulty > UtilGraph.LIST_VISUAL_MAX_DIFFICULTY)
                return 1;

            ListVisualInstruction listVisualInst = (ListVisualInstruction)instruction;
            listVisual.ExecuteInstruction(listVisualInst, true);

            //Debug.Log("List visual instruction: " + listVisualInst.DebugInfo());
        }
        else
        {
            Node node = null;

            if (gotNode)
            {
                if (instruction is TraverseInstruction)
                {
                    TraverseInstruction traverseInstruction = (TraverseInstruction)instruction;
                    //Debug.Log("Traverse instruction: " + traverseInstruction.DebugInfo());

                    // Get the Sorting element
                    node = traverseInstruction.Node;

                    // Hands out the next instruction
                    node.Instruction = traverseInstruction;

                    // Set goal
                    posManager.CurrentGoal = node;

                    // Reset if pointer must be used on the same node twice
                    if (traverseInstruction.VisitInst && pointer.prevNodeShot == node)
                        pointer.prevNodeShot = null;

                    // Reset position manager if the user already stands on top of the node
                    if (traverseInstruction.TraverseInst && posManager.ReportedNode == node)
                        posManager.ReportedNode = null;

                    // Give this sorting element permission to give feedback to progress to next intstruction
                    node.NextMove = NextIsUserMove(inst);

                    // Traverse instruction extra
                    switch (inst)
                    {
                        // Highlight the node we are currently going to work at
                        case UtilGraph.DEQUEUE_NODE_INST:
                        case UtilGraph.POP_INST:
                        case UtilGraph.PRIORITY_REMOVE_NODE:
                            // Hide all edge cost to make it easier to see node distances
                            if (graphSettings.GraphTask == UtilGraph.SHORTEST_PATH)
                                graphManager.MakeEdgeCostVisible(false);
                            break;

                        case UtilGraph.FOR_ALL_NEIGHBORS_INST:
                            // Make edge cost visible again
                            if (graphSettings.GraphTask == UtilGraph.SHORTEST_PATH)
                                graphManager.MakeEdgeCostVisible(true);
                            break;
                    }

                    // Perform list visual instruction in next step (when current instruction has been correctly performed)
                    if (traverseInstruction.ListVisualInstruction != null)
                        updateListVisualInstruction = traverseInstruction.ListVisualInstruction;

                }
                else if (instruction is ShortestPathInstruction)
                {
                    ShortestPathInstruction spInst = (ShortestPathInstruction)instruction;
                    //Debug.Log("Shortest path instruction: " + spInst.DebugInfo());

                    // Get the Sorting element
                    if (spInst.CurrentNode != null)
                        node = spInst.CurrentNode;
                    else
                        node = spInst.ConnectedNode;

                    // Hands out the next instruction
                    node.Instruction = spInst;

                    // Set goal
                    posManager.CurrentGoal = node;

                    // Give this sorting element permission to give feedback to progress to next intstruction
                    node.NextMove = NextIsUserMove(inst);

                    // Shortest path extra
                    switch (inst)
                    {
                        case UtilGraph.IF_DIST_PLUS_EDGE_COST_LESS_THAN:
                            // Turn off highlight effect of previous line
                            pseudoCodeViewer.ChangeColorOfText(8, Util.BLACKBOARD_TEXT_COLOR);

                            // Since we highlighted the color and/or text position of the node/edge we are working on (highlighting purpose) in the previous instruction, now we reset them
                            // Changed color of node
                            spInst.ConnectedNode.CurrentColor = UtilGraph.VISITED_COLOR;
                            // Reset text color/position of the edge cost
                            spInst.CurrentEdge.CurrentColor = UtilGraph.VISITED_COLOR;

                            // Perform sub task: calculate dist (current node) + edge cost to the connected node
                            if (!autoCalculation)
                            {
                                usingCalculator = true;
                                pointer.AllowShooting = false;
                                calculator.InitCalculation(Calculator.GRAPH_TASK);
                                calculator.SpawnDeviceInfrontOfPlayer();
                            }
                            break;
                    }

                    // Perform list visual instruction in next step (when current instruction has been correctly performed)
                    if (spInst.ListVisualInstruction != null)
                        updateListVisualInstruction = spInst.ListVisualInstruction;
                }
            }
        }

        // Display help on blackboard
        if (graphSettings.Difficulty <= Util.PSEUDO_CODE_HIGHTLIGHT_MAX_DIFFICULTY)
        {
            WaitForSupportToComplete++;
            StartCoroutine(graphAlgorithm.UserTestHighlightPseudoCode(instruction, gotNode));// && !noDestination));
        }

        // InstructionBase extra
        switch (inst)
        {
            case UtilGraph.SET_ALL_NODES_TO_INFINITY:
                graphManager.SetAllNodesDist(UtilGraph.INF);
                break;

            case UtilGraph.SET_START_NODE_DIST_TO_ZERO:
                // Find start node and set distance to 0
                Node startNode = graphManager.StartNode;
                startNode.Dist = 0;

                // If list visual: update value
                if (Settings.Difficulty <= UtilGraph.LIST_VISUAL_MAX_DIFFICULTY)
                    listVisual.FindNodeRepresentation(startNode).UpdateSurfaceText(UtilGraph.DIST_UPDATE_COLOR);
                break;

            case UtilGraph.MARK_END_NODE:
                Node endNode = graphManager.EndNode;
                endNode.CurrentColor = UtilGraph.SHORTEST_PATH_COLOR;
                endNode.PrevEdge.CurrentColor = UtilGraph.SHORTEST_PATH_COLOR;
                break;

            case UtilGraph.END_FOR_LOOP_INST:
                if (graphSettings.Difficulty <= UtilGraph.LIST_VISUAL_MAX_DIFFICULTY)
                    listVisual.DestroyCurrentNode();
                break;

            case UtilGraph.NO_PATH_FOUND:
            case Util.FINAL_INSTRUCTION:
                // No path found
                WaitForSupportToComplete++;
                StartCoroutine(FinishUserTest());
                break;
        }

        //Debug.Log("Got node: " + gotNode + ", no destination: " + noDestination);
        if (gotNode && !noDestination)
            return 0;
        //Debug.Log("Nothing to do for player, get another instruction");
        return 1;
    }

    // Check whether the current instruction requires an action performed by the player
    private bool NextIsUserMove(string inst)
    {
        switch (algorithmName)
        {
            case Util.BFS: return inst == UtilGraph.ENQUEUE_NODE_INST || inst == UtilGraph.DEQUEUE_NODE_INST;
            case Util.DFS:
            case Util.DFS_RECURSIVE:
                return inst == UtilGraph.PUSH_INST || inst == UtilGraph.POP_INST;

            case Util.DIJKSTRA: return inst == UtilGraph.ADD_NODE || inst == UtilGraph.PRIORITY_REMOVE_NODE || inst == UtilGraph.RELAX_NEIGHBOR_NODE || inst == UtilGraph.IF_DIST_PLUS_EDGE_COST_LESS_THAN || inst == UtilGraph.BACKTRACK;
            default: Debug.LogError("Next move rules not defined for algorithm: " + algorithmName + "'."); return false;
        }
    }


    // --------------------------------------- Finish off ---------------------------------------

    protected override IEnumerator FinishUserTest()
    {
        yield return base.FinishUserTest();

        // Do any graph visual stuff???
        //yield return graphManager.AllNodes()

        WaitForSupportToComplete--;
    }

    protected override void TaskCompletedFinishOff()
    {
        if (graphSettings.IsDemo())
        {
            switch (graphSettings.GraphTask)
            {
                case UtilGraph.TRAVERSE:
                    break;

                case UtilGraph.SHORTEST_PATH:
                    if (!backtracking)
                    {
                        if (graphAlgorithm.ShortestPathOneToAll)
                            StartCoroutine(graphManager.BacktrackShortestPathsAll(graphAlgorithm.DemoStepDuration));
                        else
                        {
                            listVisual.CreateBackTrackList(graphManager.EndNode);
                            StartCoroutine(graphManager.BacktrackShortestPath(graphManager.EndNode, graphAlgorithm.DemoStepDuration));
                        }
                        backtracking = true;
                    }
                    break;
            }

        }
        else if (Settings.IsUserTest())
        {
            if (Settings.UserTestScore)
            {
                userTestManager.SetEndTime();
                userTestManager.CalculateScore();
                blackboard.ChangeText(0, "User test details:");
                blackboard.ChangeText(1, userTestManager.GetExaminationResult() + "\n" + userTestManager.IncorrectActionDetails());
            }
        }
    }

    /* --------------------------------------- List visual / Node representation ---------------------------------------
     * Some help functions for list visual
    */

    // Used by demo to update list visual (node/list representation)
    public void UpdateListVisual(string action, Node node, int index)
    {
        switch (action)
        {
            case UtilGraph.ADD_NODE: listVisual.AddListObject(node); break;
            case UtilGraph.PRIORITY_ADD_NODE: listVisual.PriorityAdd(node, index); break;
            case UtilGraph.REMOVE_CURRENT_NODE: listVisual.RemoveCurrentNode(); break;
            case UtilGraph.DESTROY_CURRENT_NODE: listVisual.DestroyCurrentNode(); break;
        }
    }

    public bool CheckListVisual(string action, Node node)
    {
        switch (action)
        {
            case UtilGraph.HAS_NODE_REPRESENTATION: return listVisual.HasNodeRepresentation(node);
            default: Debug.LogError("Add case!"); return false;
        }
    }

    // Remove and fix methods?
    public ListVisual ListVisual
    {
        get { return listVisual; }
    }


    // --------------------------------------- Some init methods ---------------------------------------

    // Keeps only one graph structure active: Activate/deactivate components (Grid / Tree / Random) - working?
    private GraphManager ActivateDeactivateGraphComponents(string graphStructure)
    {
        switch (graphStructure)
        {
            case UtilGraph.GRID_GRAPH: return GetComponentInChildren<GridManager>();
            case UtilGraph.TREE_GRAPH: return GetComponentInChildren<TreeManager>();
            case UtilGraph.RANDOM_GRAPH: return GetComponentInChildren<RandomGraphManager>();

            default: Debug.Log("Graph structure '" + graphStructure + "' not found."); break;
        }
        return null;
    }

    protected override TeachingAlgorithm GrabAlgorithmFromObj()
    {
        switch (algorithmName)
        {
            case Util.BFS: return graphAlgorithmObj.GetComponent<BFS>();
            case Util.DFS: case Util.DFS_RECURSIVE: return graphAlgorithmObj.GetComponent<DFS>();
            case Util.DIJKSTRA: return graphAlgorithmObj.GetComponent<Dijkstra>();
            default: return null;
        }
    }

    private bool moveOut = true;
    public override void ToggleVisibleStuff()
    {
        foreach (IMoveAble moveAble in moveAbleObjects)
        {
            if (moveOut)
                moveAble.MoveOut();
            else
                moveAble.MoveBack();
        }
        moveOut = !moveOut;
    }


}
