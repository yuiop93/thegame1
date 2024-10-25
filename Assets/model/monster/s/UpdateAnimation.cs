using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

public class UpdateAnimation : Action
{
    private UnityEngine.AI.NavMeshAgent navMeshAgent;
    private Animator animator;

    public override void OnStart()
    {
        // 获取 NavMeshAgent 和 Animator 组件
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    public override TaskStatus OnUpdate()
    {
        if (navMeshAgent == null || animator == null)
            return TaskStatus.Failure;

        // 取得 NavMeshAgent 的当前速度
        float speed = navMeshAgent.velocity.magnitude;

        // 根据速度更新 Animator 中的速度参数
        if (speed > 0.1f && speed < 3f)
        {
            animator.SetFloat("Speed", 2); // 设定为走路速度
        }
        else if (speed >= 3f)
        {
            animator.SetFloat("Speed", 6); // 设定为跑步速度
        }
        else
        {
            animator.SetFloat("Speed", 0); // 设定为待机速度
        }

        return TaskStatus.Running; // 返回运行状态
    }
}
