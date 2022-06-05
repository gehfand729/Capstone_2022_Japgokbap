using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiPrj : MonoBehaviour
{
    private Transform playerTransform;
    [SerializeField] private float projectileSpeed = 10;
    [SerializeField] private float destoryTime = 2;
    private Vector3 projectileDir;
    private Rigidbody tRigid;
    [SerializeField] private float rot;

    private void Awake() {
        playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
        tRigid = GetComponent<Rigidbody>();
    }
    private void Start() {
        this.gameObject.transform.Rotate(Vector3.up, rot, Space.Self);  
        projectileDir = this.gameObject.transform.forward;  
    }
    private void Update()
    {
        ProjectileFunc();
    }

    public void ProjectileFunc(){
        tRigid.velocity = projectileDir * projectileSpeed; 
        Destroy(this.transform.parent.gameObject, destoryTime);
    }
    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.CompareTag("Monster")||other.gameObject.CompareTag("Boss")){
            Destroy(this.gameObject);
        }
    }
}
