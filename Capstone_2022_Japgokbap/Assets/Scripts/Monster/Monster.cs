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
    
    protected Vector3 targetPosition;
    protected Animator enemyAnimator;

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
    }

    protected void Move()
    {
        this.MyNavMesh.SetDestination(GameManager.instance.GetPlayerPosition());
    }

    // hp가 0이하로 떨어졌을 경우 다시 큐에 넣고 비활성화 상태로 돌려야 함
    // Die();

    protected abstract void SpawnExpObjet();

    // 파라미터로 int damage 전달해서 damage만큼 hp 감소시켜야 함
    protected abstract void GetDamaged();
}
