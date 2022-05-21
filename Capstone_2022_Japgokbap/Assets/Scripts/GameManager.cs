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

    #endregion

    #region "Private"

    private static GameManager m_instance;

    #endregion

    #region "Public Methods"

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

    public Vector3 GetPlayerPosition()
    {
        return playerInstance.transform.position;
    }

    public void SetTime(int settime)
    {
        time = settime;
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