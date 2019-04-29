using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectionBorder : MonoBehaviour {

    /* >>> Section border
     * - The lines around a button group (section)
     * - Default color set
     *   - If another color wanted, change in editor (new color will set change on Awake)
    */


    [SerializeField]
    private Material borderMaterial;

    private void Awake()
    {
        Component[] borders = GetComponentsInChildren<Renderer>();

        foreach (Component border in borders)
        {
            border.GetComponent<Renderer>().material = borderMaterial;
        }
    }

}
