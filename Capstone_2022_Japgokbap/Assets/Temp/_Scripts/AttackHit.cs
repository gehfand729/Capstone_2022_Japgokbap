using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHit : MonoBehaviour
{
    int skillDamage;
    [SerializeField] SkillSO Skill;
    private void Awake() {
        skillDamage = Skill.damage;
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag);
        switch(other.tag){
            case "Monster":
                Debug.Log($"{Skill.skillName} : {skillDamage}");
                other.SendMessage("GetDamaged", skillDamage);
            break;
            case "Box":
                Debug.Log("Crushed Box");
                other.SendMessage("SpawnItem");
            break;
        }
    }
}
