using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherAttack : Skill
{
    [SerializeField] private float skillDuration;

    [SerializeField] private float DestoryTime;

    private void Start() {
        skillSO.skillLevel = 1;
        StopCoroutine(DoSkill());
        StartCoroutine(DoSkill());
    }
    
    public override IEnumerator DoSkill()
    {
        PlayerController.lockBehaviour = true;
        playerAnimator.SetTrigger("doShoot");
        yield return new WaitForSeconds(skillDuration);

        GameObject instantePrefab = Instantiate(skillPrefab, playerTransform.position + new Vector3(0, 3.0f, 0), playerTransform.rotation);
        yield return new WaitForSeconds(0.5f);

        PlayerController.lockBehaviour = false;

        Destroy(this.gameObject, DestoryTime);
    }
}
