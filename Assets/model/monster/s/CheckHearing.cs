using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

public class CheckHearing : Conditional
{
    private Hearing hearingComponent; // 参考Hearing组件
    
    public override void OnStart()
    {
        hearingComponent = GetComponent<Hearing>(); 
        if (hearingComponent == null)
        {
            Debug.LogError("未找到 Hearing 组件！");
        }
        else
        {
            Debug.Log("成功找到 Hearing 组件！");
        }
    }


    public override TaskStatus OnUpdate()
    {
        if (hearingComponent == null)
        {
            Debug.LogError("未找到 Hearing 组件！");
            return TaskStatus.Failure; // 组件未找到
        }

        bool heard = hearingComponent.HasHeardNoise();
        //Debug.Log($"HasHeardNoise: {heard}"); // 添加调试信息

        if (!heard)
        {
            return TaskStatus.Running; // 如果没有听到噪音，返回進行
        }

        return TaskStatus.Success; // 否则返回成功
    }
}
