using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class exro : MonoBehaviour
{
    public float rotationSpeed = 5.0f; // 旋转速度
    private Quaternion targetRotation; // 目标旋转
    private bool isRotating = false; // 控制是否正在旋转

    private void Update()
    {
        // 如果正在旋转，进行旋转
        if (isRotating)
        {
            RotateToTarget();
        }
    }

    // 开始旋转并设置目标角度
    public void StartRotation(float targetAngle)
    {
        targetRotation = Quaternion.Euler(0, targetAngle, 0) * transform.rotation;
        isRotating = true; // 开始旋转
    }

    // 停止旋转
    public void StopRotation()
    {
        isRotating = false; // 停止旋转
    }

    // 进行旋转
    private void RotateToTarget()
    {
        if (Quaternion.Angle(transform.rotation, targetRotation) > 0.1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
        else
        {
            transform.rotation = targetRotation; // 确保最终的旋转角度准确
            StopRotation(); // 达到目标角度后停止旋转
        }
    }
}