using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using TMPro;

public class Monitor : SettingsBase {

    public const string CASSETTE_TAG = "Cassette";

    public const string CHANNEL_CONTROL_SECTION = "Channel control", NEXT_VIDEO = "Next", PREV_VIDEO = "Prev";
    public const string PLAY_CONTROL_SECTION = "Play control", PLAY = "Play";
    public const string VOLUME_CONTROL_SECTION = "Volume control", VOLUME_DOWN = "Volume down", VOLUME_UP = "Volume up";

    private int currentClipIndex, numberOfVideos;

    private ToggleButton playButton;

    private TextMeshPro centerScreenText, cornerScreenText;

    private WaitForSeconds displayScreenTextDuration = new WaitForSeconds(3f);

    private VideoPlayer videoPlayer;

    [SerializeField]
    private VideoClip[] videoClips;

    protected override void Awake()
    {
        base.Awake();

        currentClipIndex = 0;
        numberOfVideos = videoClips.Length;

        // Get screen UI text
        centerScreenText = GetComponentInChildren<TextMeshPro>();
        centerScreenText.text = "";

        cornerScreenText = GetComponentInChildren<TextMeshPro>();
        cornerScreenText.text = "";

        videoPlayer = GetComponent<VideoPlayer>();
    }

    private void Start()
    {
        if (numberOfVideos > 0)
        {
            videoPlayer.clip = videoClips[0];
            
        }
        // else; bool ? 
    }

    public override void UpdateInteraction(string sectionID, string itemID, string itemDescription)
    {
        switch (sectionID)
        {
            case CHANNEL_CONTROL_SECTION:

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

                // Set video clip
                videoPlayer.clip = videoClips[currentClipIndex];
                StartCoroutine(UpdateCenterScreenText(itemDescription));
                break;

            case PLAY:
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

            default: break;
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

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");
        Debug.Log("Trigger: " + other.name);

        UrlContainer vhs = other.GetComponent<UrlContainer>();

        if (vhs != null)
        {
            string url = vhs.Url;

            videoPlayer.Stop();

            videoPlayer.url = url;

            videoPlayer.Prepare();
            videoPlayer.Play();
        }
    }





}
