using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class MoveToTargetTask : Action
{
    private Animator animator; // 动画控制器
    private NavMeshAgent navMeshAgent; // NavMesh代理
    private Collider enemyCollider; // 敌人的碰撞器
    public Transform target; // 目标位置，可以在Inspector中设置
    public Transform player; // 玩家对象，可以在Inspector中设置
    public float arrivalThreshold = 0.2f; // 到达目标的距离阈值
    private bool isFastMove = false; // 瞬移状态标志

    public override void OnStart()
    {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        enemyCollider = GetComponent<Collider>();

        // 如果没有设置目标点，默认将目标点设置为玩家的位置
        if (target == null && player != null)
        {
            target = player; // 将目标点设置为玩家的 Transform
        }

        if (navMeshAgent != null && target != null)
        {
            // 禁用碰撞器，以便无视物体碰撞
            if (enemyCollider != null)
            {
                enemyCollider.enabled = false;
            }

            // 开启瞬移
            isFastMove = true;
            
            // 播放瞬移动画
            animator.SetBool("isFastMove", true);

            // 使用协程来实现延迟移动，模拟瞬移效果
            StartCoroutine(TeleportToTarget());
        }
    }

    private IEnumerator TeleportToTarget()
    {
        // 短暂延迟以匹配动画播放时机
        yield return new WaitForSeconds(0.1f);

        if (navMeshAgent != null && target != null)
        {
            // 使用NavMeshAgent的Warp方法直接传送到目标位置
            navMeshAgent.Warp(target.position);
        }

        // 关闭瞬移标志
        isFastMove = false;
        animator.SetBool("isFastMove", false); // 确保动画停止

        // 恢复碰撞器
        if (enemyCollider != null)
        {
            enemyCollider.enabled = true;
        }
    }

    public override TaskStatus OnUpdate()
    {
        // 如果目标不存在或NavMeshAgent没有初始化，直接返回失败
        if (target == null || navMeshAgent == null)
        {
            return TaskStatus.Failure;
        }

        // 检查是否已到达目标位置
        float distanceToTarget = Vector3.Distance(transform.position, target.position);
        if (distanceToTarget <= arrivalThreshold)
        {
            return TaskStatus.Success;
        }

        // 如果瞬移尚未完成，返回运行中
        return isFastMove ? TaskStatus.Running : TaskStatus.Success;
    }

    public override void OnEnd()
    {
        // 确保任务结束时瞬移状态关闭
        isFastMove = false;
        animator.SetBool("isFastMove", false); // 确保动画已停止

        // 恢复碰撞器
        if (enemyCollider != null)
        {
            enemyCollider.enabled = true;
        }
    }
}
