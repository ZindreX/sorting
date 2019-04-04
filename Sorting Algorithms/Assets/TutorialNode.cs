using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class TutorialNode : MonoBehaviour {

    private PathObstacle obstacle;
    private Animator animator;

    [SerializeField]
    private bool startNode;

    private bool visited, traversed;

    private TutorialEdge edge;

    private void Awake()
    {
        obstacle = GetComponentInParent<PathObstacle>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        if (startNode)
            Visited = true;
    }

    public TutorialEdge Edge
    {
        set { edge = value; }
    }

    public bool Visited
    {
        get { return visited; }
        set { visited = value; animator.SetBool("NodeVisit", value); obstacle.ReportSubTaskCleared(); }
    }

    public bool Traversed
    {
        get { return traversed; }
        set { traversed = value; animator.SetBool("NodeTraverse", value); obstacle.ReportSubTaskCleared(); }
    }


}
