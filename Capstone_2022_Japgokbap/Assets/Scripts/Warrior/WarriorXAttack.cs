using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorXAttack : WarriorSkill
{
    [SerializeField] private GameObject skillParticle;
    public IEnumerator DoXAttack(){
        if(readySkill){
            readySkill = false;
            // playerAnimator.SetTrigger("XAttack");
            yield return new WaitForSeconds(animDelay);

            GameObject instantePrefab= Instantiate(skillPrefab, transform.position + transform.forward, transform.rotation);
            GameObject spawnParticle = Instantiate(skillParticle, transform.position + transform.forward, transform.rotation);

            instantePrefab.transform.parent = playerTransform;
            spawnParticle.transform.parent = playerTransform;

            yield return new WaitForSeconds(duration);
            
            Destroy(instantePrefab);
            Destroy(spawnParticle);

        }
    }
}
