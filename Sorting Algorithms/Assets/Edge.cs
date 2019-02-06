using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edge : MonoBehaviour {

    private int cost;
    private float angle;
    private Color currentColor;

    public Edge(int cost, Node node1, Node node2)
    {
        this.cost = cost;
        SetAngle(angle);
        CurrentColor = Util.STANDARD_COLOR;
    }

    private void SetAngle(float angle)
    {
        transform.Rotate(0f, angle, 0f);
    }

    public Color CurrentColor
    {
        get { return currentColor; }
        set { currentColor = value; GetComponentInChildren<Renderer>().material.color = value; }
    }
}
