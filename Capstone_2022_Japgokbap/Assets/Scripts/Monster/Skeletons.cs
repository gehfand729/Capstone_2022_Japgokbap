using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Skeletons : Monster
{
    [Header ("Enemy Stats")]
    [SerializeField] protected int m_enemyHp;
    [SerializeField] protected int m_enemyOffensePower;
    [SerializeField] protected int m_enemyDefensePower;
    [SerializeField] protected int m_enemyExperience;

    void Update() 
    {
        if(m_enemyHp > 0)
        {
            NavMesh.SamplePosition(GameManager.instance.GetPlayerPosition(), out NavMeshHit hit, 1f, 1);

            this.MyNavMesh.SetDestination(hit.position);
        }
        else
        {
            SpawnExpObjet();
            Destroy(this.gameObject);
        }
    }

    protected override void SpawnExpObjet()
    {
        GameObject expClone = Instantiate(StageManager.instance.expObject, this.transform.position , Quaternion.identity);
        expClone.transform.parent = StageManager.instance.expClones.transform;
    }

    protected override void GetDamaged()
    {
        Destroy(this.gameObject);
        SpawnExpObjet();
    }
}