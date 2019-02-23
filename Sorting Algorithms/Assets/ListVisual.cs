using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListVisual : MonoBehaviour {

    /* -------------------------------------------- List representation ----------------------------------------------------
     * > Gives a visual representation of the nodes in the algorithms
     *  - DFS: Stack
     *  - BFS: Queue
     *  - Dijkstra: Priority list
     * > Nodes are put on top of each other, where
     *  - Stack: removes the object on top
     *  - Queue/Priority list: removes the object on the bottom
     * 
     * > Note:
     *  - Priority list keeps elements sorted from least (index 0) to biggest (index -1) element (index opposite of Dijkstra's list) : fix?
    */

    [SerializeField]
    private GameObject listObjPrefab, listRoof;

    [SerializeField]
    private TextMesh listTypeTitle;

    [SerializeField]
    private Transform spawnPointList, outPoint;

    private NodeRepresentation currentNode;
    private List<NodeRepresentation> nodeRepresentations;

    private void Awake()
    {
        nodeRepresentations = new List<NodeRepresentation>();
    }


    public void SetListType(string listType)
    {
        listTypeTitle.text = listType;
    }

    public bool NodeHasRepresentation(Node node)
    {
        for (int i = 0; i < nodeRepresentations.Count; i++)
        {
            if (nodeRepresentations[i].GetComponent<NodeRepresentation>().Node.NodeAlphaID == node.NodeAlphaID)
                return true;
        }
        return false;
    }

    public int ListIndexOf(Node node)
    {
        for (int i = 0; i < nodeRepresentations.Count; i++)
        {
            if (nodeRepresentations[i].GetComponent<NodeRepresentation>().Node.NodeAlphaID == node.NodeAlphaID)
                return i;
        }
        return -1;
    }

    //private void MoveListRoof(bool increase)
    //{
    //    if (increase)
    //        listRoof.transform.position += new Vector3(0f, 1f, 0f);
    //    else
    //        listRoof.transform.position -= new Vector3(0f, 1f, 0f);
    //}

    private void UpdateListRoofPosition()
    {
        listRoof.transform.position = new Vector3(spawnPointList.transform.position.x, nodeRepresentations.Count + 2f, spawnPointList.transform.position.z);
    }


    // Adding a visual representation of the node
    public void AddListObject(Node node)
    {
        // Update roof to prevent list element to become buggy (jumping, shaking etc.)
        UpdateListRoofPosition();

        // Instantiate element
        Vector3 pos = spawnPointList.position + new Vector3(0f, 1f, 0f) * nodeRepresentations.Count;
        GameObject listObject = Instantiate(listObjPrefab, pos, Quaternion.identity);
        listObject.GetComponent<NodeRepresentation>().InitNodeRepresentation(node, nodeRepresentations.Count);
        listObject.GetComponent<TextHolder>().SetSurfaceText(node.NodeAlphaID.ToString());
        listObject.GetComponent<MoveObject>().SetDestination(pos);

        // Add to list
        nodeRepresentations.Add(listObject.GetComponent<NodeRepresentation>());
    }

    // Adding a visual representation of the node, with priority based on its distance value)
    public void PriorityAdd(Node node, int index)
    {
        UpdateListRoofPosition();

        // Move all existing list elements up (from the point where we want to insert the new element)
        for (int i=0; i < index; i++) //for (int i=listObjects.Count-1; i >= index; i--)
        {
            // Remove gravity to make it easier to move the objects
            nodeRepresentations[i].GetComponent<Rigidbody>().useGravity = false;
            // Find new position and move it
            Vector3 newPos = nodeRepresentations[i].transform.position + new Vector3(0f, 1f, 0f);
            nodeRepresentations[i].GetComponent<MoveObject>().SetDestination(newPos);
        }

        // Add new element into the open slot
        Vector3 pos = spawnPointList.position + new Vector3(0f, 1f, 0f) * (nodeRepresentations.Count - index);
        GameObject listObject = Instantiate(listObjPrefab, pos, Quaternion.identity);
        listObject.GetComponent<NodeRepresentation>().InitNodeRepresentation(node, index); // nodeRepresentations.Count);
        listObject.GetComponent<TextHolder>().SetSurfaceText(node.NodeAlphaID, node.Dist);

        // Change color of the node representation we are looking for
        if (node.IsEndNode)
            listObject.GetComponent<TextHolder>().ChangeColor(UtilGraph.SHORTEST_PATH_COLOR);

        listObject.GetComponent<MoveObject>().SetDestination(pos);
        nodeRepresentations.Insert(index, listObject.GetComponent<NodeRepresentation>());

        // Enable gravity again
        foreach (NodeRepresentation nodeRep in nodeRepresentations)
        {
            nodeRep.GetComponent<Rigidbody>().useGravity = true;
        }
    }

    // Prepares the moving of the 'current node', removing it from the list/queue/stack
    public void RemoveCurrentNode()
    {
        switch (listTypeTitle.text)
        {
            case Util.QUEUE:
                currentNode = nodeRepresentations[0]; // change to queue for dequeuing instead?
                nodeRepresentations.RemoveAt(0);
                MoveCurrentNode(currentNode, true);
                break;

            case Util.STACK:
                currentNode = nodeRepresentations[nodeRepresentations.Count - 1];
                nodeRepresentations.RemoveAt(nodeRepresentations.Count - 1);
                MoveCurrentNode(currentNode, false);
                break;

            case Util.PRIORITY_LIST:
                currentNode = nodeRepresentations[nodeRepresentations.Count - 1];
                nodeRepresentations.RemoveAt(nodeRepresentations.Count - 1);
                MoveCurrentNode(currentNode, true);
                break;
        }
    }

    // Does the actual moving of the node
    private void MoveCurrentNode(NodeRepresentation currentObj, bool moveOther)
    {
        // Move object to current node location
        currentObj.MoveNodeRepresentation(outPoint.position);
        
        if (moveOther)
        {
            // Move other elements in Queue/Priority list
            foreach (NodeRepresentation otherobj in nodeRepresentations)
            {
                otherobj.MoveNodeRepresentation(otherobj.transform.position - new Vector3(0f, 1f, 0f));
            }
        }
    }

    public void HelpUpdateNodeRepresentation(bool before)
    {
        foreach (NodeRepresentation nodeRep in nodeRepresentations)
        {
            nodeRep.EnableGravity(!before);

            if (before)
            {
                Vector3 pos = nodeRep.transform.position + new Vector3(-1f, 0f, 0f);
                nodeRep.MoveNodeRepresentation(pos);
            }
        }
    }

    public void MoveUpdatedNode(Node node, int index, int nodeRepIndex)
    {
        Debug.LogError("TODO: FIKS HER");
        int swaps = Mathf.Abs(index - nodeRepIndex);
        int direction = (index - nodeRepIndex) / swaps;

        NodeRepresentation nodeRep = FindNodeRepresentation(node);
        while (swaps > 0)
        {
            int newIndex = nodeRepIndex + direction;
            NodeRepresentation temp = nodeRepresentations[newIndex];
            nodeRepresentations[newIndex] = nodeRep;
            nodeRep.UpdateIndexPosition(newIndex);

            //temp.UpdateIndexPosition()

            swaps--;
        }
    }

    // Updates one node represenation at a time
    public IEnumerator UpdateNodeRepresentation(Node node, int index, bool updateValue)
    {
        NodeRepresentation nodeRep = FindNodeRepresentation(node);

        // Check if index changed
        if (nodeRep.ListIndex - index != 0)
        {
            // Moving from index
            int prevIndex = nodeRepresentations.IndexOf(nodeRep);

            // Update coordinate position
            nodeRep.UpdateIndexPosition(index);

            // Swapping
            NodeRepresentation temp = nodeRepresentations[index];
            //temp.MoveNodeRepresentation(temp.transform.position + new Vector3(-1f, 0f, 0f)); // Push slightly to the side
            nodeRepresentations[index] = nodeRep;
            nodeRepresentations[prevIndex] = temp;
        }

        // Update surface text if value (dist) changed
        if (updateValue)
            yield return nodeRep.UpdateSurfaceText();

        yield return null;
    }

    public NodeRepresentation FindNodeRepresentation(Node node)
    {
        for (int i=0; i < nodeRepresentations.Count; i++)
        {
            if (nodeRepresentations[i].Node.NodeAlphaID == node.NodeAlphaID)
                return nodeRepresentations[i];
        }
        return null;
    }

    // Updates node representation according to the algorithm (priority list value updates)
    //public IEnumerator UpdateNodeRepresentation(List<Node> list, Node valueUpdateNode)
    //{
    //    Debug.Log("Updating node representation!");
    //    Dictionary<char, int> updatedIndexes = new Dictionary<char, int>();

    //    // Collect ID and new indexes
    //    for (int i=0; i < list.Count; i++)
    //    {
    //        updatedIndexes.Add(list[i].NodeAlphaID, i);
    //    }

    //    foreach (NodeRepresentation obj in nodeRepresentations)
    //    {
    //        NodeRepresentation nodeRep = obj.GetComponent<NodeRepresentation>();
    //        char nodeID = nodeRep.Node.NodeAlphaID;

    //        if (nodeRep.ListIndex - updatedIndexes[nodeID] != 0)
    //            nodeRep.UpdateIndexPosition(updatedIndexes[nodeID]);

    //        if (valueUpdateNode.NodeAlphaID == nodeID)
    //            nodeRep.UpdateSurfaceText();

    //        yield return null;
    //    }
    //    nodeRepresentations.Sort();
    //    Debug.Log("Done updating");
    //}

    public void DestroyOutElement()
    {
        Destroy(currentNode.gameObject);
    }


    // *** Debugging ***

    public string PrintList()
    {
        string result = "Node Representation: ";
        foreach (NodeRepresentation obj in nodeRepresentations)
        {
            Node node = obj.GetComponent<NodeRepresentation>().Node;
            result += "[" + node.NodeAlphaID + "," + node.Dist + "], ";
        }
        return result + "\n";
    }
}
