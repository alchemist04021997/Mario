using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    Vector3 localPosition, position, rootCameraPosition;
    private void Awake()
    {
        rootCameraPosition = Camera.main.transform.position;
        localPosition = rootCameraPosition - transform.position;
    }
    private void FixedUpdate()
    {
        position = Vector3.Lerp(Camera.main.transform.position, transform.position + localPosition, 0.1f);
        position.y = rootCameraPosition.y;
        position.z = rootCameraPosition.z;
        Camera.main.transform.position = position;
    }
}
