using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Demon : Monster
{
    [Header ("Pattern Info")]
    [SerializeField] protected float rushDelay;
    [SerializeField] protected float rushCooltime;
    [SerializeField] protected float rushRange;
    [SerializeField] protected float originSpeed;
    [SerializeField] protected float rushSpeed;


    [Header("Pattern2 Info")]
    [SerializeField] protected bool isFireFielding;
    [SerializeField] protected float fireFieldDelay;
    [SerializeField] protected float fireFieldCooltime;
    [SerializeField] protected float fireFieldRange;
    [SerializeField] protected GameObject fireFieldSpell;
    [SerializeField] protected GameObject fireFieldPrefab;

    [Header("Pattern3 Info")]
    [SerializeField] protected bool isBreathing;
    [SerializeField] protected float breathDelay;
    [SerializeField] protected float breathCooltime;
    [SerializeField] protected float breathRange;
    [SerializeField] protected GameObject breathSpell;
    [SerializeField] protected GameObject breathPrefab;


    protected void Follow_Enter()
    {
        this.isFollowingPlayer = this.isFollowingPlayer ? false : true;
    }

    protected void Follow_Update()
    {
        CoolTimeCast();

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

        if (distance < this.fireFieldRange && fireFieldDelay < 0)
        {
            fireFieldDelay = fireFieldCooltime;

            fsm.ChangeState(States.Pattern2);
        }

        if (distance < this.breathRange && breathDelay < 0)
        {
            breathDelay = breathCooltime;

            fsm.ChangeState(States.Pattern3);
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
        CoolTimeCast();

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
        this.enemyAttackDelay = 0;
        rushDelay = rushCooltime;
    }

    protected void Pattern2_Enter()
    {
        AttackSetting();

        isFireFielding = true;
        this.enemyAnimator.SetTrigger("firefieldTrigger");
    }

    protected void Pattern2_Update()
    {
        if (!isFireFielding)
        {
            fsm.ChangeState(States.Follow);
        }
    }

    protected void Pattern2_Exit()
    {
        FollowSetting();
    }

    protected void Pattern3_Enter()
    {
        AttackSetting();

        isBreathing = true;
        this.enemyAnimator.SetTrigger("firebreathTrigger");
    }

    protected void Pattern3_Update()
    {
        if (!isBreathing)
        {
            fsm.ChangeState(States.Follow);
        }
    }

    protected void Pattern3_Exit()
    {
        FollowSetting();
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
        this.enemyAnimator.SetTrigger("attackTrigger");
    }
    
    protected void CoolTimeCast()
    {
        rushDelay -= Time.deltaTime;
        fireFieldDelay -= Time.deltaTime;
        breathDelay -= Time.deltaTime;
    }
    protected void ThrowAttackPrefab()
    {
        GameObject attack = Instantiate(this.attackPrefab, this.transform.position + (targetPosition - this.transform.position).normalized * this.enemyAttackRange, this.transform.rotation);
        attack.transform.parent = this.transform;
        attack.GetComponent<EnemyAttackHit>().SetDamage(this.enemyOffensePower);
        Destroy(attack, 2f);
    }

    protected IEnumerator FireField()
    {
        GameObject spell = Instantiate(fireFieldSpell,
            this.transform.position + (GameManager.instance.GetPlayerPosition() - this.transform.position).normalized * 23f + new Vector3(0,3,0),
            Quaternion.Euler(90,0,0));

        spell.transform.parent = StageManager.instance.enemyPrefabs.transform;
        Destroy(spell, 5f);

        yield return new WaitForSeconds(2f);

        SpawnAttackPrefab(spell);
    }

    protected void SpawnAttackPrefab(GameObject spell)
    {
        GameObject attack = Instantiate(fireFieldPrefab, spell.transform.position
            + (targetPosition - this.transform.position).normalized * 5f
            , Quaternion.identity);
        attack.GetComponent<EnemyAttackHit>().SetDamage(this.enemyOffensePower * 3);
        attack.transform.parent = StageManager.instance.enemyPrefabs.transform;
        Destroy(attack, 5f);
    }

    protected void FireBreath()
    {
        GameObject attack = Instantiate(breathPrefab, this.transform.position + new Vector3(0, 2, 0), Quaternion.identity);
        Rigidbody rb = attack.GetComponent<Rigidbody>();

        attack.transform.parent = StageManager.instance.enemyPrefabs.transform;
        Vector3 dir = targetPosition - attack.transform.position;

        Quaternion rot = Quaternion.LookRotation(dir.normalized);
        attack.GetComponent<EnemyAttackHit>().SetDamage(this.enemyOffensePower * 3);

        attack.transform.rotation = rot;

        Vector3 movePosition = (targetPosition + new Vector3(0, 2, 0)) - attack.transform.position;
        rb.AddForce(movePosition, ForceMode.Impulse);

        Destroy(attack, 5f);
    }

    protected void FinishedBreath()
    {
        isBreathing = false;
    }

    protected void FinishedFireField()
    {
        isFireFielding = false;
    }
}