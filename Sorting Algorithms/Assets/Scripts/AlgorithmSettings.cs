using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlgorithmSettings : MonoBehaviour {

    // Fix this class such that settings buttons only needs to be fixed once?

    private int numberOfElements = 8;
    private string teachingMode = Util.TUTORIAL, difficulty = Util.BEGINNER, sortingCase = Util.NONE;
    private bool duplicates = true;

    [SerializeField]
    private AlgorithmManagerBase algorithmManager;

    public string TeachingMode
    {
        set { teachingMode = value; Debug.Log(value); }
    }

    public string Difficulty
    {
        set { difficulty = value; Debug.Log(value); }
    }

    public string SortingCase
    {
        set { sortingCase = value; Debug.Log(value); }
    }

    public int NumberOfElements
    {
        set { numberOfElements = value; Debug.Log(value); }
    }

    public bool Duplicates
    {
        set { duplicates = value; Debug.Log(value); }
    }

    public void StartSorting()
    {
        algorithmManager.InstantiateSetup();
    }

}
