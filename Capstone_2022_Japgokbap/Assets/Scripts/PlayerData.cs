using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerData : MonoBehaviour
{
    public static PlayerData Instance{
        get{
            if(instance==null){
                instance = FindObjectOfType<PlayerData>();
                if(instance == null){
                    var instanceContainer = new GameObject ("PlayerData");
                    instance = instanceContainer.AddComponent<PlayerData>();
                }
            }
            return instance;
        }
            
    }

    [SerializeField] private Dictionary<byte, Skills> playerSkills;
    
    public static PlayerData instance;
    public GameObject ItemEXP;
    public int skillNumber;
    public GameObject selectAbillityTap;
    public GameObject skillBar1;
    public Sprite sample1;
    public GameObject skillBar2;
    public Sprite sample2;

    //스킬을 리스트로 관리
    public List<int> PlayerSkill = new List<int>();

    public float playerMaxHP = 100.0f;
    public int PlayerLv = 1;
    public float PlayerCurrentExp = 0f;
    public float PlayerLvUpExp = 100f;

    public void PlayerExpCalc(float exp){
        PlayerCurrentExp += exp;
        if(PlayerCurrentExp >= PlayerLvUpExp){
            PlayerLv++;
            PlayerCurrentExp -= PlayerLvUpExp;
            PlayerLvUpExp *= 1.3f;
            selectAbillityTap.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public bool SkillCheck(int skillNumber){
        if(PlayerSkill.Contains(skillNumber)){
            return true;
        }else return false;
    }

    public void AddAbillity(int skillNumber){
        PlayerData.Instance.PlayerSkill.Add(skillNumber);
        switch (skillNumber)
        {
            case 1:
                skillBar1.GetComponent<Image>().sprite = sample1;
                break;
            case 2:
                skillBar2.GetComponent<Image>().sprite = sample2;
                break;
        }
        
        selectAbillityTap.SetActive(false);
        Time.timeScale = 1;
    }
}
