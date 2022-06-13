using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _Iokheira : Skill
{
    [SerializeField] private float m_cooltime;
    [SerializeField] private float animDelay;
    [SerializeField] private float skillDuration;
    private Vector3 MousePos;
    [SerializeField] private GameObject arrowPrefab;

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
            playerTransform.rotation = Quaternion.LookRotation(PlayerController.mouseDir);
            MousePos = PlayerController.mouseVec;
            skillSO.coolCheck = false;
            PlayerController.lockBehaviour = true;
            playerAnimator.SetTrigger("doRain");
            yield return new WaitForSeconds(0.8f);
            GetComponent<AudioSource>().Play();

            GameObject shootArrow = Instantiate(arrowPrefab, playerTransform.position + Vector3.up, Quaternion.identity);

            yield return new WaitForSeconds(animDelay - 1.4f);
            PlayerController.lockBehaviour = false;

            Destroy(shootArrow);
            GameObject instantePrefab= Instantiate(skillPrefab, MousePos + new Vector3(0, 2, 0), playerTransform.rotation);
            // GameObject spawnParticle = Instantiate(skillParticle, playerTransform.position + playerTransform.forward + new Vector3(0,2,0), transform.rotation);

            // instantePrefab.transform.parent = this.transform;
            //spawnParticle.transform.parent = this.transform;
            yield return new WaitForSeconds(skillDuration - 0.8f);

            Destroy(instantePrefab);
        } 
    }
}
