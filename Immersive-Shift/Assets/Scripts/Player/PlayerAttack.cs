using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    // --- 可在 Inspector 中調整的公開參數 ---
    public float attackCooldown = 0.5f; // 攻擊冷卻時間
    public float attackRange = 1.5f;    // 攻擊範圍
    public float attackDamage = 10f;    // 攻擊傷害
    public LayerMask attackableLayer;   // 可攻擊的 Layer (例如 Enemy Layer)

    // --- 私有變數 ---
    private PlayerInputController inputController; // 引用 PlayerInputController 腳本
    private Animator playerAnimator;               // 引用 Animator 元件 (如果有的話)
    private float lastAttackTime = -Mathf.Infinity; // 上次攻擊的時間點

    void Awake()
    {
        inputController = GetComponent<PlayerInputController>();
        // playerAnimator = GetComponent<Animator>(); // 如果沒有 Animator，會是 null
    }

    void OnEnable()
    {
        inputController.OnAttack += TryAttack;
    }

    void OnDisable()
    {
        inputController.OnAttack -= TryAttack;
    }

    private void TryAttack()
    {
        // 檢查攻擊冷卻時間
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;
            Debug.Log("PlayerCombat: 攻擊動作觸發！ (待動畫事件執行實際判定)");

            // // 觸發攻擊動畫 (如果有的話)
            // if (playerAnimator != null)
            // {
            //     playerAnimator.SetTrigger("Attack"); // 需要在 Animator Controller 中設定 "Attack" Trigger 參數
            // }
            //
            // // *** 臨時性：這裡可以放一個 Invoke 來模擬動畫觸發攻擊判定 ***
            // // *** 最終會用動畫事件來觸發 PerformAttack() ***
            // Invoke("PerformAttack", 0.2f); // 假設動畫前搖 0.2 秒後進行判定
        }
    }

    // 這個方法將來會由動畫事件調用
    // public void PerformAttack()
    // {
    //     Debug.Log("PlayerCombat: 執行攻擊判定！");
    //
    //     // 使用球體檢測在角色前方查找敵人
    //     Collider[] hitEnemies = Physics.OverlapSphere(
    //         transform.position + transform.forward * attackRange * 0.5f, // 攻擊球體中心
    //         attackRange * 0.5f,                                       // 攻擊球體半徑
    //         attackableLayer);                                         // 只檢測特定 Layer
    //
    //     foreach (Collider enemyCollider in hitEnemies)
    //     {
    //         // 假設敵人有一個 EnemyHealth 腳本來處理傷害
    //         EnemyHealth enemyHealth = enemyCollider.GetComponent<EnemyHealth>();
    //         if (enemyHealth != null)
    //         {
    //             Debug.Log($"PlayerCombat: 擊中敵人: {enemyCollider.name}，造成 {attackDamage} 傷害。");
    //             enemyHealth.TakeDamage(attackDamage);
    //         }
    //     }
    // }
    //
    // // 可視化攻擊範圍 (僅在 Scene 視窗中顯示)
    // void OnDrawGizmosSelected()
    // {
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawWireSphere(transform.position + transform.forward * attackRange * 0.5f, attackRange * 0.5f);
    // }
}
