using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class LookAtPlayerService : Action
{
    private Transform player;
    public float rotationSpeed = 5.0f;
    public SharedFloat extraRotationAngle;

    public override void OnStart()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public override TaskStatus OnUpdate()
    {
        if (player == null) return TaskStatus.Failure;
        LookAtPlayer();
        return TaskStatus.Running;
    }

    private void LookAtPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0;
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            lookRotation *= Quaternion.Euler(0, extraRotationAngle.Value, 0); 
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }
    }
}
