using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SitOnChair : MonoBehaviour
{
    public Transform sitPosition; // 椅子的坐下位置
    public Animator playerAnimator; // 玩家角色的动画控制器
    public KeyCode interactKey = KeyCode.E; // 互动按键

    private bool isSitting = false; // 判断玩家是否正在坐下
    private CharacterController characterController; // 玩家角色的 CharacterController

    void Start()
    {
        // 获取 CharacterController 组件
        characterController = GetComponent<CharacterController>();

        // 检查 CharacterController 是否存在
        if (characterController == null)
        {
            Debug.LogError("CharacterController 未找到！");
        }
    }

    void Update()
    {
        // 检测按键输入，确保玩家还未坐下
        if (Input.GetKeyDown(interactKey) && !isSitting)
        {
            Debug.Log("按下 E 键，开始坐下");
            SitDown();
        }

        // 检测站起的输入
        if (Input.GetKeyDown(KeyCode.Z) && isSitting)
        {
            StandUp();
        }
    }

    void SitDown()
    {
        // 检查是否有正确设置 sitPosition
        if (sitPosition != null)
        {
            // 移动玩家到椅子的坐下位置
            Debug.Log("移动玩家到坐下位置：" + sitPosition.position);
            
            // 使用 CharacterController 移动到坐下位置
            Vector3 targetPosition = sitPosition.position;
            Vector3 moveDirection = (targetPosition - transform.position).normalized; // 计算方向
            
            // 确保角色在一定的高度
            targetPosition.y = transform.position.y; // 保持 Y 轴高度不变
            
            // 移动角色
            characterController.Move(moveDirection * Time.deltaTime); // 注意，这里可以通过时间来控制移动速度
            
            // 播放坐下动画
            Debug.Log("触发坐下动画");
            playerAnimator.SetTrigger("Sit");

            // 设置玩家为正在坐下状态
            isSitting = true;
        }
        else
        {
            Debug.LogError("SitPosition 未正确设置！");
        }
    }

    void StandUp()
    {
        // 播放站起动画
        Debug.Log("触发站起动画");
        playerAnimator.SetTrigger("Stand");

        // 重置 isSitting 状态
        isSitting = false;
    }
}
