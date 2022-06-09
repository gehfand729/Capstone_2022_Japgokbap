using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterLove.StateMachine;
using UnityEngine.AI;

public abstract class Monster : MonoBehaviour
{
    protected NavMeshAgent MyNavMesh { get; private set; }

    protected enum States
    {
        Follow,
        Attack,
        Pattern,
        Pattern2,
        Pattern3,
        Die
    }

    protected StateMachine<States, StateDriverUnity> fsm;

    [Header ("Enemy Stats")]
    [SerializeField] protected float enemyHp;
    [SerializeField] protected float originHp;
    [SerializeField] protected int enemyOffensePower;
    [SerializeField] protected int enemyDefensePower;
    [SerializeField] protected int enemyExperience;
    [SerializeField] protected int enemyScore;
    [SerializeField] protected float enemyAttackRange;
    [SerializeField] protected float enemyAttackDelay;
    [SerializeField] protected float enemyAttackSpeed;
    [SerializeField] protected bool isFollowingPlayer;
    [SerializeField] protected bool isAttacking;

    [SerializeField] protected GameObject attackPrefab;
    
    protected Vector3 targetPosition;
    protected Animator enemyAnimator;

    //test
    [SerializeField] protected GameObject hudDamageText;
    private float TimeLeft = 1.0f;
    private float nextTime = 0.0f;

    void Awake()
    {
        fsm = new StateMachine<States, StateDriverUnity>(this);
        fsm.ChangeState(States.Follow);

        isFollowingPlayer = true;
        originHp = enemyHp;

        MyNavMesh = GetComponent<NavMeshAgent>();
        enemyAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        targetPosition = GameManager.instance.GetPlayerPosition();

        fsm.Driver.Update.Invoke();

        enemyAttackDelay -= Time.deltaTime;

        if (enemyHp < 0)
        {
            BoxSpawnManager.killedEnemyCount++;
            Debug.Log($"Killed Count is {BoxSpawnManager.killedEnemyCount}");
            fsm.ChangeState(States.Die);
        }
    }

    public int GetExp()
    {
        return enemyExperience;
    }

    public int GetEnemyPower()
    {
        return enemyOffensePower;
    }

    public float GetEnemyHp()
    {
        return enemyHp;
    }

    public float GetOriginHp()
    {
        return originHp;
    }

    protected void FollowSetting()
    {
        isAttacking = false;

        MyNavMesh.ResetPath();
        MyNavMesh.isStopped = false;
        MyNavMesh.updatePosition = true;
        MyNavMesh.updateRotation = true;
    }

    protected void AttackSetting()
    {
        isAttacking = true;

        MyNavMesh.isStopped = true;
        MyNavMesh.updatePosition = false;
        MyNavMesh.updateRotation = false;
        MyNavMesh.velocity = Vector3.zero;
    }

    protected void GetDamaged(int damage)
    {
        if(Time.time > nextTime){
            nextTime = Time.time + TimeLeft;
            GameObject hudText = Instantiate(hudDamageText, transform.position + Vector3.up, Quaternion.identity);
            int defeatedDamage = damage - enemyDefensePower;

            if (defeatedDamage < 0)
            {
                defeatedDamage = 0;
            }

            hudText.GetComponent<DamageTextTest>().damage = defeatedDamage;
            enemyHp -= defeatedDamage;
        }
    }

    protected void Move()
    {
        this.MyNavMesh.SetDestination(GameManager.instance.GetPlayerPosition());
    }

    protected void SpawnExpObjet()
    {
        GameObject expClone = Instantiate(StageManager.instance.expObject, transform.position , Quaternion.identity);
        expClone.GetComponent<ExpObject>().SetExp(enemyExperience);
        expClone.transform.parent = StageManager.instance.expClones.transform;
    }
}
