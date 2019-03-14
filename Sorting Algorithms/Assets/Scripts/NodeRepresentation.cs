using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MoveObject))]
[RequireComponent(typeof(TextHolder))]
public class NodeRepresentation : MonoBehaviour {

    private MoveObject moveObject;
    private TextHolder textHolder;
    private Node node;
    private Color currentColor;

    [SerializeField]
    private int listIndex;

    [SerializeField]
    private Vector3 coordinateIndex;

    private void Awake()
    {
        moveObject = GetComponent(typeof(MoveObject)) as MoveObject;
        textHolder = GetComponent(typeof(TextHolder)) as TextHolder;
        currentColor = Color.white;
    }

    public void InitNodeRepresentation(Node node, int index)
    {
        this.node = node;
        UpdateIndexPosition(index);
        coordinateIndex = transform.position;
        name = "NodeRep " + node.NodeAlphaID + " (" + index + ")";
    }

    public Node Node
    {
        get { return node; }
    }

    public Color CurrentColor
    {
        get { return currentColor; }
        set { currentColor = value; GetComponent<TextHolder>().ChangeColor(value); }
    }

    public int ListIndex
    {
        get { return listIndex; }
        set { listIndex = value; name = "NodeRep " + node.NodeAlphaID + " (" + value + ")"; }
    }

    public IEnumerator HighlightNodeRepresentation(Color color, float seconds)
    {
        textHolder.ChangeColor(color);
        yield return new WaitForSeconds(seconds);
        textHolder.ChangeColor(currentColor);
    }

    public void UpdateSurfaceText(Color color)
    {
        textHolder.SetSurfaceText(node.NodeAlphaID, node.Dist);
        StartCoroutine(HighlightNodeRepresentation(color, 1f));
    }

    public void MoveNodeRepresentation(Vector3 pos)
    {
        moveObject.SetDestination(pos);
    }

    public void UpdateIndexPosition(int newListIndex)
    {
        // Find new y-axis value
        int yPosIndex = newListIndex - listIndex; // ?

        // Update current list index
        listIndex = newListIndex;

        // Set new position
        float yPos = coordinateIndex.y + yPosIndex;
        //Debug.Log("Node " + node.NodeAlphaID + ": list index=" + listIndex + ", coordinate index=" + yPos);
        coordinateIndex = new Vector3(coordinateIndex.x, yPos, coordinateIndex.z);
        MoveNodeRepresentation(coordinateIndex);
    }

    public void SetGravity(bool enabled)
    {
        GetComponent<Rigidbody>().useGravity = enabled;
    }
}
