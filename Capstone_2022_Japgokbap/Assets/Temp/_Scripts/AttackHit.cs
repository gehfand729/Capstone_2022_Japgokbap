using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHit : MonoBehaviour
{
    int skillDamage;
    [SerializeField] SkillSO Skill;

    private float TimeLeft = 1.0f;
    private float nextTime = 0.0f;
    private void Awake() {
        skillDamage = Skill.damage;
    }
    // private void OnTriggerEnter(Collider other)
    // {
    //     switch(other.tag){
    //         case "Monster":
    //             Debug.Log($"{Skill.skillName} : {skillDamage}");
    //             //if(Time.time > nextTime){
    //                 other.SendMessage("GetDamaged", skillDamage);
    //             //}
    //         break;
    //         case "Boss":
    //             Debug.Log($"{Skill.skillName} : {skillDamage}");
    //                 other.SendMessage("GetDamaged", skillDamage);
    //             break;
    //         case "Box":
    //             Debug.Log("Crushed Box");
    //             other.SendMessage("SpawnItem");
    //             break;
    //     }
    //     return;
    // }
    private void OnTriggerStay(Collider other) {
            switch(other.tag){
                case "Monster":
                    Debug.Log($"{Skill.skillName} : {skillDamage}");
                    other.SendMessage("GetDamaged", skillDamage);
                break;
                case "Boss":
                    Debug.Log($"{Skill.skillName} : {skillDamage}");
                    if(Time.time > nextTime){
                        nextTime = Time.time + TimeLeft;
                        other.SendMessage("GetDamaged", skillDamage);
                    }
                    break;
                case "Box":
                    Debug.Log("Crushed Box");
                    other.SendMessage("SpawnItem");
                    break;
            }
//        }
        return;
    }
}
