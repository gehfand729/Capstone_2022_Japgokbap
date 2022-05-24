using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GolemBoss : Monster
{
    [SerializeField] protected GameObject attackSpell;

    [Header ("Pattern Info")]
    [SerializeField] protected float strayDelay;
    [SerializeField] protected float strayCoolTime;
    [SerializeField] protected GameObject strayPrefab;

    protected void Follow_Enter()
    {
        this.isFollowingPlayer = this.isFollowingPlayer ? false : true;
    }

    protected void Follow_Update()
    {
        strayDelay -= Time.deltaTime;
        
        if (this.isFollowingPlayer)
        {
            FollowSetting();

            Move();
        }

        float distance = Vector3.Distance(targetPosition, this.transform.position);

        if (distance <= this.enemyAttackRange)
        {
            fsm.ChangeState(States.Attack);
        }

        if (strayDelay < 0)
        {
            SpawnStaryPrefab();
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

    }

    protected void Attack_Update()
    {
        strayDelay -= Time.deltaTime;

        if (this.enemyAttackDelay < 0)
        {
            this.enemyAttackDelay = this.enemyAttackSpeed;;
            
            Attack();
        }

        if (strayDelay < 0)
        {
            SpawnStaryPrefab();
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
        //Destroy(this.gameObject);
    }

    protected void Die_Update()
    {

    }

    protected void Die_Exit()
    {
        
    }

    protected void Attack()
    {
        //Quaternion.Lerp(this.transform.rotation, GameManager.instance.playerInstance.transform.rotation, Time.deltaTime);
        this.transform.LookAt(targetPosition);

        this.enemyAnimator.SetTrigger("attackTrigger");
    }

    protected void ThrowAttackPrefab()
    {
        GameObject attack = Instantiate(attackSpell, this.transform.position + (targetPosition - this.transform.position).normalized * 10f + new Vector3(0, 4f, 0), Quaternion.identity);
        attack.transform.LookAt(targetPosition);
        attack.transform.parent = this.transform;

        attack = Instantiate(this.attackPrefab, targetPosition + new Vector3(0,3,0), Quaternion.Euler(-90,0,0));
        Destroy(attack, this.enemyAttackSpeed / 2);
    }

    protected void SpawnStaryPrefab()
    {
        strayDelay = strayCoolTime;

        GameObject stary = Instantiate(strayPrefab, this.transform.position + new Vector3(0, 6, 0), Quaternion.identity);
        stary.transform.parent = this.transform;
        Destroy(stary, strayDelay);
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