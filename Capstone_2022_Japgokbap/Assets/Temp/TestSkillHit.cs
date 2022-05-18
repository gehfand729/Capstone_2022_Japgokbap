using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSkillHit : MonoBehaviour
{
    int skillDamage;
    [SerializeField] SkillSO Skill;
    private void Awake() {
        skillDamage = Skill.damage;
    }
    private void OnTriggerEnter(Collider other)
    {
        // skillDamage = skillDamage + skillLevel;
        Debug.Log(Skill.skillName + ": " + skillDamage);
        if(other.CompareTag("Monster")){
            other.SendMessage("GetDamaged", skillDamage);
        }
    }
}
