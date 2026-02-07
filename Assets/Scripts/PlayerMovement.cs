using ExternPropertyAttributes;
using System;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float DistanceTillGround = 1;

    [SerializeField, Range(0f, 90f)]
    public float maxGroundAngle = 25f;
    [ReadOnly]
    [AllowNesting]
    public float minGroundDotProduct;

    [SerializeField, Range(0f, 90f)]
    public float minSlopeAngle = 25f;
    [ReadOnly]
    [AllowNesting]
    public float minSlopeDotProduct;
    public int groundContactCount;
    bool grounded, onSlope;
    [ReadOnly]
    [AllowNesting]
    public Vector3 contactNormal;

    [ReadOnly]
    [AllowNesting]
    public Vector3[] allContactNormals;



    public float cameraSensitivityX, cameraSensitivityY;
    public bool invertYCamera, invertXCamera;
    public float moveSpeed, jumpPower;
    public float CustomGravity = -9.81f;
    Rigidbody rb;

    InputSystem_Actions inputSystemActions;
    InputManager inputManager;

    //for now the player camera is the main camera
    public Camera playerCamera;

    void Awake()
    {
        inputSystemActions = new InputSystem_Actions();
        inputManager = new InputManager(new InputAction[] {
                                            inputSystemActions.Player.Move,
                                            inputSystemActions.Player.Jump,
                                            inputSystemActions.Player.Interact,
                                            inputSystemActions.Player.Look
                                            });
        inputManager.AddActionToInput(inputSystemActions.Player.Jump, Jump);

        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void FixedUpdate()
    {
        ApplyGravity();
    }

    // Update is called once per frame
    void Update()
    {
        CheckGrounded();
        MoveInDirection();
        RotateView();
    }

    void MoveInDirection()
    {
        Vector2 moveDirV2 = inputSystemActions.Player.Move.ReadValue<Vector2>();
        Vector3 moveDir = new Vector3(playerCamera.transform.forward.x, 0, playerCamera.transform.forward.z).normalized * moveDirV2.y + new Vector3(playerCamera.transform.right.x, 0, playerCamera.transform.right.z).normalized * moveDirV2.x;
        rb.MovePosition(rb.position + (moveDir * moveSpeed * Time.deltaTime));
    }

    void RotateView()
    {
        Vector2 mouseVector = inputSystemActions.Player.Look.ReadValue<Vector2>();
        Debug.Log(mouseVector);
        float yRotation = playerCamera.transform.localRotation.eulerAngles.y + (mouseVector.x * cameraSensitivityX * Time.deltaTime * (invertXCamera ? -1 : 1));
        float xRotation = playerCamera.transform.localRotation.eulerAngles.x + (mouseVector.y * cameraSensitivityY * Time.deltaTime * (invertYCamera ? -1 : 1));

        if (xRotation > 180)
            xRotation = xRotation - 360;

        if(xRotation > 60)
            xRotation = 60;
        if(xRotation < -60) 
            xRotation = -60;

        playerCamera.transform.localRotation = Quaternion.Euler(xRotation,yRotation, playerCamera.transform.localRotation.eulerAngles.z);
    }

    void ApplyGravity()
    {
        if (grounded)
            return;

        rb.AddForce(Vector3.up * CustomGravity);
    }

    void Jump(InputAction.CallbackContext context)
    {
        if (!grounded)
            return;
        rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
    }

    private void OnEnable()
    {
        inputManager.WhenEnabled();
    }

    private void OnDisable()
    {
        inputManager.WhenDisabled();
    }

    private void OnCollisionEnter(Collision collision)
    {
        EvaluateCollision(collision);
    }

    private void OnCollisionStay(Collision collision)
    {
        EvaluateCollision(collision);
    }

    private void OnCollisionExit(Collision collision)
    {
        EvaluateCollision(collision);
    }

    void EvaluateCollision(Collision collision)
    {
        onSlope = false;

        allContactNormals = new Vector3[collision.contactCount];
        groundContactCount = 0;
        for (int i = 0; i < collision.contactCount; i++)
        {

            Vector3 normal = collision.GetContact(i).normal;
            allContactNormals[i] = normal;

            if (normal.y >= minGroundDotProduct)
            {
                if (normal.y <= minSlopeDotProduct)
                    onSlope = true;

                groundContactCount++;
                contactNormal += normal;
            }
        }
        if (groundContactCount > 1)
            contactNormal.Normalize();
        else if (groundContactCount == 0)
            contactNormal = Vector3.zero;
    }

    void CheckGrounded()
    {
        if (groundContactCount > 0)
            grounded = true;
        else
            grounded = false;
    }
}
