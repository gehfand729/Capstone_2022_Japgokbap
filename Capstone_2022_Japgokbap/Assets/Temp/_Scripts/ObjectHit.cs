using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHit : MonoBehaviour
{
    //test
    [SerializeField] private GameObject item;

    private void SpawnItem(){
        Destroy(this.gameObject);
        GameObject itemClone = Instantiate(item, this.transform.position , Quaternion.AngleAxis(90.0f, Vector3.left));
        itemClone.transform.parent = StageManager.instance.itemClones.transform;
    }
}