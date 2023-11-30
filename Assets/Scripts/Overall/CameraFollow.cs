using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    [Header("Camera Reference")]
    [SerializeField] private Transform target;
    [SerializeField] private float yOffSet = 2f;

    void Update()
    {
        transform.position = new Vector3(target.position.x, target.position.y + yOffSet ,transform.position.z);
    }
}
