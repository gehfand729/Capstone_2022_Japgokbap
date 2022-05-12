using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSample1 : Skills
{
    protected byte m_skillCode { get; private set; }
    protected float m_skillDamage;

    public float m_skillCooltime;
    public float m_animDelay;
    public float m_skillDuration;
    public GameObject m_skillPrefab;

    protected override void CoolTimeCheck()
    {
        
    }

    protected override void OnTriggerEnter(Collider other)
    {
        
    }
}
