using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This is the FPS controller setup for the Tracer Moveset Feature
/// </summary>

[RequireComponent(typeof(CharacterController))]
public class FPSController : MonoBehaviour
{

    public Camera playerCamera;
    public float movementSpeed = 6f;
    public float runSpeed = 12f;
    public float jumpHeight = 7f;
    public float gravity = 10f;
    public float lookSpeed = 2f;
    public float lookXLimit = 45f;
    public float defaultHeight = 2f;
    public float crouchHeight = 1f;
    public float crouchSpeed = 3f;


    private Vector3 moveDiretion = Vector3.zero;
    private float rotationX = 0;
    private bool willMove = true;
    private CharacterController characterController;

    


    
    void Start()
    {
       characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

   
    void Update()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float cursorSpeedX = willMove ? (isRunning ? runSpeed : movementSpeed) * Input.GetAxis("Vertical") : 0;
        float cursorSpeedY = willMove ? (isRunning ? runSpeed : movementSpeed) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDiretion.y;
        moveDiretion = (forward * cursorSpeedX) + (right * cursorSpeedY);

        if (Input.GetButton("Jump") && willMove && characterController.isGrounded)
        {
            moveDiretion.y = jumpHeight;
        }
        else
        {
            moveDiretion.y = movementDirectionY;
        }
        if (!characterController.isGrounded)
        {
            moveDiretion.y -= gravity * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.R) && willMove)
        {
            characterController.height = crouchHeight;
            movementSpeed = crouchSpeed;
            runSpeed = crouchSpeed;
        }
        else
        {
            characterController.height = defaultHeight;
            movementSpeed = 6f;
            runSpeed = 12f;
        }

        characterController.Move(moveDiretion * Time.deltaTime);

        if (willMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }
       
    }

    
}
