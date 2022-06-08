using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Goblins : Monster
{
    protected void Follow_Enter()
    {
        this.isFollowingPlayer = this.isFollowingPlayer ? false : true;
    }

    protected void Follow_Update()
    {
        if (this.isFollowingPlayer)
        {
            FollowSetting();

            Move();
        }

        float distance = Vector3.Distance(targetPosition, this.transform.position);

        if (distance <= this.enemyAttackRange && this.enemyAttackRange > 0)
        {
            fsm.ChangeState(States.Attack);
        }
    }

    protected void Follow_Exit()
    {
        this.isFollowingPlayer = this.isFollowingPlayer ? false : true;
    }

    protected void Attack_Enter()
    {

    }

    protected void Attack_Update()
    {
        this.transform.LookAt(targetPosition);

        if (this.enemyAttackDelay < 0)
        {
            this.enemyAttackDelay = this.enemyAttackSpeed;;
            
            Attack();
        }

        float distance = Vector3.Distance(targetPosition, this.transform.position);

        if (distance > this.enemyAttackRange && !this.isAttacking)
        {
            fsm.ChangeState(States.Follow);
        }
    }

    protected void Attack_Exit()
    {
        if (this.fsm.NextState == States.Follow)
            this.enemyAnimator.SetTrigger("moveTrigger");
    }

    protected void Die_Enter()
    {
        this.enemyHp = 0;
        SpawnExpObjet();
        GameManager.instance.AddScore(this.enemyScore);
        StageManager.instance.DespawnMonster(this.gameObject);
    }

    protected void Die_Update()
    {

    }

    protected void Die_Exit()
    {
        
    }

    protected void Attack()
    {
        this.enemyAnimator.SetTrigger("attackTrigger");
    }

    protected void ThrowAttackPrefab()
    {
        GameObject attack = Instantiate(this.attackPrefab, this.transform.position + new Vector3(0, 2, 0), Quaternion.identity);
        Rigidbody rb = attack.GetComponent<Rigidbody>();

        Vector3 movePosition = (targetPosition + new Vector3(0, 2, 0)) - attack.transform.position;
        attack.transform.LookAt(targetPosition);

        attack.GetComponent<EnemyAttackHit>().SetDamage(this.enemyOffensePower);
        attack.transform.parent = StageManager.instance.enemyPrefabs.transform;
        rb.AddForce(movePosition, ForceMode.Impulse);

        Destroy(attack, 5f);
    }
}