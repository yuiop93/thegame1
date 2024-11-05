using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinuousRotationControl : MonoBehaviour
{
    private bool isRotating = false;  // 控制是否旋转
    private float targetAngle; // 目标额外旋转角度
    public float rotationSpeed = 5.0f; // 旋转速度

    private void Update()
    {
        // 如果在旋转状态，持续应用额外旋转
        if (isRotating)
        {
            ApplyExtraRotation();
        }
    }

    // 设置 -30 度旋转
    public void SetRotationAngleNegative30()
    {
        targetAngle = -30f;
        StartRotation();
    }

    // 设置 -60 度旋转
    public void SetRotationAngleNegative60()
    {
        targetAngle = -60f;
        StartRotation();
    }
    public void SetRotationAngleNegative3()
    {
        targetAngle = 65f;
        StartRotation();
    }

    public void StartRotation()
    {
        isRotating = true;
    }

    public void StopRotation()
    {
        isRotating = false;
    }

    private void ApplyExtraRotation()
    {
        Quaternion extraRotation = Quaternion.Euler(0, targetAngle, 0);
        Quaternion targetRotation = transform.rotation * extraRotation; // 目标旋转

        // 平滑旋转到目标方向
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

        // 如果接近目标角度，停止旋转
        if (Quaternion.Angle(transform.rotation, targetRotation) < 0.5f) // 增加缓冲值
        {
            transform.rotation = targetRotation; // 确保最终的旋转角度准确
            StopRotation(); // 达到目标角度后停止旋转
        }
    }
}
