using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using TMPro;

public class Monitor : SettingsBase {

    // Const variables
    public const string CASSETTE_TAG = "Cassette";
    public const string CHANNEL_CONTROL_SECTION = "Channel control", NEXT_VIDEO = "Next", PREV_VIDEO = "Prev";
    public const string PLAY_CONTROL_SECTION = "Play control", PLAY = "Play", STOP = "Stop";
    public const string VOLUME_CONTROL_SECTION = "Volume control", VOLUME_DOWN = "Volume down", VOLUME_UP = "Volume up";

    // Animation
    private Animator animator;
    private readonly string NO_VHS_IDLE = "MonitorNoVHS", PLAY_VHS = "VHSPlay", STOP_VHS = "VHSStopPlaying";
    private bool vhsReceived;

    [SerializeField]
    private Transform hatchEntrance, returnPosition;

    private Video currentPlayingVHS;
    private bool currentPlayingVideoIsCompleted;

    // Variables
    private int currentClipIndex, numberOfVideos;
    private int prevSecond = -1, videoDuration;
    private bool preinstalledVideos;

    // Buttons
    private ToggleButton playButton;

    [SerializeField]
    private RenderTexture monitorRendererTexture;

    // Screen text
    private TextMeshPro centerScreenText, cornerScreenText;

    // Duration stuff
    private WaitForSeconds displayScreenTextDuration = new WaitForSeconds(3f);
    private WaitForSeconds enterVideoDuration = new WaitForSeconds(1f);

    // Video stuff
    private VideoPlayer videoPlayer;

    [SerializeField]
    private VideoClip[] videoClips;

    private ProgressTracker progressTracker;

    private BoxCollider vhsTrigger;

    protected override void Awake()
    {
        base.Awake();

        monitorRendererTexture.Release();

        currentClipIndex = 0;
        numberOfVideos = videoClips.Length;
        if (numberOfVideos > 0)
            preinstalledVideos = true;

        // Get screen UI text
        Component[] screenTexts = GetComponentsInChildren<TextMeshPro>();
        for (int i=0; i < screenTexts.Length; i++)
        {
            TextMeshPro temp = (TextMeshPro)screenTexts[i];

            if (i < 2)
                temp.text = "";

            switch (i)
            {
                case 0: centerScreenText = temp; break;
                case 1: cornerScreenText = temp; break;
                //default: Debug.Log("No screen text"); break;
            }
        }

        videoPlayer = GetComponent<VideoPlayer>();

        // Buttons
        playButton = GetComponentInChildren<ToggleButton>();

        // Progress bar
        progressTracker = GetComponentInChildren<ProgressTracker>();

        // Animator
        animator = GetComponentInChildren<Animator>();

        // Trigger
        vhsTrigger = GetComponent<BoxCollider>();
    }

    private void Start()
    {
        if (numberOfVideos > 0)
        {
            videoPlayer.clip = videoClips[0];
            videoDuration = (int)CalculateVideoDuration(videoClips[0]);
            progressTracker.InitProgressTracker(videoDuration);
        }
        // else; bool ? 
    }

    private void Update()
    {
        if (videoPlayer.isPlaying && (int)videoPlayer.time != prevSecond)
        {
            cornerScreenText.text = (int)videoPlayer.time + "/" + videoDuration;
            progressTracker.Increment();
            prevSecond = (int)videoPlayer.time;
        }
        else if ((int)videoPlayer.time == videoDuration)
        {
            if (currentPlayingVHS != null)
                EjectVideo(null);
        }
        else if (vhsReceived)
        {
            if (currentPlayingVHS != null && currentPlayingVHS.ReleasedFromHand)
            {
                StartCoroutine(EnterVideo(currentPlayingVHS));
                vhsReceived = false;
            }
        }


    }

    public override void UpdateInteraction(string sectionID, string itemID, string itemDescription)
    {
        //Debug.Log("Section: " + sectionID + ", item: " + itemID + ", description: " + itemDescription);
        switch (sectionID)
        {
            case CHANNEL_CONTROL_SECTION:
                videoPlayer.source = VideoSource.VideoClip;

                // Set video clip
                if (preinstalledVideos)
                {
                    switch (itemID)
                    {
                        case NEXT_VIDEO:
                            // Set video index
                            currentClipIndex++;
                            if (currentClipIndex >= numberOfVideos)
                                currentClipIndex = 0;
                            break;

                        case PREV_VIDEO:
                            // Set video index
                            currentClipIndex--;

                            if (currentClipIndex < 0)
                                currentClipIndex = numberOfVideos - 1;
                            break;

                    }

                    // Stop current video
                    videoPlayer.Stop();
                    playButton.InitToggleButton(false);

                    currentPlayingVideoIsCompleted = false;
                    videoPlayer.clip = videoClips[currentClipIndex];
                    StartCoroutine(UpdateCenterScreenText(itemDescription));
                }
                break;

            case PLAY_CONTROL_SECTION:
                switch (itemID)
                {
                    case PLAY:
                        if (playButton.State)
                            videoPlayer.Play();
                        else
                            videoPlayer.Pause();
                        break;

                    case STOP:
                        if (videoPlayer.isPlaying)
                            videoPlayer.Stop();

                        if (currentPlayingVHS != null)
                            StartCoroutine(EjectVideo(null));

                        monitorRendererTexture.Release();
                        break;
                }
                break;

            case VOLUME_CONTROL_SECTION:
                switch (itemID)
                {
                    case VOLUME_DOWN: break;
                    case VOLUME_UP: break;
                }
                break;

            default: Debug.Log("No section found with name '" + sectionID + "'."); break;
        }
    }

    protected override void InitButtons()
    {
        throw new System.NotImplementedException();
    }

    private IEnumerator UpdateCenterScreenText(string text)
    {
        centerScreenText.text = text;
        yield return displayScreenTextDuration;
        centerScreenText.text = "";
    }

    private IEnumerator UpdateCornerScreenText(string text)
    {
        cornerScreenText.text = text;
        yield return displayScreenTextDuration;
        cornerScreenText.text = "";
    }

    private IEnumerator StartVideo(Video video)
    {
        currentPlayingVideoIsCompleted = false;
        videoPlayer.Stop();

        if (video.UseUrl)
        {
            videoPlayer.source = VideoSource.Url;
            videoPlayer.url = video.Url;
        }
        else
        {
            videoPlayer.source = VideoSource.VideoClip;
            videoPlayer.clip = video.VideoClip;
        }

        videoPlayer.Prepare();

        // Reset and init progresstracker
        progressTracker.ResetProgress();
        progressTracker.InitProgressTracker(videoDuration);

        yield return UpdateCenterScreenText(video.Title);

        if (videoPlayer.isPrepared)
        {
            videoPlayer.Play();
            playButton.InitToggleButton(true);
        }

        MoveVideo(returnPosition.position);
        video.gameObject.SetActive(false);
    }

    private void PrepareEnterVideo(Video vhs)
    {
        currentPlayingVHS = vhs;
        vhsReceived = true;

        if (!vhs.UseUrl)
            videoDuration = (int)CalculateVideoDuration(vhs.VideoClip); //(int)(vhs.VideoClip.frameCount / vhs.VideoClip.frameRate); // video clip time (seconds)
        else
            videoDuration = (int)vhs.UrlLength; // url time (seconds)
    }

    private double CalculateVideoDuration(VideoClip videoClip)
    {
        return videoClip.frameCount / videoClip.frameRate;
    }

    private IEnumerator EnterVideo(Video vhs)
    {
        vhsTrigger.enabled = false;

        // Play animation (vhs entering)
        MoveVideo(hatchEntrance.position);
        StartCoroutine(vhs.Enter());
        yield return enterVideoDuration;

        // Monitor animation
        animator.Play(PLAY_VHS);


        yield return StartVideo(vhs);
    }

    private IEnumerator EjectVideo(Video vhs)
    {
        vhsTrigger.enabled = true;

        animator.Play(STOP_VHS);

        currentPlayingVHS.gameObject.SetActive(true);
        currentPlayingVHS.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;

        yield return enterVideoDuration;

        currentPlayingVideoIsCompleted = true;
        currentPlayingVHS = null;

        if (vhs != null)
            PrepareEnterVideo(vhs);
    }

    private void MoveVideo(Vector3 pos)
    {
        if (currentPlayingVHS != null)
        {
            currentPlayingVHS.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            currentPlayingVHS.transform.position = pos;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Video vhs = other.GetComponent<Video>();

        if (vhs != null && currentPlayingVHS == null)
            PrepareEnterVideo(vhs);
    }


}
