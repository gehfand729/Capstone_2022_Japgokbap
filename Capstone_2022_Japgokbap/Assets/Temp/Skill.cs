using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : MonoBehaviour {
    protected bool readySkill = true;
    protected float realtime = 0.0f;
    protected Animator playerAnimator;
    protected Transform playerTransform;
    public SkillSO skillSO;
    public int skillDamage;
    [SerializeField] protected GameObject skillPrefab;
    [SerializeField] protected GameObject skillParticle;

    private void Awake(){
        playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
        playerAnimator = GameObject.FindWithTag("Player").GetComponent<Animator>();
        skillPrefab = this.transform.GetChild(0).gameObject;
        // skillDamage = skillSO.damage;
        // Debug.Log(skillDamage);
    }
    

    public void CoolTimeCheck(float skillCooltime)
    {
        if (!readySkill) {
            if (realtime <= skillCooltime) {
                realtime += Time.deltaTime;
            } else {
                readySkill = true;
                realtime = 0.0f;
            }
        }
    }
    public abstract IEnumerator DoSkill();
//    protected abstract void OnTriggerEnter(Collider other);
    // protected void OnTriggerEnter(Collider other)
    // {
    //     Debug.Log(other.tag);
    //     if(other.CompareTag("Monster")){
    //         // skillDamage = skillDamage + skillLevel;
    //         Debug.Log(skillDamage);
    //         other.SendMessage("GetDamaged", skillDamage);
    //     }
    //     else return;
    // }
}