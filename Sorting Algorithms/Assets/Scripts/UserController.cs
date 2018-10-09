using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class UserController : MonoBehaviour {

    /* -------------------------------------------- User Control --------------------------------------------
     * 
     * 
    */

    // User control variables (movement)
    [SerializeField]
    private float movementSpeed = 15f, mouseSensitivity = 3f;

    // Camera rotation (mouse)
    private float currentCameraRotationX, cameraRotationLimit = 85f;

    [SerializeField]
    private Camera cam;

    private Rigidbody rb;

    [SerializeField]
    private GameObject AMObject;
    private AlgorithmManagerBase am;

    void Awake()
    {
        am = AMObject.GetComponent(typeof(AlgorithmManagerBase)) as AlgorithmManagerBase;        
    }

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        // Apply movement
        PerformMovement();

        // Apply Rotation on camera
        PerformRotation();

        // Algorithm control
        PerformAlgorithmControl();
	}

    /* ---------------------------------------------------------------------------------------------------------------
    * Movement control
    * Allows the player to use WASD and arrow keys, a controller pad or other devices to move the player, by default
    * ---------------------------------------------------------------------------------------------------------------
    */
    private void PerformMovement()
    {
        float x = Input.GetAxis("Horizontal") * Time.deltaTime * movementSpeed;
        float z = Input.GetAxis("Vertical") * Time.deltaTime * movementSpeed;

        transform.Translate(x, 0f, z);
    }

    /* ---------------------------------------------------------------------------------------------------------------
     * Mouse control
     * Moving on X-axis, so rotating around Y-axis
     * ---------------------------------------------------------------------------------------------------------------
    */
    private void PerformRotation()
    {
        // Calculate camera rotation (turning around)
        float yRot = Input.GetAxisRaw("Mouse X");
        Vector3 rotation = new Vector3(0f, yRot, 0f) * mouseSensitivity;

        // Calculate camera rotation (tilting)
        float cameraRotX = Input.GetAxisRaw("Mouse Y") * mouseSensitivity;

        rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));

        if (cam != null)
        {
            // Set our rotation and clamp it
            currentCameraRotationX -= cameraRotX;
            currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

            // Apply our rotation to the transform of our camera
            cam.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
        }
    }

    private void PerformAlgorithmControl()
    {
        if (Input.GetKeyDown(KeyCode.I))
            am.InstantiateSetup();
        else if (Input.GetKeyDown(KeyCode.U))
            am.DestroyAndReset();
        else if (Input.GetKeyDown(KeyCode.T))
        {
            if (am.IsTutorial)
                am.PerformAlgorithmTutorial();
            else
            {
                if (am.GetComponent<ScoreManager>().DifficultyLevel == null)
                {
                    am.GetComponent<ScoreManager>().SetDifficulty(ScoreManager.INTERMEDIATE);
                }
                am.PerformAlgorithmUserTest();

            }
        }
        else if (Input.GetKeyDown(KeyCode.F1))
            am.GetComponent<ScoreManager>().SetDifficulty(ScoreManager.BEGINNER);
        else if (Input.GetKeyDown(KeyCode.F2))
            am.GetComponent<ScoreManager>().SetDifficulty(ScoreManager.INTERMEDIATE);
        else if (Input.GetKeyDown(KeyCode.F3))
            am.GetComponent<ScoreManager>().SetDifficulty(ScoreManager.PRO);
    }


    private void PerformAlgorithmControlVR()
    {

    }


}
