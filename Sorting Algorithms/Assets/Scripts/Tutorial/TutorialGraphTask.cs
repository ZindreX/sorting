using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialGraphTask : TutorialTask {

    [HideInInspector]
    public bool undirectedGraphStarted, directedGraphStarted;

    [SerializeField]
    private TutorialPointer pointer;

    private TutorialNode[] nodes;
    private TutorialEdge[] edges;

    private void Start()
    {
        nodes = GetComponentsInChildren<TutorialNode>();
        edges = GetComponentsInChildren<TutorialEdge>();
    }

    public override void InitTask()
    {
        pointer.AllowShooting = true;
    }

    public override void ResetTask()
    {
        base.ResetTask();

        undirectedGraphStarted = false;
        directedGraphStarted = false;

        foreach (TutorialNode node in nodes)
        {
            node.ResetNode();
        }

        foreach (TutorialEdge edge in edges)
        {
            edge.ResetEdge();
        }
    }

    public override void StopTask()
    {
        pointer.AllowShooting = false;
    }

}
