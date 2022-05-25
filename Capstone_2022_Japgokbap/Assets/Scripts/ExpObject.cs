using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpObject : MonoBehaviour
{
    private int expAmount;

    public void SetExp(int exp)
    {
        expAmount = exp;
    }

    public int CalcExp()
    {
        return expAmount;
    }
}
