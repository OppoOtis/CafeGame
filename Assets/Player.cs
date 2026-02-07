using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float DistanceTillGround = 1;

    bool grounded;

    public float cameraSensitivityX, cameraSensitivityY;
    public bool invertYCamera, invertXCamera;
    public float moveSpeed;
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

        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        grounded = CheckIfGrounded();

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
        //xRotation = Mathf.Clamp(xRotation, -60, 60);

        playerCamera.transform.localRotation = Quaternion.Euler(xRotation,yRotation, playerCamera.transform.localRotation.eulerAngles.z);
    }


    bool CheckIfGrounded()
    {
        if (Physics.Raycast(transform.position, Vector3.down, DistanceTillGround))
            return true;

        else
            return false;
    }

    private void OnEnable()
    {
        inputManager.WhenEnabled();
    }

    private void OnDisable()
    {
        inputManager.WhenDisabled();
    }
}
