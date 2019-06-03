using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementInteraction : MonoBehaviour {

    /* Used to check if user has interacted with the sorting element
     * For instance, picked up the element for comparison (Bubblesort was kinda screwed at higher difficulty levels, because it just skipped all instruction but the swap instruction) 
     * 
    */

    private Transform parent;

    private bool pickedUp;


    public void SetParent(Transform parent)
    {
        transform.parent = parent;
        this.parent = parent;
    }

    public bool PickedUp
    {
        get { return pickedUp; }
        set { pickedUp = value; }
    }

    public void PutBack()
    {
        transform.parent = parent;
    }
}
