using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : Skill{


    [SerializeField] private float skillDuration;

    private void Start() {
        skillSO.skillLevel = 1;
        StopCoroutine(DoSkill());
        StartCoroutine(DoSkill());
    }
    
    public override IEnumerator DoSkill()
    {
        PlayerController.lockBehaviour = true;
        playerAnimator.SetTrigger("doSlash");

        GameObject instantePrefab = Instantiate(skillPrefab, playerTransform.position + playerTransform.forward, playerTransform.rotation);
        GameObject spawnParticle = Instantiate(skillParticle, playerTransform.position + playerTransform.forward, playerTransform.rotation);

        instantePrefab.transform.parent = this.transform;
        spawnParticle.transform.parent = this.transform;

        yield return new WaitForSeconds(skillDuration);
        PlayerController.lockBehaviour = false;

        Destroy(this.gameObject);
    }
}