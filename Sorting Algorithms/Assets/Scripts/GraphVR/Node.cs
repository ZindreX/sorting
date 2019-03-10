﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Valve.VR.InteractionSystem;

public abstract class Node : MonoBehaviour, IComparable<Node>, IInstructionAble {

    // Counter for number of active nodes
    public static int NODE_ID;
    public static bool nodeSetup = false, startNodeChosen = false, endNodeChosen = false;

    // Basic 
    [Header("Basic")]
    [SerializeField]
    protected int nodeID;
    protected char nodeAlphaID;
    protected string algorithm;
    protected bool isStartNode, isEndNode;

    protected Color currentColor;
    protected TextMeshPro textNodeID, textNodeDist;
    protected Animator animator;

    private WaitForSeconds selectedDuration = new WaitForSeconds(0.5f);

    // Instruction variables
    protected int userMove = 0, validatedUserMove = 0;
    protected bool visitNextMove, traverseNextMove;
    protected bool nextMove;
    protected TraverseInstruction nodeInstruction;


    // Traversal / Shortest path variables
    [Space(2)]
    [Header("Traversal / Shortest path")]
    [SerializeField]
    protected int dist;
    private bool traversed, visited; // need traversed? just check if in list/stack...

    [SerializeField]
    private Edge prevEdge;

    [SerializeField]
    protected List<Edge> edges;

    // Position handling
    private Camera playerCamera;
    private Vector3 nodePosition;
    private float withinNode = 0.6f;
    private PositionManager positionManager;

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
        if (algorithm == Util.DIJKSTRA)
            Dist = UtilGraph.INF;
        else
            textNodeDist.text = "";
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

        // Node position
        nodePosition = transform.position;

        // "Set"
        ResetNode();
    }

    private void Update()
    {
        if (playerCamera != null)
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
                positionManager.ReportPlayerOnNode(this);
                PerformUserTraverse();
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

    public bool IsStartNode
    {
        get { return isStartNode; }
        set { isStartNode = value; startNodeChosen = true; StartCoroutine(PlayerSelectedNode()); }
    }

    public bool IsEndNode
    {
        get { return isEndNode; }
        set { isEndNode = value; endNodeChosen = true; StartCoroutine(PlayerSelectedNode()); }
    }
       
    public int Dist
    {
        get { return dist; }
        set { dist = value; StartCoroutine(UpdateTextNodeDist(value)); }
    }

    private IEnumerator UpdateTextNodeDist(int newDist)
    {
        textNodeDist.text = UtilGraph.ConvertIfInf(newDist);

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
        set { traversed = value; CurrentColor = UtilGraph.TRAVERSED_COLOR; }
    }

    public bool Visited
    {
        get { return visited; }
        set { visited = value; CurrentColor = UtilGraph.VISITED_COLOR; }
    }

    public Color CurrentColor
    {
        get { return currentColor; }
        set { currentColor = value; ChangeApperance(value); }
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
        edges = new List<Edge>();
        traversed = false;
        visited = false;
        //TotalCost = UtilGraph.INF;
        prevEdge = null;
        CurrentColor = UtilGraph.STANDARD_COLOR;
        isEndNode = false;
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
        set { nodeInstruction = (TraverseInstruction)value; UpdateNodeState(); }
    }

    private bool shootStatus, traverseStatus;
    public void PerformUserShoot()
    {
        if (nodeSetup)
        {
            if (!startNodeChosen)
                IsStartNode = true;
            else if (!endNodeChosen && !isStartNode)
                IsEndNode = true;
        }
        else
        {
            userMove++;

            if (validatedUserMove < userMove)
            {
                string validation = IsCorrectlyShot();
                switch (validation)
                {
                    case Util.INIT_OK:
                        shootStatus = true;
                        break;

                    case UtilGraph.NODE_VISITED:
                        shootStatus = true;
                        // usertestmanager
                        break;

                    case Util.INIT_ERROR: case UtilGraph.NODE_ERROR:
                        shootStatus = false;
                        // score mistake
                        break;

                    default: Debug.Log("'Add '" + validation + "' case, or ignore"); break;
                }

                if (shootStatus)
                {
                    status = Util.EXECUTED_INST;

                    if (NextMove)
                    {
                        // Usertestmanager readyfornext += 1
                        NextMove = false;
                    }
                }
                validatedUserMove++;
            }
        }

    }

    private string IsCorrectlyShot()
    {
        if (nodeInstruction != null)
        {
            switch (nodeInstruction.Instruction)
            {
                case Util.INIT_INSTRUCTION:
                    return (!visitNextMove && !traverseNextMove) ? Util.INIT_OK : Util.INIT_ERROR;

                case UtilGraph.ENQUEUE_NODE_INST:
                    return visitNextMove ? UtilGraph.NODE_VISITED : UtilGraph.NODE_ERROR;

                default: return UtilGraph.NODE_ERROR;
            }
        }
        return UtilSort.CANNOT_VALIDATE_ERROR;
    }

    protected void PerformUserTraverse()
    {
        userMove++;

        if (validatedUserMove < userMove)
        {
            string validation = IsCorrectlyTraversed();
            switch (validation)
            {
                case Util.INIT_OK:
                    traverseStatus = true;
                    break;

                case UtilGraph.NODE_VISITED:
                    traverseStatus = true;
                    // usertestmanager
                    break;

                case Util.INIT_ERROR:
                case UtilGraph.NODE_ERROR:
                    traverseStatus = false;
                    // score mistake
                    break;

                default: Debug.Log("'Add '" + validation + "' case, or ignore"); break;
            }

            if (traverseStatus)
            {
                status = Util.EXECUTED_INST;

                if (NextMove)
                {
                    // Usertestmanager readyfornext += 1
                    NextMove = false;
                }
            }
            validatedUserMove++;
        }
    }

    private string IsCorrectlyTraversed()
    {
        //if (nodeInstruction != null)
        //{
        //    switch (nodeInstruction.Instruction)
        //    {
        //        case Util.INIT_INSTRUCTION:
        //            return (!visitNextMove && !traverseNextMove) ? Util.INIT_OK : Util.INIT_ERROR;

        //        case UtilGraph.ENQUEUE_NODE_INST:
        //            return visitNextMove ? UtilGraph.NODE_VISITED : UtilGraph.NODE_ERROR;

        //        default: return UtilGraph.NODE_ERROR;
        //    }
        //}
        return UtilSort.CANNOT_VALIDATE_ERROR;
    }

    protected void UpdateNodeState()
    {
        if (nodeInstruction != null)
        {
            // Debugging
            instruction = nodeInstruction.Instruction;

            if (instruction == UtilGraph.DEQUEUE_NODE_INST)
                nextNode = true;
            

            switch (instruction)
            {
                case UtilSort.INIT_INSTRUCTION: status = "Init pos"; break;
                case UtilGraph.ENQUEUE_NODE_INST: status = "Enqueue node"; break;
                case UtilGraph.MARK_VISITED_INST: status = "Visit node (mark)"; break;
                case UtilGraph.DEQUEUE_NODE_INST: status = "Dequeue node (traverse)"; break;
                case UtilSort.EXECUTED_INST: status = UtilSort.EXECUTED_INST; break;
                default: Debug.LogError("UpdateNodeState(): Add '" + instruction + "' case, or ignore"); break;
            }

            // This node is the next to be visited (shot at)
            if (nodeInstruction.Visited)
                visitNextMove = true;

            // This node is the next to be traversed (player move to node)
            if (nodeInstruction.Traversed)
                traverseNextMove = true;

        }
    }

    // remove when done
    public void PerformUserMove()
    {
        // Check if the user moved the element to a new holder,         TODO: in case of mistake -> avoid new error when fixing the mistake
        if (true) //holder.HolderID != prevHolderID)
        {
            //CurrentStandingOn = holder;
            userMove++;
            //holder.HasPermission = true;

            if (validatedUserMove < userMove)
            {
                string validation = ""; //IsCorrectlyPlaced();
                switch (validation)
                {
                    case UtilSort.INIT_OK:
                        //standingInCorrectHolder = true;
                        break;

                    case UtilSort.CORRECT_HOLDER:
                        //standingInCorrectHolder = true;
                        //parent.GetComponent<UserTestManager>().IncrementTotalCorrect();
                        break;

                    case UtilSort.INIT_ERROR:
                    case UtilSort.WRONG_HOLDER:
                        //standingInCorrectHolder = false;
                        ////parent.GetComponent<ScoreManager>().Mistake();
                        break;

                    default: Debug.Log("Add '" + validation + "' case, or ignore"); break;
                }

                // Mark instruction as executed if correct
                if (true) //standingInCorrectHolder && !IntermediateMove)
                {
                    //Instruction.Status = Util.EXECUTED_INST;
                    status = UtilSort.EXECUTED_INST; // + "***"; // Debugging

                    // Check if ready for next round
                    if (NextMove)
                    {
                        //parent.GetComponent<UserTestManager>().ReadyForNext += 1;
                        NextMove = false;
                    }
                }
                validatedUserMove++;
            }
        }
        else
            Debug.Log("Back to the same");
            //CurrentStandingOn = holder; // Back to the same
                                        //standingInCorrectHolder = true;
    }


    // Instruction methods end


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
