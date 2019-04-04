﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialEdge : MonoBehaviour {

    [SerializeField]
    private TutorialNode node1, node2;

    [SerializeField]
    private bool isDirectedEdge;

    private void Awake()
    {
        node1.AddEdge(this);
        node2.AddEdge(this);
    }

    public bool IsDirectedEdge
    {
        get { return isDirectedEdge; }
    }

    public TutorialNode OtherNode(TutorialNode node)
    {
        if (!isDirectedEdge)
            return (node1 == node) ? node2 : node1;
        return (node1 == node) ? node2 : null;
    }

    public void ChangeAppearance(Color color)
    {
        GetComponentInChildren<Renderer>().material.color = color;
    }

}
