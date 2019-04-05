using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Valve.VR.InteractionSystem;

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
    private Transform transportPoint;

    // New feature
    private int countdown;
    private float withinWidth = 1f, withinDepth = 0.5f;
    private bool playerStandingNearPortal, countdownStarted;

    private WaitForSeconds portalSubLoadDuration = new WaitForSeconds(1f);

    [SerializeField]
    private TextMeshPro portalTitle;

    private Rigidbody rb;
    private Player player;
    private Animator animator;

    // Audio
    private AudioSource audioSource;

    private void Awake()
    {
        rb = GetComponent(typeof(Rigidbody)) as Rigidbody;
        portalTitle.text = ConvertSceneBuildIndexToName(sceneBuildIndex);

        player = FindObjectOfType<Player>();
        audioSource = GetComponent<AudioSource>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        Vector3 playerPos = player.transform.position;

        // >>> Check if player is standing near this portal
        float playerRelToNodeX = Mathf.Abs(playerPos.x - transportPoint.position.x);
        float playerRelToNodeZ = Mathf.Abs(playerPos.z - transportPoint.position.z);

        if (playerRelToNodeX < withinWidth && playerRelToNodeZ < withinDepth)
        {
            playerStandingNearPortal = true;

            if (sceneBuildIndex > LARGEST_BUILD_INDEX)
                return;

            if (!countdownStarted)
            {
                StartCoroutine(PortalStart());
                animator.SetBool("EnterPortal", true);
            }
        }
        else
        {
            playerStandingNearPortal = false;
            animator.SetBool("EnterPortal", false);
        }
    }

    private IEnumerator PortalStart()
    {
        countdownStarted = true;
        countdown = 3;
        audioSource.Play();
        for (int i=0; i < 3; i++)
        {
            Debug.Log("Countdown: " + countdown);
            yield return portalSubLoadDuration;
            countdown--;
        }

        if (playerStandingNearPortal)
            LoadLevel();
        else
            countdownStarted = false;
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
        Player player = collision.collider.GetComponent<Player>();

        if (player != null)
        {
            if (sceneBuildIndex <= LARGEST_BUILD_INDEX)
                LoadLevel();
            else
                Debug.Log("Scene '" + sceneBuildIndex + "' not implemented, or not added yet.\nIf added, check LARGEST_BUILD_INDEX");
        }
    }

}
