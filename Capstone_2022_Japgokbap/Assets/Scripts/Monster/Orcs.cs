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
    [SerializeField] protected GameObject ragePrefab;
    [SerializeField] protected GameObject rageAura;

    protected void Follow_Enter()
    {
        this.isFollowingPlayer = this.isFollowingPlayer ? false : true;
    }

    protected void Follow_Update()
    {    
        rushDelay -= Time.deltaTime;

        if (this.isFollowingPlayer)
        {
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
        rushDelay -= Time.deltaTime;

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

    protected void Pattern_Enter()
    {
        originSpeed = this.MyNavMesh.speed;
        this.MyNavMesh.speed = rushSpeed;

        this.enemyAnimator.SetTrigger("rushTrigger");
    }

    protected void Pattern_Update()
    {
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
        Destroy(this.gameObject);
        GameManager.instance.AddScore(this.enemyScore);
        StageManager.instance.bossCleared = true;
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
        GameObject attack = Instantiate(this.attackPrefab, this.transform.position + this.transform.forward, this.transform.rotation);
        attack.transform.parent = this.transform;
        attack.GetComponent<EnemyAttackHit>().SetDamage(this.enemyOffensePower);
        Destroy(attack, 2f);
    }

    protected void ThrowRagePrefab()
    {
        GameObject rage = Instantiate(this.ragePrefab, this.transform.position + new Vector3(0,1,0), Quaternion.Euler(-90, 0, 0));
        rage.transform.parent = this.transform;
        rage = Instantiate(this.rageAura, this.transform.position + new Vector3(0,1,0), Quaternion.Euler(-90, 0, 0));
        rage.transform.parent = this.transform;
    }
}