using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialEdge : MonoBehaviour {

    [SerializeField]
    private TutorialNode node1, node2;

    [SerializeField]
    private bool isDirectedEdge;

    private void Awake()
    {
        node1.Edge = this;
        node2.Edge = this;
    }

    public TutorialNode OtherNode(TutorialNode node)
    {
        if (!isDirectedEdge)
            return (node1 == node) ? node2 : node1;
        return (node1 == node) ? node2 : null;
    }

}
