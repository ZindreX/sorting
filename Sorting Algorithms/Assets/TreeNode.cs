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


    public TreeNode(int nodeID, TreeNode parent, int treeLevel) : base(nodeID)
    {
        children = new List<TreeNode>();
        this.parent = parent;
        this.treeLevel = treeLevel;
    }

    public void InitTreeNode(TreeNode parent, int treeLevel)
    {
        children = new List<TreeNode>();
        this.parent = parent;
        this.treeLevel = treeLevel;

        if (parent != null)
            parent.AddChildren(this);
    }

    public override string TotalCost()
    {
        throw new System.NotImplementedException();
    }

    public void AddChildren(TreeNode child)
    {
        if (!children.Contains(child))
        {
            children.Add(child);
            if (child.parent != this)
                child.SetParent(this);
        }

    }

    public void SetParent(TreeNode parent)
    {
        this.parent = parent;
        parent.AddChildren(this);
    }

    public int TreeLevel
    {
        get { return treeLevel; }
    }


}
