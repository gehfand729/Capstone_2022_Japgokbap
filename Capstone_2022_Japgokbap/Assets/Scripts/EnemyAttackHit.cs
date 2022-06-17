using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackHit : MonoBehaviour
{
    private float attackDamage;

    public void SetDamage(float damage)
    {
        attackDamage = damage;
    }

    public float GetDamage()
    {
        return attackDamage;
    }
}
