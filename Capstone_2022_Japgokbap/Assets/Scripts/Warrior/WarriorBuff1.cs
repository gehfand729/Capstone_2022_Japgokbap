using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorBuff1 : WarriorSkill
{
    [SerializeField] private GameObject skillParticle;
    public IEnumerator DoBuff1(){
        if(readySkill){
            readySkill = false;
            // playerAnimator.SetTrigger("XAttack");

            GameObject spawnParticle = Instantiate(skillParticle, transform.position, Quaternion.identity);

            spawnParticle.transform.parent = playerTransform;

            yield return new WaitForSeconds(duration);
            
            Destroy(spawnParticle);

        }
    }
}
