using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementInteraction : MonoBehaviour {

    /* Used to check if user has interacted with the sorting element
     * For instance, picked up the element for comparison (Bubblesort was kinda screwed at higher difficulty levels, because it just skipped to the next swap instruction) 
     * 
    */

    private bool pickedUp;

    public bool PickedUp
    {
        get { return pickedUp; }
        set { pickedUp = value; }
    }
}
