using UnityEngine;
using System;

public class PlayerHP : MonoBehaviour
{
    public int maxHealth = 100; // 角色最大生命值
    [SerializeField] private int currentHealth;

    // 事件註冊
    
    // currentHealth, maxHealth
    public event Action<int, int> OnHealthChanged; 
    //血量歸零
    public event Action OnPlayerDied;

    void Awake()
    {
        currentHealth = maxHealth;
        Debug.Log($"PlayerHealth: 血量初始化 {currentHealth}/{maxHealth}");
        // OnHealthChanged?.Invoke(currentHealth, maxHealth); // 初始廣播
    }

    //受傷
    public void TakeDamage(int damage)
    {
        if (currentHealth <= 0) return; 
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0); 

        Debug.Log($"PlayerHealth: 受到 {damage} 點傷害。當前血量: {currentHealth}/{maxHealth}");
        OnHealthChanged?.Invoke(currentHealth, maxHealth); 

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("PlayerHealth: 玩家已死亡！");
        OnPlayerDied?.Invoke(); // 廣播死亡事件

        // 在這裡可以加入角色死亡的視覺或物理效果
        // 例如：禁用角色控制器、播放死亡動畫等
        // GetComponent<CharacterController>().enabled = false;
        // GetComponent<PlayerMovement>().enabled = false; // 禁用移動腳本
        // GetComponent<PlayerInputController>().enabled = false; // 禁用輸入腳本
    }

    // 可以提供一個治療方法
    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, maxHealth); // 確保血量不超過最大值
        Debug.Log($"PlayerHealth: 恢復 {amount} 點生命。當前血量: {currentHealth}/{maxHealth}");
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }
}
