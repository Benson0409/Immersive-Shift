using UnityEngine;
using UnityEngine.UI;
public class EnemyHP : MonoBehaviour
{

    [Header("血量設定")]
    public float maxHealth = 100f;
    [SerializeField]private float currentHealth;

    // [Header("血條 UI")]
    // public Canvas healthBarCanvas;      // World Space Canvas（掛在敵人頭上）
    // public Image healthBarFill;         // Image 的 Fill，更新寬度用
    //
    // private Transform mainCameraTransform;

    void Awake()
    {
        currentHealth = maxHealth;
        // if (Camera.main != null)
        //     mainCameraTransform = Camera.main.transform;
    }

    // void Update()
    // {
    //     // 讓血條 Canvas 面向攝影機
    //     if (healthBarCanvas != null && mainCameraTransform != null)
    //     {
    //         Vector3 lookDirection = healthBarCanvas.transform.position - mainCameraTransform.position;
    //         healthBarCanvas.transform.rotation = Quaternion.LookRotation(lookDirection);
    //     }
    // }

    // 被攻擊時調用
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        // UpdateHealthBar();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // // 更新 UI 血條長度
    // void UpdateHealthBar()
    // {
    //     if (healthBarFill != null)
    //     {
    //         healthBarFill.fillAmount = currentHealth / maxHealth;
    //     }
    // }

    // 死亡邏輯
    void Die()
    {
        Debug.Log($"{gameObject.name} 已死亡！");
        // 你可以在這裡加入死亡動畫、刪除物件等等
        Destroy(gameObject);
    }
}

