using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCameraFollow : MonoBehaviour
{
    public Transform target; // The target the camera is following
    public Vector3 offset = new Vector3(0f, 2f, -5f); // Offset from the target
    public float followSpeed = 10f; // Speed at which the camera follows
    public float lookSpeed = 10f; // Speed at which the camera rotates to look at the target

    private void FixedUpdate()
    {
        HandleTranslation();
        HandleRotation();
    }

    private void HandleTranslation()
    {
        Vector3 targetPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
    }

    private void HandleRotation()
    {
        Vector3 direction = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(direction, Vector3.up);
        Quaternion smoothedRotation = Quaternion.Slerp(transform.rotation, lookRotation, lookSpeed * Time.deltaTime);
        transform.rotation = smoothedRotation;
    }
}
