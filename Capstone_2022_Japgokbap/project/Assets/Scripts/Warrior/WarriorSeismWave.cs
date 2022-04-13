using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorSeismWave : WarriorSkill
{
    public IEnumerator DoSeismWave(){
        if(readySkill){
            readySkill = false;
            PlayerMovement.lockBehaviour = true;
            playerAnimator.SetTrigger("doSeismWave");
            yield return new WaitForSeconds(animDelay);

            GameObject instantePrefab= Instantiate(skillPrefab, transform.position, transform.rotation);
            //player의 자식으로 생성
            instantePrefab.transform.parent = playerTransform;
            yield return new WaitForSeconds(duration);

            PlayerMovement.lockBehaviour = false;
            Destroy(instantePrefab);
        }
    }
}