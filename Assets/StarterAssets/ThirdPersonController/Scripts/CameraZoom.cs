using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraZoom : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    public float zoomSpeed = 10f;
    public float minZoom = 15f;
    public float maxZoom = 60f;

    void Update()
    {
        // 获取滑鼠滚轮输入
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        // 获取当前的FOV (Field of View)
        float currentFOV = virtualCamera.m_Lens.FieldOfView;

        // 根据滚轮输入缩放视角
        currentFOV -= scrollInput * zoomSpeed;
        currentFOV = Mathf.Clamp(currentFOV, minZoom, maxZoom);

        // 设置新的FOV
        virtualCamera.m_Lens.FieldOfView = currentFOV;
    }
}