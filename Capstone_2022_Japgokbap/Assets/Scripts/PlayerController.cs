using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    #region "Private"
    private bool isAlive = true;
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

    //attack
    [Header("Attack")]
    [SerializeField] private SkillSO combat;
    

    private Rigidbody rb;

    
    private Dictionary<KeyCode, Action> keyDictionary;
    private InterfaceManager interfaceManager;

    private bool deadCheck = false;

    private bool readySkill;
    private float realtime;
    private bool QCoolTimeCheck = true;
    private bool ECoolTimeCheck = true;
    private bool RCoolTimeCheck = true;

    private AudioSource sound_Step;
    #endregion

    #region "Public"

    [Header("SkillList")]
    public List<SkillSO> skillList = new List<SkillSO>();
    public ClassSO classJob;
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

    #region "Static"
    public static bool lockBehaviour =false;
    public static Vector3 mouseDir;
    public static Vector3 mouseVec;
    public static bool attackLock = false;

    #endregion

    private void Awake() {
        _camera = GetComponentInChildren<Camera>();
        playerAnimator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        interfaceManager = GameObject.FindWithTag("InterfaceManager").GetComponent<InterfaceManager>();
        sound_Step = GetComponent<AudioSource>();
        keyDictionary = new Dictionary<KeyCode, Action>
        {
            { KeyCode.Q, Skill_Q },
            { KeyCode.E, Skill_E },
            { KeyCode.R, Skill_R }
        };

        #region "AwakeTest"
        //test
        playerMaxHP = classJob.hp;
        playerCurrentHP = playerMaxHP;
        playerMoveSpeed = classJob.moveSpeed;
        playerOffensePower = classJob.offensePower;
        playerDeffencePower = classJob.deffencePower;
        #endregion
    }

    private void FixedUpdate() {
        if(!isAlive) return;
        Move();
    }
    private void Update() {
        // combat.damage = playerOffensePower;
        if(!isAlive) return;
        if(!attackLock){
            AttackToMouse();
            InputKey();
        }

        //test
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit rayHit;
        if(Physics.Raycast(ray, out rayHit)) {
            mouseVec = new Vector3(rayHit.point.x, transform.position.y, rayHit.point.z);
            mouseDir = mouseVec - transform.position;
        }
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
        if(lockBehaviour) return;
        moveDirX = Input.GetAxisRaw("Horizontal");
        moveDirZ = Input.GetAxisRaw("Vertical");
        inputDir = new Vector3(moveDirX, 0, moveDirZ).normalized;

        rb.MovePosition(transform.position + inputDir*Time.deltaTime*playerMoveSpeed);
        playerAnimator.SetBool("isWalk",inputDir != Vector3.zero);
        PlayerRotate();
    }

    private void PlayerRotate(){
        if(moveDirX == 0 && moveDirZ == 0)
            return;
        Quaternion newRotation = Quaternion.LookRotation(inputDir);
        transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, rotateSpeed * Time.deltaTime);
    }

    //마우스로 공격하는 함수
    private void AttackToMouse(){     
        if(lockBehaviour) return;
        if(Input.GetMouseButtonDown(0)){
            //행동 제한
            lockBehaviour =true;
            
            transform.rotation = Quaternion.LookRotation(mouseDir);
            GameObject instObject = Instantiate(combat.skillPrefab,transform.position, Quaternion.identity);
            instObject.transform.parent = transform;
        }
    }
    private void Skill_Q(){
        if(skillList[0] == null) return;
        if(lockBehaviour) return;
        if(!skillList[0].coolCheck) return;
        Debug.Log(skillList[0].coolCheck);
        GameObject instObject = Instantiate(skillList[0].skillPrefab);
        instObject.transform.parent = transform;
        StartCoroutine(interfaceManager.CoolSlot(skillList[0].skillCooltime, 0));
    }
    private void Skill_E(){
        if(skillList[1] == null) return;
        if(lockBehaviour) return;
        if(!skillList[1].coolCheck) return;
        GameObject instObject = Instantiate(skillList[1].skillPrefab);
        instObject.transform.parent = transform;
        StartCoroutine(interfaceManager.CoolSlot(skillList[1].skillCooltime, 1));
    }
    private void Skill_R(){
        if(skillList[2] == null) return;
        if(lockBehaviour) return;
        if(!skillList[2].coolCheck) return;
        GameObject instObject = Instantiate(skillList[2].skillPrefab);
        instObject.transform.parent = transform;
        StartCoroutine(interfaceManager.CoolSlot(skillList[2].skillCooltime, 2));
    }
    private void GetDamaged(int damage)
    {
        int defeatedDamage = damage - playerDeffencePower;

        if (defeatedDamage < 0)
        {
            defeatedDamage = 50;
        }

        playerCurrentHP -= defeatedDamage;

        if(deadCheck) return;
        if(playerCurrentHP <= 0){
            deadCheck = true;
            playerAnimator.SetTrigger("Death");
            isAlive = false;
            Destroy(this.gameObject.GetComponent<CapsuleCollider>());
        }
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

    public void PlayStepSound(){
        sound_Step.Play();
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
            case "Boss":
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
                //potion의 고유 hp로 바꿔야함.
                playerCurrentHP += 100;
                Debug.Log("CurrentHP has recovered");
                if(playerCurrentHP >= playerMaxHP){
                    playerCurrentHP = playerMaxHP;
                    Debug.Log("Already CurrentHP is Full");
                }
                Destroy(other.gameObject);
            break;
            case "AttackPrefab" :
                GetDamaged(Mathf.RoundToInt(other.gameObject.GetComponent<EnemyAttackHit>().GetDamage()));
                Destroy(other.gameObject);
            break;
        }
    }
    
    #endregion
}