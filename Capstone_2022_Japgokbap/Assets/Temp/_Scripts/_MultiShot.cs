using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _MultiShot : Skill
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
            skillSO.coolCheck = false;
            playerTransform.rotation = Quaternion.LookRotation(PlayerController.mouseDir);
            playerAnimator.SetTrigger("doMulti");

            PlayerController.lockBehaviour = true;
            yield return new WaitForSeconds(skillDuration);

            GameObject test1 = Instantiate(skillPrefab, playerTransform.position + new Vector3(0, 3.0f, 0), playerTransform.rotation);
            
            yield return new WaitForSeconds(0.5f);
            PlayerController.lockBehaviour = false;

            // Destroy(this.gameObject, DestoryTime);
        }
    }
}
