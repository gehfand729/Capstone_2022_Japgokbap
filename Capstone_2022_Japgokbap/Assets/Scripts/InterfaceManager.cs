using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InterfaceManager : MonoBehaviour
{
    #region "Private"
    private PlayerController playerController;
    private Text playerLv;
    [SerializeField] private Text currentGameScore;

    [Header("SliderBar")]
    [SerializeField] private Slider playerHPBar;
    [SerializeField] private Slider playerExpBar;

    [Header("SelectAbillity")]
    [SerializeField] private GameObject selectAbillityPanel;
    [SerializeField] private GameObject skillExplainPanel;

    [Header("UseableSkillBar")]
    [SerializeField] private Text[] skillBarLevels;
    private Button[] skillBars;
    private GameObject skillBarsParent;

    [Header("SkillList")]
    [SerializeField] private List<SkillSO> warriorSkills = new List<SkillSO>();
    [SerializeField] private List<SkillSO> archerSkills = new List<SkillSO>();
    private List<SkillSO> skillsOfChoices = new List<SkillSO>();
    private GameObject selectButtonsParent;
    private RandomSkillSystem RSS;
    [SerializeField] private List<SkillSO> selectedSkillList = new List<SkillSO>();
    [SerializeField] private List<SkillSO> skillsByClass;


    private GameObject leaderBoard;
    [SerializeField] private GameObject gameoverPanel;

    #endregion

    #region "Public"
    public SelectButton[] selectButtons = new SelectButton[3];
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
        skillExplainPanel = GameObject.FindWithTag("Canvas").transform.Find("SkillExplainPanel").gameObject;
        leaderBoard = GameObject.FindWithTag("Canvas").transform.Find("LeaderBoard").gameObject;
        switch (LobbyManager.selectName){
            case "Warrior":
                skillsByClass = warriorSkills;
            break;
            case "Archer":
                skillsByClass = archerSkills;
            break;
        }
    }
    private void Update()
    {
        CalHP();
        CalExp();
        CurrentLv();
        CalScore();
        AddUsableBar();
        ActiveLeaderBoard();
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

    private void CalScore()
    {
        currentGameScore.text = string.Format("Score: " + GameManager.instance.gameScore);
    }

    private void ChoicedSkillManage(SkillSO test){
        if(!selectedSkillList.Contains(test)){
            selectedSkillList.Add(test);
        }else return;
    }

    private void ActiveLeaderBoard(){
        if(Input.GetKeyDown(KeyCode.Tab)){
            leaderBoard.SetActive(true);
        }
        if(Input.GetKeyUp(KeyCode.Tab)){
            leaderBoard.SetActive(false);
        }
        if(playerController.playerCurrentHP <= 0){
            StartCoroutine(DeadLeaderBoard());
        }
    }

    private IEnumerator DeadLeaderBoard(){
        yield return new WaitForSeconds(3.0f);
        Image image = leaderBoard.GetComponent<Image>();
        Color color = image.color;
        color.a = 1f;
        image.color = color;

        image = gameoverPanel.GetComponent<Image>();
        color = image.color;

        //fade out

        leaderBoard.SetActive(true);
    }
    #endregion

    #region "Public Methods"
    
    public void ActiveSelectAbillity(){
        selectAbillityPanel.SetActive(true);
        if(playerController.skillList.Count < 3){
            RSS.RandomSkillSys(skillsByClass, skillsOfChoices);
        }else RSS.RandomSkillSys(selectedSkillList, skillsOfChoices);
        Time.timeScale = 0;
        Debug.Log($"SelectAbillityTest{selectButtons.Length}");
    }

    public void PopUpExplain(SkillSO test){
        skillExplainPanel.SetActive(true);
        skillExplainPanel.transform.GetComponentInChildren<Text>().text = test.skillDescription;
    }

    public void AddAbillity(int selectNumber){
        if(!selectButtons[selectNumber].skill.isPassive){
            if(!playerController.skillList.Contains(selectButtons[selectNumber].skill)){
                playerController.skillList.Add(selectButtons[selectNumber].skill);
                ChoicedSkillManage(selectButtons[selectNumber].skill);
            }
            playerController.skillList.Find(x => x.skillCode == selectButtons[selectNumber].skill.skillCode).skillLevel += 1;
            playerController.skillList.Find(x => x.skillCode == selectButtons[selectNumber].skill.skillCode).damage = playerController.skillList.Find(x => x.skillCode == selectButtons[selectNumber].skill.skillCode).baseDamage *playerController.skillList.Find(x => x.skillCode == selectButtons[selectNumber].skill.skillCode).skillLevel;
        }else {
            if(selectButtons[selectNumber].skill.skillName == "근력강화"){
                playerController.playerOffensePower += selectButtons[selectNumber].skill.enhancementStatus;
            }else if(selectButtons[selectNumber].skill.skillName == "방어력 강화"){
                playerController.playerDeffencePower += selectButtons[selectNumber].skill.enhancementStatus;
            }
        }
        selectAbillityPanel.SetActive(false);
        skillExplainPanel.SetActive(false);
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