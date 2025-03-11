using System;
using Unity.Cinemachine;
using UnityEngine;

public class TargetGroupCamera : MonoBehaviour
{
    private CinemachineTargetGroup targetGroup;
    [SerializeField] private CinemachinePositionComposer positionComposer;
    private bool rightRotation;
    private bool leftRotation;

    void Awake()
    {
        targetGroup = GetComponent<CinemachineTargetGroup>();
    }
    void Update()
    {
        rightRotation = true;
        leftRotation = true;
        foreach (var target in targetGroup.Targets)
        {
            float rotationY = target.Object.rotation.eulerAngles.y;
            // Debug.Log($"Target Camera Rotation Y: {rotationY}");
            if (Mathf.Abs(rotationY) > 0.01f && Mathf.Abs(rotationY - 360) > 0.01f) 
            {
                rightRotation = false;
            }
            
            if (Mathf.Abs(rotationY + 180) > 0.01f && Mathf.Abs(rotationY - 180) > 0.01f) 
            {
                leftRotation = false;
            }
        }

        positionComposer.TargetOffset = new Vector3(5, 0, 0);
        if (rightRotation)
        {
            // Debug.Log("Camera Right Rotation");
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (leftRotation)
        {
            // Debug.Log("Camera Left Rotation");
            transform.rotation = Quaternion.Euler(0, -180, 0);
        } else
        {
            // Debug.Log("Camera No Rotation");
            positionComposer.TargetOffset = new Vector3(0, 0, 0);
        }
    }
}
