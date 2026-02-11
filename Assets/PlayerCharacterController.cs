using ExternPropertyAttributes;
using System;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows.Speech;

public class PlayerCharacterController : MonoBehaviour
{
    public float DistanceTillGround = 1;
    CharacterController characterController;
    bool grounded;

    public float cameraSensitivityX, cameraSensitivityY;
    public bool invertYCamera, invertXCamera;
    public float moveSpeed, jumpPower;
    public float CustomGravity = -9.81f;
    Rigidbody rb;

    bool readyToJump;
    Vector3 velocity;

    InputSystem_Actions inputSystemActions;
    InputManager inputManager;

    //for now the player camera is the main camera
    public Camera playerCamera;

    void Awake()
    {
        inputSystemActions = new InputSystem_Actions();
        Blackboard.inputSystemActions = inputSystemActions;
        inputManager = new InputManager(new InputAction[] {
                                            inputSystemActions.Player.Move,
                                            inputSystemActions.Player.Jump,
                                            inputSystemActions.Player.Interact,
                                            inputSystemActions.Player.Look,
                                            inputSystemActions.Player.Attack,
                                            inputSystemActions.Player.Cancel
                                            });
        Blackboard.inputManager = inputManager;
        inputManager.AddActionToInput(inputSystemActions.Player.Jump, Jump);

        characterController = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    // Update is called once per frame
    void Update()
    {
        CheckGrounded();
        if (grounded)
            velocity = Vector3.zero;
        MoveInDirection();
        RotateView();


        if(readyToJump)
        {
            velocity.y = Mathf.Sqrt(jumpPower * -2f * CustomGravity);
            readyToJump = false;
        }

        ApplyGravity();
        characterController.Move(velocity * Time.deltaTime);
    }

    void MoveInDirection()
    {
        Vector2 moveDirV2 = inputSystemActions.Player.Move.ReadValue<Vector2>();
        Vector3 moveDir = new Vector3(playerCamera.transform.forward.x, 0, playerCamera.transform.forward.z).normalized * moveDirV2.y + new Vector3(playerCamera.transform.right.x, 0, playerCamera.transform.right.z).normalized * moveDirV2.x;
        Vector3 newVelocity = moveDir * moveSpeed;
        velocity += newVelocity;
    }

    void RotateView()
    {
        Vector2 mouseVector = inputSystemActions.Player.Look.ReadValue<Vector2>();
        float yRotation = playerCamera.transform.localRotation.eulerAngles.y + (mouseVector.x * cameraSensitivityX * Time.deltaTime * (invertXCamera ? -1 : 1));
        float xRotation = playerCamera.transform.localRotation.eulerAngles.x + (mouseVector.y * cameraSensitivityY * Time.deltaTime * (invertYCamera ? -1 : 1));

        if (xRotation > 180)
            xRotation = xRotation - 360;

        if (xRotation > 60)
            xRotation = 60;
        if (xRotation < -60)
            xRotation = -60;

        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, yRotation, playerCamera.transform.localRotation.eulerAngles.z);
    }

    void ApplyGravity()
    {
        velocity += Vector3.up * CustomGravity * Time.deltaTime;
    }

    void Jump(InputAction.CallbackContext context)
    {
        if (!grounded)
            return;
        readyToJump = true;
    }

    private void OnEnable()
    {
        inputManager.WhenEnabled();
    }

    private void OnDisable()
    {
        inputManager.WhenDisabled();
    }

    void CheckGrounded()
    {
        if (Physics.Raycast(transform.position, Vector3.down, DistanceTillGround))
            grounded = true;
        else
            grounded = false;
    }
}
