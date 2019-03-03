using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GraphManager))]
[RequireComponent(typeof(UserTestManager))]
public class GraphMain : MainManager {

    private GraphSettings graphSettings;
    private GraphAlgorithm graphAlgorithm;
    private GraphManager graphManager;

    private UserTestManager userTestManager;
    private PositionManager posManager;

    [SerializeField]
    private GameObject graphAlgorithmObj;

    [SerializeField]
    private PseudoCodeViewer pseudoCodeViewer;

    [SerializeField]
    private ListVisual listVisual;

    private bool backtracking = false, userTestGoalActive = false;


    protected virtual void Awake()
    {
        Debug.Log("I am alive!");

        // >>> Basic components
        userTestManager = GetComponent(typeof(UserTestManager)) as UserTestManager;
        posManager = FindObjectOfType<PositionManager>();
        graphSettings = (GraphSettings)settings;
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
                        userTestManager.ReadyForNext += graphManager.PrepareNextInstruction(userTestManager.GetInstruction());
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

    // Update is called once per frame
    void oldUpdate() // OLD NOT USED: DELETE WHEN RDY
    {
        if (graphAlgorithm.IsTaskCompleted && !backtracking)
        {
            if (graphAlgorithm is IShortestPath)
            {
                Debug.Log("Starting backtracking");
                if (graphAlgorithm.ShortestPathOneToAll)
                    StartCoroutine(graphManager.BacktrackShortestPathsAll(graphAlgorithm.DemoStepDuration));
                else
                    StartCoroutine(graphManager.BacktrackShortestPath(graphManager.EndNode, graphAlgorithm.DemoStepDuration));
                backtracking = true;
            }
        }
        else if (graphSettings.IsDemo())
        {

        }
        else if (graphSettings.IsUserTest())
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
                            userTestManager.ReadyForNext += graphManager.PrepareNextInstruction(userTestManager.GetInstruction());
                        //else if (elementManager.AllSorted())
                        //    StartCoroutine(FinishUserTest());

                    }
                }
                //displayUnitManager.BlackBoard.ChangeText(displayUnitManager.BlackBoard.TextIndex, userTestManager.FillInBlackboard());
            }
        }
    }

    public override TeachingAlgorithm GetTeachingAlgorithm()
    {
        return graphAlgorithm;
    }

    // Keeps only one graph structure active
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
        listVisual.SetAlgorithmSpeed = graphSettings.AlgorithmSpeed;
        switch (algorithmName)
        {
            case Util.BFS:
                //graphAlgorithmObj.GetComponent<BFS>().enabled = true;
                //graphAlgorithmObj.GetComponent<DFS>().enabled = false;
                //graphAlgorithmObj.GetComponent<Dijkstra>().enabled = false;
                listVisual.SetListType(Util.QUEUE);
                return graphAlgorithmObj.GetComponent<BFS>();

            case Util.DFS:
            case Util.DFS_RECURSIVE:
                //graphAlgorithmObj.GetComponent<DFS>().enabled = true;
                //graphAlgorithmObj.GetComponent<BFS>().enabled = false;
                //graphAlgorithmObj.GetComponent<Dijkstra>().enabled = false;
                listVisual.SetListType(Util.STACK);
                return graphAlgorithmObj.GetComponent<DFS>();

            case Util.DIJKSTRA:
                //graphAlgorithmObj.GetComponent<Dijkstra>().enabled = true;
                //graphAlgorithmObj.GetComponent<BFS>().enabled = false;
                //graphAlgorithmObj.GetComponent<DFS>().enabled = false;
                listVisual.SetListType(Util.PRIORITY_LIST);
                return graphAlgorithmObj.GetComponent<Dijkstra>();

            default: return null;
        }
    }

    public override void InstantiateSetup()
    {
        graphSettings.PrepareSettings();

        // From awake
        // >>> Algorithm
        algorithmName = graphSettings.Algorithm;
        graphAlgorithm = (GraphAlgorithm)GrabAlgorithmFromObj();
        graphAlgorithm.MainManager = this;

        graphAlgorithm.GraphStructure = graphSettings.Graphstructure;
        graphAlgorithm.DemoStepDuration = new WaitForSeconds(graphSettings.AlgorithmSpeed);
        // Extra settings
        graphAlgorithm.ShortestPathOneToAll = graphSettings.ShortestPathOneToAll;
        graphAlgorithm.VisitLeftFirst = graphSettings.VisitLeftFirst;
        // >>> Extra learning material
        graphAlgorithm.ListVisual = listVisual;
        // Pseudocode
        graphAlgorithm.PseudoCodeViewer = pseudoCodeViewer;

        // end

        // Setup graph manager + Activate/deactivate components (Grid / Tree / Random)
        graphManager = ActivateDeactivateGraphComponents(graphSettings.Graphstructure);
        graphManager.InitGraphManager(graphSettings, graphAlgorithm, graphSettings.RNGDict());

        pseudoCodeViewer.PseudoCodeSetup();

        // Get variables for graph setup
        graphManager.InitGraph(graphSettings.GraphSetup());
        graphManager.SetupImportantNodes(graphSettings.StartNode(), graphSettings.EndNode(), true);

        // Create graph based on init variables
        graphManager.CreateGraph();

        // Mark start-/end nodes
        graphManager.SetupImportantNodes(null, null, false);

        // Init teaching mode
        switch (graphSettings.TeachingMode)
        {
            case Util.DEMO: PerformAlgorithmDemo(); break;
            case Util.STEP_BY_STEP: PerformAlgorithmStepByStep(); break;
            case Util.USER_TEST: PerformAlgorithmUserTest(); break;
            default: Debug.LogError("Teaching mode '" + graphSettings.TeachingMode + "' unknown."); break;
        }

        initialized = true;
    }

    public override void DestroyAndReset()
    {
        initialized = false;

    }


    public override void PerformAlgorithmDemo()
    {
        switch (graphSettings.Algorithm)
        {
            case Util.BFS: StartCoroutine(((BFS)graphAlgorithm).Demo(graphManager.StartNode)); break;

            case Util.DFS:
                StartCoroutine(((DFS)graphAlgorithm).Demo(graphManager.StartNode));
                break;

            case Util.DFS_RECURSIVE:
                //algorithm.ListVisual.AddListObject(GetNode(startNode[0], startNode[1]).NodeAlphaID);
                StartCoroutine(((DFS)graphAlgorithm).DemoRecursive(graphManager.StartNode));
                break;

            case Util.DIJKSTRA:
                StartCoroutine(((Dijkstra)graphAlgorithm).Demo(graphManager.StartNode, graphManager.EndNode));
                //StartCoroutine(((Dijkstra)graphAlgorithm).DemoNoPseudocode(startNode, endNode));
                break;

            default: Debug.LogError("'" + algorithmName + "' unknown."); break;
        }
    }

    public override void PerformAlgorithmStepByStep()
    {
        throw new System.NotImplementedException();
    }

    public override void PerformAlgorithmUserTest()
    {
        Debug.Log(">>> Performing " + algorithmName + " user test.");

        // Getting instructions for this sample of sorting elements
        Dictionary<int, InstructionBase> instructions = ((BFS)graphAlgorithm).UserTestInstructions(graphManager.StartNode);

        // Initialize user test
        int movesNeeded = 1;
        userTestManager.InitUserTest(instructions, movesNeeded, FindNumberOfUserAction(instructions));

        // Set start time
        userTestManager.SetStartTime();

        if (graphSettings.Algorithm == Util.BFS)
        {
            string result = "";
            foreach (KeyValuePair<int, InstructionBase> entry in instructions)
            {
                int instNr = entry.Key;
                InstructionBase inst = entry.Value;

                if (inst is TraverseInstruction && inst.Instruction == UtilGraph.DEQUEUE_NODE_INST)
                {
                    result += ((TraverseInstruction)inst).Node.NodeAlphaID + " ";
                }
            }
            //result.Substring(0, result.Length - 4);
            Debug.Log(result);
        }

        graphManager.ResetGraph();
        userTestReady = true;
        Debug.Log("Ready for user test!");
    }



    // Moved to graphmanager
    //public int PrepareNextInstruction(InstructionBase instruction)
    //{
    //    Debug.Log("Preparing next instruction: " + instruction.Instruction);
    //    Node node = null;
    //    bool gotNode = !graphAlgorithm.SkipDict[Util.SKIP_NO_ELEMENT].Contains(instruction.Instruction);
    //    bool noDestination = graphAlgorithm.SkipDict[Util.SKIP_NO_DESTINATION].Contains(instruction.Instruction);

    //    if (gotNode)
    //    {
    //        TraverseInstruction traverseInstruction = (TraverseInstruction)instruction;

    //        // Get the Sorting element
    //        node = traverseInstruction.Node;

    //        // Hands out the next instruction
    //        node.Instruction = traverseInstruction;

    //        // Set goal
    //        posManager.CurrentGoal = node.gameObject;

    //        // Give this sorting element permission to give feedback to progress to next intstruction
    //        if (instruction.Instruction == UtilGraph.DEQUEUE_NODE_INST)
    //            node.NextMove = true;
    //    }

    //    // Display help on blackboard
    //    if (true) //algorithmSettings.Difficulty <= UtilSort.BEGINNER)
    //    {
    //        Debug.Log("Fixing pseudocode!");
    //        BeginnerWait = true;
    //        StartCoroutine(graphAlgorithm.UserTestHighlightPseudoCode(instruction, gotNode && !noDestination));

    //        // Node representation
    //        switch (instruction.Instruction)
    //        {
    //            case UtilGraph.ENQUEUE_NODE_INST: graphAlgorithm.ListVisual.AddListObject(node); break;
    //            case UtilGraph.DEQUEUE_NODE_INST: graphAlgorithm.ListVisual.RemoveCurrentNode(); break;
    //            case UtilGraph.END_FOR_LOOP_INST: graphAlgorithm.ListVisual.DestroyCurrentNode(); break;
    //        }
    //    }

    //    Debug.Log("Got node: " + gotNode + ", no destination: " + noDestination);
    //    if (gotNode && !noDestination)
    //        return 0;
    //    Debug.Log("Nothing to do for player, get another instruction");
    //    return 1;
    //}

}
