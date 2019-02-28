using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextHolder : MonoBehaviour {

    [SerializeField]
    private TextMeshPro[] surfaceTexts;

    public void SetSurfaceText(string text)
    {
        foreach (TextMeshPro textMesh in surfaceTexts)
        {
            textMesh.text = text;
        }
    }

    public void SetSurfaceText(char id, int value)
    {
        for (int i = 0; i < surfaceTexts.Length; i++)
        {
            if (i % 2 == 0)
                surfaceTexts[i].text = id.ToString();
            else
                surfaceTexts[i].text = value.ToString();
        }
    }

    public void ChangeColor(Color color)
    {
        foreach (TextMeshPro textMesh in surfaceTexts)
        {
            textMesh.color = color;
        }
    }
}
