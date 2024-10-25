using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

public class AttackTowardsPlayer : Action
{
    [BehaviorDesigner.Runtime.Tasks.Tooltip("The gun muzzle transform.")]
    public Transform gunMuzzle; // 敌人枪口
    [BehaviorDesigner.Runtime.Tasks.Tooltip("The target player transform.")]
    public Transform player;    // 玩家目标

    // 设定一个角速度，用来平滑旋转
    public float rotationSpeed = 5.0f;

    private Animator animator;  // 动画控制

    public override void OnStart()
    {
        animator = GetComponent<Animator>();
        if (gunMuzzle == null || player == null)
        {
            Debug.LogError("Gun Muzzle or Player not assigned.");
        }
    }

    public override TaskStatus OnUpdate()
    {
        // 仅当攻击状态时执行旋转和瞄准操作
        if (animator.GetBool("isAttacking"))
        {
            RotateTowardsPlayer();
            return TaskStatus.Running; // 继续运行
        }

        return TaskStatus.Success; // 如果没有攻击，返回成功
    }

    // 旋转敌人和枪口，使其朝向玩家
    private void RotateTowardsPlayer()
    {
        // 获取枪口到玩家的方向向量
        Vector3 directionToPlayer = (player.position - gunMuzzle.position).normalized;

        // 获取枪口当前的前向向量 (x 轴方向)
        Vector3 currentGunForward = gunMuzzle.right;

        // 计算枪口需要旋转的角度
        float angleDifference = Vector3.SignedAngle(currentGunForward, directionToPlayer, Vector3.up);

        // 控制敌人整体水平旋转，使枪口朝向玩家
        float rotationStep = angleDifference * Time.deltaTime * rotationSpeed;
        gunMuzzle.parent.Rotate(0, rotationStep, 0); // 假设枪口的父对象是角色

        // 确保枪口的 x 轴对准玩家
        gunMuzzle.right = Vector3.Lerp(gunMuzzle.right, directionToPlayer, Time.deltaTime * rotationSpeed);
    }

    public override void OnEnd()
    {
        // 结束时可以重置状态或执行其他逻辑
    }
}
