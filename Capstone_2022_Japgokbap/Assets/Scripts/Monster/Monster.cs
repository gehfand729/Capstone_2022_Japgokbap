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
        Die
    }

    StateMachine<States> fsm;

    protected int enemyHp;
    protected int enemyOffensePower;
    protected int enemyDefensePower;
    protected int enemyExperience;
    [SerializeField] protected bool isFollowingPlayer;

    void Awake()
    {
        fsm = new StateMachine<States>(this);
        fsm.ChangeState(States.Follow);

        isFollowingPlayer = true;

        MyNavMesh = GetComponent<NavMeshAgent>();
    }

    protected abstract void Move();

    protected abstract void SpawnExpObjet();

    protected abstract void GetDamaged();
}
