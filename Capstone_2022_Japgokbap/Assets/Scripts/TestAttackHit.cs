using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAttackHit : MonoBehaviour
{
    // private PlayerController pCtrl;
    // private void Awake(){
    //     pCtrl = 
    // }
    private void OnTriggerEnter(Collider other)
    {
        // skillDamage = skillDamage + skillLevel;
        // Debug.Log(skillDamage);
        if(other.CompareTag("Monster")){
            other.SendMessage("GetDamaged",250);
        }
    }
}
