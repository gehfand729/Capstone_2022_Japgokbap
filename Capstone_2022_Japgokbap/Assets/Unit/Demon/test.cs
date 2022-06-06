using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public GameObject prefab;

    void FireBreath(GameObject prefab)
    {
        this.transform.LookAt(GameManager.instance.GetPlayerPosition());

        //for debugging
        GameObject attack = Instantiate(prefab, 
            this.transform.position + (GameManager.instance.GetPlayerPosition() - this.transform.position).normalized * 23f,
            Quaternion.identity);

        Vector3 dir = GameManager.instance.GetPlayerPosition() - attack.transform.position;
        dir.y = 0f;

        Quaternion rot = Quaternion.LookRotation(dir.normalized);

        attack.transform.rotation = rot;
    }
}