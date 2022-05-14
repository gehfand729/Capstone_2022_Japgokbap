using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GoblinArcher : Goblins, IRangeAttackable
{
    Coroutine m_currentCoroutine;
    Animator m_animator;
    Vector3 targetPosition;

    [SerializeField] private float m_attackRange;
    [SerializeField] private float m_attackDelay;

    private void Start() 
    {
        m_animator = GetComponent<Animator>();

        targetPosition = GameManager.instance.GetPlayerPosition();

        StartCoroutine(Move());
        //SetRange(m_attackRange);
    }

    #region "Coroutine Methods"

    public IEnumerator RangedAttack()
    {
        this.MyNavMesh.isStopped = true;

        //실제 공격 구현
        Vector3 dir = targetPosition - this.transform.position;
        this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime);

        Debug.Log("attacking");
        m_animator.SetTrigger("attackTrigger");
        m_animator.SetBool("isAttacking", true);
        //실제 공격 구현

        yield return new WaitForSeconds(m_attackDelay);

        m_animator.SetBool("isAttacking", false);

        float distance = Vector3.Distance(targetPosition, this.transform.position);

        if (m_attackRange >= distance)
        {
            m_currentCoroutine = StartCoroutine(RangedAttack());
        }
        else
        {
            isFollowingPlayer = true;
            this.MyNavMesh.isStopped = false;
            m_animator.SetBool("isMoving", true);

            m_currentCoroutine = StartCoroutine(Move());
        }
    }

    #endregion

    #region "Override Methods"

    public void SetRange(float range)
    {
        //공격 범위 조절
        
    }

    protected override IEnumerator Move()
    {
        NavMesh.SamplePosition(GameManager.instance.GetPlayerPosition(), out NavMeshHit hit, 1f, 1);

        this.MyNavMesh.SetDestination(hit.position);

        float distance = Vector3.Distance(targetPosition, this.transform.position);

        if (isFollowingPlayer && m_attackRange >= distance)
        {
            if (m_currentCoroutine != null)
            {
                StopCoroutine(m_currentCoroutine);
            }

            isFollowingPlayer = false;
            m_animator.SetBool("isMoving", false);
            m_currentCoroutine = StartCoroutine(RangedAttack());
        }

        yield return null;
    }

    #endregion

}