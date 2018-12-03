using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextHolder : MonoBehaviour {

    [SerializeField]
    private TextMesh[] surfaceTexts;

    public void SetSurfaceText(string text)
    {
        foreach (TextMesh textMesh in surfaceTexts)
        {
            textMesh.text = text;
        }
    }

}
