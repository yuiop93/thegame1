using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

public class LookAtPlayer : Action
{
    public Transform player; // 玩家 Transform
    public float rotationSpeed = 5.0f; // 旋转速度

    public override TaskStatus OnUpdate()
    {
        if (player == null)
        {
            Debug.LogWarning("Player transform is not assigned.");
            return TaskStatus.Failure; // 如果没有玩家对象，返回失败
        }

        // 计算朝向玩家的方向
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);

        // 平滑旋转
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);

        return TaskStatus.Running; // 继续运行这个任务
    }
}
