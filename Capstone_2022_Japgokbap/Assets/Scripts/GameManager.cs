using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region "Pulbic"
    public GameObject player;
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
    public int minute;
    public float second;
    public Text timerText;

    #endregion

    #region "Private"

    private static GameManager m_instance;

    #endregion

    #region "Public Methods"

    private void Update() 
    {
        StartTimer();
    }

    public Vector3 GetPlayerPosition()
    {
        return player.transform.position;
    }

    #endregion

    #region "Private Methods"

    private void StartTimer()
    {
        second += Time.deltaTime;

        timerText.text = string.Format("{0:D2}:{1:D2}", minute, (int)second);

        if((int)second > 59)
        {
            second = 0;
            minute++;
        }
    }

    #endregion
}