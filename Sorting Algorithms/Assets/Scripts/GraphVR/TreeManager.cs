using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeManager : GraphManager {

    private int treeLevel, nTree, levelDepthLength;
    private List<TreeNode> tree;
 
    // N^(L+1) - 1. || (N^L-1) / (N-1).
    public override int GetMaxNumberOfNodes()
    {
        return (int)Mathf.Pow(nTree, treeLevel + 1) - 1;
    }

    private int NumberOfInternalNodes()
    {
        return (int)Mathf.Pow(nTree, treeLevel) - 1;
    }

    public override int GetNumberOfEdges()
    {
        return tree.Count - 1;
    }

    public int NumberOfLeafs()
    {
        return GetMaxNumberOfNodes() - NumberOfInternalNodes();
    }

    public List<TreeNode> Tree
    {
        get { return tree; }
    }

    public override void InitGraph(int[] graphStructure)
    {
        treeLevel = graphStructure[0];
        nTree = graphStructure[1];
        levelDepthLength = graphStructure[2];
    }

    public override void CreateNodes(string mode)
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

            if (parent == null)
                continue;

            // Fix position variables when next level
            if (parent.TreeLevel != currentLevel)
            {
                currentLevel = parent.TreeLevel;

                // Next level Z position (from player towards the blackboard)
                zPos = UtilGraph.GRAPH_MIN_Z + (currentLevel + 1) * levelDepthLength;

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
                if (mode == UtilGraph.PARTIAL_EDGES && Util.RollRandom(LookUpRNGDict(UtilGraph.PARTIAL_BUILD_TREE_CHILD_CHANCE)))
                {
                    tree.Add(null);
                    xPos -= widthSplit;
                    continue;
                }

                tree.Add(GenerateNode(parent, new Vector3(xPos, 0f, zPos), parent.TreeLevel + 1));
                xPos -= widthSplit;

                // *** Add edge between parent and child
                Vector3 n2 = tree[tree.Count - 1].transform.position;

                // Find center between node and child
                Vector3 centerPos = new Vector3(parent.transform.position.x + n2.x, 0f, parent.transform.position.z + n2.z) / 2;

                // Find angle
                float angle = -Mathf.Atan2(levelDepthLength, n2.x - parent.transform.position.x) * Mathf.Rad2Deg;

                // Initialize edge
                CreateEdge(parent, tree[tree.Count - 1], centerPos, angle);
            }
        }
    }

    private TreeNode GenerateNode(TreeNode parent, Vector3 pos, int treeLevel)
    {
        GameObject node = Instantiate(graphSettings.nodePrefab, pos, Quaternion.identity);
        node.AddComponent<TreeNode>();
        node.GetComponent<TreeNode>().InitTreeNode(graphAlgorithm.AlgorithmName, parent, treeLevel);
        return node.GetComponent<TreeNode>();
    }

    public override void CreateEdges(string mode)
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

    public override IEnumerator BacktrackShortestPathsAll(WaitForSeconds demoStepDuration)
    {
        int numberOfInternalNodes = NumberOfInternalNodes();
        for (int i=tree.Count-1; i >= numberOfInternalNodes; i--)
        {
            Node node = tree[i];

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
