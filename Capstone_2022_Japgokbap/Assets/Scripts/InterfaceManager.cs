using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;

public class InterfaceManager : MonoBehaviour
{
    #region "Private"

    private static InterfaceManager m_instance;

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

    [Header("When Game Ended")]
    [SerializeField] private GameObject leaderBoard;
    [SerializeField] private Text clearClass;
    [SerializeField] private Text clearStage;
    [SerializeField] private Text clearTime;
    [SerializeField] private Text clearLevel;
    [SerializeField] private Text finalScore;
    [SerializeField] private Text canGetGold;
    [SerializeField] private GameObject gameoverPanel;
    private bool isFaded;
    [SerializeField] private GameObject youdiedImage;
    [SerializeField] private GameObject restartButton;
    private int gold;

    #endregion

    #region "Public"
    public SelectButton[] selectButtons = new SelectButton[3];

    public static InterfaceManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<InterfaceManager>();
            }

            return m_instance;
        }
    }
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

        if (playerController.playerCurrentHP <= 0)
        {
            ActiveLeaderBoard();
        }
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

    public void ActiveLeaderBoard(){
        if(!isFaded)
        {
            isFaded = true;

            StartCoroutine(DeadLeaderBoard());
        }
    }

    private IEnumerator DeadLeaderBoard(){
        gold = Mathf.RoundToInt(GameManager.instance.gameScore / 5);
        
        yield return new WaitForSeconds(3.0f);
        Image image = leaderBoard.GetComponent<Image>();
        image.color = new Color(255, 255, 255, 1f);

        //fade out
        gameoverPanel.SetActive(true);
        image = gameoverPanel.GetComponent<Image>();
        StartCoroutine(Fadeout(image));

        StageManager.instance.fsm.ChangeState(StageManager.States.Finish);

        yield return new WaitForSeconds(2.0f);

        youdiedImage.SetActive(true);

        yield return new WaitForSeconds(2.0f);

        image = youdiedImage.GetComponent<Image>();
        StartCoroutine(Fadeout(image));
        youdiedImage.SetActive(false);

        yield return new WaitForSeconds(1.0f);

        SetLeaderBoard();
        leaderBoard.SetActive(true);

        yield return new WaitForSeconds(1.0f);

        clearClass.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.0f);

        clearStage.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.0f);

        clearTime.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.0f);

        clearLevel.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.0f);

        finalScore.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.0f);

        canGetGold.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.0f);

        canGetGold.text = string.Format("획득한 골드 : " + gold);

        yield return new WaitForSeconds(1.0f);
        restartButton.SetActive(true);

    }

    private IEnumerator Fadeout(Image image)
    {
        float fadeAlpha = 0;

        while (fadeAlpha < 1.0f)
        {
            fadeAlpha += 0.01f;
            yield return new WaitForSeconds(0.01f);
            image.color = new Color(0, 0, 0, fadeAlpha);
        }
    }

    private void SetLeaderBoard()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        ClearInfomation info =  new ClearInfomation();
        info.className = player.name;
        info.clearStage = StageManager.instance.stageCount;
        info.clearRound = StageManager.instance.roundCount;
        info.clearMinute = ((int)GameManager.instance.fullTIme / 60 % 60);
        info.clearSecond = ((int)GameManager.instance.fullTIme % 60);
        info.clearScore = GameManager.instance.gameScore;
        info.clearLevel = player.GetComponent<PlayerController>().playerLv;

        clearClass.text = string.Format("직업 : " + info.className);
        clearStage.text = string.Format("클리어한 스테이지 : "
            + info.clearStage + "-" + info.clearRound);
        clearTime.text = string.Format("플레이한 시간 : " 
            + info.clearMinute + "분 " + info.clearSecond + "초");
        clearLevel.text = string.Format("최종 레벨 : " + info.clearLevel);
        finalScore.text = string.Format("최종 점수 : " + info.clearScore);
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

    public IEnumerator CoolSlot(float skillCool, int slotNum){
        float leftTime = skillCool;
        while (leftTime > 0.0f){
            leftTime -= Time.deltaTime;
            skillBars[slotNum].image.fillAmount = 1.0f - (leftTime / skillCool);
            yield return new WaitForFixedUpdate();
        }
    }

    public void GoToLobby()
    {
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(),
                (result) =>
                {
                    result.VirtualCurrency["GD"] = gold;
                },
                (error) =>
                {
                    //error
                });

        Application.Quit();
    }

    #endregion
}