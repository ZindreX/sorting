using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GraphAlgorithm : MonoBehaviour {

    protected float seconds;


    public float Seconds
    {
        get { return seconds; }
        set { seconds = value; }
    }
}
