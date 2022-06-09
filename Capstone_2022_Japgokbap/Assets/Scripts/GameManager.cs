using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region "Pulbic"

    [HideInInspector] public GameObject playerInstance;

    //싱글톤
    public static GameManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<GameManager>();
            }

            return m_instance;
        }
    }

    public float time;
    public int minute;
    public float second;
    public Text timerText;
    public int gameScore;
    public Slider bossHpBar;

    #endregion

    #region "Private"

    private static GameManager m_instance;

    //temp
    [SerializeField] private List<ClassSO> classList = new List<ClassSO>();

    #endregion

    #region "Public Methods"

    public Vector3 GetPlayerPosition()
    {
        return playerInstance.transform.position;
    }

    public void AddScore(int score)
    {
        gameScore += score;
    }

    public void SetTime(int settime)
    {
        time = settime;
    }

    public void ActiveBossUi(bool isbool)
    {
        bossHpBar.gameObject.SetActive(isbool);
    }

    #endregion

    #region "Unity Callbacks"

    //temp
    private void Awake(){
        switch(LobbyManager.selectName){
            case "Warrior":
                Instantiate(classList[1].classPrefab, new Vector3(613, 11, 544), Quaternion.identity);
            break;
            case "Archer":
                Instantiate(classList[0].classPrefab, new Vector3(613, 11, 544), Quaternion.identity);
            break;
        }
    }

    private void Start()
    {
        playerInstance = FindObjectOfType<PlayerController>().gameObject;
    }

    private void Update()
    {
        if (StageManager.instance.fsm.State != StageManager.States.Ready)
        {
            StartTimer();
        }

    }

    #endregion

    #region "Private Methods"

    private void StartTimer()
    {
        time -= Time.deltaTime;

        second = ((int)time % 60);
        minute = ((int)time / 60 % 60);

        timerText.text = string.Format("{0:D2}:{1:D2}", minute, (int)second);

        if((int)time < 0)
        {
            time = 0;
        }
    }

    #endregion
}
