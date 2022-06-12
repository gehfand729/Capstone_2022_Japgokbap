using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainArrow : MonoBehaviour
{
    Vector3 MousePos;
    private void OnEnable() {
        MousePos = PlayerController.mouseVec;
    }
    void Update()
    {
        transform.position = Vector3.Lerp(this.gameObject.transform.position + Vector3.up, MousePos - new Vector3(0, 19, 0), 0.05f);
    }
}
