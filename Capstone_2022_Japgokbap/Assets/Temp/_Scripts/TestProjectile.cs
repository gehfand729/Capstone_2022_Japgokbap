using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestProjectile : MonoBehaviour
{
    private Transform playerTransform;
    [SerializeField] private float projectileSpeed = 10;
    [SerializeField] private float destoryTime = 2;
    private Vector3 projectileDir;
    private Rigidbody tRigid;

    private void Awake() {
        playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
        projectileDir = playerTransform.transform.forward;
        tRigid = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        ProjectileFunc(projectileDir);
    }

    public void ProjectileFunc(Vector3 dir){
        tRigid.velocity = dir * projectileSpeed; 
        Destroy(this.gameObject, destoryTime);
    }
    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.CompareTag("Monster")){
            Destroy(this.gameObject);
        }
    }
}
