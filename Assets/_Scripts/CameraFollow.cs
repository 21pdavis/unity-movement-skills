using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // distance from player
    private Vector3 positionOffset;

    // optional speed up by fixed amount
    private Vector3 currentVelocity = Vector3.zero;

    [SerializeField] private Transform targetTransform;

    [SerializeField] private float smoothTime;

    private void Awake()
    {
        // set offset to the distance vector between camera position and target position
        positionOffset = transform.position - targetTransform.position;
    }

    private void LateUpdate()
    {
        //Vector3 targetPosition = targetTransform.position + positionOffset;
        //transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, smoothTime);
    }
}
