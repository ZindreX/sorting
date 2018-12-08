using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipsHolder : MonoBehaviour {

    [SerializeField]
    private GameObject tooltipsText;

    [SerializeField]
    private string message;


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag(Util.TOOLTIPS_ELEMENT))
        {
            tooltipsText.GetComponent<TextMesh>().text = message;
        }
    }
}
