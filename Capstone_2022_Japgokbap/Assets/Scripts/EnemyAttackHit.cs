using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackHit : MonoBehaviour
{
    private int attackDamage;

    public void SetDamage(int damage)
    {
        attackDamage = damage;
    }

    public int GetDamage()
    {
        return attackDamage;
    }
}
