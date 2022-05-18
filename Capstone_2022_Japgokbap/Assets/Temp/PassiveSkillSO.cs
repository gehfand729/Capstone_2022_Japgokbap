using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PassiveSkill Name", menuName = "Scriptable Object Asset/PassiveSkill Data")]
public class PassiveSkillSO : ScriptableObject
{
    public string skillCode;
    public string skillName;
    public string skillDescription;
    public int enhancementStatus;
}