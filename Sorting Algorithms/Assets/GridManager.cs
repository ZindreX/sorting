using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : GraphManager {

    public readonly int GRID_MIN_X = -8, GRID_MAX_X = 8, GRID_MIN_Z = 0, GRID_MAX_Z = 16, GRID_SPACE = 4;

    private Dictionary<int, Vector3> gridMapPositions;
    private HashSet<int> usedNodes;


    protected override void Awake()
    {
        gridMapPositions = new Dictionary<int, Vector3>();

        int nodeID = 0;
        for (int i=GRID_MIN_Z; i < GRID_MAX_Z; i+=GRID_SPACE)
        {
            for (int j=-GRID_MIN_X; j < GRID_MAX_X; j+=GRID_SPACE)
            {
                gridMapPositions.Add(nodeID, new Vector3(j, 0f, i));
                nodeID++;
            }
        }

        usedNodes = new HashSet<int>();
        base.Awake();
    }

    protected override int GetMaxNumberOfNodes()
    {
        return ((GRID_MAX_X - GRID_MIN_X) / GRID_SPACE + 1) * ((GRID_MAX_Z - GRID_MIN_Z) / GRID_SPACE + 1);
    }

    protected override IEnumerator CreateNodes(int numberOfNodes)
    {
        Debug.Log("Creating");
        nodes = new Node[numberOfNodes];
        for (int x=0; x < numberOfNodes; x++)
        {
            Debug.Log(gridMapPositions[x]);
            Debug.Log(nodes[x]);
            nodes[x] = Instantiate(nodePrefab, gridMapPositions[x], Quaternion.identity);
        }
        yield return new WaitForSeconds(1f);
    }

    private void AddNeededNodes(HashSet<int> reachAbleNodes, List<int> addList)
    {
        for (int x = 0; x < addList.Count; x++)
        {
            if (!usedNodes.Contains(addList[x]))
            {
                reachAbleNodes.Add(addList[x]);
            }
            else
                Debug.Log("Not added: " + addList[x]);
        }
    }

    private List<int> GridMapUpdateNextNodes(int nextNode)
    {
        switch (nextNode)
        {
            case 0: return new List<int>(new int[] { 1, 2, 3 });
            case 1: return new List<int>(new int[] { 0, 5 });
            case 2: return new List<int>(new int[] { 0, 4 });
            case 3: return new List<int>(new int[] { 4, 5, 6 });
            case 4: return new List<int>(new int[] { 2, 3, 8 });
            case 5: return new List<int>(new int[] { 1, 3, 7 });
            case 6: return new List<int>(new int[] { 3, 7, 8, 9 });
            case 7: return new List<int>(new int[] { 5, 6, 11 });
            case 8: return new List<int>(new int[] { 4, 6, 10 });
            case 9: return new List<int>(new int[] { 10, 11 });
            case 10: return new List<int>(new int[] { 8, 9 });
            case 11: return new List<int>(new int[] { 7, 9 });
            default: Debug.Log("Node not found"); return null;
        }
    }


    protected override IEnumerator CreateEdges(Node[] nodes, string mode)
    {
        throw new System.NotImplementedException();
    }



    private void OldCode()
    {
        //// Init node list
        //nodes = new Node[numberOfNodes];
        //// Init first node
        //nodes[0] = Instantiate(nodePrefab, gridMapPositions[0], Quaternion.identity);
        ////nodes[nodesCreated].Spawn();
        //usedNodes.Add(0);

        //// Create set of nodes which can be reached from the first node, and adds these
        //HashSet<int> reachAbleNodes = new HashSet<int>();
        //AddNeededNodes(reachAbleNodes, GridMapUpdateNextNodes(0));

        //// Add the rest of the nodes randomly
        //int nodesCreated = 1;
        //while (nodesCreated < numberOfNodes)
        //{
        //    // Optimize....
        //    int selectedNode = Random.Range(0, MAX_NODES);
        //    while (!reachAbleNodes.Contains(selectedNode))
        //        selectedNode = Random.Range(0, MAX_NODES);

        //    // Init node
        //    nodes[nodesCreated] = Instantiate(nodePrefab, gridMapPositions[selectedNode], Quaternion.identity);
        //    usedNodes.Add(selectedNode);
        //    reachAbleNodes.Remove(selectedNode);

        //    // Add nodes reach able from selected node
        //    AddNeededNodes(reachAbleNodes, GridMapUpdateNextNodes(selectedNode));

        //    nodesCreated++;
        //}
    }

}
