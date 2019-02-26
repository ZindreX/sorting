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
    public GameObject listObjPrefab, listRoof;

    [SerializeField]
    private TextMesh listTypeTitle;

    [SerializeField]
    private Transform spawnPointList, currentNodePoint;

    private float seconds;
    private NodeRepresentation currentNode;
    private List<NodeRepresentation> nodeRepresentations;

    private void Awake()
    {
        nodeRepresentations = new List<NodeRepresentation>();
    }

    // Set in settings based on which algorithm which is used
    public void SetListType(string listType)
    {
        listTypeTitle.text = listType;
    }

    public float SetAlgorithmSpeed
    {
        set { seconds = value; }
    }

    // Check whether a node has a visual representation of itself
    public bool HasNodeRepresentation(Node node)
    {
        for (int i = 0; i < nodeRepresentations.Count; i++)
        {
            if (nodeRepresentations[i].GetComponent<NodeRepresentation>().Node.NodeAlphaID == node.NodeAlphaID)
                return true;
        }
        return false;
    }

    // Find the index of a node represenation
    public int ListIndexOf(Node node)
    {
        for (int i = 0; i < nodeRepresentations.Count; i++)
        {
            if (nodeRepresentations[i].GetComponent<NodeRepresentation>().Node.NodeAlphaID == node.NodeAlphaID)
                return i;
        }
        return -1;
    }

    // An invisible roof is is kept above the list to prevent it from getting buddy (jumping away etc.)
    private void UpdateListRoofPosition()
    {
        listRoof.transform.position = new Vector3(spawnPointList.transform.position.x, nodeRepresentations.Count + 2f, spawnPointList.transform.position.z);
    }

    // Creates a new node representation of a visited node
    private NodeRepresentation CreateNodeRepresentation(Node node, Vector3 pos, int index)
    {
        // Instantiate element
        GameObject listObject = Instantiate(listObjPrefab, pos, Quaternion.identity);
        listObject.GetComponent<NodeRepresentation>().InitNodeRepresentation(node, index);
        listObject.GetComponent<TextHolder>().SetSurfaceText(node.NodeAlphaID.ToString());
        listObject.GetComponent<MoveObject>().SetDestination(pos);
        return listObject.GetComponent<NodeRepresentation>();
    }

    // Adding a visual representation of the node
    public void AddListObject(Node node)
    {
        // Update roof to prevent list element to become buggy (jumping, shaking etc.)
        UpdateListRoofPosition();

        // Find position and index
        Vector3 pos = spawnPointList.position + new Vector3(0f, 1f, 0f) * nodeRepresentations.Count;
        int index = nodeRepresentations.Count;

        // Create object and add to list
        NodeRepresentation newNodeRep = CreateNodeRepresentation(node, pos, index);
        nodeRepresentations.Add(newNodeRep);
    }

    // Adding a visual representation of the node, with priority based on its distance value)
    public void PriorityAdd(Node node, int index)
    {
        UpdateListRoofPosition();

        // Move all existing list elements up (from the point where we want to insert the new element)
        for (int i=0; i < index; i++)
        {
            // Remove gravity to make it easier to move the objects
            nodeRepresentations[i].GetComponent<Rigidbody>().useGravity = false;
            // Find new position and move it
            Vector3 newPos = nodeRepresentations[i].transform.position + new Vector3(0f, 1f, 0f);
            nodeRepresentations[i].GetComponent<MoveObject>().SetDestination(newPos);
        }

        // Add new element into the open slot
        Vector3 pos = spawnPointList.position + new Vector3(0f, 1f, 0f) * (nodeRepresentations.Count - index);
        NodeRepresentation newPrioNodeRep = CreateNodeRepresentation(node, pos, index);
        //newPrioNodeRep.GetComponent<TextHolder>().SetSurfaceText(node.NodeAlphaID, node.Dist);
        newPrioNodeRep.UpdateSurfaceText(UtilGraph.VISITED_COLOR);
        newPrioNodeRep.MoveNodeRepresentation(pos);
        nodeRepresentations.Insert(index, newPrioNodeRep);

        // Change color of the node representation we are looking for
        if (node.IsEndNode)
            newPrioNodeRep.CurrentColor = UtilGraph.SHORTEST_PATH_COLOR;


        // Enable gravity again and update indexes
        for (int i=0; i < nodeRepresentations.Count; i++)
        {
            NodeRepresentation nodeRep = nodeRepresentations[i];
            nodeRep.SetGravity(true);
            nodeRep.ListIndex = i;
        }
    }

    // Removes the next element from the list/queue/stack
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

    // Moves the removed element to the 'current node' spot
    private void MoveCurrentNode(NodeRepresentation currentNodeRep, bool moveOther)
    {
        // Move object to current node location
        currentNodeRep.MoveNodeRepresentation(currentNodePoint.position);
        currentNodeRep.CurrentColor = UtilGraph.CURRENT_NODE_COLOR;
        
        if (moveOther)
        {
            // Move other elements in Queue/Priority list
            foreach (NodeRepresentation otherobj in nodeRepresentations)
            {
                otherobj.MoveNodeRepresentation(otherobj.transform.position - new Vector3(0f, 1f, 0f));
            }
        }
    }

    // Enable/Disable gravity for multiple node representations
    private void SetGravityForMultipleNodeReps(int from, int to, bool enable)
    {
        for (int i=from; i <= to; i++)
        {
            nodeRepresentations[i].SetGravity(enable);
        }
    }

    // Updates value and position of one node representation and other involved node reps.
    public IEnumerator UpdateValueAndPositionOf(Node node, int index)
    {
        Color prevColor = node.CurrentColor;
        node.CurrentColor = UtilGraph.DIST_UPDATE_COLOR;
        // Node representation we want to move
        NodeRepresentation mainNodeRep = FindNodeRepresentation(node);

        // Update surface text
        mainNodeRep.UpdateSurfaceText(UtilGraph.DIST_UPDATE_COLOR);

        // Moving from index
        int mainNodeRepIndex = mainNodeRep.ListIndex;
        //Debug.Log("Updating value of node '" + node.NodeAlphaID + "'. Current index=" + currentNodeRepIndex + " == " + nodeRepresentations.IndexOf(nodeRep) + " ???");

        // Check if index changed
        if (mainNodeRepIndex - index != 0)
        {
            // Disable gravity of swapping node reps
            SetGravityForMultipleNodeReps(0, index, false);

            // Move node left and down
            Vector3 moveLeft = mainNodeRep.transform.position + new Vector3(-1f, 0f, 0f);
            mainNodeRep.MoveNodeRepresentation(moveLeft);
            yield return new WaitForSeconds(0.5f);

            int moveDownYpos = mainNodeRepIndex - index;
            Vector3 moveDown = mainNodeRep.transform.position + new Vector3(-1f, moveDownYpos, 0f);
            mainNodeRep.MoveNodeRepresentation(moveDown);
            yield return new WaitForSeconds(1f);

            // Move all the other involved up
            // Move all existing list elements up (from the point where we want to insert the new element) *** Copied - make new method?
            for (int i = mainNodeRepIndex + 1; i <= index; i++) //for (int i=listObjects.Count-1; i >= index; i--)
            {
                NodeRepresentation involvedNodeRep = nodeRepresentations[i];

                // Find new position and move it
                Vector3 newPos = involvedNodeRep.transform.position + new Vector3(0f, 1f, 0f);
                involvedNodeRep.MoveNodeRepresentation(newPos);
                involvedNodeRep.ListIndex = i - 1;
                nodeRepresentations[i - 1] = involvedNodeRep;
                //Debug.Log("Moving up node '" + nodeRepresentations[i].Node.NodeAlphaID + "' to index=" + (i - 1));
                involvedNodeRep.HighlightNodeRepresentation(UtilGraph.DIST_UPDATE_COLOR, seconds); // 1f);
            }

            // Move back into list
            //Debug.Log("Moving node '" + mainNodeRep.Node.NodeAlphaID + "' back into list index=" + index);
            Vector3 backInTheList = new Vector3(spawnPointList.position.x, mainNodeRep.transform.position.y, mainNodeRep.transform.position.z);
            mainNodeRep.MoveNodeRepresentation(backInTheList); //nodeRep.UpdateIndexPosition(index);
            mainNodeRep.ListIndex = index;
            nodeRepresentations[index] = mainNodeRep;
            yield return mainNodeRep.HighlightNodeRepresentation(UtilGraph.DIST_UPDATE_COLOR, seconds); // 1f);

            // Enable gravity again
            SetGravityForMultipleNodeReps(0, index, true);
        }
        node.CurrentColor = prevColor;
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




    // Swap 1 by 1: Not implemented/used
    private void SwapPositionsOf(NodeRepresentation np1, NodeRepresentation np2)
    {
        np1.SetGravity(false);
        np2.SetGravity(false);


        Debug.Log("Switching");
        Vector3 np1MoveLeft = np1.transform.position + new Vector3(-0.5f, 0f, 0f);
        np1.MoveNodeRepresentation(np1MoveLeft);

        Vector3 np2MoveRight = np2.transform.position + new Vector3(0.5f, 0f, 0f);
        np2.MoveNodeRepresentation(np2MoveRight);

        Debug.Log("Done switching");
    }


}
