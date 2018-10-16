using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HolderBase : MonoBehaviour, IChild {

    /* -------------------------------------------- Holder (object) --------------------------------------------
     * 
     * 
    */

    public static int HOLDER_NR = 0;
    protected int holderID, prevElementID;

    protected Color currentColor, prevColor;
    protected bool errorNotified = false, hasPermission = true;

    protected GameObject parent;
    protected SortingElementBase currentHolding;

    void Awake()
    {
        holderID = HOLDER_NR++;
    }

    void Update()
    {
        if (parent.GetComponent<AlgorithmManagerBase>().IsUserTest())
        {
            // Always checking the status of the sorting element this holder is holding, and changing color thereafter
            if (isValidSortingElement(currentHolding))
                UpdateColorOfHolder();
        }
    }

    private bool isValidSortingElement(SortingElementBase element)
    {
        return element != null && element.ElementInstruction != null;
    }

    public virtual string MyRole()
    {
        return Util.HOLDER_TAG + holderID;
    }

    public GameObject Parent
    {
        get { return parent; }
        set { parent = value; }
    }

    public int HolderID
    {
        get { return holderID; }
    }

    public Color CurrentColor
    {
        get { return GetComponentInChildren<Renderer>().material.color; }
        set { prevColor = currentColor; GetComponentInChildren<Renderer>().material.color = value; }
    }

    public Color PrevColor
    {
        get { return prevColor; }
    }

    public SortingElementBase CurrentHolding
    {
        get { return currentHolding; }
        set { currentHolding = value; }
    }

    public bool HasPermission
    {
        set { hasPermission = value; }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.tag == Util.SORTING_ELEMENT_TAG)
        {
            // Tutorial
            if (parent.GetComponent<AlgorithmManagerBase>().IsTutorial())
            {

            }
            else // User test
            {
                prevElementID = currentHolding.SortingElementID;
            }
            currentHolding = null;
            CurrentColor = Util.STANDARD_COLOR;
        }
    }



    // --------------------------------------- Implemented in subclass ---------------------------------------

    protected abstract void UpdateColorOfHolder();
    protected abstract void OnCollisionEnter(Collision collision);





    //public void ChangeColorTo(Color color)
    //{
    //    // Get the renderer from the object
    //    Renderer rend = GetComponentInChildren<Renderer>();

    //    // Set the Color of the material to given color
    //    rend.material.shader = Shader.Find("_Color");
    //    rend.material.SetColor("_Color", color);

    //    // Find the specular shader and change its color too
    //    rend.material.shader = Shader.Find("Specular");
    //    rend.material.SetColor("_SpecColor", color);
    //}

}
