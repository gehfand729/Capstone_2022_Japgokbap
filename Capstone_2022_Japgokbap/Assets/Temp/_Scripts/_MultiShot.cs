using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _MultiShot : Skill
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
        playerAnimator.SetTrigger("doMulti");

        PlayerController.lockBehaviour = true;
        yield return new WaitForSeconds(skillDuration);

        // instantPrefabs[arraySize] = Instantiate(skillPrefab, playerTransform.position, playerTransform.rotation);
        // GameObject instantPrefab = Instantiate(skillPrefab, playerTransform.position, playerTransform.rotation);
        // GameObject instantPrefab = Instantiate(skillPrefab, playerTransform.position, playerTransform.rotation);
        GameObject test1 = Instantiate(skillPrefab, playerTransform.position + new Vector3(0, 3.0f, 0), playerTransform.rotation);
        // test1.transform.Rotate(new Vector3(0,-30,0), Space.Self);
        // GameObject test2 = Instantiate(skillPrefab, playerTransform.position, playerTransform.rotation);
        // GameObject test3 = Instantiate(skillPrefab, playerTransform.position, playerTransform.rotation);
        // test1.transform.Rotate(new Vector3(0,30,0), Space.Self);
        //GameObject spawnParticle = Instantiate(skillParticle, playerTransform.position + playerTransform.forward, playerTransform.rotation);

        // instantePrefab.transform.parent = this.transform;
        //spawnParticle.transform.parent = this.transform;

        
        PlayerController.lockBehaviour = false;

        Destroy(this.gameObject, DestoryTime);
    }
}
