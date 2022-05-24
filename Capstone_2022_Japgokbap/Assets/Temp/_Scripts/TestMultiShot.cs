using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMultiShot : MonoBehaviour
{   
    private Transform playerTransform;
    [SerializeField] private float projectileSpeed = 10;
    private Vector3 dir;
    [SerializeField] private float destoryTime = 2;

    private void Awake() {
        playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
        dir = playerTransform.forward.normalized;
    }
    private void Update()
    {
        this.gameObject.transform.position += dir * projectileSpeed * Time.deltaTime; 
        Destroy(this.gameObject, destoryTime);
    }
}
