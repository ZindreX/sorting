using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeNode : Node {

    private int treeLevel;

    [SerializeField]
    private TreeNode parent;
    [SerializeField]
    private List<TreeNode> children;

    public void InitTreeNode(string algorithm, TreeNode parent, int treeLevel)
    {
        InitNode(algorithm);
        children = new List<TreeNode>();
        this.treeLevel = treeLevel;

        //this.parent = parent;
        //if (parent != null)
        //    parent.AddChildren(this);
    }

    public override string NodeType
    {
        get { return UtilGraph.TREE_NODE; }
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

    public override void AddEdge(Edge edge)
    {
        if (edge.OtherNodeConnected(this) == parent)
            PrevEdge = edge;
        else
            base.AddEdge(edge);
    }
}
