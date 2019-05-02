using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoPanel : MonoBehaviour {

    [SerializeField]
    private GameObject algorithmObjects;

    private Dictionary<string, GameObject> infoPlates;

    private void Awake()
    {
        infoPlates = new Dictionary<string, GameObject>();

        // Collect infoplates from algorithms
        TeachingAlgorithm[] algorithms = algorithmObjects.GetComponentsInChildren<TeachingAlgorithm>();
        
        foreach (TeachingAlgorithm algorithm in algorithms)
        {
            //GameObject infoPlate = algorithm.InfoPlate;

            //if (infoPlate != null)
            //    infoPlates.Add(algorithm.AlgorithmName, algorithm.InfoPlate);
        }
    }

    public void InitInfoPanel(string algorithm)
    {
        if (infoPlates.ContainsKey(algorithm))
        {
            Instantiate(infoPlates[algorithm]);
        }
    }

}
