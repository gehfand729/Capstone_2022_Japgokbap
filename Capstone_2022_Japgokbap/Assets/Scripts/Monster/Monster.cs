using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Monster : MonoBehaviour
{
    protected NavMeshAgent MyNavMesh { get; private set; }
    protected int enemyHp;
    protected int enemyOffensePower;
    protected int enemyDefensePower;
    protected int enemyExperience;
    [SerializeField] protected bool isFollowingPlayer;

    void Awake()
    {
        isFollowingPlayer = true;

        MyNavMesh = GetComponent<NavMeshAgent>();
    }
    protected abstract IEnumerator Move();

    protected abstract void SpawnExpObjet();

    protected abstract void GetDamaged();
}
