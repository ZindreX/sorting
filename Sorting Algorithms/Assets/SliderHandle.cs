using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderHandle : MonoBehaviour {

    [SerializeField]
    private Transform startpos, endPos;

    [SerializeField]
    private int fromValue, toValue, interval;
    private int sliderLength;


    [SerializeField]
    private GameObject sliderHandleObj, sliderLengthObj;

    [SerializeField]
    private TextMesh value;

    private void Awake()
    {
        sliderLength = (int)sliderLengthObj.GetComponent<Renderer>().bounds.size.x;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        int sliderHandlePosX = (int)sliderHandleObj.transform.position.x;

        
    }
}
