using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using MonsterLove.StateMachine;

using KeyType = System.String;

[Serializable]
public class StageData
{
    // 2-1 이면 2가 stage 1이 level
    public int stage;
    public int level;
    // 해당 스테이지에 나오는 몬스터 리스트
    public int[] monsterCount;
}

[DisallowMultipleComponent]
public class StageManager : MonoBehaviour
{
    #region "Pulbic"

    //싱글톤
    public static StageManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<StageManager>();
            }

            return m_instance;
        }
    }

    public enum States
    {
        Test, // for debugging
        Ready,
        Stage1,
        Stage2,
        Stage3,
        BossStage,
        Finish
    }

    public StateMachine<States, StateDriverUnity> fsm;

    public int debugCount;

    [Header ("Settings")]
    public float waitingTime;
    public int spawnerCount;
    public bool roundEnded;
    public int stageCount;
    public int roundCount;
    public Text roundInfoText;
    public bool bossCleared;
    [SerializeField] private GameObject currentBoss;

    [Header ("Characters")]
    [SerializeField] private GameObject worriorInstance;
    [SerializeField] private GameObject arhcerInstance;
    [SerializeField] private GameObject magicianInstance;

    [Header ("Objects")]
    public GameObject expClones;
    public GameObject expObject;
    //test
    public GameObject itemClones;
    public GameObject enemyPrefabs;

    [Header ("1-1")]
    public int round1ClearTime;
    public GameObject monsters1;
    public int count1;
    public int count2;

    [Header ("1-2")]
    public GameObject monsters2;
    public int count3;
    public int count4;
    public int count5;
    public int count6;

    [Header ("1-3")]
    public GameObject monsters3;
    public int count7;
    public int count8;
    public int count9;

    [Header ("2-1")]
    public int round2ClearTime;
    public GameObject monsters4;
    public int count10;
    public int count11;

    [Header ("2-2")]
    public GameObject monsters5;
    public int count12;
    public int count13;
    public int count14;

    [Header ("2-3")]
    public GameObject monsters6;
    public int count15;
    public int count16;
    public int count17;
    public int count18;

    [Header ("2-4")]
    public GameObject monsters7;
    public int count19;
    public int count20;
    public int count21;
    public int count22;

    [Header ("3-1")]
    public int round3ClearTime;
    public GameObject monsters8;
    public int count23;
    public int count24;

    [Header ("3-2")]
    public GameObject monsters9;
    public int count25;
    public int count26;
    public int count27;

    [Header ("3-3")]
    public GameObject monsters10;
    public int count28;
    public int count29;
    public int count30;

    [Header ("3-4")]
    public GameObject monsters11;
    public int count31;
    public int count32;
    public int count33;
    public int count34;

    [Header ("3-5")]
    public int round4ClearTime;
    public GameObject monsters12;

    /// <summary> 풀에서 꺼내오기 </summary>
    public GameObject SpawnMonster(KeyType key, Transform parent)
    {
        // 키가 존재하지 않는 경우 null 리턴
        if (!poolDict.TryGetValue(key, out var pool))
        {
            return null;
        }

        GameObject go;

        // 1. 풀에 재고가 있는 경우 : 꺼내오기
        if (pool.Count > 0)
        {
            go = pool.Pop();
        }
        // 2. 재고가 없는 경우 샘플로부터 복제
        else
        {
            go = CloneFromSample(key);
            clonePoolDict.Add(go, pool); // Clone-Stack 캐싱
        }

        go.transform.position = enemySpawner[spawnerCount++ % enemySpawner.Length].transform.position;
        go.SetActive(true);
        go.transform.SetParent(parent);

        return go;
    }

    /// <summary> 풀에 집어넣기 </summary>
    public void DespawnMonster(GameObject go)
    {
        // 캐싱된 게임오브젝트가 아닌 경우 파괴
        if (!clonePoolDict.TryGetValue(go, out var pool))
        {
            Destroy(go);
            return;
        }

        // 집어넣기
        go.GetComponent<Monster>().InitState();
        go.transform.position = monsterPool.transform.position;
        go.transform.parent = monsterPool.transform;
        go.SetActive(false);
        pool.Push(go);
    }

    #endregion

    #region "Private"

    private static StageManager m_instance;

    // 인스펙터에서 오브젝트 풀링 대상 정보 추가
    [SerializeField] private List<MonsterPoolData> monsterPoolDataList = new List<MonsterPoolData>();

    // Key - 복제용 오브젝트 원본
    private Dictionary<KeyType, GameObject> sampleDict;

    // Key - 풀 정보
    private Dictionary<KeyType, MonsterPoolData> dataDict;

    // Key - 풀
    private Dictionary<KeyType, Stack<GameObject>> poolDict;

    // 복제된 게임오브젝트 - 풀
    private Dictionary<GameObject, Stack<GameObject>> clonePoolDict;

    [Header ("Spawners")]
    [SerializeField] private GameObject monsterPool;
    [SerializeField] private GameObject[] enemySpawner;
    [SerializeField] private GameObject bossSpawner;

    [Header ("Boss Monsters")]
    [SerializeField] private GameObject lich;
    [SerializeField] private GameObject orc;
    [SerializeField] private GameObject golem;
    [SerializeField] private GameObject demon;

    #endregion
    
    private void Awake() 
    {
        fsm = new StateMachine<States, StateDriverUnity>(this);

        fsm.ChangeState(States.Ready);
        //fsm.ChangeState(States.Stage2); // for debugging
    }

    private void Start()
    {
        PoolInit();
    }

    private void Update() 
    {
        fsm.Driver.Update.Invoke();
    }

    private void PoolInit()
    {
        int len = monsterPoolDataList.Count;
        if (len == 0) return;

        // 1. Dictionary 생성
        sampleDict = new Dictionary<KeyType, GameObject>(len);
        dataDict = new Dictionary<KeyType, MonsterPoolData>(len);
        poolDict = new Dictionary<KeyType, Stack<GameObject>>(len);
        clonePoolDict = new Dictionary<GameObject, Stack<GameObject>>(len * MonsterPoolData.INITIAL_COUNT);

        // 2. Data로부터 새로운 Pool 오브젝트 정보 생성
        foreach (var data in monsterPoolDataList)
        {
            PoolRegister(data);
        }
    }

    /// <summary> Pool 데이터로부터 새로운 Pool 오브젝트 정보 등록 </summary>
    private void PoolRegister(MonsterPoolData data)
    {
        // 중복 키는 등록 불가능
        if (poolDict.ContainsKey(data.key))
        {
            return;
        }

        // 1. 샘플 게임오브젝트 생성, PoolObject 컴포넌트 존재 확인
        GameObject sample = Instantiate(data.prefab, monsterPool.transform.position, Quaternion.identity);
        sample.name = data.prefab.name;
        sample.transform.parent = monsterPool.transform;
        sample.SetActive(false);

        // 2. Pool Dictionary에 풀 생성 + 풀에 미리 오브젝트들 만들어 담아놓기
        Stack<GameObject> pool = new Stack<GameObject>(data.maxObjectCount);
        for (int i = 0; i < data.initialObjectCount; i++)
        {
            GameObject clone = Instantiate(data.prefab, monsterPool.transform.position, Quaternion.identity);
            clone.SetActive(false);
            clone.transform.position = monsterPool.transform.position;
            clone.transform.parent = monsterPool.transform;
            pool.Push(clone);

            clonePoolDict.Add(clone, pool); // Clone-Stack 캐싱
        }

        // 3. 딕셔너리에 추가
        sampleDict.Add(data.key, sample);
        dataDict.Add(data.key, data);
        poolDict.Add(data.key, pool);
    }

    /// <summary> 샘플 오브젝트 복제하기 </summary>
    private GameObject CloneFromSample(KeyType key)
    {
        if (!sampleDict.TryGetValue(key, out GameObject sample)) return null;

        return Instantiate(sample);
    }

    IEnumerator Ready_Enter()
    {
        roundInfoText.text = "3초 뒤 시작됩니다.";
		yield return new WaitForSeconds(1f);
		roundInfoText.text = "2초 뒤 시작됩니다.";
		yield return new WaitForSeconds(1f);
        roundInfoText.text = "1초 뒤 시작됩니다.";
		yield return new WaitForSeconds(1f);

        fsm.ChangeState(States.Stage1);
    }

    void Ready_Exit()
    {
        roundInfoText.text = "Stage 1-1";

        GameManager.instance.SetTime(round1ClearTime);
    }

    IEnumerator Stage1_Enter()
    {
        for (int i = 0; i < count1; i++)
        {
            SpawnMonster("Skeleton1", monsters1.transform);

            debugCount += 1;

            yield return new WaitForSeconds(waitingTime);
        }

        debugCount = 0;

        for (int i = 0; i < count2; i++)
        {
            SpawnMonster("Skeleton2", monsters1.transform);

            debugCount += 1;

            yield return new WaitForSeconds(waitingTime);
        }

        debugCount = 0;

        yield return new WaitForSeconds(10f);

        roundInfoText.text = "Stage 1-2";
        roundCount = 2;

        for (int i = 0; i < count3; i++)
        {
            SpawnMonster("Skeleton1", monsters2.transform);

            yield return new WaitForSeconds(waitingTime);
        }

        for (int i = 0; i < count4; i++)
        {
            SpawnMonster("Skeleton2", monsters2.transform);

            yield return new WaitForSeconds(waitingTime);
        }

        for (int i = 0; i < count5; i++)
        {
            SpawnMonster("Skeleton3", monsters2.transform);

            yield return new WaitForSeconds(waitingTime);
        }

        for (int i = 0; i < count6; i++)
        {
            SpawnMonster("Skeleton4", monsters2.transform);

            yield return new WaitForSeconds(waitingTime);
        }

        yield return new WaitForSeconds(10f);

        roundInfoText.text = "Stage 1-3";
        roundCount = 3;

        for (int i = 0; i < count7; i++)
        {
            SpawnMonster("Skeleton1", monsters3.transform);

            yield return new WaitForSeconds(waitingTime);
        }

        for (int i = 0; i < count7; i++)
        {
            SpawnMonster("Skeleton2", monsters3.transform);

            yield return new WaitForSeconds(waitingTime);
        }

        for (int i = 0; i < count8; i++)
        {
            SpawnMonster("Skeleton3", monsters3.transform);

            yield return new WaitForSeconds(waitingTime);
        }

        for (int i = 0; i < count9; i++)
        {
            SpawnMonster("Skeleton4", monsters3.transform);

            yield return new WaitForSeconds(waitingTime);
        }

        yield return new WaitForSeconds(10f);

        roundInfoText.text = "Stage 1 Boss";
        roundCount = 4;

        GameManager.instance.ActiveBossUi(true);
        GameObject monster = Instantiate(lich, bossSpawner.transform.position, Quaternion.identity);
        monster.transform.parent = monsters3.transform;
        currentBoss = monster;
    }

    void Stage1_Update()
    {
        if (!bossCleared)
        {
            GameManager.instance.bossHpBar.value = currentBoss.GetComponent<Monster>().GetEnemyHp() / currentBoss.GetComponent<Monster>().GetOriginHp();
        }
        else if (bossCleared)
        {
            GameManager.instance.AddScore(Mathf.RoundToInt(GameManager.instance.time) * 50);
            GameManager.instance.ActiveBossUi(false);

            fsm.ChangeState(States.Stage2);
        }
        else if (GameManager.instance.time == 0 && !bossCleared)
        {
            GameManager.instance.ActiveBossUi(false);

            fsm.ChangeState(States.Finish);
        }
    }

    void Stage1_Exit()
    {
        bossCleared = false;

        ClearChildObject(monsters1);
        ClearChildObject(monsters2);
        ClearChildObject(monsters3);
    }

    IEnumerator Stage2_Enter()
    {
        roundInfoText.text = "3초 뒤 시작됩니다.";
		yield return new WaitForSeconds(1f);
		roundInfoText.text = "2초 뒤 시작됩니다.";
		yield return new WaitForSeconds(1f);
        roundInfoText.text = "1초 뒤 시작됩니다.";
		yield return new WaitForSeconds(1f);

        GameManager.instance.SetTime(round2ClearTime);
        
        roundInfoText.text = "Stage 2-1";
        stageCount = 2;
        roundCount = 1;

        for (int i = 0; i < count10; i++)
        {
            SpawnMonster("Goblin1", monsters4.transform);

            yield return new WaitForSeconds(waitingTime);
        }

        for (int i = 0; i < count11; i++)
        {
            SpawnMonster("Goblin2", monsters4.transform);

            yield return new WaitForSeconds(waitingTime);
        }

        yield return new WaitForSeconds(10f);

        roundInfoText.text = "Stage 2-2";
        roundCount = 2;

        for (int i = 0; i < count12; i++)
        {
            SpawnMonster("Goblin1", monsters5.transform);

            yield return new WaitForSeconds(waitingTime);
        }

        for (int i = 0; i < count13; i++)
        {
            SpawnMonster("Goblin2", monsters5.transform);

            yield return new WaitForSeconds(waitingTime);
        }

        for (int i = 0; i < count14; i++)
        {
            SpawnMonster("Goblin3", monsters5.transform);

            yield return new WaitForSeconds(waitingTime);
        }

        yield return new WaitForSeconds(10f);

        roundInfoText.text = "Stage 2-3";
        roundCount = 3;

        for (int i = 0; i < count15; i++)
        {
            SpawnMonster("Goblin1", monsters6.transform);

            yield return new WaitForSeconds(waitingTime);
        }

        for (int i = 0; i < count16; i++)
        {
            SpawnMonster("Goblin2", monsters6.transform);

            yield return new WaitForSeconds(waitingTime);
        }

        for (int i = 0; i < count17; i++)
        {
            SpawnMonster("Goblin3", monsters6.transform);

            yield return new WaitForSeconds(waitingTime);
        }

        for (int i = 0; i < count18; i++)
        {
            SpawnMonster("Goblin4", monsters6.transform);

            yield return new WaitForSeconds(waitingTime);
        }

        yield return new WaitForSeconds(10f);

        roundInfoText.text = "Stage 2-4";
        roundCount = 4;

        for (int i = 0; i < count19; i++)
        {
            SpawnMonster("Goblin1", monsters7.transform);

            yield return new WaitForSeconds(waitingTime);
        }

        for (int i = 0; i < count20; i++)
        {
            SpawnMonster("Goblin2", monsters7.transform);

            yield return new WaitForSeconds(waitingTime);
        }

        for (int i = 0; i < count21; i++)
        {
            SpawnMonster("Goblin3", monsters7.transform);

            yield return new WaitForSeconds(waitingTime);
        }

        for (int i = 0; i < count22; i++)
        {
            SpawnMonster("Goblin4", monsters7.transform);

            yield return new WaitForSeconds(waitingTime);
        }

        yield return new WaitForSeconds(10f);

        roundInfoText.text = "Stage 2 Boss";
        roundCount = 5;

        GameManager.instance.ActiveBossUi(true);
        GameObject monster = Instantiate(orc, bossSpawner.transform.position, Quaternion.identity);
        monster.transform.parent = monsters7.transform;
        currentBoss = monster;
    }

    void Stage2_Update()
    {
        if (!bossCleared)
        {
            GameManager.instance.bossHpBar.value = currentBoss.GetComponent<Monster>().GetEnemyHp() / currentBoss.GetComponent<Monster>().GetOriginHp();
        }
        else if (bossCleared)
        {
            GameManager.instance.AddScore(Mathf.RoundToInt(GameManager.instance.time) * 60);
            GameManager.instance.ActiveBossUi(false);

            fsm.ChangeState(States.Stage3);
        }
        else if (GameManager.instance.time == 0 && !bossCleared)
        {
            GameManager.instance.ActiveBossUi(false);

            fsm.ChangeState(States.Finish);
        }
    }

    void Stage2_Exit()
    {
        bossCleared = false;

        ClearChildObject(monsters4);
        ClearChildObject(monsters5);
        ClearChildObject(monsters6);
        ClearChildObject(monsters7);
    }

    IEnumerator Stage3_Enter()
    {
        roundInfoText.text = "3초 뒤 시작됩니다.";
		yield return new WaitForSeconds(1f);
		roundInfoText.text = "2초 뒤 시작됩니다.";
		yield return new WaitForSeconds(1f);
        roundInfoText.text = "1초 뒤 시작됩니다.";
		yield return new WaitForSeconds(1f);

        GameManager.instance.SetTime(round3ClearTime);

        roundInfoText.text = "Stage 3-1";
        stageCount = 3;
        roundCount = 1;

        for (int i = 0; i < count23; i++)
        {
            SpawnMonster("Golem1", monsters8.transform);

            yield return new WaitForSeconds(waitingTime);
        }

        for (int i = 0; i < count24; i++)
        {
            SpawnMonster("Golem2", monsters8.transform);

            yield return new WaitForSeconds(waitingTime);
        }

        yield return new WaitForSeconds(10f);

        roundInfoText.text = "Stage 3-2";
        roundCount = 2;

        for (int i = 0; i < count25; i++)
        {
            SpawnMonster("Golem1", monsters9.transform);

            yield return new WaitForSeconds(waitingTime);
        }

        for (int i = 0; i < count26; i++)
        {
            SpawnMonster("Golem2", monsters9.transform);

            yield return new WaitForSeconds(waitingTime);
        }

        for (int i = 0; i < count27; i++)
        {
            SpawnMonster("Golem3", monsters9.transform);

            yield return new WaitForSeconds(waitingTime);
        }

        yield return new WaitForSeconds(10f);

        roundInfoText.text = "Stage 3-3";
        roundCount = 3;

        for (int i = 0; i < count28; i++)
        {
            SpawnMonster("Golem2", monsters10.transform);

            yield return new WaitForSeconds(waitingTime);
        }

        for (int i = 0; i < count29; i++)
        {
            SpawnMonster("Golem3", monsters10.transform);

            yield return new WaitForSeconds(waitingTime);
        }

        for (int i = 0; i < count30; i++)
        {
            SpawnMonster("Golem4", monsters10.transform);

            yield return new WaitForSeconds(waitingTime);
        }

        yield return new WaitForSeconds(10f);

        roundInfoText.text = "Stage 3-4";
        roundCount = 4;

        for (int i = 0; i < count31; i++)
        {
            SpawnMonster("Golem1", monsters11.transform);

            yield return new WaitForSeconds(waitingTime);
        }

        for (int i = 0; i < count32; i++)
        {
            SpawnMonster("Golem2", monsters11.transform);

            yield return new WaitForSeconds(waitingTime);
        }

        for (int i = 0; i < count33; i++)
        {
            SpawnMonster("Golem3", monsters11.transform);

            yield return new WaitForSeconds(waitingTime);
        }

        for (int i = 0; i < count34; i++)
        {
            SpawnMonster("Golem4", monsters11.transform);

            yield return new WaitForSeconds(waitingTime);
        }

        yield return new WaitForSeconds(10f);

        roundInfoText.text = "Stage 3 Boss";
        roundCount = 5;

        GameManager.instance.ActiveBossUi(true);
        GameObject monster = Instantiate(golem, bossSpawner.transform.position, Quaternion.identity);
        monster.transform.parent = monsters11.transform;
        currentBoss = monster;
    }

    void Stage3_Update()
    {
        if (!bossCleared)
        {
            GameManager.instance.bossHpBar.value = currentBoss.GetComponent<Monster>().GetEnemyHp() / currentBoss.GetComponent<Monster>().GetOriginHp();
        }
        else if (bossCleared)
        {
            GameManager.instance.AddScore(Mathf.RoundToInt(GameManager.instance.time) * 70);
            GameManager.instance.ActiveBossUi(false);

            fsm.ChangeState(States.BossStage);
        }
        else if (GameManager.instance.time == 0 && !bossCleared)
        {
            GameManager.instance.ActiveBossUi(false);
            fsm.ChangeState(States.Finish);
        }
    }

    void Stage3_Exit()
    {
        bossCleared = false;

        ClearChildObject(monsters8);
        ClearChildObject(monsters9);
        ClearChildObject(monsters10);
        ClearChildObject(monsters11);
    }

    IEnumerator BossStage_Enter()
    {
        roundInfoText.text = "3초 뒤 시작됩니다.";
		yield return new WaitForSeconds(1f);
		roundInfoText.text = "2초 뒤 시작됩니다.";
		yield return new WaitForSeconds(1f);
        roundInfoText.text = "1초 뒤 시작됩니다.";
		yield return new WaitForSeconds(1f);

        GameManager.instance.SetTime(round4ClearTime);

        roundInfoText.text = "Final Boss";
        roundCount = 6;

        GameManager.instance.ActiveBossUi(true);

        GameObject monster = Instantiate(demon, bossSpawner.transform.position, Quaternion.identity);
        monster.transform.parent = monsters12.transform;
        currentBoss = monster;
    }

    void BossStage_Update()
    {
        if (!bossCleared)
        {
            GameManager.instance.bossHpBar.value = currentBoss.GetComponent<Monster>().GetEnemyHp() / currentBoss.GetComponent<Monster>().GetOriginHp();
        }
        else if (bossCleared)
        {
            GameManager.instance.ActiveBossUi(false);
            GameManager.instance.AddScore(Mathf.RoundToInt(GameManager.instance.time) * 80);

            fsm.ChangeState(States.Finish);
        }
    }

    void BossStage_Exit()
    {
        //ClearChildObject(monsters12);
    }

    void Finish_Enter()
    {
        roundInfoText.text = "끝";
        //결과화면
    }

    void Finish_Update()
    {
        //로비로 돌아가기
    }

    void Finish_Exit()
    {
        //인게임 설정값 초기화
    }

    /*
    public void InsertMonster(Stack<GameObject> stack, GameObject monster)
    {
        stack.Push(monster);
        monster.transform.parent = monsterPool.transform;
        monster.SetActive(false);
    }
    */

    /*
    public GameObject GetMonster(Stack<GameObject> stack)
    {
        GameObject monster = stack.Pop();

        return monster;
    }
    */

    /*
    private void SpawnMonster(Stack<GameObject> stack, GameObject parent)
    {
        if (GetMonster(stack) == null)
        { 
            
        }

        GameObject monster = GetMonster(stack);
        monster.transform.position = enemySpawner[spawnerCount++ % 3].transform.position;
        monster.SetActive(true);
        monster.transform.parent = parent.transform;
    }
    */

    private void ClearChildObject(GameObject parent)
    {
        Transform[] childList = parent.GetComponentsInChildren<Transform>();

        if (childList != null)
        {
            for (int i = 1; i < childList.Length; i++)
            {
                if (childList[i] != transform)
                    DespawnMonster(childList[i].gameObject);
            }
        }
    }

    /*
    void Start() 
    {
        StageData stageData = new StageData();
        stageData.stage = 1;
        stageData.level = 1;
        stageData.monsterCount = new int[2] { 40, 10 };

        string json = JsonUtility.ToJson(stageData);
        Debug.Log("ToJson : " + json);

        string fileName = "1-1";
        string path = Application.dataPath + "/" + fileName + ".Json";

        FileStream fileStream = new FileStream(path, FileMode.Create);
        byte[] data = Encoding.UTF8.GetBytes(json);
        fileStream.Write(data, 0, data.Length);
        fileStream.Close();
    }*/
}
