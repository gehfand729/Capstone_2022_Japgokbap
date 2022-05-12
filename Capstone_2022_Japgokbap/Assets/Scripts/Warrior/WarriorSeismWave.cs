using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorSeismWave : WarriorSkill
{
    [SerializeField] private GameObject skillParticle;
    public IEnumerator DoSeismWave(){
        if(readySkill){
            readySkill = false;
            PlayerMovement.lockBehaviour = true;
            playerAnimator.SetTrigger("doSeismWave");
            yield return new WaitForSeconds(animDelay);

            GameObject instantePrefab= Instantiate(skillPrefab, transform.position, transform.rotation);
            GameObject spawnParticle = Instantiate(skillParticle, transform.position + transform.forward + new Vector3(0,2,0), transform.rotation);
            //player의 자식으로 생성
            instantePrefab.transform.parent = playerTransform;
            spawnParticle.transform.parent = playerTransform;
            yield return new WaitForSeconds(duration);

            PlayerMovement.lockBehaviour = false;
            Destroy(instantePrefab);
            Destroy(skillParticle);
        }
    }
}