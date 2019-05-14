using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustableObject : MonoBehaviour {

    [SerializeField]
    private float adjustHeightBy = 0.1f, minVal = -0.2f, maxVal = 1.8f;


    // TODO: Fix sorting table issues: sorting elements get bugged sometimes (transform.parent changed)
    //private bool hasMoveAbleObjectsOntop;
    //private GameObject[] attachedObjects;

    //private void Awake()
    //{
    //    if (GetComponent<SortingTable>() != null)
    //        hasMoveAbleObjectsOntop = true;
    //}

    public void AdjustUp()
    {
        if (transform.position.y < maxVal)
            transform.position += new Vector3(0f, adjustHeightBy, 0f);
    }

    public void AdjustDown()
    {
        if (transform.position.y > minVal)
            transform.position -= new Vector3(0f, adjustHeightBy, 0f);
    }

}
