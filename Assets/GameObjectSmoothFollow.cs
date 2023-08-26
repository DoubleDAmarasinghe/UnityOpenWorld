using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectSmoothFollow : MonoBehaviour
{

    public Transform target;
    public float smoothSpeed = 0.125f;
    public float distance = 2f;
    public LayerMask obstacleLayer;

    private Vector3 offset;
    private Vector3 desiredPosition;
    private Quaternion desiredRotation;

    private void Start()
    {
        offset = transform.position - target.position;
    }

    private void LateUpdate()
    {
        Vector3 rotatedOffset = Quaternion.Euler(target.rotation.eulerAngles.x, target.rotation.eulerAngles.y, 0f) * new Vector3(0f, 0f, -distance);
        desiredPosition = target.position + rotatedOffset;

        // Cast a ray from the camera to the desired position
        Vector3 cameraPosition = Camera.main.transform.position;
        Vector3 cameraToTarget = desiredPosition - cameraPosition;
        Ray ray = new Ray(cameraPosition, cameraToTarget.normalized);
        RaycastHit hit;
        bool hasHit = Physics.Raycast(ray, out hit, cameraToTarget.magnitude, obstacleLayer);

        // If the ray hit an obstacle, move the desired position to the point of intersection
        if (hasHit)
        {
            desiredPosition = hit.point;
        }

        // Move the object to the desired position
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Set the object's rotation to face the target
        Vector3 upDirection = target.up;
        desiredRotation = Quaternion.LookRotation(target.position - transform.position, upDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, smoothSpeed);
    }
}
