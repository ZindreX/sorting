using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ZoneReader : MonoBehaviour {

    [SerializeField]
    private GameObject player;

    [SerializeField]
    private float enterTutorialX, enterAppX;
    private bool countdownStarted = false;

    [SerializeField]
    private TextMesh updateText;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        float playerPositionX = player.transform.position.x;

        if (!countdownStarted)
        {
		    if (playerPositionX < enterTutorialX)
            {
                updateText.text = "Entering tutorial";
                StartCoroutine(countdownForNewScene(1));
                countdownStarted = true;
            }
            else if (playerPositionX > enterAppX)
            {
                updateText.text = "Entering SortingVR";
                StartCoroutine(countdownForNewScene(2));
                countdownStarted = true;
            }
            else
            {
                updateText.text = "Step L/R to choose";
                countdownStarted = false;
            }
        }
        else
        {
            if (playerPositionX >= enterTutorialX && playerPositionX <= enterAppX)
            {
                updateText.text = "Step L/R to choose";
                countdownStarted = false;
            }
        }
	}


    private IEnumerator countdownForNewScene(int sceneBuilderIndex)
    {
        int countdown = 3;

        while (countdown > 0 || !countdownStarted)
        {
            updateText.text = "Tutorial starts: " + countdown;
            yield return new WaitForSeconds(1f);
            countdown--;
        }

        if (countdownStarted)
            SceneManager.LoadScene(sceneBuilderIndex);

    }
}
