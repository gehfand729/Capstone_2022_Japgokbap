using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasingArrow : MonoBehaviour
{
    private Transform playerTransform;
    [SerializeField] private float projectileSpeed = 10;
    [SerializeField] private float destoryTime = 2;
    private Vector3 projectileDir;

    
    private Rigidbody m_rigid;
    [SerializeField]private Transform m_tfTarget;
    
    private float TimeLeft = 3.0f;
    private float nextTime = 0.0f;

    [SerializeField] LayerMask m_layerMask = 0;
    
    int count = 0;

    private void Awake() {
        playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
        projectileDir = playerTransform.transform.forward;
        m_rigid = this.gameObject.GetComponent<Rigidbody>();
    }

    private void Start(){
        Destroy(this.gameObject, destoryTime);
    }
    private void Update()
    {   
        if(Time.time > nextTime){
            nextTime = Time.time + TimeLeft;
            
            // Vector3 t_dir = Vector3.Lerp(m_tfTarget.position, transform.position, 5);
            SearchMonster();
        }
        if(m_tfTarget != null){
            // Vector3 t_dir = (m_tfTarget.position - transform.position).normalized;
            ChasingFunc();
        }
        // }else ProjectileFunc(projectileDir);
        
    }

    private void ChasingFunc(){
        count ++;
        this.gameObject.transform.LookAt(m_tfTarget);
        if(count % 30 == 0){
            GetComponent<AudioSource>().Play();
        }
         m_rigid.AddForce(this.gameObject.transform.forward * projectileSpeed, ForceMode.Impulse);
        // m_rigid.velocity = dir.normalized * projectileSpeed; 
            
    }

    private void ProjectileFunc(Vector3 dir){
        m_rigid.velocity = dir.normalized * projectileSpeed; 
        Destroy(this.gameObject, destoryTime);
    }
    
    private void SearchMonster(){
        m_rigid.velocity = Vector3.zero;
        Collider[] t_cols = Physics.OverlapSphere(transform.position, 30.0f, m_layerMask);

        if(t_cols.Length > 0){
            m_tfTarget = t_cols[Random.Range(0, t_cols.Length)].transform;
        }
    }

}
