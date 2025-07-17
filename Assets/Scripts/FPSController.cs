using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FPSController : MonoBehaviour
{
    [Header("Movement Speeds")]
    public float walkingSpeed = 3f;
    public float runningSpeed = 6f;

    [Header("Jump / Gravity")]
    public float jumpSpeed = 8f;
    public float gravity = 20f;

    [Header("Camera Look")]
    public Camera playerCamera;
    public float lookSpeed = 2f;
    public float lookXLimit = 45f;

    [Header("Cursor")]
    public bool lockCursorOnStart = true;

    [Header("Character Controller Overrides")]
    public bool overrideStepOffset = true;
    public float stepOffsetValue = 0.6f;

    [Header("Debug")]
    public bool enableDebugLogs = false;

    private CharacterController characterController;
    private float rotationX = 0f;
    private float verticalVelocity = 0f;

    [HideInInspector] public bool canMove = true;

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        if (overrideStepOffset)
        {
            characterController.stepOffset = stepOffsetValue;
        }

        if (lockCursorOnStart)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        if (playerCamera == null)
        {
            playerCamera = GetComponentInChildren<Camera>();
        }
    }

    void Update()
    {
        HandleLook();
        HandleMove();
    }

    void HandleLook()
    {
        if (!canMove || playerCamera == null) return;

        rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);

        float mouseX = Input.GetAxis("Mouse X") * lookSpeed;
        transform.rotation *= Quaternion.Euler(0f, mouseX, 0f);
    }

    void HandleMove()
    {
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float targetSpeed = isRunning ? runningSpeed : walkingSpeed;


        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        float inputV = canMove ? Input.GetAxis("Vertical") : 0f;
        float inputH = canMove ? Input.GetAxis("Horizontal") : 0f;

        Vector3 planarMove = forward * inputV + right * inputH;
        if (planarMove.sqrMagnitude > 1f)
            planarMove.Normalize();
        planarMove *= targetSpeed;

        if (characterController.isGrounded)
        {
            if (verticalVelocity < 0f) verticalVelocity = -1f;

            if (canMove && Input.GetButton("Jump"))
            {
                verticalVelocity = jumpSpeed;
            }
        }
        else
        {
            verticalVelocity -= gravity * Time.deltaTime;
        }

        Vector3 move = new Vector3(planarMove.x, verticalVelocity, planarMove.z);
        characterController.Move(move * Time.deltaTime);

        if (enableDebugLogs && (Mathf.Abs(inputV) > 0.01f || Mathf.Abs(inputH) > 0.01f))
        {
            Debug.Log($"[FPSController] run={isRunning} speed={targetSpeed} planar={planarMove.magnitude:F2} stepOffset={characterController.stepOffset}");
        }
    }
}