using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Golems : Monster
{
    [SerializeField] protected GameObject attackSpell;

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
        if (!this.enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
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
        //Quaternion.Lerp(this.transform.rotation, GameManager.instance.playerInstance.transform.rotation, Time.deltaTime);
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
        GameObject attack = Instantiate(attackPrefab, spell.transform.position + new Vector3(0, 1, 0), Quaternion.Euler(-90, 0, 0));
        attack.GetComponent<EnemyAttackHit>().SetDamage(this.enemyOffensePower);
        attack.transform.parent = StageManager.instance.enemyPrefabs.transform;
        Destroy(attack, this.enemyAttackSpeed);
    }
}