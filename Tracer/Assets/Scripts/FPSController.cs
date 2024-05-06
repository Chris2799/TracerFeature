using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    Vector3 moveDirection = Vector3.zero;
    
    float rotationX = 0;

    public bool willMove = true;

    CharacterController characterController;


    
    void Start()
    {
        //Controller setup and hide the cursor.
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

   
    void Update()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        //this is the running input
        bool goingToRun = Input.GetKey(KeyCode.LeftShift);
        float cursorSpeedX = willMove ? (goingToRun ? runSpeed : movementSpeed) * Input.GetAxis("Vertical") : 0;
        float cursorSpeedY = willMove ? (goingToRun ? runSpeed : movementSpeed) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * cursorSpeedX) + (right * cursorSpeedY);


        //this is the jumping input
        if (Input.GetButton("Jummp") && willMove && characterController.isGrounded)
        {
            moveDirection.y = jumpHeight;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }
        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }



        //this is controller camera rotation
    }
}
