using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinArcher : Goblins, IRangeAttackable
{
    Coroutine m_currentCoroutine;
    SphereCollider m_sphereCollider;
    Animator m_animator;

    [SerializeField] private float m_attackRange;
    [SerializeField] private float m_attackDelay;

    private void Start() 
    {
        m_sphereCollider = GetComponent<SphereCollider>();
        m_animator = GetComponent<Animator>();
    }

    #region "Trigger Methods"

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && isFollowingPlayer)
        {
            if (m_currentCoroutine != null)
            {
                StopCoroutine(m_currentCoroutine);
            }

            m_currentCoroutine = StartCoroutine(RangedAttack());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        isFollowingPlayer = true;
        this.MyNavMesh.isStopped = false;
        m_animator.SetTrigger("walkTrigger");

        if (other.gameObject.CompareTag("Player"))
        {
            if (m_currentCoroutine != null)
            {
                StopCoroutine(m_currentCoroutine);
            }

            m_currentCoroutine = StartCoroutine(Move());
        }
    }

    #endregion

    #region "Coroutine Methods"

    public IEnumerator RangedAttack()
    {
        isFollowingPlayer = false;
        this.MyNavMesh.isStopped = true;
        m_animator.SetTrigger("attackTrigger");
        
        while(true)
        {
            //실제 공격 구현
            Debug.Log("attacking");

            //attackDely 만큼 두번째 애니메이션 진행 수정
            yield return new WaitForSeconds(m_attackDelay);
        }
    }

    #endregion

    #region "Override Methods"

    public void SetRange(float range)
    {
        //공격 범위에 따라 collider 크기 조절
        m_sphereCollider.radius = range;
    }

    #endregion

}
