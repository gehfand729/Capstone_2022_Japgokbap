using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomSkillSystem : MonoBehaviour
{
    #region "Private"
    private InterfaceManager interfaceManager;
    #endregion
    private void Awake() {
        interfaceManager = GetComponent<InterfaceManager>();
    }

    #region "Public Methods"
    public void RandomSkillSys(List<SkillSO> skillsByClass, List<SkillSO> skillsOfChoices){
        List<SkillSO> skillsBeforeRandom = new List<SkillSO>();
        for (int i = 0; i < skillsByClass.Count; i++){
            skillsBeforeRandom.Add(skillsByClass[i]);
        }
        for (int i = 0; i < interfaceManager.selectButtons.Length; i++){
            int randomIndex = Random.Range(0, skillsBeforeRandom.Count);
            skillsOfChoices.Add(skillsBeforeRandom[randomIndex]);
            interfaceManager.selectButtons[i].AddSkill(skillsBeforeRandom[randomIndex]);
            skillsBeforeRandom.RemoveAt(randomIndex);
        }
    }
    #endregion
}
