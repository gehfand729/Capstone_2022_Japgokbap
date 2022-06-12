using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _IronBody : Skill
{
    [SerializeField] private float m_cooltime;
    [SerializeField] private float animDelay;

    [SerializeField] private float skillDuration;
    private PlayerController playerctrl;

    private void Start() {
        m_cooltime = skillSO.skillCooltime;
        StopCoroutine(DoSkill());
        StartCoroutine(DoSkill());
    }
    private void Update() {
        CoolTimeCheck(m_cooltime);
    }
    public override IEnumerator DoSkill(){
        if(skillSO.coolCheck){
            Destroy(this.gameObject, m_cooltime + 1.0f);
            PlayerController playerctrl = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            int originStat = playerctrl.playerDeffencePower;

            skillSO.coolCheck = false;
            PlayerController.lockBehaviour = true;
            playerAnimator.SetTrigger("doIronBody");
            yield return new WaitForSeconds(animDelay);

            GameObject spawnParticle = Instantiate(skillParticle, playerTransform.transform.position + new Vector3(0, 1.3f, 0), Quaternion.identity);
            spawnParticle.transform.parent = this.transform;

            playerctrl.playerDeffencePower += skillSO.enhancementStatus * skillSO.skillLevel;
            yield return new WaitForSeconds(1.42f);
            PlayerController.lockBehaviour = false;

            yield return new WaitForSeconds(skillDuration);
            playerctrl.playerDeffencePower = originStat;
            
        }
    }
}
