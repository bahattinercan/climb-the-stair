using UnityEngine;

public class SmoothCameraFollow : MonoBehaviour
{
    private Vector3 offset;
    [SerializeField] private Transform target;
    [SerializeField] private float smoothTime;
    private Vector3 currentVelocity = Vector3.zero;

    private void Awake()
    {
        offset = transform.position - new Vector3(0, target.position.y, 0);
    }

    private void LateUpdate()
    {
        Vector3 targetPosition = new Vector3(0, target.position.y, 0) + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, smoothTime);
    }
}