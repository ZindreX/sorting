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

    private GameObject outElement;
    private List<GameObject> listObjects;

    private char searchForID;

    private void Awake()
    {
        listObjects = new List<GameObject>();
    }


    public void SetListType(string listType)
    {
        listTypeTitle.text = listType;
    }

    private void MoveListRoof(bool increase)
    {
        if (increase)
            listRoof.transform.position += new Vector3(0f, 1f, 0f);
        else
            listRoof.transform.position -= new Vector3(0f, 1f, 0f);
    }

    private void UpdateListRoofPosition()
    {
        listRoof.transform.position = new Vector3(spawnPointList.transform.position.x, listObjects.Count + 2f, spawnPointList.transform.position.z);
    }

    public void AddListObject(char id)
    {
        // Update roof to prevent list element to become buggy (jumping, shaking etc.)
        UpdateListRoofPosition();

        // Instantiate element
        Vector3 pos = spawnPointList.position + new Vector3(0f, 1f, 0f) * listObjects.Count;
        GameObject listObject = Instantiate(listObjPrefab, pos, Quaternion.identity);
        listObject.GetComponent<TextHolder>().SetSurfaceText(id.ToString());
        listObject.GetComponent<MoveObject>().SetDestination(pos);

        // Add to list
        listObjects.Add(listObject);
    }

    public void PriorityAdd(char id, int value, int index)
    {
        UpdateListRoofPosition();

        // Move all existing list elements up (from the point where we want to insert the new element)
        for (int i=listObjects.Count-1; i >= index; i--)
        {
            // Remove gravity to make it easier to move the objects
            listObjects[i].GetComponent<Rigidbody>().useGravity = false;
            // Find new position and move it
            Vector3 newPos = listObjects[i].transform.position + new Vector3(0f, 1f, 0f);
            listObjects[i].GetComponent<MoveObject>().SetDestination(newPos);
        }

        // Add new element into the open slot
        Vector3 pos = spawnPointList.position + new Vector3(0f, 1f, 0f) * index;
        GameObject listObject = Instantiate(listObjPrefab, pos, Quaternion.identity);
        
        listObject.GetComponent<TextHolder>().SetSurfaceText(id, value);
        if (id == searchForID)
            listObject.GetComponent<TextHolder>().ChangeColor(UtilGraph.TRAVERSED_COLOR);

        listObject.GetComponent<MoveObject>().SetDestination(pos);
        listObjects.Insert(index, listObject);

        // Enable gravity again
        foreach (GameObject obj in listObjects)
        {
            obj.GetComponent<Rigidbody>().useGravity = true;
        }
    }

    public void RemoveAndMoveElementOut()
    {
        switch (listTypeTitle.text)
        {
            case Util.QUEUE:
            case Util.PRIORITY_LIST:
                outElement = listObjects[0];
                listObjects.RemoveAt(0);
                MoveObjectOut(outElement, true);
                break;

            case Util.STACK:
                outElement = listObjects[listObjects.Count - 1];
                listObjects.RemoveAt(listObjects.Count - 1);
                MoveObjectOut(outElement, false);
                break;

            //case Util.PRIORITY_LIST:
            //    outElement = listObjects[listObjects.Count - 1];
            //    listObjects.RemoveAt(listObjects.Count - 1);
            //    MoveObjectOut(outElement, true);
            //    break;
        }
    }

    private void MoveObjectOut(GameObject currentObj, bool moveOther)
    {
        // Move object to current node location
        currentObj.GetComponent<MoveObject>().SetDestination(outPoint.position);
        
        if (moveOther)
        {
            // Move other elements in Queue/Priority list
            foreach (GameObject otherobj in listObjects)
            {
                otherobj.GetComponent<MoveObject>().SetDestination(otherobj.transform.position - new Vector3(0f, 1f, 0f));
            }
        }
    }

    public void DestroyOutElement()
    {
        Destroy(outElement);
    }

    public char SearchingForID
    {
        set { searchForID = value; }
    }

}
