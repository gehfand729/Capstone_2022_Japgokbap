using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _Iokheira : Skill
{
    [SerializeField] private float m_cooltime;
    [SerializeField] private float animDelay;
    [SerializeField] private float skillDuration;
    private Vector3 MousePos;
    
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
            playerTransform.rotation = Quaternion.LookRotation(PlayerController.mouseDir);
            MousePos = PlayerController.mouseVec;
            readySkill = false;
            PlayerController.lockBehaviour = true;
            playerAnimator.SetTrigger("doRain");
            yield return new WaitForSeconds(animDelay);
            PlayerController.lockBehaviour = false;

            GameObject instantePrefab= Instantiate(skillPrefab, MousePos + new Vector3(0, 2, 0), playerTransform.rotation);
            // GameObject spawnParticle = Instantiate(skillParticle, playerTransform.position + playerTransform.forward + new Vector3(0,2,0), transform.rotation);

            // instantePrefab.transform.parent = this.transform;
            //spawnParticle.transform.parent = this.transform;
            yield return new WaitForSeconds(skillDuration);

            Destroy(this.gameObject);
            Destroy(instantePrefab);
        } 
    }
}
