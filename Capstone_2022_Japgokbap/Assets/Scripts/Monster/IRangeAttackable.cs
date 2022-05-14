using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRangeAttackable
{
    IEnumerator RangedAttack();

    void SetRange(float range);
}
