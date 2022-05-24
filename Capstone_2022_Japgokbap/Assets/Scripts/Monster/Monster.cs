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
    [SerializeField] protected int enemyHp;
    [SerializeField] protected float originHp;
    [SerializeField] protected int enemyOffensePower;
    [SerializeField] protected int enemyDefensePower;
    [SerializeField] protected int enemyExperience;
    [SerializeField] protected float enemyAttackRange;
    [SerializeField] protected float enemyAttackDelay;
    [SerializeField] protected float enemyAttackSpeed;
    [SerializeField] protected bool isFollowingPlayer;
    [SerializeField] protected bool isAttacking;

    [SerializeField] protected GameObject attackPrefab;
    
    protected Vector3 targetPosition;
    protected Animator enemyAnimator;

        #region "Test"
    //test
    [SerializeField] protected GameObject hudDamageText;
    [SerializeField] protected Transform hudPos;
    #endregion

    void Awake()
    {
        fsm = new StateMachine<States, StateDriverUnity>(this);
        fsm.ChangeState(States.Follow);

        isFollowingPlayer = true;
        originHp = enemyHp;

        MyNavMesh = GetComponent<NavMeshAgent>();
        enemyAnimator = GetComponent<Animator>();

        hudPos = transform.Find("HudDamage").gameObject.GetComponent<Transform>();
    }

    void Update()
    {
        targetPosition = GameManager.instance.GetPlayerPosition();

        fsm.Driver.Update.Invoke();

        enemyAttackDelay -= Time.deltaTime;
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

    protected void Move()
    {
        this.MyNavMesh.SetDestination(GameManager.instance.GetPlayerPosition());
    }

    protected abstract void SpawnExpObjet();

    // 파라미터로 int damage 전달해서 damage만큼 hp 감소시켜야 함
    protected abstract void GetDamaged(int damage);
}
