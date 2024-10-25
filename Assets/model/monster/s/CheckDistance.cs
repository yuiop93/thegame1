using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class CheckDistance : Conditional
{
    public Transform target; // 玩家或目标对象
    public float distanceThreshold = 5.0f; // 距离阈值

    public override TaskStatus OnUpdate()
    {
        // 如果没有目标，则返回失败
        if (target == null) return TaskStatus.Failure;

        // 计算怪物与目标的距离
        float distance = Vector3.Distance(transform.position, target.position);

        // 如果距离小于等于阈值，返回成功；否则返回失败
        if (distance <= distanceThreshold)
        {
            return TaskStatus.Success;
        }
        else
        {
            return TaskStatus.Failure;
        }
    }
}
