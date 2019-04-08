using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Valve.VR.InteractionSystem;

public class TutorialNode : MonoBehaviour {

    /* ------------------------------------- Tutorial Node -------------------------------------
     * Use idea for future?
     * - Creating instruction on the spot (visit order up to the user to choose)
     * - Traverse order according to algorithm
     * - To be optimized later
     * 
     * 
    */

    private string nodeID;

    private bool visited, traversed;
    private bool currentTraversing;

    private bool visitNextMove, traverseNextMove;
    private InstructionBase instruction;

    private List<TutorialEdge> edges;
    private TutorialEdge prevEdge;
    
    private TextMeshPro textNodeID;

    private GameObject player;
    private TutorialGraphTask tutorialTask;
    private Animator animator;


    private void Awake()
    {
        edges = new List<TutorialEdge>();
        player = FindObjectOfType<Player>().GetComponentInChildren<Camera>().gameObject;
        tutorialTask = GetComponentInParent<TutorialGraphTask>();
        animator = GetComponentInChildren<Animator>();

        textNodeID = GetComponentInChildren<TextMeshPro>();
        nodeID = textNodeID.text;

        Instruction = new InstructionBase(Util.INIT_INSTRUCTION, Util.NO_INSTRUCTION_NR);
    }

    private void Update()
    {
        Vector3 pos = player.transform.position;
        textNodeID.transform.LookAt(2 * textNodeID.transform.position - pos);
    }

    public string NodeID
    {
        get { return nodeID; }
    }

    public void AddEdge(TutorialEdge edge)
    {
        if (!edges.Contains(edge))
        {
            edges.Add(edge);
        }
    }

    public List<TutorialEdge> Edges
    {
        get { return edges; }
    }

    public TutorialEdge PrevEdge
    {
        get { return prevEdge; }
        set { prevEdge = value; }
    }

    public bool CurrentTraversing
    {
        get { return currentTraversing; }
    }

    public bool Visited
    {
        get { return visited; }
        set { visited = value; ChangeAppearance(UtilGraph.VISITED_COLOR); }
    }

    public bool AllMyNeighborsVisited()
    {
        foreach (TutorialEdge edge in edges)
        {
            TutorialNode otherNode = edge.OtherNode(this);
            if (!otherNode.Visited)
                return false;
        }
        return true;
    }

    public bool Traversed
    {
        get { return traversed; }
        set { traversed = value; ChangeAppearance(UtilGraph.TRAVERSED_COLOR); }
    }

    public void ChangeAppearance(Color color)
    {
        GetComponentInChildren<Renderer>().material.color = color;
    }

    public InstructionBase Instruction
    {
        get { return instruction; }
        set { instruction = value; UpdateNodeStatus(value); }
    }

    public const string NODE_VISIT_HIGHLIGHT = "Node visit highlight", NODE_TRAVERSE_HIGHLIGHT = "Node traverse highlight";
    public static TraverseInstruction visitHighlightInstruction = new TraverseInstruction(NODE_VISIT_HIGHLIGHT, Util.NO_INSTRUCTION_NR, null, true, false);
    public static TraverseInstruction traverseHighlightInstruction = new TraverseInstruction(NODE_TRAVERSE_HIGHLIGHT, Util.NO_INSTRUCTION_NR, null, false, true);


    private void UpdateNodeStatus(InstructionBase instruction)
    {
        if (instruction is TraverseInstruction)
        {
            TraverseInstruction travInst = (TraverseInstruction)instruction;

            // If visit (shoot at node)
            if (travInst.VisitInst)
            {
                visitNextMove = true;
                animator.SetBool(UtilGraph.NODE_VISIT_ANIMATION, true);
            }

            // This node is the next to be traversed (player move to node)
            if (travInst.TraverseInst)
            {
                traverseNextMove = true;
                animator.SetBool(UtilGraph.NODE_TRAVERSE_ANIMATION, true);
            }
        }
    }

    public void PerformUserAction(string userAction)
    {
        Debug.Log("User action: " + userAction);

        // Check if first node
        if (!tutorialTask.undirectedGraphStarted)
        {
            Visited = true;
            Instruction = traverseHighlightInstruction;
            tutorialTask.undirectedGraphStarted = true;
            return;
        }

        string validation = "";
        switch (userAction)
        {
            case UtilGraph.NODE_VISITED: validation = IsCorrectlyShot(); break;
            case UtilGraph.NODE_TRAVERSED: validation = IsCorrectlyTraversed(); break;
            default: Debug.LogError("Invalid user move: " + userAction); break;
        }

        Debug.Log("Validation: " + validation);

        switch (validation)
        {
            case UtilGraph.NODE_VISITED:
                animator.SetBool(UtilGraph.NODE_VISIT_ANIMATION, false);
                Visited = true;
                FindEdgeAndChangeAppearance(UtilGraph.VISITED_COLOR, true);

                visitNextMove = false;
                Instruction = traverseHighlightInstruction;
                break;

            case UtilGraph.NODE_TRAVERSED:
                animator.SetBool(UtilGraph.NODE_TRAVERSE_ANIMATION, false);

                Traversed = true;

                if (prevEdge != null)
                    prevEdge.ChangeAppearance(UtilGraph.TRAVERSED_COLOR);

                // Give a hint which nodes can be shot
                foreach (TutorialEdge edge in edges)
                {
                    TutorialNode otherNode = edge.OtherNode(this);

                    if (otherNode != null && !otherNode.Visited)
                        otherNode.Instruction = visitHighlightInstruction;
                }


                traverseNextMove = false;
                tutorialTask.Progress();
                break;

            case UtilGraph.NODE_ERROR:
                if (traversed)
                    return;

                animator.Play(UtilGraph.NODE_ERROR_ANIMATION);
                break;
        }
    }

    // Check whether the player visited (shot) the correct node
    private string IsCorrectlyShot()
    {
        if (instruction != null)
        {
            switch (instruction.Instruction)
            {
                case Util.INIT_INSTRUCTION:
                    return UtilGraph.NODE_ERROR;

                case NODE_VISIT_HIGHLIGHT:
                    return visitNextMove ? UtilGraph.NODE_VISITED : UtilGraph.NODE_ERROR;

                default: return UtilGraph.NODE_ERROR;
            }
        }
        return UtilSort.CANNOT_VALIDATE_ERROR;
    }

    // Check whether the player traversed (teleported) the correct node
    private string IsCorrectlyTraversed()
    {
        if (instruction != null)
        {
            switch (instruction.Instruction)
            {
                case Util.INIT_INSTRUCTION:
                    return UtilGraph.NODE_ERROR;

                case NODE_TRAVERSE_HIGHLIGHT:
                    return traverseNextMove ? UtilGraph.NODE_TRAVERSED : UtilGraph.NODE_ERROR;

                default: return UtilGraph.NODE_ERROR;
            }
        }
        return UtilSort.CANNOT_VALIDATE_ERROR;
    }

    private void FindEdgeAndChangeAppearance(Color color, bool setPrevEdge)
    {
        // Get edge leading from the node we just shot back to the current node
        foreach (TutorialEdge edge in edges)
        {
            if (edge.PlayerCurrentlyAtOtherNode(this))
            {
                edge.ChangeAppearance(color);

                if (setPrevEdge)
                    prevEdge = edge;

                break;
            }
        }
    }






    private void OnTriggerEnter(Collider other)
    {
        if (other.name == Util.HEAD_COLLIDER)
        {
            currentTraversing = true;
            PerformUserAction(UtilGraph.NODE_TRAVERSED);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == Util.HEAD_COLLIDER)
        {
            currentTraversing = false;

        }
    }


    public void ResetNode()
    {
        ChangeAppearance(Util.STANDARD_COLOR);
        Instruction = new InstructionBase(Util.INIT_INSTRUCTION, Util.NO_INSTRUCTION_NR);

        visited = false;
        traversed = false;
        visitNextMove = false;
        traverseNextMove = false;

        animator.SetBool(UtilGraph.NODE_VISIT_ANIMATION, false);
        animator.SetBool(UtilGraph.NODE_TRAVERSE_ANIMATION, false);

        prevEdge = null;
    }
}
