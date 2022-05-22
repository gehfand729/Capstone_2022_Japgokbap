using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectButton : MonoBehaviour
{
    public SkillSO skill;
    public Image skillImage;
    private void Awake() {
        skillImage = GetComponent<Image>();
    }
    public void AddSkill(SkillSO _skill){
        skill = _skill;
        skillImage.sprite = skill.skillImage;
    }
}