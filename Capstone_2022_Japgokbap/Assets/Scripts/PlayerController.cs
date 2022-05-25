using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    #region "Private"
    private Camera _camera;
    private Animator playerAnimator;

    //캐릭터 움직임 관련
    [Header("Move")]
    //[SerializeField] private float speed = 10.0f;
    private float moveDirX;
    private float moveDirZ;
    private Vector3 inputDir;
    
    //캐릭터 회전 관련
    [Header("Rotate")]
    [SerializeField] private float rotateSpeed = 5.0f;

    #region "TempSkills"
    //스킬과 일반 공격 관련
    ////Attack은 스크립트 분리하였음.
    private WarriorBuff1 buff1;
    private WarriorBuff2 buff2;
    private PlayerAttack attack;
    #endregion "TempSkills"

    private Rigidbody rb;

    //키 입력을 Dict와 delegate 사용하여 구현하기 위함.
    private Dictionary<KeyCode, Action> keyDictionary;
    private InterfaceManager interfaceManager;

    [Header("SkillList")]
    public List<SkillSO> skillList = new List<SkillSO>();
    #endregion

    #region "Public"
    [HideInInspector] public List<int> PlayerSkill = new List<int>();
    
    [Header("PlayerStatus")]
    public float playerMaxHP;
    public float playerCurrentHP;
    public int playerOffensePower;
    public int playerDeffencePower;
    public float playerMoveSpeed;

    [Header("PlayerLv")]
    public int playerLv;
    public float playerCurrentExp;
    public float playerLvUpExp;
    #endregion

    #region "Test"
    public ClassSO classJob;
    #endregion

    #region "Static"
    public static bool lockBehaviour =false;
    #endregion

    private void Awake() {
        _camera = GetComponentInChildren<Camera>();
        playerAnimator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        interfaceManager = GameObject.FindWithTag("InterfaceManager").GetComponent<InterfaceManager>();
        attack = GetComponent<PlayerAttack>();
        keyDictionary = new Dictionary<KeyCode, Action>
        {
            { KeyCode.Q, Skill_Q },
            { KeyCode.E, Skill_E },
            { KeyCode.R, Skill_R }
        };

        #region "AwakeTest"
        //test
        //평타의 데미지 하드 코딩 상태;
        playerMaxHP = classJob.hp;
        playerCurrentHP = playerMaxHP;
        playerMoveSpeed = classJob.moveSpeed;
        playerOffensePower = classJob.offensePower;
        playerDeffencePower = classJob.deffencePower;
        #endregion
    }

    private void FixedUpdate() {
        Move();
    }

    private void Update() {
        combat.damage = playerOffensePower;

        AttackToMouse();
        InputKey();
    }

    #region "Private Methods"
    private void InputKey(){
        if(Input.anyKeyDown)
        {
            foreach (var dic in keyDictionary)
            {
                if(Input.GetKeyDown(dic.Key))
                {
                    dic.Value();
                }
            }
        }
    }

    private void Move(){
        if(!lockBehaviour){
            moveDirX = Input.GetAxisRaw("Horizontal");
            moveDirZ = Input.GetAxisRaw("Vertical");
            inputDir = new Vector3(moveDirX, 0, moveDirZ).normalized;

            rb.MovePosition(transform.position + inputDir*Time.deltaTime*playerMoveSpeed);
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
    //test
    [SerializeField] SkillSO combat;
    private void AttackToMouse(){     
        if(!lockBehaviour){
            if(Input.GetMouseButtonDown(0)){
                //행동 제한
                lockBehaviour =true;
                Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
                
                RaycastHit rayHit;
                if(Physics.Raycast(ray, out rayHit)) {
                    Vector3 mouseDir = new Vector3(rayHit.point.x, transform.position.y, rayHit.point.z) - transform.position;
                    transform.rotation = Quaternion.LookRotation(mouseDir);
                }
                // attack.StartCoroutine(attack.Attack(attack.attackDelay));
                GameObject instObject = Instantiate(combat.skillPrefab,transform.position, Quaternion.identity);
                instObject.transform.parent = transform;
            }
        }
    }
    private void Skill_Q(){
        if(skillList[0] != null){
            if(!lockBehaviour)
            {
                GameObject instObject = Instantiate(skillList[0].skillPrefab);
                instObject.transform.parent = transform;
            }
        }else return;
    }
    private void Skill_E(){
        if(skillList[1] != null){
            if(!lockBehaviour)
            {
                GameObject instObject = Instantiate(skillList[1].skillPrefab);
                instObject.transform.parent = transform;
            
            }
        }else return;
    }
    private void Skill_R(){
        if(skillList[2] != null){
            if(!lockBehaviour)
            {
                GameObject instObject = Instantiate(skillList[2].skillPrefab);
                instObject.transform.parent = transform;
            }
        }else return;
    }
    private void GetDamaged(int damage)
    {
        int defeatedDamage = damage - playerDeffencePower;

        if (defeatedDamage < 0)
        {
            defeatedDamage = 0;
        }

        playerCurrentHP -= defeatedDamage;
    }
    #endregion

    #region  "Public Methods"
    public void PlayerExpCalc(float exp){
        playerCurrentExp += exp;
        if(playerCurrentExp >= playerLvUpExp){
            playerLv++;
            playerCurrentExp -= playerLvUpExp;
            playerLvUpExp *= 1.3f;
            interfaceManager.ActiveSelectAbillity();
        }
    }

    public bool SkillCheck(int skillNumber){
        if(PlayerSkill.Contains(skillNumber)){
            return true;
        }else return false;
    }
    #endregion

    #region "CallBack"
    
    private void OnCollisionEnter(Collision other) 
    {
        switch(other.transform.tag)
        {
            case "Monster" :
                GetDamaged(other.gameObject.GetComponent<Monster>().GetEnemyPower());
            break;
        }
    }

    /*
    private void OnCollisionStay(Collision other) 
    {        
        switch(other.transform.tag)
        {
            case "Monster" :
                Debug.Log("닿고있는데에");
                GetDamaged(other.gameObject.GetComponent<Monster>().GetEnemyPower()/10);
            break;
        }
    }*/

    private void OnTriggerEnter(Collider other)
    {
        // if(other.transform.CompareTag("EXP")){
        //     PlayerExpCalc(100.0f);
        //     Destroy(other.gameObject);
        // }
        switch(other.tag){
            case "EXP":
                PlayerExpCalc(other.GetComponent<ExpObject>().CalcExp());
                Destroy(other.gameObject);
            break;
            case "Potion":
                playerCurrentHP += 10;
                Debug.Log("CurrentHP has recovered");
                if(playerCurrentHP >= playerMaxHP){
                    playerCurrentHP = playerMaxHP;
                    Debug.Log("Already CurrentHP is Full");
                }
                Destroy(other.gameObject);
            break;
            case "AttackPrefab" :
                GetDamaged(other.gameObject.GetComponent<EnemyAttackHit>().GetDamage());
                Destroy(other.gameObject);
            break;
        }
    }
    
    #endregion
}