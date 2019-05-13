using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Valve.VR.InteractionSystem;
using UnityEngine.Video;

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
    private Transform transportPoint;
    private BoxCollider trigger;

    // New feature
    private int countdown;
    private bool playerStandingNearPortal, countdownStarted;

    private WaitForSeconds portalSubLoadDuration = new WaitForSeconds(1f);

    [SerializeField]
    private TextMeshPro portalTitle;

    private Rigidbody rb;
    private Player player;
    private Animator animator;

    private GameObject previewer;
    private VideoPlayer videoPlayer;

    // Audio
    private AudioSource audioSource;

    private void Awake()
    {
        rb = GetComponent(typeof(Rigidbody)) as Rigidbody;
        portalTitle.text = ConvertSceneBuildIndexToName(sceneBuildIndex);

        player = FindObjectOfType<Player>();
        audioSource = GetComponent<AudioSource>();
        animator = GetComponentInChildren<Animator>();
        trigger = GetComponentInChildren<BoxCollider>();

        // Previewer
        previewer = GetComponentInChildren<VideoPanel>().gameObject;
        videoPlayer = GetComponentInChildren<VideoPlayer>();
        if (videoPlayer.clip == null)
            Destroy(previewer);

            
    }

    private IEnumerator PortalStart()
    {
        countdownStarted = true;
        countdown = 3;
        audioSource.Play();
        for (int i=0; i < 3; i++)
        {
            //Debug.Log("Countdown: " + countdown);
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
            case 5: return "Auditorium";
            default: return "X";
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == Util.HEAD_COLLIDER)
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
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == Util.HEAD_COLLIDER)
        {
            playerStandingNearPortal = false;
            animator.SetBool("EnterPortal", false);
        }
    }

}
