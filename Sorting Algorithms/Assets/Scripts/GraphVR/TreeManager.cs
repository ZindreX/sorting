using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeManager : GraphManager {

    private int treeLevel, nTree, nodeSpaceX, nodeSpaceZ;
    private List<TreeNode> tree;
 
    // N^(L+1) - 1. || (N^L-1) / (N-1).
    public override int GetMaxNumberOfNodes()
    {
        return (int)Mathf.Pow(nTree, treeLevel + 1) - 1;
    }

    public List<TreeNode> Tree
    {
        get { return tree; }
    }

    protected override void InitGraph(int[] graphStructure)
    {
        treeLevel = graphStructure[0];
        nTree = graphStructure[1];
        nodeSpaceX = graphStructure[2];
        nodeSpaceZ = graphStructure[3];
    }

    protected override void CreateNodes(string s)
    {
        tree = new List<TreeNode>();

        // Create root
        tree.Add(GenerateNode(null, new Vector3(0f, 0f, 0f), 0));

        // Number of nodes w/children (aka no leafs included)
        int nonLeafNodes = NumberOfInternalNodes();

        // Set level (starting one level below root)
        int currentLevel = 1;
        
        // Variables for x and z position of nodes, and split var
        float xPos = 0, zPos = 0, widthSplit = 0;
        for (int z = 1; z <= nonLeafNodes; z++)
        {
            // Get parent
            TreeNode parent = tree[z-1];

            // Fix position variables when next level
            if (parent.TreeLevel != currentLevel)
            {
                currentLevel = parent.TreeLevel;

                // Next level Z position (from player towards the blackboard)
                zPos = UtilGraph.GRAPH_MIN_Z + (currentLevel + 1) * nodeSpaceZ;

                /* Split width of map into pieces for where to place nodes
                 * |--------O--------|
                 * |----O-------O----|
                 * |--O---O---O---O--|
                 * |-O-O-O-O-O-O-O-O-|
                */
                widthSplit = (UtilGraph.GRAPH_MAX_X - UtilGraph.GRAPH_MIN_X) / (z * nTree);
                xPos = UtilGraph.GRAPH_MAX_X - widthSplit / 2;
            }

            // Create children w/edge
            for (int x = 0; x < nTree; x++)
            {
                tree.Add(GenerateNode(parent, new Vector3(xPos, 0f, zPos), parent.TreeLevel + 1));
                xPos -= widthSplit;

                // *** Add edge between parent and child
                Vector3 n2 = tree[tree.Count - 1].transform.position;

                // Find center between node and child
                Vector3 centerPos = new Vector3(parent.transform.position.x + n2.x, 0f, parent.transform.position.z + n2.z) / 2;

                // Find angle
                float angle = -Mathf.Atan2(nodeSpaceZ, n2.x - parent.transform.position.x) * Mathf.Rad2Deg;

                // Instantiate and fix edge
                Edge edge = Instantiate(edgePrefab, centerPos, Quaternion.identity);
                edge.transform.Rotate(0, angle, 0, Space.Self);

                // Set edge cost (no cost unless shortest path)
                int edgeCost = UtilGraph.NO_COST;
                if (algorithm is IShortestPath)
                    edgeCost = Random.Range(0, UtilGraph.EDGE_MAX_WEIGHT);
                edge.InitEdge(parent, tree[tree.Count - 1], edgeCost, UtilGraph.TREE);

            }
        }
    }

    private TreeNode GenerateNode(TreeNode parent, Vector3 pos, int treeLevel)
    {
        GameObject node = Instantiate(nodePrefab, pos, Quaternion.identity);
        node.AddComponent<TreeNode>();
        node.GetComponent<TreeNode>().InitTreeNode(algorithm.AlgorithmName, parent, treeLevel);
        return node.GetComponent<TreeNode>();
    }

    private int NumberOfInternalNodes()
    {
        return (int)Mathf.Pow(nTree, treeLevel) - 1;
    }

    public override int GetNumberOfEdges()
    {
        return tree.Count - 1;
    }

    protected override void CreateEdges(string mode)
    {
        //return;
        //Debug.LogError("No need for this method anymore (tree)");
        ////int edgeNr = 0;
        //Vector3 n1, n2;
        //// Go through all nodes w/children
        //for (int node=0; node < NumberOfInternalNodes(); node++)
        //{
        //    TreeNode currentNode = tree[node];
        //    n1 = currentNode.transform.position;

        //    // Go through all children of current node
        //    for (int child=0; child < nTree; child++)
        //    {
        //        n2 = currentNode.Children[child].transform.position;

        //        // Find center between node and child
        //        Vector3 centerPos = new Vector3(n1.x + n2.x, 0f, n1.z + n2.z) / 2;

        //        // Find angle
        //        //Debug.Log("Angle: " + -Mathf.Atan2(nodeSpaceZ, n2.x - n1.x) * Mathf.Rad2Deg);
        //        float angle = -Mathf.Atan2(nodeSpaceZ, n2.x - n1.x) * Mathf.Rad2Deg;

        //        // Instantiate and fix edge
        //        Edge edge = Instantiate(edgePrefab, centerPos, Quaternion.identity);
        //        edge.transform.Rotate(0, angle, 0, Space.Self);
        //        edge.InitEdge(currentNode, currentNode.Children[child], 0);
                
        //        //edgeNr++;
        //    }
        //}
    }

    // TODO: fix later
    // 
    public override Node GetNode(int a, int b)
    {
        if (a >= 0 && a < tree.Count)
            return tree[a];
        return null;
    }

    public override void ResetGraph()
    {
        for (int i=0; i < tree.Count; i++)
        {
            tree[i].ResetNode();
        }
    }

    public override void DeleteGraph()
    {
        Node.NODE_ID = 0;
        for (int i=0; i < tree.Count; i++)
        {
            Destroy(tree[i]);
        }
    }
}
