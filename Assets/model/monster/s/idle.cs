using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

public class Idle : Action
{
    public override TaskStatus OnUpdate()
    {
        Animator animator = GetComponent<Animator>();
        animator.SetFloat("Speed", 0); // 设置速度参数为0
        return TaskStatus.Success;
    }
}
