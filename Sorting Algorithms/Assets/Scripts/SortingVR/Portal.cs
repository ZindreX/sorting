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

    private static int LARGEST_BUILD_INDEX = 5;

    [SerializeField]
    private int sceneBuildIndex;

    [SerializeField]
    private TextMeshPro portalTitle;

    private Rigidbody rb;


    private void Awake()
    {
        rb = GetComponent(typeof(Rigidbody)) as Rigidbody;
        portalTitle.text = UtilSort.ConvertSceneBuildIndexToName(sceneBuildIndex);
    }

    private void LoadLevel()
    {
        SceneManager.LoadScene(sceneBuildIndex);
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
