using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Lich : Monster
{
    [SerializeField] protected GameObject spawnSpell;
    [SerializeField] protected GameObject s_skeleton;
    [SerializeField] protected GameObject spawnPos1;
    [SerializeField] protected GameObject spawnPos2;
    [SerializeField] protected GameObject spawnPos3;
    [SerializeField] protected GameObject spawnPos4;

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
        //Quaternion.Lerp(this.transform.rotation, GameManager.instance.playerInstance.transform.rotation, Time.deltaTime);
        this.transform.LookAt(targetPosition);
        this.enemyAnimator.SetTrigger("spawnTrigger");
    }

    protected void ThrowSpawnPrefab()
    {
        GameObject spawn = Instantiate(spawnSpell, this.transform.position + (targetPosition - this.transform.position).normalized * 20f + new Vector3(0, 4f, 0), Quaternion.Euler(90, 0, 0));
        spawn.transform.parent = this.transform;

        SpawnSkeletons();

        Destroy(spawn, this.enemyAttackSpeed / 2);
    }

    protected void SpawnSkeletons()
    {
        GameObject monster = Instantiate(s_skeleton, spawnPos1.transform.position, Quaternion.identity);
        monster.transform.parent = StageManager.instance.monsters3.transform;

        monster = Instantiate(s_skeleton, spawnPos2.transform.position, Quaternion.identity);
        monster.transform.parent = StageManager.instance.monsters3.transform;

        monster = Instantiate(s_skeleton, spawnPos3.transform.position, Quaternion.identity);
        monster.transform.parent = StageManager.instance.monsters3.transform;

        monster = Instantiate(s_skeleton, spawnPos4.transform.position, Quaternion.identity);
        monster.transform.parent = StageManager.instance.monsters3.transform;
    }
}