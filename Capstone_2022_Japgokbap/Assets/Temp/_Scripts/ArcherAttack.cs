using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherAttack : Skill
{
    [SerializeField] private float skillDuration;

    [SerializeField] private float DestoryTime;

    private void Start() {
        Debug.Log("test");
        StopCoroutine(DoSkill());
        StartCoroutine(DoSkill());
    }
    
    public override IEnumerator DoSkill()
    {
        PlayerController.lockBehaviour = true;
<<<<<<< Updated upstream
        playerAnimator.SetTrigger("doShoot");

        yield return new WaitForSeconds(skillDuration);
=======
        playerAnimator.SetTrigger("isShoot");
        yield return new WaitForSeconds(skillDuration);

>>>>>>> Stashed changes

        GameObject instantePrefab = Instantiate(skillPrefab, playerTransform.position, playerTransform.rotation);
        //GameObject spawnParticle = Instantiate(skillParticle, playerTransform.position + playerTransform.forward, playerTransform.rotation);

        // instantePrefab.transform.parent = this.transform;
        //spawnParticle.transform.parent = this.transform;

        
        PlayerController.lockBehaviour = false;

        Destroy(this.gameObject, DestoryTime);
    }
}
