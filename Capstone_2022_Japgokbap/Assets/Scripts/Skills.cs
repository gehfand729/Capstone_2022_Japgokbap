using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skills : MonoBehaviour
{
    protected byte skillCode { get; private set; }
    protected float skillDamage;

    public float skillCooltime;
    public float animDelay;
    public float skillDuration;
    public GameObject skillPrefab;

    // 스킬의 쿨타임 체크
    protected abstract void CoolTimeCheck();

    // 스킬 판정 체크
    protected abstract void OnTriggerEnter(Collider other);
}
