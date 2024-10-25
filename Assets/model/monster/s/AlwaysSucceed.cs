using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

public class AlwaysSucceed : Conditional
{
    public override TaskStatus OnUpdate()
    {
        Debug.Log("AlwaysSucceed任务被调用，返回成功！");
        return TaskStatus.Success; // 始终返回成功
    }
}