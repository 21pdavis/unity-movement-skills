using UnityEngine;

// https://www.youtube.com/watch?v=UCwwn2q4Vys

public class ThirdPersonCam : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Transform player;
    public Transform playerObj;
    public Rigidbody rb;
    public float rotationSpeed;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // TODO: Maybe move into InputActions Handler?+
    private void Update()
    {
        // rotate orientation (get vector in xz-plane for where camera is pointing
        Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        // Vector3.normalized = unit vector
        orientation.forward = viewDir.normalized;

        // rotate player object
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (inputDir != Vector3.zero)
        {
            playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
        }
    }
}
