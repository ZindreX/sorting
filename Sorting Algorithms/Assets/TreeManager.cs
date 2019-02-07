using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeManager : GraphManager {

    private int treeLevel, nTree, nodeSpaceX, nodeSpaceZ;
    private List<TreeNode> tree;
    

    // N^(L+1) - 1. || (N^L-1) / (N-1).
    protected override int GetMaxNumberOfNodes()
    {
        return (int)Mathf.Pow(nTree, treeLevel + 1) - 1;
    }

    protected override void InitGraph(int[] graphStructure)
    {
        treeLevel = graphStructure[0];
        nTree = graphStructure[1];
        nodeSpaceX = graphStructure[2];
        nodeSpaceZ = graphStructure[3];
    }

    protected override void CreateNodes()
    {
        tree = new List<TreeNode>();

        // Create root
        tree.Add(GenerateNode(null, new Vector3(0f, 0f, 0f), 0));

        int nonLeafNodes = GetNumberOfNonLeafNodes();
        for (int z = 1; z <= nonLeafNodes; z++)
        {
            // Get parent
            TreeNode parent = tree[z-1];

            // Next level Z position
            int zPos = UtilGraph.GRAPH_MIN_Z + (parent.TreeLevel + 1) * nodeSpaceZ;

            //float levelSplit = (UtilGraph.GRAPH_MAX_X - UtilGraph.GRAPH_MIN_X) / ((numberOfLeafs * z) * 2);

            // Create children
            for (int x = 0; x < nTree; x++)
            {
                tree.Add(GenerateNode(parent, new Vector3(CalculateChildPosition(nTree, parent.transform, x), 0f, zPos), z));
            }
        }
    }

    private TreeNode GenerateNode(TreeNode parent, Vector3 pos, int treeLevel)
    {
        GameObject node = Instantiate(nodePrefab, pos, Quaternion.identity);
        node.AddComponent<TreeNode>();
        node.GetComponent<TreeNode>().InitTreeNode(parent, treeLevel);
        return node.GetComponent<TreeNode>();
    }

    private int GetNumberOfNonLeafNodes()
    {
        return (int)Mathf.Pow(nTree, treeLevel) - 1;
    }

    private float CalculateChildPosition(int nTree, Transform parentPos, int childNr)
    {
        switch (nTree)
        {
            case 2:
                if (parentPos.position.x > 0)
                {
                    if (childNr == 0)
                        return parentPos.position.x + (UtilGraph.GRAPH_MAX_X - parentPos.position.x) / 2;
                    else
                        return parentPos.position.x - (UtilGraph.GRAPH_MAX_X - parentPos.position.x) / 2;
                }
                else
                    if (childNr == 1)
                        return parentPos.position.x + (UtilGraph.GRAPH_MIN_X - parentPos.position.x) / 2;
                    else
                        return parentPos.position.x - (UtilGraph.GRAPH_MIN_X - parentPos.position.x) / 2;

            case 3:
                switch (childNr)
                {
                    case 0: return parentPos.position.x + (UtilGraph.GRAPH_MAX_X - parentPos.position.x) / 2;
                    case 1: return parentPos.position.x;
                    case 2: return parentPos.position.x + (UtilGraph.GRAPH_MIN_X - parentPos.position.x) / 2;
                    default: return -1;
                }
            default: return -1;
        }
    }


    protected override void CreateEdges(string mode)
    {
        throw new System.NotImplementedException();
    }


}
