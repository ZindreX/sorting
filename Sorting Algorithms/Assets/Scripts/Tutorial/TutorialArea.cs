using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialArea : Area {

    [Header("Tutorial area")]
    [SerializeField]
    private TutorialArea nextArea;

    private TutorialManager tutorialManager;

    private void Awake()
    {
        tutorialManager = FindObjectOfType<TutorialManager>();
    }

    public TutorialArea NextArea
    {
        get { return nextArea; }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        tutorialManager.PlayerInArea(this);
    }

    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
    }



}
