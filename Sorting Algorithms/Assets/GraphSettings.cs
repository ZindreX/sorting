using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphSettings : MonoBehaviour {

    [SerializeField]
    private GraphStructureEditor graphStructureEditor;
    private enum GraphStructureEditor { Grid, Tree }

    private string graphStructure;

    void Awake()
    {
        switch ((int)graphStructureEditor)
        {
            case 0: graphStructure = UtilGraph.GRID; break;
            case 1: graphStructure = UtilGraph.TREE; break;
        }
    }

    public string Graphstructure
    {
        get { return graphStructure; }
    }
}
