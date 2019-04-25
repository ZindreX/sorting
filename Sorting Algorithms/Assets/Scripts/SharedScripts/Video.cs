using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using TMPro;

public class Video : MonoBehaviour {

    [SerializeField]
    private string title;

    [SerializeField]
    private string url;

    [SerializeField]
    private VideoClip videoClip;

    [SerializeField]
    private bool useUrl;

    private VideoPlayer videoPlayer;
    private AudioSource audioSource;

    private Vector3 startPos;

    private Animator animator;
    private readonly string VHS_ENTER_ANIMATION = "Enter";

    private bool releasedFromHand;

    private WaitForSeconds enterDuration = new WaitForSeconds(1f);

    private void Awake()
    {
        startPos = transform.position;
        animator = GetComponentInChildren<Animator>();

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

        if (useUrl)
        {
            // Video clip from Url
            videoPlayer.source = VideoSource.Url;
            videoPlayer.url = url;
        }
        else
        {
            videoPlayer.source = VideoSource.VideoClip;
            videoPlayer.clip = videoClip;
        }

        //Set Audio Output to AudioSource
        videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;

        //Assign the Audio from Video to AudioSource to be played
        videoPlayer.EnableAudioTrack(0, true);
        videoPlayer.SetTargetAudioSource(0, audioSource);

        //Set video To Play then prepare Audio to prevent Buffering        
        videoPlayer.Prepare();

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

    public bool UseUrl
    {
        get { return useUrl; }
    }

    public double UrlLength
    {
        get { return videoPlayer.length; }
    }

    public VideoClip VideoClip
    {
        get { return videoClip; }
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
        animator.SetBool(VHS_ENTER_ANIMATION, true);
        yield return enterDuration;
        animator.SetBool(VHS_ENTER_ANIMATION, false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag(Util.FLOOR_TAG))
        {
            transform.position = startPos;
        }
    }

}
