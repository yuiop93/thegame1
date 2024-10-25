using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class Patrol : Action
{
    public Transform[] patrolPoints; // 預設的巡邏點
    public float stopTime = 2f; // 停留時間
    private int currentPatrolIndex; // 當前巡邏點索引
    private float stopTimer; // 停留計時器
    private NavMeshAgent agent;
    private Animator animator;
    //public bool shouldStopPatrol = false; // 用來控制巡邏是否停止
    private Hearing hearingComponent;
    public override void OnStart()
    {
        hearingComponent = GetComponent<Hearing>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        currentPatrolIndex = 0; // 開始於第一個巡邏點
        stopTimer = 0f; // 初始化停留計時器
    }

    public override TaskStatus OnUpdate()
    {
        PatrolArea(); // 执行巡逻逻辑
        if (CheckIfHeardNoise()) // 检查是否听到声音
        {
            return TaskStatus.Failure; // 停止巡逻，进入攻击状态
        }
        return TaskStatus.Running; // 继续巡逻
    }

    private bool CheckIfHeardNoise()
    {
        // 这里调用你的 Hearing 组件的方法，判断是否听到噪音
        return hearingComponent != null && hearingComponent.HasHeardNoise();
    }
    private void PatrolArea()
    {
        // 檢查是否到達巡邏點
        if (agent.remainingDistance < 0.5f)
        {
            if (stopTimer < stopTime)
            {
                stopTimer += Time.deltaTime; // 增加停留時間
                animator.SetFloat("Speed", 0.0f); // 設置 speed 為 0
                return; // 停止巡邏直到停留時間結束
            }
            else
            {
                stopTimer = 0f; // 重置停留計時器
                currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length; // 移動到下一個巡邏點
            }
        }
        else
        {
            animator.SetFloat("Speed", 2.0f); // 設置 speed 為 2 播放走路動畫
        }

        // 設置目標巡邏點
        agent.SetDestination(patrolPoints[currentPatrolIndex].position);
        
    }
    
}
