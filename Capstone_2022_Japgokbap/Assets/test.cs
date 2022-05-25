using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    void FireBreath(GameObject prefab)
    {
        this.transform.LookAt(GameManager.instance.GetPlayerPosition());

        //for debugging
        GameObject attack = Instantiate(prefab, this.transform.position + (GameManager.instance.GetPlayerPosition() - this.transform.position).normalized * 20f, Quaternion.Euler(0, GameManager.instance.playerInstance.transform.rotation.y, 0));
    }
}
