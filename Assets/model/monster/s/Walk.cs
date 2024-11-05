using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class Walk : Action
{
    public float walkSpeed = 1.0f; // 步行速度
    private Animator animator;
    private Transform player;
    private Vector3 targetPosition;

    public override void OnStart()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        
        // 確定走動的目標位置，這裡可以根據需求調整
        targetPosition = player.position + (player.position - transform.position).normalized * 2; // 假設向玩家移動
    }

    public override TaskStatus OnUpdate()
    {
        // 計算移動方向
        Vector3 direction = (targetPosition - transform.position).normalized;

        if (direction != Vector3.zero)
        {
            // 移動角色
            transform.position += direction * walkSpeed * Time.deltaTime;

            // 更新動畫
            animator.SetFloat("walkSpeed", walkSpeed);

            // 檢查是否到達目標位置
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                return TaskStatus.Success; // 成功完成移動
            }
        }
        return TaskStatus.Running; // 繼續執行
    }
}
