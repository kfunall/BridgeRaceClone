using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Vector3 offset;
    [SerializeField] float lerpValue = 2f;
    [SerializeField] Vector3 endPosition; // 0,9,45

    private void LateUpdate()
    {
        if (!GameManager.Instance.GameEnded)
            FollowPlayer();
    }
    private void FollowPlayer()
    {
        Vector3 desiredPosition = target.transform.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, lerpValue);
        transform.position = smoothedPosition;
    }
    public void End()
    {
        transform.position = endPosition;
    }
}