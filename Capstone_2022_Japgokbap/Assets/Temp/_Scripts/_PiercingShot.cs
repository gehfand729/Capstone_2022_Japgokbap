using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _PiercingShot : Skill
{
    [SerializeField] private float skillDuration;

    [SerializeField] private float DestoryTime;

    private void Start() {
        StopCoroutine(DoSkill());
        StartCoroutine(DoSkill());
    }
    
    public override IEnumerator DoSkill()
    {
        playerTransform.rotation = Quaternion.LookRotation(PlayerController.mouseDir);
        PlayerController.lockBehaviour = true;
        playerAnimator.SetTrigger("doShoot");
        yield return new WaitForSeconds(skillDuration);


        GameObject instantePrefab = Instantiate(skillPrefab, playerTransform.position + new Vector3(0, 3.0f, 0), playerTransform.rotation);
        //GameObject spawnParticle = Instantiate(skillParticle, playerTransform.position + playerTransform.forward, playerTransform.rotation);

        // instantePrefab.transform.parent = this.transform;
        //spawnParticle.transform.parent = this.transform;

        
        PlayerController.lockBehaviour = false;

        Destroy(this.gameObject, DestoryTime);
    }
}
