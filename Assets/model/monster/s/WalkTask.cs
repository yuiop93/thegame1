using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class WalkTask : Action
{
    private Animator animator; // 动画控制器
    private Transform enemyTransform; // 敌人变换
    private Vector3 sidestepTarget; // 偏移目标位置
    private NavMeshAgent navMeshAgent; // NavMesh代理
    public float sidestepDistance = 1.5f; // 左右偏移距离
    public float forwardStep = 0.5f; // 向前偏移距离 (沿 Z 轴)
    public float sidestepSpeed = 2.0f; // 偏移速度
    private bool isLeftStep = true; // 控制左右偏移方向的标志

    public override void OnStart()
    {
        animator = GetComponent<Animator>();
        enemyTransform = transform;
        navMeshAgent = GetComponent<NavMeshAgent>();
        
        // 禁用 NavMeshAgent 以避免干扰偏移行为
        navMeshAgent.enabled = false; 

        StartSidestep();
    }

    public override TaskStatus OnUpdate()
    {
        // 执行偏移
        Sidestep();
        
        // 检查是否到达目标
        if (Vector3.Distance(enemyTransform.position, sidestepTarget) < 0.1f)
        {
            animator.SetBool("isWalking", false);
            return TaskStatus.Success;
        }

        return TaskStatus.Running;
    }

    private void StartSidestep()
    {
        // 启用 NavMeshAgent
        navMeshAgent.enabled = true;

        // 设置左右偏移方向
        float direction = isLeftStep ? -1 : 1;
        Vector3 rightOffset = enemyTransform.right * direction * sidestepDistance;

        // 设置 Z 轴正向（前进方向）偏移
        Vector3 forwardOffset = new Vector3(0, 0, forwardStep);

        // 计算最终的 sidestepTarget
        sidestepTarget = enemyTransform.position + rightOffset + forwardOffset;
        isLeftStep = !isLeftStep; // 切换左右偏移方向
        animator.SetBool("isWalking", true);

        // 使用 NavMeshAgent 设置目标位置
        navMeshAgent.SetDestination(sidestepTarget);
    }

    private void Sidestep()
    {
        // 平滑移动到目标位置
        enemyTransform.position = Vector3.MoveTowards(enemyTransform.position, sidestepTarget, Time.deltaTime * sidestepSpeed);
    }

    public override void OnEnd()
    {
        // 在偏移结束时，重新启用 NavMeshAgent
        if (navMeshAgent != null)
        {
            navMeshAgent.enabled = true;
        }
    }
}
