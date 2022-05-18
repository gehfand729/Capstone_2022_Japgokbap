using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//워리어 클래스의 스킬들의 부모 클래스
public class WarriorSkill : MonoBehaviour
{
    #region "Public"
    [HideInInspector] public Transform playerTransform;
    [HideInInspector] public Animator playerAnimator;
    [HideInInspector] public bool readySkill = true;
    [HideInInspector] public float realtime;
    #endregion

    #region "Protected"
    [SerializeField] protected GameObject skillPrefab;
    [SerializeField] protected float coolTime;
    [SerializeField] protected float animDelay;
    [SerializeField] protected float duration;
    protected float Damage;
    #endregion

    void Start(){
        playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
        playerAnimator = GetComponent<Animator>();
    }

    private void Update(){
        CoolCheck();
    }
    
    private void CoolCheck(){
        if (!readySkill) {
            if (realtime <= coolTime) {
                realtime += Time.deltaTime;
            } else {
                readySkill = true;
                realtime = 0.0f;
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.tag=="Monster"){
            //추후 체력 감소로 바꿀 예정
            //Debug.Log("tag");
            //Destroy(other.gameObject);
        }
    }
}
