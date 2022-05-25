using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ClassName", menuName = "Scriptable Object Asset/Class Data")]
public class ClassSO : ScriptableObject
{
    public string className;
    public float hp;
    public int offensePower;
    public int deffencePower;
    public float moveSpeed;
    public GameObject classPrefab;
}