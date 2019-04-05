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

    private TutorialTask tutorialTask;

    private void Awake()
    {
        reported = false;
        tutorialTask = GetComponentInParent<TutorialTask>();
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag(UtilSort.TOOLTIPS_ELEMENT))
        {
            tooltipsText.GetComponent<TextMeshPro>().text = message;

            if (!reported)
            {
                reported = true;
                tutorialTask.Progress();
            }
        }
    }
}
