using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testHIt : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Monster")
        {
            other.SendMessage("GetDamaged");
        }
    }
}
