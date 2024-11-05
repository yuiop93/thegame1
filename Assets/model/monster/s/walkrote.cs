using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class walkrote : MonoBehaviour
{
    public float extraRotationAngle = 0.0f; // 额外旋转角度
    private bool isRotating = false;  // 控制是否旋转
    public float rotationSpeed = 5.0f; // 旋转速度

    private void Update()
    {
        // 如果在旋转状态，持续应用额外旋转
        if (isRotating)
        {
            ApplyExtraRotation();
        }
    }

    // 开始额外旋转
    public void StartRotation()  
    {
        isRotating = true;
    }

    // 停止额外旋转
    public void StopRotation()  
    {
        isRotating = false;
    }

    // 应用额外旋转
    private void ApplyExtraRotation()
    {
        // 根据当前方向进行额外旋转
        Quaternion extraRotation = Quaternion.Euler(0, extraRotationAngle, 0);
        Quaternion targetRotation = transform.rotation * extraRotation; // 目标旋转

        // 平滑旋转到目标方向
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

        // 如果接近目标角度，停止旋转
        if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
        {
            transform.rotation = targetRotation; // 确保最终的旋转角度准确
            StopRotation(); // 达到目标角度后停止旋转
        }
    }
}