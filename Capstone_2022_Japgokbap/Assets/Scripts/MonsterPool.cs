using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using KeyType = System.String;

[DisallowMultipleComponent]
public class MonsterPool : MonoBehaviour
{
    public KeyType key;

    /// <summary> 게임오브젝트 복제 </summary>
    public MonsterPool Clone()
    {
        GameObject go = Instantiate(gameObject);
        if (!go.TryGetComponent(out MonsterPool po))
            po = go.AddComponent<MonsterPool>();
        go.SetActive(false);

        return po;
    }

    /// <summary> 게임오브젝트 활성화 </summary>
    public void Activate()
    {
        gameObject.SetActive(true);
    }

    /// <summary> 게임오브젝트 비활성화 </summary>
    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
