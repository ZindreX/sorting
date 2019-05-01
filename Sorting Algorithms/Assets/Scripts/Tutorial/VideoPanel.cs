using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoPanel : MonoBehaviour {

    [SerializeField]
    private GameObject screen;

    [SerializeField]
    private RenderTexture renderTexturePrefab;

    private RenderTexture renderTextureClone;
    private VideoPlayer videoPlayer;

    private void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        renderTextureClone = new RenderTexture(renderTexturePrefab);
        screen.GetComponent<Renderer>().material.mainTexture = renderTextureClone;
        videoPlayer.targetTexture = renderTextureClone;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == Util.HEAD_COLLIDER)
        {
            videoPlayer.Play();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == Util.HEAD_COLLIDER)
        {
            videoPlayer.Stop();
            renderTextureClone.Release();
        }
    }
}
