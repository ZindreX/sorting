using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Valve.VR.InteractionSystem;

public abstract class Node : MonoBehaviour, IComparable<Node>, IInstructionAble {

    // Counter for number of active nodes
    public static int NODE_ID;

    // Basic 
    [Header("Basic")]
    [SerializeField]
    protected int nodeID;
    protected char nodeAlphaID;
    protected string algorithm;
    protected bool isStartNode, isEndNode;

    protected Color currentColor, prevColor;
    protected TextMeshPro textNodeID, textNodeDist;
    protected Animator animator;

    private WaitForSeconds selectedDuration = new WaitForSeconds(0.5f);

    // Instruction variables
    private bool shootStatus, traverseStatus;
    protected int userMove = 0, validatedUserMove = 0;
    protected bool visitNextMove, traverseNextMove;
    protected bool nextMove;
    protected InstructionBase nodeInstruction; // TraverseIntruction


    // Traversal / Shortest path variables
    [Space(2)]
    [Header("Traversal / Shortest path")]
    [SerializeField]
    protected int dist;
    private bool traversed, visited; // need traversed? just check if in list/stack...

    [SerializeField]
    private Edge currentEdge;

    [SerializeField]
    private Edge prevEdge;

    [SerializeField]
    protected List<Edge> edges;

    // Position handling
    private Camera playerCamera;
    private Vector3 nodePosition;
    private float withinNode = 0.6f;

    private PositionManager positionManager;
    private GraphMain graphMain;
    private AudioManager audioManager;

    // Debugging
    #region Debugging variables:
    [Space(5)]
    [Header("Debugging")]
    [SerializeField]
    protected string instruction;
    [SerializeField]
    protected string status;

    [SerializeField]
    protected bool nextNode;
    #endregion

    protected void InitNode(string algorithm)
    {
        SetNodeTextID(true);

        this.algorithm = algorithm;
        Dist = UtilGraph.INIT_NODE_DIST;
    }

    private void Awake()
    {
        // Set ID's
        nodeID = NODE_ID++;
        NodeAlphaID = UtilGraph.ConvertIDToAlphabet(nodeID);

        // Name shown in the editor
        name = NodeType + nodeID + "(" + nodeAlphaID + ")";

        // Not used
        animator = GetComponent<Animator>();

        // Get textmesh pros
        Component[] textHolders = GetComponentsInChildren(typeof(TextMeshPro));
        textNodeID = textHolders[0].GetComponent<TextMeshPro>();
        textNodeDist = textHolders[1].GetComponent<TextMeshPro>();


        // Find player object and its camera
        playerCamera = FindObjectOfType<Player>().gameObject.GetComponentInChildren<Camera>();
        positionManager = FindObjectOfType<PositionManager>();
        graphMain = FindObjectOfType<GraphMain>();
        audioManager = FindObjectOfType<AudioManager>();

        // Node position
        nodePosition = transform.position;

        // "Set"
        edges = new List<Edge>();
        ResetNode();
    }

    private void Update()
    {
        if (playerCamera != null && positionManager.ReportedNode != this)
        {
            Vector3 playerPos = playerCamera.transform.position;

            // >>> Rotate text (ID & dist) according to player position, making it readable
            textNodeID.transform.LookAt(2 * textNodeID.transform.position - playerPos);
            textNodeDist.transform.LookAt(2 * textNodeDist.transform.position - playerPos);

            // >>> Check if player is standing on this node
            float playerRelToNodeX = Mathf.Abs(playerPos.x - nodePosition.x);
            float playerRelToNodeZ = Mathf.Abs(playerPos.z - nodePosition.z);

            // If so report it
            if (playerRelToNodeX < withinNode && playerRelToNodeZ < withinNode)
            {
                // Hide text
                textNodeDist.text = "";
                textNodeID.text = "";

                // Perform user move first
                if (positionManager.ReportedNode != this)
                {
                    // Report 
                    positionManager.ReportPlayerOnNode(this);
                    // Perform user move (traverse)
                    PerformUserMove(UtilGraph.NODE_TRAVERSED);
                }
            }
        }
    }

    public int NodeID
    {
        get { return nodeID; }
    }

    public char NodeAlphaID
    {
        get { return nodeAlphaID; }
        set { nodeAlphaID = value; }
    }

    private void SetNodeTextID(bool useAlpha)
    {
        if (useAlpha)
            textNodeID.text = UtilGraph.ConvertIDToAlphabet(nodeID).ToString();
        else
            textNodeID.text = nodeID.ToString();
    }

    // Used by position manager to turn on text again
    public void DisplayNodeInfo()
    {
        SetNodeTextID(true);
        textNodeDist.text = UtilGraph.ConvertDist(dist);
    }

    public bool IsStartNode
    {
        get { return isStartNode; }
        set { isStartNode = value; StartCoroutine(PlayerSelectedNode()); }
    }

    public bool IsEndNode
    {
        get { return isEndNode; }
        set { isEndNode = value; StartCoroutine(PlayerSelectedNode()); }
    }
    
    // Remember to convert through Utilgraph.ConvertDist
    public int Dist
    {
        get { return dist; }
        set { dist = value; StartCoroutine(UpdateTextNodeDist(value)); }
    }

    private IEnumerator UpdateTextNodeDist(int newDist)
    {
        textNodeDist.text = UtilGraph.ConvertDist(newDist);

        for (int i = 0; i < 4; i++)
        {
            textNodeDist.color = currentColor;
            yield return selectedDuration;
            textNodeDist.color = UtilGraph.STANDARD_COST_TEXT_COLOR;
            yield return selectedDuration;
        }
    }

    public bool Traversed
    {
        get { return traversed; }
        set { traversed = value;
            if (value)
            {
                CurrentColor = UtilGraph.TRAVERSED_COLOR;
                audioManager.Play("Traverse");
            }
            else
                CurrentColor = UtilGraph.TRAVERSE_COLOR;
        }

    }

    public bool Visited
    {
        get { return visited; }
        set
        {
            visited = value;
            if (value)
            {
                CurrentColor = UtilGraph.VISITED_COLOR;
                audioManager.Play("Visit");
            }
            else
                CurrentColor = Util.STANDARD_COLOR;
        }
    }

    public Color CurrentColor
    {
        get { return currentColor; }
        set { prevColor = currentColor; currentColor = value; ChangeApperance(value); }
    }

    public Color PrevColor
    {
        get { return prevColor; }
    }

    private void ChangeApperance(Color color)
    {
        GetComponentInChildren<Renderer>().material.color = color;
        if (color == UtilGraph.TRAVERSE_COLOR)
        {
            textNodeID.color = color;
            textNodeDist.color = color;
        }
        else
        {
            textNodeID.color = UtilGraph.STANDARD_COST_TEXT_COLOR;
            textNodeDist.color = UtilGraph.STANDARD_COST_TEXT_COLOR;
        }
    }

    public void DisplayEdgeCost(bool display)
    {
        foreach (Edge edge in edges)
        {
            edge.DisplayCost(display);
        }
    }

    public List<Edge> Edges
    {
        get { return edges; }
    }

    public virtual void AddEdge(Edge edge)
    {
        if (!edges.Contains(edge))
            edges.Add(edge);
    }

    public void RemoveEdge(Edge edge)
    {
        if (edges.Contains(edge))
            edges.Remove(edge);
    }

    public Edge PrevEdge
    {
        get { return prevEdge; }
        set { prevEdge = value; }
    }

    // Obs: inverted [biggest, ..., smallest]
    public int CompareTo(Node other)
    {
        if (dist < other.Dist)
            return 1;
        else if (dist > other.Dist)
            return -1;
        return 0;
    }

    public virtual void ResetNode()
    {
        traversed = false;
        visited = false;
        prevEdge = null;
        Dist = UtilGraph.INIT_NODE_DIST;
        CurrentColor = Util.STANDARD_COLOR;

        //isEndNode = false;
    }

    // --------------------------------------- User test ---------------------------------------

    public bool NextMove
    {
        get { return nextMove; }
        set { nextMove = value; }
    }

    public InstructionBase Instruction
    {
        get { return nodeInstruction; }
        set {
            if (value is TraverseInstruction)
                nodeInstruction = (TraverseInstruction)value;
            else if (value is ShortestPathInstruction)
                nodeInstruction = (ShortestPathInstruction)value;
                
            UpdateNodeState();
        }
    }

    // Update the node status based on the instruction
    protected void UpdateNodeState()
    {
        if (nodeInstruction != null)
        {
            Debug.Log(nodeInstruction.DebugInfo());

            // Debugging
            instruction = nodeInstruction.Instruction;
            switch (instruction)
            {
                case Util.INIT_INSTRUCTION: status = "Init pos"; break;
                case UtilGraph.ENQUEUE_NODE_INST: // BFS
                case UtilGraph.PUSH_INST: // DFS
                case UtilGraph.ADD_NODE: // Dijkstra
                case UtilGraph.VISIT_CONNECTED_NODE: // Dijkstra <- // PRIORITY_ADD_NODE ?
                    status = "Shot node";
                    break;

                case UtilGraph.DEQUEUE_NODE_INST: // BFS
                case UtilGraph.POP_INST: // DFS
                case UtilGraph.PRIORITY_REMOVE_NODE: // Dijkstra
                    status = "Move to node";
                    break;

                case UtilGraph.BACKTRACK: status = "Backtracking moving from end to start node"; break;

                case Util.EXECUTED_INST: status = Util.EXECUTED_INST; break;
                default: Debug.Log(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> UpdateNodeState(): Add '" + instruction + "' case, or ignore"); break;
            }

            // This node is the next to be visited (shot at)
            if (nodeInstruction is TraverseInstruction)
            {
                TraverseInstruction travInst = (TraverseInstruction)nodeInstruction;

                // If visit (shoot at node)
                if (travInst.VisitInst)
                    visitNextMove = true;

                // This node is the next to be traversed (player move to node)
                if (travInst.TraverseInst)
                    traverseNextMove = true;

                // Set the edge between the node we traversed from to reach this node
                if (travInst.PrevEdge != null)
                {
                    switch (algorithm)
                    {
                        case Util.BFS: case Util.DFS: prevEdge = travInst.PrevEdge; break;
                        case Util.DIJKSTRA:
                            currentEdge = travInst.PrevEdge; break;
                    }
                }
            }
            else if (nodeInstruction is ShortestPathInstruction)
            {
                ShortestPathInstruction spInst = (ShortestPathInstruction)nodeInstruction;

                switch (spInst.Instruction)
                {
                    case UtilGraph.UPDATE_CONNECTED_NODE_DIST:
                        if (spInst.CurrentNode == this)
                            spInst.ConnectedNode.Dist = spInst.ConnectedNodeNewDist;
                        else if (spInst.ConnectedNode == this)
                            Dist = spInst.ConnectedNodeNewDist;
                        break;

                    case UtilGraph.UPDATE_CONNECTED_NODE_PREV_EDGE:
                        if (spInst.CurrentNode == this)
                            spInst.ConnectedNode.PrevEdge = spInst.PrevEdge;
                        else if (spInst.ConnectedNode == this)
                            prevEdge = spInst.PrevEdge;

                        break;
                }

                //if (spInst.PrevEdge != null)
                //    PrevEdge = spInst.PrevEdge;
            }
        }
    }

    public void PerformUserMove(string userAction)
    {
        userMove++;

        if (validatedUserMove < userMove)
        {
            string validation = "";
            switch (userAction)
            {
                case UtilGraph.NODE_VISITED: validation = IsCorrectlyShot(); break;
                case UtilGraph.NODE_TRAVERSED: validation = IsCorrectlyTraversed(); break;
                default: Debug.LogError("Invalid user move: " + userAction); break;
            }

            switch (validation)
            {
                case Util.INIT_OK:
                    shootStatus = true; // ??
                    break;

                case UtilGraph.NODE_VISITED:
                    shootStatus = true;

                    // Mark as visited, and change color of visited node & edge leading to this node
                    Visited = true;

                    if (algorithm == Util.DIJKSTRA)
                    {
                        if (currentEdge != null)
                            currentEdge.CurrentColor = UtilGraph.TRAVERSE_COLOR;
                    }
                    else
                    {
                        if (prevEdge != null)
                            prevEdge.CurrentColor = UtilGraph.VISITED_COLOR;
                    }

                    //graphMain.UpdateListVisual(UtilGraph.ADD_NODE, this, Util.NO_VALUE);
                    break;

                case UtilGraph.NODE_TRAVERSED:
                    traverseStatus = true;

                    // Display the edge cost of each edge connected to this node
                    DisplayEdgeCost(true);

                    // Mark as traversed, and change color of visited node & edge leading to this node
                    Traversed = true;
                    if (prevEdge != null && algorithm != Util.DIJKSTRA)
                        prevEdge.CurrentColor = UtilGraph.TRAVERSED_COLOR;

                    //graphMain.UpdateListVisual(UtilGraph.REMOVE_CURRENT_NODE, this, Util.NO_VALUE);
                    break;

                case UtilGraph.NODE_BACKTRACKED:
                    if (prevEdge != null)
                        prevEdge.CurrentColor = UtilGraph.SHORTEST_PATH_COLOR;

                    traverseStatus = true;
                    CurrentColor = UtilGraph.SHORTEST_PATH_COLOR;
                    break;

                case Util.INIT_ERROR:
                case UtilGraph.NODE_ERROR:
                    shootStatus = false;
                    traverseStatus = false;
                    graphMain.GetComponent<UserTestManager>().Mistake();
                    break;

                default: Debug.Log("'Add '" + validation + "' case, or ignore"); break;
            }
        }

        if (shootStatus && userAction == UtilGraph.NODE_VISITED || traverseStatus && userAction == UtilGraph.NODE_TRAVERSED)
        {
            status = Util.EXECUTED_INST;
            
            // Reset
            shootStatus = false;
            traverseStatus = false;

            if (NextMove)
            {
                graphMain.GetComponent<UserTestManager>().ReadyForNext += 1;
                graphMain.GetComponent<UserTestManager>().IncrementTotalCorrect();

                // Reset
                NextMove = false;
                visitNextMove = false;
                traverseNextMove = false;
            }
        }
        validatedUserMove++;
    }

    // Check whether the player visited (shot) the correct node
    private string IsCorrectlyShot()
    {
        if (nodeInstruction != null)
        {
            switch (nodeInstruction.Instruction)
            {
                case Util.INIT_INSTRUCTION:
                    return (!visitNextMove && !traverseNextMove) ? Util.INIT_OK : Util.INIT_ERROR;

                case UtilGraph.ENQUEUE_NODE_INST:
                case UtilGraph.PUSH_INST:
                case UtilGraph.ADD_NODE:
                case UtilGraph.VISIT_CONNECTED_NODE:
                    return visitNextMove ? UtilGraph.NODE_VISITED : UtilGraph.NODE_ERROR;

                default: return UtilGraph.NODE_ERROR;
            }
        }
        return UtilSort.CANNOT_VALIDATE_ERROR;
    }

    // Check whether the player traversed (teleported) the correct node
    private string IsCorrectlyTraversed()
    {
        if (nodeInstruction != null)
        {
            switch (nodeInstruction.Instruction)
            {
                case Util.INIT_INSTRUCTION:
                    return (!visitNextMove && !traverseNextMove) ? Util.INIT_OK : Util.INIT_ERROR;

                case UtilGraph.DEQUEUE_NODE_INST:
                case UtilGraph.POP_INST:
                case UtilGraph.PRIORITY_REMOVE_NODE:
                    return traverseNextMove ? UtilGraph.NODE_TRAVERSED : UtilGraph.NODE_ERROR;

                case UtilGraph.BACKTRACK:
                    return traverseNextMove ? UtilGraph.NODE_BACKTRACKED : UtilGraph.NODE_ERROR;

                default: return UtilGraph.NODE_ERROR;
            }
        }
        return UtilSort.CANNOT_VALIDATE_ERROR;
    }

    // Instruction methods end

    // Visualization
    private IEnumerator PlayerSelectedNode()
    {
        for (int i=0; i < 4; i++)
        {
            CurrentColor = Color.white;
            yield return selectedDuration;
            CurrentColor = Util.STANDARD_COLOR;
            yield return selectedDuration;
        }
    }


    // Abstract methods

    public abstract string NodeType { get; }

  

}
