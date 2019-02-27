using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GraphSettings))]
[RequireComponent(typeof(GraphManager))]
[RequireComponent(typeof(UserTestManager))]
public class GraphMain : MainManager {

    private GraphSettings gs;
    private GraphAlgorithm graphAlgorithm;
    private GraphManager gm;

    private UserTestManager userTestManager;

    private Node startNode, endNode;
    private bool backtracking = false;


    protected virtual void Awake()
    {
        // Basic components
        userTestManager = GetComponent(typeof(UserTestManager)) as UserTestManager;


        // Get settings from editor
        gs = GetComponent(typeof(GraphSettings)) as GraphSettings;
        gs.PrepareSettings();

        // Algorithm
        graphAlgorithm = gs.GetGraphAlgorithm();
        teachingAlgorithm = graphAlgorithm; // clean up
        teachingAlgorithm.MainManager = this; // clean up (use only this instead of graphAl?

        graphAlgorithm.GraphStructure = gs.Graphstructure;
        graphAlgorithm.Seconds = gs.AlgorithmSpeed;
        graphAlgorithm.ListVisual = gs.ListVisual;
        graphAlgorithm.ShortestPathOneToAll = gs.ShortestPathOneToAll;
        graphAlgorithm.VisitLeftFirst = gs.VisitLeftFirst;
        algorithmName = graphAlgorithm.AlgorithmName;

        // Setup graph manager + Activate/deactivate components (Grid / Tree / Random)
        gm = ActivateDeactivateGraphComponents(gs.Graphstructure);
        gm.InitGraphManager(gs, graphAlgorithm);

        // Pseudocode
        graphAlgorithm.PseudoCodeViewer = gs.PseudoCodeViewer;

    }

    // Use this for initialization
    void Start()
    {
        gs.PseudoCodeViewer.PseudoCodeSetup();

        // Get variables for graph setup
        gm.InitGraph(gs.GraphSetup());

        // Create graph based on init variables
        gm.CreateGraph();

        // Set startnode ( and endnode)
        SetupImportantNodes();

        // Init teaching mode
        switch (gs.TeachingMode)
        {
            case Util.DEMO: PerformAlgorithmDemo(); break;
            case Util.STEP_BY_STEP: PerformAlgorithmStepByStep(); break;
            case Util.USER_TEST: PerformAlgorithmUserTest(); break;
            default: Debug.LogError("Teaching mode '" + gs.TeachingMode + "' unknown."); break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (graphAlgorithm.IsTaskCompleted && !backtracking)
        {
            Debug.Log("Starting backtracking");

            if (graphAlgorithm is IShortestPath)
            {
                if (graphAlgorithm.ShortestPathOneToAll)
                    StartCoroutine(gm.BacktrackShortestPathsAll(graphAlgorithm.Seconds));
                else
                    StartCoroutine(gm.BacktrackShortestPath(endNode, graphAlgorithm.Seconds));
                backtracking = true;
            }
        }
        else if (gs.IsDemo())
        {

        }
        else if (gs.IsUserTest())
        {
            // First check if user test setup is complete
            if (userTestManager.HasInstructions() && !beginnerWait)
            {
                //// Check if user has done a move, and is ready for next round
                //if (elementManager.CurrentMoving != null)
                //{
                //    // Dont do anything while moving element
                //} else if
                if (userTestManager.ReadyForNext == userTestManager.UserActionToProceed)
                {
                    Debug.Log("Ready for next instruction");
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
                        Debug.Log("Go for next instruction");
                        // Still some elements not sorted, so go on to next round
                        bool hasInstruction = userTestManager.IncrementToNextInstruction();

                        // Hot fix - solve in some other way?
                        if (hasInstruction)
                            userTestManager.ReadyForNext += PrepareNextInstruction(userTestManager.GetInstruction());
                        //else if (elementManager.AllSorted())
                        //    StartCoroutine(FinishUserTest());

                    }
                }
                // displayUnitManager.BlackBoard.ChangeText(displayUnitManager.BlackBoard.TextIndex, userTestManager.FillInBlackboard());
            }
        }
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

    private void SetupImportantNodes()
    {
        int[] startNodeCell = gs.StartNode();
        startNode = gm.GetNode(startNodeCell[0], startNodeCell[1]);
        startNode.IsStartNode = true;
        int[] endNodeCell = gs.EndNode();
        Node endNode = gm.GetNode(endNodeCell[0], endNodeCell[1]);
        endNode.IsEndNode = true;
    }

    public override void PerformAlgorithmDemo()
    {
        switch (algorithmName)
        {
            case Util.BFS: StartCoroutine(((BFS)graphAlgorithm).Demo(startNode)); break;

            case Util.DFS:
                StartCoroutine(((DFS)graphAlgorithm).Demo(startNode));
                break;

            case Util.DFS_RECURSIVE:
                //algorithm.ListVisual.AddListObject(GetNode(startNode[0], startNode[1]).NodeAlphaID);
                StartCoroutine(((DFS)graphAlgorithm).DemoRecursive(startNode));
                break;

            case Util.DIJKSTRA:
                StartCoroutine(((Dijkstra)graphAlgorithm).Demo(startNode, endNode));
                //StartCoroutine(((Dijkstra)algorithm).DemoNoPseudocode(startNode, endNode));
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
        Dictionary<int, InstructionBase> instructions = ((BFS)graphAlgorithm).UserTestInstructions(startNode);

        // Initialize user test
        int movesNeeded = 1;
        userTestManager.InitUserTest(instructions, movesNeeded, FindNumberOfUserAction(instructions));

        // Set start time
        userTestManager.SetStartTime();

        if (gs.UseAlgorithm == Util.BFS)
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

        gm.ResetGraph();
        userTestReady = true;
        Debug.Log("Ready for user test!");
    }

    protected override int PrepareNextInstruction(InstructionBase instruction)
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

            // Give this sorting element permission to give feedback to progress to next intstruction
            if (instruction.Instruction == UtilGraph.DEQUEUE_NODE_INST)
                node.NextMove = true;
        }

        // Display help on blackboard
        if (true) //algorithmSettings.Difficulty <= UtilSort.BEGINNER)
        {
            Debug.Log("Fixing pseudocode!");
            BeginnerWait = true;
            StartCoroutine(graphAlgorithm.UserTestHighlightPseudoCode(instruction, gotNode && !noDestination));

            // Node representation
            switch (instruction.Instruction)
            {
                case UtilGraph.ENQUEUE_NODE_INST: graphAlgorithm.ListVisual.AddListObject(node); break;
                case UtilGraph.DEQUEUE_NODE_INST: graphAlgorithm.ListVisual.RemoveCurrentNode(); break;
                case UtilGraph.END_FOR_LOOP_INST: graphAlgorithm.ListVisual.DestroyCurrentNode(); break;
            }
        }

        Debug.Log("Got node: " + gotNode + ", no destination: " + noDestination);
        if (gotNode && !noDestination)
            return 0;
        Debug.Log("Nothing to do for player, get another instruction");
        return 1;
    }

}
