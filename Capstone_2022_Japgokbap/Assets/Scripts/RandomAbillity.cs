using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomAbillity : MonoBehaviour
{
    public Sprite[] skillSprite;
    public Button[] skillImages;

    List<int> skillList = new List<int> ();
    List<int> resultSkillList = new List<int>();
    int skillCnt = 5;
    int buttonCnt = 3;

    public void RandomSkill(){
        for (int i = 0; i < skillCnt; i++){
            skillList.Add(i);
        }
        for (int i = 0; i < buttonCnt; i++){
            int randomIndex = Random.Range(0, skillList.Count);
            resultSkillList.Add(skillList[randomIndex]);
            skillImages[i].image.sprite = skillSprite[skillList[randomIndex]];
            skillList.RemoveAt(randomIndex);
        }
    }
    
}
