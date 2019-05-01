using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using TMPro;
using UnityEngine.Windows;

public class Video : MonoBehaviour {

    [SerializeField]
    private string title;

    [SerializeField]
    private string url;

    [SerializeField]
    protected GetVideoFrom getVideoFrom;
    protected enum GetVideoFrom { Streaming, LocalPath }

    //
    private string baseFolderPath = "D:\\sindrw\\Videos\\", format = ".mp4";

    private VideoPlayer videoPlayer;
    private AudioSource audioSource;

    private Vector3 startPos, insertEjectMovement = new Vector3(-0.164f, 0f, 0f);

    private Rigidbody rb;
    private Animator animator;
    private readonly string VHS_INSERT_ANIMATION = "Insert", VHS_EJECT_ANIMATION = "Eject";

    private bool releasedFromHand;

    private WaitForSeconds enterDuration = new WaitForSeconds(1f);

    private void Awake()
    {
        startPos = transform.position;
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();

        releasedFromHand = true;

        // >>> Set title
        Component[] components = GetComponentsInChildren<TextMeshPro>();
        foreach (Component comp in components)
        {
            ((TextMeshPro)comp).text = title;
        }

        // >>> Prepare video
        videoPlayer = gameObject.AddComponent<VideoPlayer>();
        //Add AudioSource
        audioSource = gameObject.AddComponent<AudioSource>();

        //Disable Play on Awake for both Video and Audio
        videoPlayer.playOnAwake = false;
        audioSource.playOnAwake = false;
        audioSource.Pause();

        videoPlayer.source = VideoSource.Url;
        switch ((int)getVideoFrom)
        {
            case 0:
                videoPlayer.url = url;
                break;

            case 1:
                url = baseFolderPath + url + format;
                videoPlayer.url = url;
                break;
        }

        //Set Audio Output to AudioSource
        videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;

        //Assign the Audio from Video to AudioSource to be played
        videoPlayer.EnableAudioTrack(0, true);
        videoPlayer.SetTargetAudioSource(0, audioSource);

        //Set video To Play then prepare Audio to prevent Buffering        
        videoPlayer.Prepare();
        Debug.Log("Change computer? Check filepath!");

        //try
        //{
        //}
        //catch ()
        //{

        //}

        ////Play Video
        //videoPlayer.Play();
        ////Play Sound
        //audioSource.Play();
    }

    private void Update()
    {
        if (transform.position.y < 0)
            transform.position = startPos;
    }

    public string Title
    {
        get { return title; }
    }

    public string Url
    {
        get { return url; }
    }

    public double UrlLength
    {
        get { return videoPlayer.length; }
    }

    public bool ReleasedFromHand
    {
        get { return releasedFromHand; }
        set { releasedFromHand = value; }
    }

    public IEnumerator Insert()
    {
        transform.rotation = Quaternion.identity;
        transform.Rotate(0, -90, 0);
        animator.SetBool(VHS_INSERT_ANIMATION, true);
        yield return enterDuration;
        transform.position += insertEjectMovement;
        animator.SetBool(VHS_INSERT_ANIMATION, false);
    }

    public IEnumerator Eject(Vector3 ejectPos)
    {
        transform.position = ejectPos;
        animator.SetBool(VHS_EJECT_ANIMATION, true);
        yield return enterDuration;
        animator.SetBool(VHS_EJECT_ANIMATION, false);
        transform.position -= insertEjectMovement;
        rb.constraints = RigidbodyConstraints.None;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag(Util.FLOOR_TAG))
        {
            transform.position = startPos;
        }
    }

}
