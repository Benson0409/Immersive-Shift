using UnityEngine;
using System;

public class PlayerAttack : MonoBehaviour
{
    //避免攻擊移動
    public bool IsAttacking { get; private set; }
    private bool isCombo = false;

    public float attackDamage = 10f; 
    public LayerMask attackableLayer;
    public GameObject attackHitbox;

    // --- 私有變數 ---
    private PlayerInputController inputController;
    private Animator playerAnimator;

    [SerializeField]private int currentComboStep = 0;
    private bool canReceiveComboInput = false;


    void Awake()
    {
        inputController = GetComponent<PlayerInputController>();
        playerAnimator = GetComponentInChildren<Animator>();

        if (attackHitbox)
        {
            attackHitbox.SetActive(false);
        }
    }

    void OnEnable()
    {
        if (inputController)
        {
            inputController.OnAttack += HandleAttackInput;
        }
    }

    void OnDisable()
    {
        if (inputController)
        {
            inputController.OnAttack -= HandleAttackInput;
        }
    }
    

    // --- 處理攻擊輸入的方法 ---
    private void HandleAttackInput()
    {
        if (playerAnimator == null) return;
        
        IsAttacking = true;
        isCombo = false;
        // 如果在可接受連擊輸入的窗口內
        if (canReceiveComboInput)
        {
            // 如果當前在第一段或第二段連擊，嘗試進入下一段
            if (currentComboStep >= 1 && currentComboStep < 3)
            {
                currentComboStep++;
                isCombo = true;
                Debug.Log($"Combo: 進入第 {currentComboStep} 段攻擊! (通過動畫窗口觸發)");
                playerAnimator.SetInteger("ComboStep", currentComboStep);
                return; 
            }
        }

        // 如果不在連擊中，或不在可接受輸入窗口，則啟動第一段攻擊
        if (currentComboStep == 0)
        {
            currentComboStep = 1;
            Debug.Log("Combo: 啟動第一段攻擊!");
            playerAnimator.SetTrigger("Attack");
            playerAnimator.SetInteger("ComboStep", 1);
            canReceiveComboInput = false;
            
        }

    }

    public float GetCurrentDamage()
    {
        attackDamage = attackDamage + currentComboStep * 3;
        return attackDamage;
    }
    
    // --- 連擊重置方法 (由 Update 或動畫事件調用) ---
    public void ResetCombo()
    {
        currentComboStep = 0; // 重置連擊階段為閒置
        if (playerAnimator)
        {
            playerAnimator.SetInteger("ComboStep", 0);
        }

        canReceiveComboInput = false; // 關閉所有輸入窗口
        IsAttacking = false;
        DisableHitbox();
        Debug.Log("Combo: 連擊重置。");
    }

    // --- 動畫事件：開啟連擊輸入窗口 ---
    // 這個方法將由動畫事件在每段攻擊動畫的特定點觸發
    public void OpenComboInputWindow()
    {
        canReceiveComboInput = true;
        Debug.Log($"Combo: 攻擊 {currentComboStep} 可接收連擊輸入窗口已開啟！");
    }

    // --- 動畫事件：關閉連擊輸入窗口 ---
    public void CloseComboInputWindow()
    {
        canReceiveComboInput = false;
    
        // ResetCombo();
        Debug.Log($"Combo: 攻擊 {currentComboStep} 可接收連擊輸入窗口已關閉！");
    }

    // --- 攻擊判定方法 (由動畫事件調用) ---
    public void PerformAttack()
    {
        Debug.Log($"PlayerAttack: 執行攻擊判定！Hitbox啟用 ({currentComboStep})。");
        if (attackHitbox != null)
        {
            attackHitbox.SetActive(true);
        }
    }

    // --- 禁用 Hitbox 方法 (由動畫事件調用) ---
    public void DisableHitbox()
    {
        Debug.Log("PlayerAttack: 禁用 Hitbox。");
        if (attackHitbox)
        {
            attackHitbox.SetActive(false); 
        }
    }

}