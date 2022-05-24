using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _IronBody : Skill
{
    [SerializeField] private float m_cooltime;
    [SerializeField] private float animDelay;

    [SerializeField] private float skillDuration;

    private void Start() {
        StopCoroutine(DoSkill());
        StartCoroutine(DoSkill());
    }
    private void Update() {
        CoolTimeCheck(m_cooltime);
    }
    public override IEnumerator DoSkill(){
        if(readySkill){
            readySkill = false;
            PlayerController.lockBehaviour = true;
            // playerAnimator.SetTrigger("XAttack");
            yield return new WaitForSeconds(animDelay);

            GameObject spawnParticle = Instantiate(skillParticle, playerTransform.transform.position + new Vector3(0, 1.3f, 0), Quaternion.identity);
            Debug.Log("ironBody Particle test");
            spawnParticle.transform.parent = this.transform;
            PlayerController.lockBehaviour = false;

            yield return new WaitForSeconds(skillDuration);
            
            Destroy(spawnParticle);

        }
    }
}
