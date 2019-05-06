using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomGraphManager : GraphManager {

    private int numberOfNodes, numberOfEdges, minimumSpaceBetweenNodes;

    private List<RandomNode> nodes;

    public override void InitGraph(int[] graphStructure)
    {
        nodes = new List<RandomNode>();

        numberOfNodes = graphStructure[0];
        numberOfEdges = graphStructure[1];
        minimumSpaceBetweenNodes = graphStructure[2];
    }

    public override int GetMaxNumberOfNodes()
    {
        return numberOfNodes;
    }

    public override int GetNumberOfEdges()
    {
        return numberOfEdges;
    }

    public override void CreateNodes(string s)
    {
        // Start node
        nodes.Add(GenerateNode(new Vector3(0f, 0f, 0f)));

        // Create rest of nodes randomly
        int counter = 1;
        while (counter < numberOfNodes)
        {
            int xPos = Random.Range(UtilGraph.GRAPH_MIN_X, UtilGraph.GRAPH_MAX_X);
            int zPos = Random.Range(UtilGraph.GRAPH_MIN_Z, UtilGraph.GRAPH_MAX_Z);

            // Check whether any other nodes are closer than sqrt((x2 - x1)^2 + (z2 - z1)^2)
            bool flag = false;
            for (int i=0; i < nodes.Count; i++)
            {
                if (Mathf.Sqrt(Mathf.Pow(nodes[i].transform.position.x - xPos, 2) + Mathf.Pow(nodes[i].transform.position.z - zPos, 2)) < minimumSpaceBetweenNodes)
                    flag = true;
            }

            // Redo node if any "collisions" found
            if (flag)
                continue;

            // Generate new node
            nodes.Add(GenerateNode(new Vector3(xPos, 0f, zPos)));

            counter++;
        }
    }

    private RandomNode GenerateNode(Vector3 pos)
    {
        GameObject node = Instantiate(nodePrefab, pos, Quaternion.identity);
        node.AddComponent<RandomNode>();
        node.GetComponent<RandomNode>().InitRandomNode(algorithmName);
        node.transform.parent = nodeContainerObject.transform;
        return node.GetComponent<RandomNode>();
    }



    //private Dictionary<RandomNode, List<RandomNode>>
    public override void CreateEdges(string mode)
    {
        // Dictionary for counting number of times nodeID has been used - Dict<nodeID, counter>
        Dictionary<int, int> nodeUsedCounter = new Dictionary<int, int>();
        for (int x=0; x < nodes.Count; x++)
        {
            nodeUsedCounter.Add(nodes[x].NodeID, 0);
        }

        int edgesAdded = 0, overLapCheck = 3;
        while (edgesAdded < numberOfEdges || overLapCheck < 0)
        {
            // Find least used node
            int leastUsedNode = 0;
            int value = UtilGraph.INF;
            foreach (var node in nodeUsedCounter)
            {
                if (node.Value < value)
                {
                    value = node.Value;
                    leastUsedNode = node.Key;
                }
            }
            RandomNode node1 = nodes[leastUsedNode];

            // Find a random node
            RandomNode node2 = nodes[RandomRangeExcept(0, numberOfNodes, leastUsedNode)];
            bool extraChance = true;
            while (node1.IsNeighborWith(node2) && extraChance)
            {
                node2 = nodes[RandomRangeExcept(0, numberOfNodes, leastUsedNode)];

                if (node1.IsNeighborWith(node2))
                    extraChance = false;
            }

            if (!extraChance)
                break;

            // Add edge between nodes (if no collisions with other edges or nodes)
            if (AddEdgeBetween(node1, node2))
            {
                // Update frequency of these nodes
                nodeUsedCounter[node1.NodeID] += 1;
                nodeUsedCounter[node2.NodeID] += 1;
                edgesAdded++;
            }
            else
            {
                overLapCheck--;
            }
        }
    }

    private int RandomRangeExcept(int min, int max, int except)
    {
        int value = -1, attempts = 3;
        do
        {
            value = Random.Range(min, max);
            attempts--;
        } while (value == except || attempts < 0);
        return value;
    }

    private bool AddEdgeBetween(RandomNode node1, RandomNode node2)
    {
        Vector3 n1 = node1.transform.position;
        Vector3 n2 = node2.transform.position;

        // Find center between nodes
        Vector3 centerPos = new Vector3(n1.x + n2.x, 0f, n1.z + n2.z) / 2;

        // Find angle
        float angle = -Mathf.Atan2(n2.z - n1.z, n2.x - n1.x) * Mathf.Rad2Deg;
        //Debug.Log(node1.NodeType + " connecting to " + node2.NodeType + ", angle=" + angle + ", dist=" + UtilGraph.DistanceBetweenNodes(node1.transform, node2.transform));

        // Initialize edge
        CreateEdge(node1, node2, centerPos, angle);

        // Check if any overlaps occured
        //Debug.Log("Test: " + edge.CollisionOccured);
        //if (edge.CollisionOccured)
        //{
        //    Debug.Log(">>>>>>>>>>>>>>>>> Destroying edge!!");
        //    Destroy(edge.gameObject);
        //    return false;
        //}
        return true;
    }

    public override Node GetNode(int a, int b)
    {
        if (a >= 0 && a < nodes.Count)
            return nodes[a];
        return null;
    }

    public override void ResetGraph()
    {
        for (int i=0; i < nodes.Count; i++)
        {
            nodes[i].ResetNode();
        }

        base.ResetGraph();
    }

    public override void DeleteGraph()
    {
        Node.NODE_ID = 0;
        for (int i = 0; i < nodes.Count; i++)
        {
            Destroy(nodes[i].gameObject);
        }

        // Delete edges
        base.DeleteGraph();
    }

    public override void SetAllNodesDist(int value)
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            nodes[i].Dist = value;
        }
    }

    public override IEnumerator BacktrackShortestPathsAll(WaitForSeconds demoStepDuration)
    {
        for (int i=nodes.Count-1; i >= 0; i--)
        {
            Node node = nodes[i];

            // Start backtracking from end node back to start node
            while (true)
            {
                // Change color of node
                node.CurrentColor = UtilGraph.SHORTEST_PATH_COLOR;

                // Change color of edge leading to previous node
                Edge backtrackEdge = node.PrevEdge;

                if (node.PrevEdge == null || node.PrevEdge.CurrentColor == UtilGraph.SHORTEST_PATH_COLOR)
                {
                    node.CurrentColor = UtilGraph.SHORTEST_PATH_COLOR;
                    break;
                }

                backtrackEdge.CurrentColor = UtilGraph.SHORTEST_PATH_COLOR;

                // Set "next" node
                node = backtrackEdge.OtherNodeConnected(node);
                yield return demoStepDuration;
            }
        }
    }
}
