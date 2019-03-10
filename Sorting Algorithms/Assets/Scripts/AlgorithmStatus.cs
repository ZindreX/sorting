using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlgorithmStatus : MonoBehaviour {


    public bool AlgorithmHasDemo(string algorithm)
    {
        switch (algorithm)
        {
            // SortingVR
            case Util.BUBBLE_SORT: case Util.INSERTION_SORT: case Util.BUCKET_SORT: return true;
            
            // GraphVR
            case Util.BFS: case Util.DFS: case Util.DIJKSTRA: return true;
        }
        return false;
    }

    public bool AlgorithmHasStepByStep(string algorithm)
    {
        switch (algorithm)
        {
            // SortingVR
            case Util.BUBBLE_SORT: case Util.INSERTION_SORT: case Util.BUCKET_SORT: return true;

            // GraphVR
            case Util.BFS: case Util.DFS: case Util.DIJKSTRA: return false;
        }
        return false;
    }

    public bool AlgorithmHasUserTest(string algorithm)
    {
        switch (algorithm)
        {
            // SortingVR
            case Util.BUBBLE_SORT: case Util.INSERTION_SORT: case Util.BUCKET_SORT: return true;

            // GraphVR
            case Util.BFS: case Util.DFS: case Util.DIJKSTRA: return false;
        }
        return false;
    }
}
