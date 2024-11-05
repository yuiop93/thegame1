using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class Attack : Action
{
    private Animator animator;
    private Transform player;
    private float holdTime = 1.0f;  // 停留时间
    private float timer = 0.0f;
    private bool isHolding = false;

    public bool isDiscover = true;  // 控制是否播放发现动画
    
    public override void OnStart()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        if (isDiscover)
        {
            PlayDiscoverAnimation();  // 播放发现玩家动画
        }
        else
        {
            StartAttack();  // 直接开始攻击
        }
    }

    public override TaskStatus OnUpdate()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // 发现动画播放完成后，进入攻击逻辑
        if (isDiscover && stateInfo.IsName("Discover"))
        {
            if (stateInfo.normalizedTime >= 1.0f)
            {
                StopDiscoverAnimation();  // 停止发现动画
                StartAttack();  // 播放攻击动画
            }
        }
        else if (!isDiscover)  // 已经播放发现动画，进入攻击逻辑
        {
            if (isHolding)
            {
                timer += Time.deltaTime;
                if (timer >= holdTime)
                {
                    isHolding = false;
                    StartNextAttack();
                }
            }
            else
            {
                if (stateInfo.normalizedTime >= 1.0f)  // 当前攻击动画播放完成
                {
                    isHolding = true;
                    timer = 0.0f;
                    animator.SetBool("isAttacking", false);
                    return TaskStatus.Success;  // 返回成功，进入下一个任务
                }
            }
        }

        return TaskStatus.Running;  // 任务正在进行中
    }

    private void PlayDiscoverAnimation()
    {
        
        animator.SetBool("isDiscover", true);  // 启动发现动画
    }

    private void StopDiscoverAnimation()
    {
        animator.SetBool("isDiscover", false);  // 停止发现动画
        isDiscover = false;  // 仅播放一次，设为 false 进入攻击逻辑
    }

    private void StartAttack()
    {
        animator.SetBool("isAttacking", true);
        timer = 0.0f;
        isHolding = false;
    }

    private void StartNextAttack()
    {
        animator.SetBool("isAttacking", true);
        timer = 0.0f;
    }
}
