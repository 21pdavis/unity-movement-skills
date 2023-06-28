using UnityEngine;
using CallbackContext = UnityEngine.InputSystem.InputAction.CallbackContext;

public class AnimationStateController : MonoBehaviour
{
    Animator animator;
    Player player;

    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
        player = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal void Jump()
    {
    
    }

    internal void Move()
    {
        bool isWalking = animator.GetBool("isWalking");
        bool isRunning = animator.GetBool("isRunning");
        bool moving = (player.playerDirection.x != 0 || player.playerDirection.z != 0);

        // if idle and not already running and moving at all, walk
        if (!isWalking && moving)
        {
            animator.SetBool("isWalking", true);
        }
        else if (isWalking && !moving)
        {
            animator.SetBool("isWalking", false);
        }

        Debug.Log($"isSprinting: {player.isSprinting}");
        if (!isRunning && moving && player.isSprinting)
        {
            animator.SetBool("isRunning", true);
        }
        else if (isRunning && !player.isSprinting)
        {
            animator.SetBool("isRunning", false);
        }
    }
}
