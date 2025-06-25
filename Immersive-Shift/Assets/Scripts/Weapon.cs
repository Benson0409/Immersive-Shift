using System;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private PlayerAttack playerAttack;


    // --- Hitbox 碰撞檢測方法 (保持不變) ---
    void OnTriggerEnter(Collider other)
    {
        playerAttack = FindFirstObjectByType<PlayerAttack>();
        Debug.Log($"PlayerAttack: Hitbox 擊中敵人: {other.name}");
        EnemyHP enemyHp = other.GetComponent<EnemyHP>();
        if (enemyHp != null)
        {
            // attackDamage = attackDamage + currentComboStep * 2;
            enemyHp.TakeDamage(playerAttack.GetCurrentDamage());
        }



    }
}
