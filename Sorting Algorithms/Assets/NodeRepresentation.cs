using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MoveObject))]
[RequireComponent(typeof(TextHolder))]
public class NodeRepresentation : MonoBehaviour {

    private MoveObject moveObject;
    private TextHolder textHolder;
    private Node node;
    private float listIndex;

    private void Awake()
    {
        moveObject = GetComponent(typeof(MoveObject)) as MoveObject;
        textHolder = GetComponent(typeof(TextHolder)) as TextHolder;
    }

    public void InitNodeRepresentation(Node node, int index)
    {
        this.node = node;
        ValueChanged(index);
    }

    public Node Node
    {
        get { return node; }
    }

    public void ValueChanged(int newListIndex)
    {
        textHolder.SetSurfaceText(node.NodeAlphaID, node.Dist);

        // Find new y-axis value
        float yPosInList = listIndex - newListIndex;

        // Set new position
        Vector3 pos = transform.position + new Vector3(0f, yPosInList, 0f);
        moveObject.SetDestination(pos);
    }


}
