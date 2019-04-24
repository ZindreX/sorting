using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using TMPro;

public class Monitor : SettingsBase {

    // Const variables
    public const string CASSETTE_TAG = "Cassette";
    public const string CHANNEL_CONTROL_SECTION = "Channel control", NEXT_VIDEO = "Next", PREV_VIDEO = "Prev";
    public const string PLAY_CONTROL_SECTION = "Play control", PLAY = "Play";
    public const string VOLUME_CONTROL_SECTION = "Volume control", VOLUME_DOWN = "Volume down", VOLUME_UP = "Volume up";

    // Variables
    private int currentClipIndex, numberOfVideos;
    private bool preinstalledVideos;

    // Buttons
    private ToggleButton playButton;

    // Screen text
    private TextMeshPro centerScreenText, cornerScreenText;

    // Duration stuff
    private WaitForSeconds displayScreenTextDuration = new WaitForSeconds(3f);

    // Video stuff
    private VideoPlayer videoPlayer;

    [SerializeField]
    private VideoClip[] videoClips;

    private ProgressTracker progressTracker;

    protected override void Awake()
    {
        base.Awake();

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
    }

    private void Start()
    {
        if (numberOfVideos > 0)
        {
            videoPlayer.clip = videoClips[0];
            int frameCount = (int)videoPlayer.frameCount;
            progressTracker.InitProgressTracker(frameCount);
        }
        // else; bool ? 
    }

    private int prevFrame = -1;
    private void Update()
    {
        if (videoPlayer.isPlaying && (int)videoPlayer.frame != prevFrame)
        {
            cornerScreenText.text = videoPlayer.frame + "/" + videoPlayer.frameCount;
            progressTracker.Increment();

            prevFrame = (int)videoPlayer.frame;
        }

    }

    public override void UpdateInteraction(string sectionID, string itemID, string itemDescription)
    {
        Debug.Log("Section: " + sectionID + ", item: " + itemID + ", description: " + itemDescription);
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
                    //settingsSections[CHANNEL_CONTROL_SECTION].SetSectionTitle()

                    // Stop current video
                    videoPlayer.Stop();
                    playButton.InitToggleButton(false);

                    videoPlayer.clip = videoClips[currentClipIndex];
                    StartCoroutine(UpdateCenterScreenText(itemDescription));
                }
                break;

            case PLAY_CONTROL_SECTION:
                if (playButton.State)
                    videoPlayer.Play();
                else
                    videoPlayer.Pause();
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

    private void PlayVideo()
    {
        videoPlayer.Play();
        
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
        progressTracker.InitProgressTracker((int)videoPlayer.frameCount);

        yield return UpdateCenterScreenText(video.Title);

        if (videoPlayer.isPrepared)
            videoPlayer.Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        Video vhs = other.GetComponent<Video>();

        if (vhs != null)
            StartCoroutine(StartVideo(vhs));
    }





}
