using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

    protected Color currentColor;
    protected Animator animator;
    protected TextMeshPro textNodeID, textNodeDist;

    // Instruction variables
    protected int userMove = 0, validatedUserMove = 0;
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

    private void Awake()
    {
        animator = GetComponent<Animator>();

        Component[] textHolders = GetComponentsInChildren(typeof(TextMeshPro));
        textNodeID = textHolders[0].GetComponent<TextMeshPro>();
        textNodeDist = textHolders[1].GetComponent<TextMeshPro>();

        nodeID = NODE_ID++;
        NodeAlphaID = UtilGraph.ConvertIDToAlphabet(nodeID);
        name = NodeType + nodeID + "(" + nodeAlphaID + ")";
        ResetNode();
    }

    protected void InitNode(string algorithm)
    {
        SetNodeTextID(true);

        this.algorithm = algorithm;
        if (algorithm == Util.DIJKSTRA)
            Dist = UtilGraph.INF;
        else
            textNodeDist.text = "";
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
        set { isStartNode = value; }
    }

    public bool IsEndNode
    {
        get { return isEndNode; }
        set { isEndNode = value; }
    }
       
    public int Dist
    {
        get { return dist; }
        set { dist = value; UpdateTextNodeDist(value); }
    }

    private void UpdateTextNodeDist(int newDist)
    {
        textNodeDist.text = UtilGraph.ConvertIfInf(newDist);
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
        set { currentColor = value; GetComponentInChildren<Renderer>().material.color = value; }
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

            if (nodeInstruction.Visited)
                Visited = true;
            else
                Visited = false;

            //if (nodeInstruction.Traversed)
            //    Traversed = true;
            //else
            //    Traversed = false;

        }
    }

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
                string validation = IsCorrectlyPlaced();
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

    protected virtual string IsCorrectlyPlaced()
    {
        return "";
    }

    // Instruction methods end

    private WaitForSeconds selectedDuration = new WaitForSeconds(0.5f);
    public IEnumerator PlayerSelectedNode()
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
