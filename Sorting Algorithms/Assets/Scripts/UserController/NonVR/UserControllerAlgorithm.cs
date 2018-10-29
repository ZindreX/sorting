using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserControllerAlgorithm : UserControllerBase {

    [SerializeField]
    private GameObject AMObject;
    private AlgorithmManagerBase am;

    protected override void Awake()
    {
        base.Awake();
        am = AMObject.GetComponent(typeof(AlgorithmManagerBase)) as AlgorithmManagerBase;
    }

    protected override void PerformAlgorithmControl()
    {
        if (Input.GetKeyDown(KeyCode.I))
            am.InstantiateSetup();
        else if (Input.GetKeyDown(KeyCode.U))
            am.DestroyAndReset();
        else if (Input.GetKeyDown(KeyCode.T))
        {
            if (am.IsTutorialStep())
                am.PerformAlgorithmTutorialStep();
            else if (am.IsTutorial())
                am.PerformAlgorithmTutorial();
            else
            {
                if (am.GetComponent<ScoreManager>().DifficultyLevel == null)
                {
                    am.SetDifficulty(Util.INTERMEDIATE);
                }
                am.PerformAlgorithmUserTest();
            }
        }
        else if (Input.GetKeyDown(KeyCode.F1))
            am.SetDifficulty(Util.BEGINNER);
        else if (Input.GetKeyDown(KeyCode.F2))
            am.SetDifficulty(Util.INTERMEDIATE);
        else if (Input.GetKeyDown(KeyCode.F3))
            am.SetDifficulty(Util.EXAMINATION);

        else if (Input.GetKeyDown(KeyCode.Q))
        {
            am.PlayerStepByStepInput(false);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            am.PlayerStepByStepInput(true);
        }
    }

    protected override void PerformAlgorithmControlVR()
    {
        throw new System.NotImplementedException();
    }


}
