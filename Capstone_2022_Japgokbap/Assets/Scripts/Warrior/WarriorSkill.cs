using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//워리어 클래스의 스킬들의 부모 클래스
public class WarriorSkill : MonoBehaviour
{
    #region public
    [HideInInspector] public Transform playerTransform;
    [HideInInspector] public Animator playerAnimator;
    [HideInInspector] public bool readySkill = true;
    [HideInInspector] public float realtime;
    public GameObject skillPrefab;
    public float coolTime;
    public float animDelay;
    public float duration;
    //대미지 추가 시 필요.
    public float Damage;
    #endregion

    void Start(){
        playerTransform = GameObject.Find("Player").GetComponent<Transform>();
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
