using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(Animator))]
public class TutorialNode : MonoBehaviour {

    private bool visited, traversed;
    private bool currentTraversing;

    private List<TutorialEdge> edges;

    private TutorialEdge prevEdge;
    
    private TextMeshPro textNodeID;

    private GameObject player;
    private PathObstacle obstacle;
    private Animator animator;


    private void Awake()
    {
        player = FindObjectOfType<Player>().GetComponentInChildren<Camera>().gameObject;
        obstacle = GetComponentInParent<PathObstacle>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        //Vector3 pos = player.transform.position;
        //textNodeID.transform.LookAt(2 * textNodeID.transform.position - pos);
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
        set { visited = value; UpdateNodeStatus(UtilGraph.NODE_VISITED, value); }
    }

    public bool Traversed
    {
        get { return traversed; }
        set { traversed = value; UpdateNodeStatus(UtilGraph.NODE_TRAVERSED, value); }
    }

    public void ChangeAppearance(Color color)
    {
        GetComponentInChildren<Renderer>().material.color = color;
    }

    private void UpdateNodeStatus(string action, bool value)
    {
        switch (action)
        {
            case UtilGraph.NODE_VISITED:
                animator.SetBool("NodeTraverse", value);
                ChangeAppearance(UtilGraph.VISITED_COLOR);

                if (prevEdge != null)
                    prevEdge.ChangeAppearance(UtilGraph.VISITED_COLOR);
                break;

            case UtilGraph.NODE_TRAVERSED:
                ChangeAppearance(UtilGraph.TRAVERSED_COLOR);
                obstacle.ReportSubTaskCleared();

                // Give a hint which nodes can be shot
                foreach (TutorialEdge edge in edges)
                {
                    TutorialNode otherNode = edge.OtherNode(this);

                    if (!otherNode.Visited)
                        otherNode.Visited = true;
                }

                break;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.name == Util.HEAD_COLLIDER)
        {
            currentTraversing = true;

            if (visited)
            {
                Traversed = true;
                if (prevEdge != null)
                    prevEdge.ChangeAppearance(UtilGraph.TRAVERSED_COLOR);
            }
            else
                animator.Play("NodeError");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == Util.HEAD_COLLIDER)
        {
            currentTraversing = false;

        }
    }
}
