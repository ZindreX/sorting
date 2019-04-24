using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Video;

public class UrlContainer : MonoBehaviour {

    [SerializeField]
    private string title;

    [SerializeField]
    private string url;

    private VideoClip videoClip;

    private void Awake()
    {
        GetComponentInChildren<TextMeshPro>().text = title;
        
    }

    public string Title
    {
        get { return title; }
    }

    public string Url
    {
        get { return url; }
    }

    public VideoClip VideoClip
    {
        get { return videoClip; }
    }


}
