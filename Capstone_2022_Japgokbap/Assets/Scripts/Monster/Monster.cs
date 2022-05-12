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

    void Start()
    {
        MyNavMesh = GetComponent<NavMeshAgent>();
    }

    protected abstract void SpawnExpObjet();

    protected abstract void GetDamaged();
}
