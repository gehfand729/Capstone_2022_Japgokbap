using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName = "Scriptable Object Asset/Skill Data")]
public class SkillSO : ScriptableObject
{
    public string skillCode;
    public string skillName;
    public Sprite skillImage;
    public int skillLevel = 0;
    public int baseDamage = 0;
    public int damage;
    public GameObject skillPrefab;
    public float skillCooltime;
    public bool coolCheck;
    
    public bool isPassive;
    public string skillDescription;
    public int enhancementStatus;

    private void OnEnable() {
        skillLevel = 0;
        damage = 0;
        coolCheck = true;
    }
}