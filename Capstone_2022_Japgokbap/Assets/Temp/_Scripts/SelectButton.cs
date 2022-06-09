using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SelectButton : MonoBehaviour, IPointerEnterHandler
{
    public SkillSO skill;
    public Image skillImage;

    private GameObject InterfaceManager;
    
    private void Awake() {
        skillImage = GetComponent<Image>();
        InterfaceManager = GameObject.FindWithTag("InterfaceManager").gameObject;
    }
    public void AddSkill(SkillSO _skill){
        skill = _skill;
        skillImage.sprite = skill.skillImage;
    }

    public void OnPointerEnter(PointerEventData eventData){
        InterfaceManager.SendMessage("PopUpExplain", skill);
    }
}