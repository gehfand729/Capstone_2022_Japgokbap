using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceManager : MonoBehaviour
{
    #region "Private"
    private PlayerController playerController;
    private Text playerLv;

    [Header("SliderBar")]
    [SerializeField] private Slider playerHPBar;
    [SerializeField] private Slider playerExpBar;

    [Header("SelectAbillity")]
    [SerializeField] private GameObject selectAbillityPanel;

    [Header("UseableSkillBar")]
    [SerializeField] private Text[] skillBarLevels;
    private Button[] skillBars;
    private GameObject skillBarsParent;

    [Header("SkillList")]
    [SerializeField] private List<SkillSO> skillsByClass = new List<SkillSO>();
    private List<SkillSO> skillsOfChoices = new List<SkillSO>();
    private GameObject selectButtonsParent;
    private RandomSkillSystem RSS;

    #endregion

    #region "Public"
    [HideInInspector] public SelectButton[] selectButtons;
    #endregion

    private void Awake() {
        playerLv = GameObject.FindWithTag("PlayerLv").GetComponent<Text>();
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        
        //스킬 랜덤 시스템관련
        RSS = GetComponent<RandomSkillSystem>();
        selectButtonsParent = GameObject.FindWithTag("Canvas").transform.Find("SelectAbillityPanel").gameObject;
        selectButtons = selectButtonsParent.GetComponentsInChildren<SelectButton>();
        skillBarsParent = GameObject.FindWithTag("UsableSkillBar");
        skillBars = skillBarsParent.GetComponentsInChildren<Button>();
    }
    private void Update()
    {
        CalHP();
        CalExp();
        CurrentLv();
        AddUsableBar();
    }

    #region "Private Methods"
    private void CalHP(){
        playerHPBar.value = playerController.playerCurrentHP/playerController.playerMaxHP;
    }

    private void CalExp(){
        playerExpBar.value = playerController.playerCurrentExp / playerController.playerLvUpExp;
    }

    private void CurrentLv(){
        playerLv.text = string.Format("Level: "+ playerController.playerLv);
    }
    #endregion

    #region "Public Methods"
    
    public void ActiveSelectAbillity(){
        selectAbillityPanel.SetActive(true);
        RSS.RandomSkillSys(skillsByClass, skillsOfChoices);
        Time.timeScale = 0;
    }
    public void AddAbillity(int selectNumber){
        switch (selectNumber)
        {
            case 1:
            if(selectButtons[0].skill.skillPrefab != null){
                if(!playerController.skillList.Contains(selectButtons[0].skill)){
                    playerController.skillList.Add(selectButtons[0].skill);
                }
                playerController.skillList.Find(x => x.skillCode == selectButtons[0].skill.skillCode).skillLevel += 1;
                playerController.skillList.Find(x => x.skillCode == selectButtons[0].skill.skillCode).damage = playerController.skillList.Find(x => x.skillCode == selectButtons[0].skill.skillCode).baseDamage *playerController.skillList.Find(x => x.skillCode == selectButtons[0].skill.skillCode).skillLevel;
                }else playerController.playerOffensePower += selectButtons[0].skill.enhancementStatus;
                break;
            case 2:
            if(selectButtons[1].skill.skillPrefab != null){
                if(!playerController.skillList.Contains(selectButtons[1].skill)){
                    playerController.skillList.Add(selectButtons[1].skill);
                }
                playerController.skillList.Find(x => x.skillCode == selectButtons[1].skill.skillCode).skillLevel += 1;
                playerController.skillList.Find(x => x.skillCode == selectButtons[1].skill.skillCode).damage = playerController.skillList.Find(x => x.skillCode == selectButtons[1].skill.skillCode).baseDamage *playerController.skillList.Find(x => x.skillCode == selectButtons[1].skill.skillCode).skillLevel;
                }else playerController.playerOffensePower += selectButtons[1].skill.enhancementStatus;
                break;
            case 3:
            if(selectButtons[2].skill.skillPrefab != null){
                if(!playerController.skillList.Contains(selectButtons[2].skill)){
                    playerController.skillList.Add(selectButtons[2].skill);
                }
                playerController.skillList.Find(x => x.skillCode == selectButtons[2].skill.skillCode).skillLevel += 1;
                playerController.skillList.Find(x => x.skillCode == selectButtons[2].skill.skillCode).damage = playerController.skillList.Find(x => x.skillCode == selectButtons[2].skill.skillCode).baseDamage *playerController.skillList.Find(x => x.skillCode == selectButtons[2].skill.skillCode).skillLevel;
                }else playerController.playerOffensePower += selectButtons[2].skill.enhancementStatus;
                break;
        }
        selectAbillityPanel.SetActive(false);
        Time.timeScale = 1;
    }
    public void AddUsableBar(){
        for(int i = 0; i <playerController.skillList.Count; i++){
            skillBars[i].image.sprite = playerController.skillList[i].skillImage;
            skillBarLevels[i].text = string.Format("Lv."+ playerController.skillList[i].skillLevel);
        }
    }
    #endregion
}