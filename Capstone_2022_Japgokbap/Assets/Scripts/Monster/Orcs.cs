using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Orcs : Monster
{
    [Header ("Pattern Info")]
    [SerializeField] protected float rushDelay;
    [SerializeField] protected float rushCooltime;
    [SerializeField] protected float rushRange;
    [SerializeField] protected float originSpeed;
    [SerializeField] protected float rushSpeed;
    [SerializeField] protected bool rageModeCheck;

    protected void Follow_Enter()
    {
        this.isFollowingPlayer = this.isFollowingPlayer ? false : true;
    }

    protected void Follow_Update()
    {    
        this.enemyAttackDelay -= Time.deltaTime;
        rushDelay -= Time.deltaTime;

        if (this.isFollowingPlayer && !this.enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            if (!this.enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
                this.enemyAnimator.SetTrigger("moveTrigger");

            Move();
        }

        float distance = Vector3.Distance(targetPosition, this.transform.position);

        if (distance <= this.enemyAttackRange)
        {
            fsm.ChangeState(States.Attack);
        }

        if (distance >= this.rushRange && rushDelay < 0)
        {
            rushDelay = 0;

            fsm.ChangeState(States.Pattern);
        }

        if (this.enemyHp <= this.originHp / 2 && !rageModeCheck)
        {
            this.enemyAnimator.SetTrigger("rageTrigger");

            this.enemyOffensePower += this.enemyOffensePower;

            rageModeCheck = true;
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
        //this.enemyAnimator.SetBool("isAttacking", true);
        this.MyNavMesh.isStopped = true;
    }

    protected void Attack_Update()
    { 
        this.enemyAttackDelay -= Time.deltaTime;
        rushDelay -= Time.deltaTime;

        if (this.enemyAttackDelay < 0)
            this.enemyAttackDelay = 0;

        if (this.enemyAttackDelay == 0)
        {
            this.enemyAnimator.SetTrigger("attackTrigger");
        }

        float distance = Vector3.Distance(targetPosition, this.transform.position);

        if (distance > this.enemyAttackRange)
        {
            fsm.ChangeState(States.Follow);
        }
    }

    protected void Attack_Exit()
    {
        this.MyNavMesh.isStopped = false;

        this.enemyAttackDelay = this.enemyAttackSpeed;

        //this.enemyAnimator.SetBool("isAttacking", false);
        if (this.fsm.NextState == States.Follow)
            this.enemyAnimator.SetTrigger("moveTrigger");
    }

    protected void Pattern_Enter()
    {
        originSpeed = this.MyNavMesh.speed;
        this.MyNavMesh.speed = rushSpeed;

        this.enemyAnimator.SetTrigger("rushTrigger");
    }

    protected void Pattern_Update()
    {
        this.enemyAttackDelay -= Time.deltaTime;

        Move();

        float distance = Vector3.Distance(targetPosition, this.transform.position);

        if (distance <= this.enemyAttackRange)
        {
            fsm.ChangeState(States.Attack);
        }
    }

    protected void Pattern_Exit()
    {
        this.MyNavMesh.speed = originSpeed;
        rushDelay = rushCooltime;
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