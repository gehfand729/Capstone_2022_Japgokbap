using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _ContinuousShooting : Skill
{
    [SerializeField] private float skillDuration;
    [SerializeField] private float DestoryTime;


    private void Start() {
        StopCoroutine(DoSkill());
        StartCoroutine(DoSkill());

    }
    
    public override IEnumerator DoSkill()
    {
        if(readySkill){
            playerTransform.rotation = Quaternion.LookRotation(PlayerController.mouseDir);
            PlayerController.lockBehaviour = true;
            //playerAnimator.SetTrigger("doSlash");

            yield return new WaitForSeconds(skillDuration);

            GameObject test1 = Instantiate(skillPrefab, playerTransform.position + new Vector3(0, 3.0f, 0), playerTransform.rotation);
            yield return new WaitForSeconds(0.5f);
            GameObject test2 = Instantiate(skillPrefab, playerTransform.position + new Vector3(0, 3.0f, 0), playerTransform.rotation);
            yield return new WaitForSeconds(0.5f);
            GameObject test3 = Instantiate(skillPrefab, playerTransform.position + new Vector3(0, 3.0f, 0), playerTransform.rotation);
            
            //GameObject spawnParticle = Instantiate(skillParticle, playerTransform.position + new Vector3(0, 3.0f, 0) + playerTransform.forward, playerTransform.rotation);

            // instantePrefab.transform.parent = this.transform;
            //spawnParticle.transform.parent = this.transform;

            
            PlayerController.lockBehaviour = false;

            Destroy(this.gameObject, DestoryTime);
        }
    }
}
