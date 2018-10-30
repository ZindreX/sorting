﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackboard : MonoBehaviour {

    /* -------------------------------------------- Blackboard --------------------------------------------
     * 
     * 
    */

    [SerializeField]
    private TextMesh[] texts;

    public void ChangeText(int textLine, string text)
    {
        if (textLine >= 0 && textLine < texts.Length)
            texts[textLine].text = text;
        else
            Debug.Log("Cant acces that line");
    }

}
