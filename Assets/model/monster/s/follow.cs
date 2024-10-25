using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

public class Follow : Action
{
    public Transform target; // 玩家或目标
    public float speed = 2f;

    public override TaskStatus OnUpdate()
    {
        if (target == null) return TaskStatus.Failure;

        // 移动怪物靠近目标
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        
        // 更新 Animator 的速度参数
        Animator animator = GetComponent<Animator>();
        animator.SetFloat("Speed", 6); // 设置速度参数为当前速度

        return TaskStatus.Running; // 返回运行状态
    }
}
