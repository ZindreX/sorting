using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using TMPro;

public class Monitor : SettingsBase {

    // Editor
    [SerializeField]
    private Transform hatchEntrance, returnPosition;

    [SerializeField]
    private RenderTexture monitorRendererTexture;

    [SerializeField]
    private VideoClip[] videoClips;

    // Screen text
    private TextMeshPro centerScreenText, cornerScreenText;

    // Const variables
    public const string CASSETTE_TAG = "Cassette";
    public const string CHANNEL_CONTROL_SECTION = "Channel control", NEXT_VIDEO = "Next", PREV_VIDEO = "Prev";
    public const string PLAY_CONTROL_SECTION = "Play control", PLAY = "Play", STOP = "Stop";
    public const string VOLUME_CONTROL_SECTION = "Volume control", VOLUME_DOWN = "Volume down", VOLUME_UP = "Volume up";

    // Animation
    public readonly string NO_VHS_IDLE = "MonitorNoVHS", PLAY_VHS = "VHSPlay", STOP_VHS = "VHSStopPlaying";


    // Variables
    private int currentClipIndex, numberOfVideos;
    private int prevSecond = -1, videoDuration;
    private bool preinstalledVideos, vhsReceived, currentPlayingVideoIsCompleted;

    private Video currentPlayingVHS;

    // Duration stuff
    private WaitForSeconds displayScreenTextDuration = new WaitForSeconds(3f);
    private WaitForSeconds insertVideoDuration = new WaitForSeconds(1f);
    private WaitForSeconds ejectVideoDuration = new WaitForSeconds(2f);

    // Buttons
    private ToggleButton playButton;
    
    private Animator animator;
    private VideoPlayer videoPlayer;
    private BoxCollider vhsTrigger;
    private ProgressTracker progressTracker;

    protected override void Awake() {
        // Do settings base awake (collect sections/buttons)
        base.Awake();

        // Buttons
        playButton = GetComponentInChildren<ToggleButton>();
        playButton.gameObject.SetActive(false);

        // Make screen black
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
            playButton.gameObject.SetActive(true);
            videoPlayer.clip = videoClips[0];
            videoDuration = (int)CalculateVideoDuration(videoClips[0]);
            progressTracker.InitProgressTracker(videoDuration);
        }
        // else; bool ? 
    }

    private void Update()
    {
        // bool check?

        if (videoPlayer.isPlaying && (int)videoPlayer.time != prevSecond)
        {
            // Update progress bar
            cornerScreenText.text = (int)videoPlayer.time + "/" + videoDuration;
            progressTracker.Increment();
            prevSecond = (int)videoPlayer.time;
        }
        //else if ((int)videoPlayer.time == videoDuration)
        //{
        //    Debug.Log("Testing");
        //    // Eject video when done playing
        //    if (currentPlayingVHS != null)
        //        EjectVideo(null);
        //}
        else if (vhsReceived)
        {
            // Video received, and when the user's hand has let go of the cassette then start the insert animation
            if (currentPlayingVHS != null && currentPlayingVHS.ReleasedFromHand)
            {
                StartCoroutine(InsertVideo(currentPlayingVHS));
                vhsReceived = false;
            }
        }
    }

    public override void UpdateInteraction(string sectionID, string itemID, string itemDescription)
    {
        //Debug.Log("Section: " + sectionID + ", item: " + itemID + ", description: " + itemDescription);
        switch (sectionID)
        {
            case CHANNEL_CONTROL_SECTION: // Not used (set inactive)
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

            case VOLUME_CONTROL_SECTION: // Not used (set inactive)
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

    // Returns the duration of a video in seconds
    private double CalculateVideoDuration(VideoClip videoClip)
    {
        return videoClip.frameCount / videoClip.frameRate;
    }

    // --------------------------------------- Screen text update ---------------------------------------

    // Displays a text in the center of the screen for N seconds, then remove it
    private IEnumerator UpdateCenterScreenText(string text)
    {
        centerScreenText.text = text;
        yield return displayScreenTextDuration;
        centerScreenText.text = "";
    }

    // Displays a text in the up right corner of the screen for N seconds, then remove it
    private IEnumerator UpdateCornerScreenText(string text)
    {
        cornerScreenText.text = text;
        yield return displayScreenTextDuration;
        cornerScreenText.text = "";
    }

    // --------------------------------------- VHS insert stuff ---------------------------------------

    // Tells the unit that a video has been inserted - check update loop for continuation
    private void PrepareEnterVideo(Video vhs)
    {
        currentPlayingVHS = vhs;
        vhsReceived = true;

        //if (!vhs.UseUrl)
        //    videoDuration = (int)CalculateVideoDuration(vhs.VideoClip); //(int)(vhs.VideoClip.frameCount / vhs.VideoClip.frameRate); // video clip time (seconds)
        //else
        videoDuration = (int)vhs.UrlLength; // url time (seconds)
    }

    // Inserts the video, disables the trigger (for other incoming cassettes)
    private IEnumerator InsertVideo(Video vhs)
    {
        playButton.gameObject.SetActive(true);
        vhsTrigger.enabled = false;

        // Play animation (vhs entering)
        MoveVideo(hatchEntrance.position);
        StartCoroutine(vhs.Insert());
        yield return insertVideoDuration;

        // Monitor animation
        animator.Play(PLAY_VHS);
        yield return StartVideo(vhs);
    }

    // Start watching the video
    private IEnumerator StartVideo(Video video)
    {
        currentPlayingVideoIsCompleted = false;
        videoPlayer.Stop();

        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = video.Url;

        //if (video.UseUrl)
        //{
        //}
        //else
        //{
        //    videoPlayer.source = VideoSource.VideoClip;
        //    videoPlayer.clip = video.VideoClip;
        //}

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

        //MoveVideo(returnPosition.position);
        video.gameObject.SetActive(false);
    }

    // Eject video (not completly animated yet), enable trigger
    private IEnumerator EjectVideo(Video vhs)
    {
        playButton.gameObject.SetActive(false);

        animator.Play(STOP_VHS);
        yield return insertVideoDuration;
        
        // Activate the cassette which has been played
        currentPlayingVHS.gameObject.SetActive(true);
        StartCoroutine(currentPlayingVHS.Eject(hatchEntrance.position));
        yield return ejectVideoDuration;

        // Reset
        currentPlayingVideoIsCompleted = true;
        currentPlayingVHS = null;

        vhsTrigger.enabled = true;

        // Not used anymore (disabled interaction during playtime, so current need to be ejected before inserting a new cassette)
        if (vhs != null)
            PrepareEnterVideo(vhs);
    }

    // --------------------------------------- Animation preparation ---------------------------------------

    // Moving cassette and freeze its position/rotation: (1) infront of the monitor inside the insertion slot, (2) returned to the table next to the monitor
    private void MoveVideo(Vector3 pos)
    {
        if (currentPlayingVHS != null)
        {
            currentPlayingVHS.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            currentPlayingVHS.transform.position = pos;
        }
    }

    // --------------------------------------- Trigger ---------------------------------------

    private void OnTriggerEnter(Collider other)
    {
        Video vhs = other.GetComponent<Video>();

        // If video inserted, prepare for animation
        if (vhs != null && currentPlayingVHS == null)
            PrepareEnterVideo(vhs);
        //else
        //    Debug.Log("VHS is null check: " + (vhs == null) + " && currently playing VHS check: " + (currentPlayingVHS == null));
    }


}
