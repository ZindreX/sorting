using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchButton : MonoBehaviour {

    [SerializeField]
    private TextMesh[] texts;

    private bool isTutorial = false;

    private void Awake()
    {
        SwitchState();
    }

    public bool IsTutorial
    {
        get { return isTutorial; }
    }

    public void SwitchState()
    {
        isTutorial = !isTutorial;
        if (isTutorial)
        {
            texts[0].color = Color.green;
            texts[1].color = Color.white;
            gameObject.transform.RotateAround(transform.position, transform.up, 0f); // turn over (in do on children???) ---> change to animation
            Debug.Log("Tutorial");
        }
        else
        {
            texts[0].color = Color.white;
            texts[1].color = Color.green;
            gameObject.transform.RotateAround(transform.position, transform.up, 180f); // turn over (in do on children???)
            Debug.Log("User test");
        }
    }

    private void OnMouseDown()
    {
        SwitchState();   
    }


}
