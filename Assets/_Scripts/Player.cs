using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.InputSystem;
using CallbackContext = UnityEngine.InputSystem.InputAction.CallbackContext;

public class Player : MonoBehaviour
{
    [SerializeField] InputActionAsset playerInputActionAsset;

    [SerializeField] Transform followCamera;

    private CharacterController playerController;
    private AnimationStateController animationStateController;

    // should have this in a ScriptableObject
    private float playerSpeed = 5f;
    private float sprintMultiplier = 2f;
    private float gravity = -9.81f;
    private float verticalVelocity;
    private bool isGrounded;
    //internal bool jumping;
    internal bool isSprinting;

    private float playerHeight;
    private int jumps = 0;

    // passive movement fields
    [Header("Movement Fields")]
    [SerializeField] private float gravityMultiplier = 1f;
    [SerializeField] internal Vector3 playerDirection;
    [SerializeField] private float verticalTerminalVelocity = 120f;


    // Awake is called before the first frame update
    private void Awake()
    {
        playerController = GetComponent<CharacterController>();
        animationStateController = GetComponent<AnimationStateController>();
        playerHeight = playerController.height;
        gravity *= gravityMultiplier;

        // temp code to set control scheme to desired
        string controlSchemeName = "Keyboard&Mouse";
        InputControlScheme? controlScheme = playerInputActionAsset.FindControlScheme(controlSchemeName);
        if (controlScheme != null )
        {
            
        }
        else
        {
            Debug.LogError($"Control scheme '{controlSchemeName}' not found!");
        }
    }

    private void Update()
    {
        GroundedCheck();
        UpdateRotation();
        UpdateAnimation();
        UpdateMove();

        // handle gravity
        if (verticalVelocity < verticalTerminalVelocity)
        {
            verticalVelocity += gravity * Time.deltaTime;
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {

    }

    private void UpdateAnimation()
    {
        animationStateController.Move();
        animationStateController.Jump();
    }

    private void UpdateRotation()
    {
        transform.forward = Vector3.Slerp(transform.forward, new Vector3(followCamera.forward.x, 0f, followCamera.forward.z), 0.35f);
    }

    private void UpdateMove()
    {
        float forwardBackwardInput = playerDirection.z;
        float leftRightInput = playerDirection.x;

        // Calculate movement relative to camera
        Vector3 cameraForward = Vector3.ProjectOnPlane(followCamera.forward, Vector3.up).normalized;
        Vector3 cameraRight = Vector3.ProjectOnPlane(followCamera.right, Vector3.up).normalized;
        Vector3 cameraOrientedPlayerDirection = (cameraForward * forwardBackwardInput + cameraRight * leftRightInput);

        playerController.Move(cameraOrientedPlayerDirection * playerSpeed * Time.deltaTime + Vector3.up * verticalVelocity * Time.deltaTime);
    }

    private void GroundedCheck()
    {
        LayerMask groundLayer = 1 << 6 | 1 << 7;

        float raycastDistance = playerHeight / 2 + 0.1f;

        isGrounded = Physics.Raycast(transform.position, Vector3.down, raycastDistance, groundLayer);

        if (isGrounded)
        {
            jumps = 0;
        }
    }

    public void Jump(CallbackContext context)
    {
        if (isGrounded || jumps < 2)
        {
            verticalVelocity = Mathf.Sqrt(-(2f * gravity * 1f));
            jumps += 1;
        }
    }

    public void Move(CallbackContext context)
    {
        // move in direction of orientation
        if (context.performed || context.canceled)
        {
            Vector2 input = context.ReadValue<Vector2>();
            playerDirection = new Vector3(input.x, 0f, input.y);
        }
    }

    public void Sprint(CallbackContext context)
    {
        if (context.performed)
        {
            if (isGrounded)
            {
                //Debug.Log("is grounded");
                isSprinting = true;
                playerSpeed *= sprintMultiplier;
            }
        }
        else if (context.canceled)
        {
            isSprinting = false;
            playerSpeed /= sprintMultiplier;
        }
    }
}
