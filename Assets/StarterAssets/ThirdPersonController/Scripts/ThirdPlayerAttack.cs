using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using StarterAssets;
using UnityEngine;

public class ThirdPlayerAttack : MonoBehaviour
{
    public AnimationClip[] Attack1Clips;
    public AnimationClip[] Attack2Clips;
    private ThirdPersonController _thirdPersonController;
    private Animator _animator;

    private bool isInCombat = false;
    private int inputType = 0;
    public int currentAttack = 0;
    private float _outTimer = 0;
    private bool canMoveWhileAttacking = false;
    private float attackMoveSpeed = 2f; // 攻擊時的移動速度
    private bool canSwitchToMove = false; 
    public GameObject weaponCombat; // 戰鬥模式的武器
    public GameObject weaponNormal; // 非戰鬥模式的武器
    void Start()
    {
        _animator = GetComponent<Animator>();
        _thirdPersonController = GetComponent<ThirdPersonController>();
        //ToggleTaggedObjects("weaponnormal", true);
        //ToggleTaggedObjects("weapon", false);
    }

    void Update()
    {
        _outTimer -= Time.deltaTime;

        // 如果计时器结束，重置攻击状态
        if (_outTimer <= 0.1f)
        {
            ResetAttackState();
        }

        // 处理移动
        if (canMoveWhileAttacking && canSwitchToMove)
        {
            HandleAttackMovement();
        }

        // 处理攻击输入
        if (inputType != 0 && _outTimer <= 0.4f && currentAttack < Attack1Clips.Length)
        {
            PlayAttack(currentAttack, inputType);
            inputType = 0; // 处理完输入后重置
        }
    }

    // 重置攻擊狀態
    private void ResetAttackState()
    {
        _thirdPersonController.isAttacking = false;
        canMoveWhileAttacking = false;
        currentAttack = 0;
        _animator.SetTrigger("ReturnToIdle"); // 觸發回到待機
    }

    // 綁定到 Q 鍵的輸入方法，用來切換戰鬥模式
    void OnToggleCombatMode(InputValue value)
    {
        if (value.isPressed)
        {
            if (isInCombat)
            {
                ExitCombatMode();
            }
            else
            {
                EnterCombatMode();
            }
        }
    }

    // 普通攻擊
    void OnKnifeattack(InputValue value)
    {
        if (value.isPressed && isInCombat)
        {
            inputType = 1;
        }
    }

    // 重攻擊
    void OnKnifeHeavyattack(InputValue value)
    {
        if (value.isPressed && isInCombat)
        {
            inputType = 2;
        }
    }

    private void PlayAttack(int index, int type)
    {
        _thirdPersonController.isAttacking = true;
        canMoveWhileAttacking = true;
        canSwitchToMove = false; // 禁止切换到移动状态
        AnimationClip clip;

        if (type == 1)
        {
            clip = Attack1Clips[index];
            currentAttack++;
        }
        else
        {
            clip = Attack2Clips[index];
            currentAttack = 0;
        }

        _animator.ResetTrigger("ReturnToIdle");
        _animator.CrossFade(clip.name, 0.01f);
        _outTimer = clip.length - 0.1f;

        // 在动画结束时允许切换到移动状态
        StartCoroutine(AllowMoveAfterAttack(clip.length));
    }
    private IEnumerator AllowMoveAfterAttack(float delay)
    {
        yield return new WaitForSeconds(delay);
        canSwitchToMove = true; // 允许切换
    }

    private void EnterCombatMode()
    {
        isInCombat = true;
        _animator.SetLayerWeight(0, 0); // 禁用基礎層
        _animator.SetLayerWeight(1, 1); // 啟用戰鬥層
        _animator.SetTrigger("isArmed"); // 設置拿武器動畫的觸發參數

        if (weaponNormal != null)
        {
            weaponNormal.SetActive(false);
        }
        if (weaponCombat != null)
        {
            weaponCombat.SetActive(true);
        }
        Debug.Log("Entered Combat Mode");
    }

    private void ExitCombatMode()
    {
        isInCombat = false;
        _animator.SetLayerWeight(0, 1); // 啟用基礎層
        _animator.SetLayerWeight(1, 0); // 禁用戰鬥層

        // 顯示非戰鬥模式的武器，隱藏戰鬥模式的武器
        if (weaponNormal != null)
        {
            weaponNormal.SetActive(true);
        }
        if (weaponCombat != null)
        {
            weaponCombat.SetActive(false);
        }
        Debug.Log("Exited Combat Mode");
    }

   // 切換帶有特定名稱的物品的顯示狀態
    


    private void HandleAttackMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            _animator.SetFloat("MoveX", horizontal);
            _animator.SetFloat("MoveY", vertical);

            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
            Quaternion rotation = Quaternion.Euler(0, targetAngle, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * 1f);

            Vector3 move = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
            transform.position += move.normalized * attackMoveSpeed * Time.deltaTime;
        }
    }
}