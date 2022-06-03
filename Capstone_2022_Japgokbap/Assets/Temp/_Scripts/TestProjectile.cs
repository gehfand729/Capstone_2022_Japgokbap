using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestProjectile : MonoBehaviour
{
    private Transform playerTransform;
    [SerializeField] private float projectileSpeed = 10;
    [SerializeField] private float destoryTime = 2;
    private Vector3 projectileDir;
    private void Awake() {
        playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
        projectileDir = playerTransform.transform.forward;
    }
    private void Update()
    {
        ProjectileFunc(projectileDir);
    }

    public void ProjectileFunc(Vector3 dir){
        this.gameObject.transform.position += dir * projectileSpeed * Time.deltaTime; 
        Destroy(this.gameObject, destoryTime);
    }
    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.CompareTag("Monster")){
            Debug.Log("ArrowDestroyTest");
            Destroy(this.gameObject);
        }
    }
}
