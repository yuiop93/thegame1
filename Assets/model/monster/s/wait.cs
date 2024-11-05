using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class wait : Action
{
    private Transform player;
    private float holdTime = 2.0f;  // 等待时间
    private float timer = 0.0f;
    public float rotationSpeed = 5.0f;  // 旋转速度
    public float extraRotationAngle = 10.0f;  // 额外旋转角度

    public override void OnStart()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        timer = 0.0f;  // 初始化计时器
    }

    public override TaskStatus OnUpdate()
    {
        // 等待期间保持朝向玩家
        LookAtPlayer();

        timer += Time.deltaTime;
        if (timer >= holdTime)
        {
            return TaskStatus.Success;  // 完成等待任务，进入下一个任务（Walk）
        }
        return TaskStatus.Running;
    }

    private void LookAtPlayer()
    {
        if (player == null) return;

        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0;

        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction) * Quaternion.Euler(0, extraRotationAngle, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }
    }
}