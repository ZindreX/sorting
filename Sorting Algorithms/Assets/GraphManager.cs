using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GraphManager : MonoBehaviour {

    protected int MAX_NODES;

    [SerializeField]
    protected Node nodePrefab;
    [SerializeField]
    protected Edge edgePrefab;

    protected Node[] nodes;
    protected Edge[] edges;

    protected virtual void Awake()
    {
        Debug.Log("Testing");
    }

    // Use this for initialization
    void Start () {
        CreateGraph();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void CreateGraph()
    {
        MAX_NODES = GetMaxNumberOfNodes();
        Debug.Log("Max nodes: " + MAX_NODES);
        StartCoroutine(CreateNodes(MAX_NODES));
    }

    protected abstract IEnumerator CreateNodes(int numberOfNodes);
    protected abstract IEnumerator CreateEdges(Node[] nodes, string mode);
    protected abstract int GetMaxNumberOfNodes();
}
