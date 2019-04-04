using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TooltipsHolder : MonoBehaviour {

    [SerializeField]
    private GameObject tooltipsText;

    [SerializeField]
    private string message;

    private bool reported;

    private PathObstacle pathObstacle;

    private void Awake()
    {
        reported = false;
        pathObstacle = GetComponentInParent<PathObstacle>();
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag(UtilSort.TOOLTIPS_ELEMENT))
        {
            tooltipsText.GetComponent<TextMeshPro>().text = message;

            if (!reported)
            {
                Debug.Log("Reporting");
                reported = true;
                pathObstacle.ReportSubTaskCleared();
            }
        }
    }
}
