using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MoveObject))]
[RequireComponent(typeof(TextHolder))]
public class NodeRepresentation : MonoBehaviour {

    private MoveObject moveObject;
    private TextHolder textHolder;
    private Node node;
    private int listIndex;

    [SerializeField]
    private Vector3 coordinateIndex;

    private void Awake()
    {
        moveObject = GetComponent(typeof(MoveObject)) as MoveObject;
        textHolder = GetComponent(typeof(TextHolder)) as TextHolder;
    }

    public void InitNodeRepresentation(Node node, int index)
    {
        this.node = node;
        UpdateIndexPosition(index);
        coordinateIndex = transform.position;
    }

    public Node Node
    {
        get { return node; }
    }

    public int ListIndex
    {
        get { return listIndex; }
        set { listIndex = value; }
    }

    public IEnumerator UpdateSurfaceText()
    {
        textHolder.SetSurfaceText(node.NodeAlphaID, node.Dist);
        textHolder.ChangeColor(UtilGraph.DIST_UPDATE_COLOR);
        yield return new WaitForSeconds(0.25f);
        textHolder.ChangeColor(Color.white);
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

    public void EnableGravity(bool enabled)
    {
        GetComponent<Rigidbody>().useGravity = enabled;
    }
}
