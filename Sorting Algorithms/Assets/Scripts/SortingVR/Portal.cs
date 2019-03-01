using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

[RequireComponent(typeof(Rigidbody))]
public class Portal : MonoBehaviour {

    /* ----------------------------------------- Portal object -----------------------------------------
     * TODO: create a carry on user device, such that the user can always go where ever he/she wants (split into different subclasses)
     * 
    */

    private static int LARGEST_BUILD_INDEX = 4;

    [SerializeField]
    private int sceneBuildIndex;

    [SerializeField]
    private TextMeshPro portalTitle;

    private Rigidbody rb;


    private void Awake()
    {
        rb = GetComponent(typeof(Rigidbody)) as Rigidbody;
        portalTitle.text = ConvertSceneBuildIndexToName(sceneBuildIndex);
    }

    private void LoadLevel()
    {
        SceneManager.LoadScene(sceneBuildIndex);
    }

    public static string ConvertSceneBuildIndexToName(int sceneBuildIndex)
    {
        switch (sceneBuildIndex)
        {
            case 0: return Util.START_ROOM;
            case 1: return Util.TUTORIAL_ROOM;
            case 2: return Util.MAIN_MENU;
            case 3: return Util.SORTING_ALGORITHMS;
            case 4: return Util.GRAPH_ALGORITHMS;
            case 5: return "Don't enter";
            default: return "X";

        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag(UtilSort.PLAYER_TAG) || collision.collider.CompareTag(UtilSort.PORTAL_OBJECT))
        {
            if (sceneBuildIndex <= LARGEST_BUILD_INDEX)
                LoadLevel();
            else
                Debug.Log("Scene '" + sceneBuildIndex + "' not implemented, or not added yet.\nIf added, check LARGEST_BUILD_INDEX");
        }
    }

}
