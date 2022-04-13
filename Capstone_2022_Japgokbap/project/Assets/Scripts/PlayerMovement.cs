using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region private
    private Camera _camera;
    private Animator playerAnimator;

    //캐릭터 움직임 관련
    [Header("Move")]
    [SerializeField] 
    private float speed = 10.0f;
    private float moveDirX;
    private float moveDirZ;
    private Vector3 inputDir;
    
    //캐릭터 회전 관련
    [Header("Rotate")]
    [SerializeField] private float rotateSpeed = 5.0f;

    
    //스킬과 일반 공격 관련
    ////Attack은 스크립트 분리하였음.
    private WarriorSeismWave seismWave;
    private WarriorBladeStorm bladeStorm;

    private PlayerAttack attack;
    
    #endregion

    //캐릭터 상태 관련( 04/11기준 추후 캐릭터 상태 스크립트로 수정 예정)
    ////기존에 있던 isAttack은 lockBehaviour와 목적이 같이 제거함. (04/13 기준)
    #region public
    public static bool lockBehaviour =false;
    #endregion

    

    private void Awake() {
        _camera = GetComponentInChildren<Camera>();
        playerAnimator = GetComponent<Animator>();
    }

    private void Start(){
        seismWave = GetComponent<WarriorSeismWave>();
        bladeStorm = GetComponent<WarriorBladeStorm>();
        attack = GetComponent<PlayerAttack>();
    }

    private void Update() {
        Move();
        AttackToMouse();
        Skill_Q();
        Skill_E();
    }

    private void Move(){
        if(!lockBehaviour){
            moveDirX = Input.GetAxisRaw("Horizontal");
            moveDirZ = Input.GetAxisRaw("Vertical");

            inputDir = new Vector3(moveDirX,0,moveDirZ).normalized;

            transform.position += inputDir * speed * Time.deltaTime;
            
            playerAnimator.SetBool("isWalk",inputDir != Vector3.zero);

            PlayerRotate();
        }
    }

    private void PlayerRotate(){
        if(moveDirX == 0 && moveDirZ == 0)
            return;
        Quaternion newRotation = Quaternion.LookRotation(inputDir);
        transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, rotateSpeed * Time.deltaTime);
    }

    //마우스로 공격하는 함수
    private void AttackToMouse(){     
        if(!lockBehaviour){
            if(Input.GetMouseButtonDown(0)){
                //행동 제한
                lockBehaviour =true;
                Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
                
                RaycastHit rayHit;
                if(Physics.Raycast(ray, out rayHit)){
                    Vector3 mouseDir = new Vector3(rayHit.point.x, transform.position.y, rayHit.point.z) - transform.position;
                    transform.rotation = Quaternion.LookRotation(mouseDir);
                }
                attack.StartCoroutine(attack.Attack(attack.attackDelay));
            }
        }
    }

//아래의 스킬들은 임시로 특정 키를 지정하여 사용되게 작성해놓았음. 
    private void Skill_Q(){
        if(!lockBehaviour){
            if(Input.GetKeyDown(KeyCode.Q)){
                seismWave.StartCoroutine(seismWave.DoSeismWave());
            }
        }else return;
    }
    private void Skill_E(){
        if(!lockBehaviour){
            if(Input.GetKeyDown(KeyCode.E)){
                bladeStorm.StartCoroutine(bladeStorm.DoBladeStorm());
            }
        }else return;
    }
}