using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeNode : Node {

    private bool isRoot;
    private int treeLevel;

    [SerializeField]
    private TreeNode parent;
    [SerializeField]
    private List<TreeNode> children;

    public void InitTreeNode(TreeNode parent, int treeLevel)
    {
        children = new List<TreeNode>();
        this.parent = parent;
        this.treeLevel = treeLevel;

        if (parent == null)
            isRoot = true;

        if (parent != null)
            parent.AddChildren(this);
    }

    public override string NodeType
    {
        get { return "Tree node " + nodeID; }
    }

    public void AddChildren(TreeNode child)
    {
        if (!children.Contains(child))
        {
            children.Add(child);
            if (child.parent != this)
                child.Parent = this;
        }
    }

    public TreeNode Parent
    {
        get { return parent; }
        set { parent = value; }
    }
 
    public int TreeLevel
    {
        get { return treeLevel; }
    }

    public List<TreeNode> Children
    {
        get { return children; }
    }

    public override void UpdateCostText()
    {
        GetComponentInChildren<TextMesh>().text = UtilGraph.ConvertIfInf(totalCost.ToString());
    }

}
