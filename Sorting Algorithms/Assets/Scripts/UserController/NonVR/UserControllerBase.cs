using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class UserControllerBase : MonoBehaviour {

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

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();    
    }

	// Update is called once per frame
	protected virtual void Update () {
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

    protected virtual void PerformAlgorithmControl()
    {

    }

    protected virtual void PerformAlgorithmControlVR()
    {

    }
}
