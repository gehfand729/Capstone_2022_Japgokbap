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
        if (!this.enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            this.transform.LookAt(targetPosition);

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
        GameManager.instance.AddScore(this.enemyScore);
        Destroy(this.gameObject);
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
        //Quaternion.Lerp(this.transform.rotation, GameManager.instance.playerInstance.transform.rotation, Time.deltaTime);
        this.transform.LookAt(targetPosition);

        this.enemyAnimator.SetTrigger("attackTrigger");
    }

    protected IEnumerator ThrowAttackPrefab()
    {
        GameObject attack = Instantiate(attackSpell, targetPosition + new Vector3(0, 1, 0), Quaternion.Euler(-90, 0, 0));
        attack.transform.LookAt(targetPosition);
        attack.transform.parent = StageManager.instance.enemyPrefabs.transform;
        Destroy(attack, this.enemyAttackSpeed);

        yield return new WaitForSeconds(1.5f);

        SpawnAttackPrefab(attack);
    }

    protected void SpawnAttackPrefab(GameObject spell)
    {
        GameObject attack = Instantiate(attackPrefab, spell.transform.position + new Vector3(0, 5, 0), Quaternion.Euler(-90, 0, 0));
        attack.GetComponent<EnemyAttackHit>().SetDamage(this.enemyOffensePower);
        attack.transform.parent = StageManager.instance.enemyPrefabs.transform;
        Destroy(attack, this.enemyAttackSpeed);
    }

    protected void SpawnStaryPrefab()
    {
        strayDelay = strayCoolTime;

        GameObject stary = Instantiate(strayPrefab, this.transform.position + new Vector3(0, 6, 0), Quaternion.identity);
        stary.GetComponent<EnemyAttackHit>().SetDamage(this.enemyOffensePower * 2);
        stary.transform.parent = this.transform;
        Destroy(stary, strayDelay);
    }
}