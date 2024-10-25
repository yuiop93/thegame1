using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class Attack : Action
{
    private Animator animator;
    private Transform player; // 玩家 Transform
    private float holdTime = 1.0f;  // 停留时间
    private float timer = 0.0f;
    private bool isHolding = false; // 控制是否处于停留状态
    public float rotationSpeed = 5.0f; // 旋转速度
    public float extraRotationAngle = 10.0f; // 额外旋转角度

    public override void OnStart()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform; // 假设玩家有 "Player" 标签
        StartAttack();  // 开始第一个攻击
    }

    public override TaskStatus OnUpdate()
    {
        // 获取当前动画状态信息
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (isHolding)
        {
            timer += Time.deltaTime;
            if (timer >= holdTime)
            {
                // 停留时间结束，准备重新播放下一个攻击
                isHolding = false; // 结束停留状态
                StartNextAttack(); // 播放下一个攻击动画
            }
        }
        else
        {
            if (stateInfo.normalizedTime >= 1.0f) // 当前动画播放完成
            {
                // 进入停留状态
                isHolding = true; 
                timer = 0.0f; // 重置计时器
                animator.SetBool("isAttacking", false); // 停止当前攻击动画
            }
            else
            {
                // 角色朝向玩家
                LookAtPlayer(); // 只在攻击期间调用
            }
        }

        return TaskStatus.Running;  // 始终保持运行状态，循环播放动画
    }

    private void StartAttack()
    {
        animator.SetBool("isAttacking", true);  // 播放攻击动画
        timer = 0.0f;  // 重置计时器
        isHolding = false; // 确保不处于停留状态
    }

    private void StartNextAttack()
    {
        animator.SetBool("isAttacking", true);  // 播放下一个攻击动画
        timer = 0.0f;  // 重置计时器
    }

    private void LookAtPlayer()
    {
        if (player == null) return; // 确保玩家存在

        // 计算朝向玩家的方向
        Vector3 direction = (player.position - transform.position).normalized;

        // 只保留水平旋转
        direction.y = 0; 

        if (direction != Vector3.zero) // 确保方向有效
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            lookRotation *= Quaternion.Euler(0, extraRotationAngle, 0); // 应用额外旋转
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }
    }
}
