using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorBladeStorm : WarriorSkill
{
    [SerializeField] private GameObject skillParticle;
    public IEnumerator DoBladeStorm(){
        if(readySkill){
            readySkill = false;
            playerAnimator.SetTrigger("doBladeStorm");
            yield return new WaitForSeconds(animDelay);

            GameObject instantePrefab= Instantiate(skillPrefab, transform.position, Quaternion.identity);
            GameObject spawnParticle = Instantiate(skillParticle, transform.position + transform.forward + new Vector3(0,2,0), Quaternion.Euler(-90, 0, 0));

            instantePrefab.transform.parent = playerTransform;
            spawnParticle.transform.parent = playerTransform;
            yield return new WaitForSeconds(duration);
            
            Destroy(instantePrefab);
            Destroy(skillParticle);
        }
    }
}
