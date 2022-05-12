using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceManager : MonoBehaviour
{
    public Slider playerHPBar;
    public Slider playerExpBar;

    void Update()
    {
        playerHPBar.value = PlayerData.Instance.playerMaxHP;
        playerExpBar.value = PlayerData.Instance.PlayerCurrentExp / PlayerData.Instance.PlayerLvUpExp;
    }
}
