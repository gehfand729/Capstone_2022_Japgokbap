using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GolemBoss : Monster
{
    protected void Follow_Enter()
    {
        this.isFollowingPlayer = this.isFollowingPlayer ? false : true;
    }

    protected void Follow_Update()
    {
        if (this.isFollowingPlayer)
        {
            Move();
        }

        float distance = Vector3.Distance(targetPosition, this.transform.position);

        if (distance <= this.enemyAttackRange)
        {
            fsm.ChangeState(States.Attack);
        }

        if (this.enemyHp < 0)
        {
            fsm.ChangeState(States.Die);
        }
    }

    protected void Follow_Exit()
    {
        this.isFollowingPlayer = this.isFollowingPlayer ? false : true;
    }

    protected void Attack_Enter()
    {
        this.enemyAnimator.SetTrigger("attackTrigger");
        this.MyNavMesh.isStopped = true;
    }

    protected void Attack_Update()
    {
        this.enemyAttackDelay -= Time.deltaTime;

        if (this.enemyAttackDelay < 0)
            this.enemyAttackDelay = 0;
        
        if (this.enemyAttackDelay == 0)
        {
            this.enemyAnimator.SetTrigger("attackTrigger");

            this.enemyAttackDelay = this.enemyAttackSpeed;
        }

        float distance = Vector3.Distance(targetPosition, this.transform.position);

        if (distance > this.enemyAttackRange)
        {
            fsm.ChangeState(States.Follow);
        }
    }

    protected void Attack_Exit()
    {
        if (this.fsm.NextState == States.Follow)
            this.enemyAnimator.SetTrigger("moveTrigger");

        this.MyNavMesh.isStopped = false;
        this.enemyAttackDelay = this.enemyAttackSpeed;
    }

    protected void Die_Enter()
    {
        this.enemyHp = 0;
        SpawnExpObjet();
        //Destroy(this.gameObject);
    }

    protected void Die_Update()
    {

    }

    protected void Die_Exit()
    {
        
    }

    protected override void SpawnExpObjet()
    {
        GameObject expClone = Instantiate(StageManager.instance.expObject, this.transform.position , Quaternion.identity);
        expClone.transform.parent = StageManager.instance.expClones.transform;
    }

    protected override void GetDamaged()
    {
        Destroy(this.gameObject);
        SpawnExpObjet();
    }
}