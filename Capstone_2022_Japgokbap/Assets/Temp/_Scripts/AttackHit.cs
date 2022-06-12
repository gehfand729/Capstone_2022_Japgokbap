using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHit : MonoBehaviour
{
    int skillDamage;
    [SerializeField] SkillSO Skill;

    private PlayerController playerController;

    private float TimeLeft = 1.0f;
    private float nextTime = 0.0f;
    private void Start() {
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        if(Skill.skillLevel == 0){
            skillDamage = 0;
            return;
        }
        skillDamage = Skill.baseDamage + Skill.skillLevel * 5 + playerController.playerOffensePower;
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
