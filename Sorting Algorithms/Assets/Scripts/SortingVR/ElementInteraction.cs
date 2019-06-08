using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementInteraction : MonoBehaviour
{

    /* >>> Used to check if user has interacted with the sorting element
     * 
     * - Bubble- / insertion sort comparison check
     * - Restoring transform parent when deataching from hand
     * 
    */

    private Transform parent;

    private bool pickedUp;

    public void SetParent(Transform parent)
    {
        this.parent = parent;
        transform.parent = parent;
    }

    public bool PickedUp
    {
        get { return pickedUp; }
        set { pickedUp = value; }
    }

    public void PutBack()
    {
        if (parent != null)
            transform.parent = parent;
    }


}
