using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _BladeStorm : Skill
{
    [SerializeField] private float m_cooltime;
    [SerializeField] private float skillDuration;
    
    private void Start() {
        StopCoroutine(DoSkill());
        StartCoroutine(DoSkill());
    }
    private void Update() {
        CoolTimeCheck(m_cooltime);
    }
    public override IEnumerator DoSkill()
    {
        if(readySkill){
            readySkill = false;
            playerAnimator.SetTrigger("doBladeStorm");

            GameObject instantePrefab= Instantiate(skillPrefab, playerTransform.position, Quaternion.identity);
            GameObject spawnParticle = Instantiate(skillParticle, playerTransform.position + playerTransform.forward + new Vector3(0,2,0), Quaternion.Euler(-90, 0, 0));

            instantePrefab.transform.parent = this.transform;
            spawnParticle.transform.parent = this.transform;
            yield return new WaitForSeconds(skillDuration);

            PlayerController.lockBehaviour = false;
            Destroy(this.gameObject);
        } 
    }
}
