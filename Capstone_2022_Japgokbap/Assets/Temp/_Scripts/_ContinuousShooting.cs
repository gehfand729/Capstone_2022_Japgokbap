using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _ContinuousShooting : Skill
{
    [SerializeField] private float m_cooltime;
    [SerializeField] private float skillDuration;
    [SerializeField] private float DestoryTime;


    private void Start() {
        m_cooltime = skillSO.skillCooltime;

        StopCoroutine(DoSkill());
        StartCoroutine(DoSkill());
    }
    private void Update() {
        CoolTimeCheck(m_cooltime);
    }
    
    public override IEnumerator DoSkill()
    {
        if(skillSO.coolCheck){
            Destroy(this.gameObject, m_cooltime + 1.0f);
            AudioSource arrowSound = GetComponent<AudioSource>();
            skillSO.coolCheck = false;
            playerTransform.rotation = Quaternion.LookRotation(PlayerController.mouseDir);
            PlayerController.lockBehaviour = true;
            playerAnimator.SetTrigger("doTripleShot");
            GameObject spawnParticle = Instantiate(skillParticle, playerTransform.position + new Vector3(0, 3.0f, 0) + playerTransform.forward*3, playerTransform.rotation);

            yield return new WaitForSeconds(skillDuration);
            arrowSound.Play();
            GameObject instantePrefab1 = Instantiate(skillPrefab, playerTransform.position + new Vector3(0, 3.0f, 0), playerTransform.rotation);
            yield return new WaitForSeconds(0.5f);
            arrowSound.Play();
            GameObject instantePrefab2 = Instantiate(skillPrefab, playerTransform.position + new Vector3(0, 3.0f, 0), playerTransform.rotation);
            yield return new WaitForSeconds(0.5f);
            arrowSound.Play();
            GameObject instantePrefab3 = Instantiate(skillPrefab, playerTransform.position + new Vector3(0, 3.0f, 0), playerTransform.rotation);

            // instantePrefab.transform.parent = this.transform;
            spawnParticle.transform.parent = this.transform;

            
            PlayerController.lockBehaviour = false;
       }
    }
}
