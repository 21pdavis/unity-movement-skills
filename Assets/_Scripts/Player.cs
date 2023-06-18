using Cinemachine.Utility;
using UnityEngine;
using UnityEngine.InputSystem;

using CallbackContext = UnityEngine.InputSystem.InputAction.CallbackContext;

public class Player : MonoBehaviour
{
    [SerializeField] Transform orientation;

    private PlayerInputActions inputActions;
    private Rigidbody rb;

    // should have this in a ScriptableObject
    private const float speed = 5f;

    // Start is called before the first frame update
    void Awake()
    {
        inputActions = new PlayerInputActions();
        rb = GetComponent<Rigidbody>();
    }

    public void Jump(CallbackContext context)
    {
        if (context.performed)
        {
            rb.AddForce(Vector3.up * speed, ForceMode.Impulse);
        }
    }

    public void Move(CallbackContext context)
    {
        if (context.performed)
        {
            Vector2 direction = context.ReadValue<Vector2>().normalized;
            Vector3 move = (orientation.forward * direction.y * speed + orientation.right * direction.x * speed);
            rb.AddForce(move, ForceMode.Impulse);
        }
    }
}
