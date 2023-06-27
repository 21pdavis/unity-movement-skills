using Mono.Cecil.Cil;
using UnityEngine;
using CallbackContext = UnityEngine.InputSystem.InputAction.CallbackContext;

public class Player : MonoBehaviour
{
    //[SerializeField] Transform orientation;
    [SerializeField] Transform followCamera;

    private CharacterController playerController;

    // should have this in a ScriptableObject
    private const float playerSpeed = 5f;
    private const float gravity = -9.81f;
    private float verticalVelocity;
    private bool isGrounded;

    private float verticalInput = 0f;
    private float horizontalInput = 0f;
    private float playerHeight;
    private int jumps = 0;

    // passive movement fields
    [Header("Movement Fields")]
    [SerializeField] private float gravityMultiplier = 1f;
    [SerializeField] private Vector3 playerDirection;
    [SerializeField] private float verticalTerminalVelocity = 120f;


    // Awake is called before the first frame update
    private void Awake()
    {
        playerController = GetComponent<CharacterController>();

        playerHeight = playerController.height;
    }

    private void Update()
    {
        GroundedCheck();
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

    private void UpdateMove()
    {
        verticalInput = playerDirection.y;
        horizontalInput = playerDirection.x;

        // Calculate movement relative to camera
        Vector3 cameraForward = Vector3.ProjectOnPlane(followCamera.forward, Vector3.up).normalized;
        Vector3 cameraRight = Vector3.ProjectOnPlane(followCamera.right, Vector3.up).normalized;
        Vector3 cameraOrientedPlayerDirection = (cameraForward * verticalInput + cameraRight * horizontalInput);

        playerController.Move(cameraOrientedPlayerDirection * playerSpeed * Time.deltaTime + Vector3.up * verticalVelocity * Time.deltaTime);
    }

    private void GroundedCheck()
    {
        LayerMask groundLayer = 1 << 6 | 1 << 7;
        RaycastHit hit;

        float raycastDistance = playerHeight / 2 + 0.1f;

        isGrounded = Physics.Raycast(transform.position, Vector3.down, out hit, raycastDistance, groundLayer);

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
            playerDirection = context.ReadValue<Vector2>();
        }
    }
}
