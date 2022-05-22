using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using MonsterLove.StateMachine;

[Serializable]
public class StageData
{
    // 2-1 이면 2가 stage 1이 level
    public int stage;
    public int level;
    // 해당 스테이지에 나오는 몬스터 리스트
    public int[] monsterCount;
}

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
        Ready,
        Stage1,
        Stage2,
        Stage3,
        BossStage,
        Finish
    }

    public StateMachine<States, StateDriverUnity> fsm;

    //public Dictionary<Byte, GameObject> m_pool = new Dictionary<byte, GameObject>();
    public Queue<GameObject> skel_0_queue = new Queue<GameObject>();
    public Queue<GameObject> skel_1_queue = new Queue<GameObject>();
    public Queue<GameObject> skel_2_queue = new Queue<GameObject>();
    public Queue<GameObject> skel_3_queue = new Queue<GameObject>();
    public Queue<GameObject> goblin_0_queue = new Queue<GameObject>();
    public Queue<GameObject> goblin_1_queue = new Queue<GameObject>();
    public Queue<GameObject> goblin_2_queue = new Queue<GameObject>();
    public Queue<GameObject> goblin_3_queue = new Queue<GameObject>();
    public Queue<GameObject> golem_0_queue = new Queue<GameObject>();
    public Queue<GameObject> golem_1_queue = new Queue<GameObject>();
    public Queue<GameObject> golem_2_queue = new Queue<GameObject>();
    public Queue<GameObject> golem_3_queue = new Queue<GameObject>();

    [Header ("Settings")]
    public float waitingTime;
    public int spawnerCount;
    public bool roundEnded;
    public int stageCount;
    public int roundCount;
    public Text roundInfoText;
    public bool bossCleared;

    [Header ("Characters")]
    [SerializeField] private GameObject worriorInstance;
    [SerializeField] private GameObject arhcerInstance;
    [SerializeField] private GameObject magicianInstance;

    [Header ("Objects")]
    public GameObject expClones;
    public GameObject expObject;
    //test
    public GameObject itemClones;

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

    #endregion

    #region "Private"

    private static StageManager m_instance;

    [Header ("Spawners")]
    [SerializeField] private GameObject monsterPool;
    [SerializeField] private GameObject[] enemySpawner;
    [SerializeField] private GameObject bossSpawner;

    [Header ("Monsters")]
    [SerializeField] private GameObject[] skeletons;
    [SerializeField] private GameObject[] goblins;
    [SerializeField] private GameObject[] orc;
    [SerializeField] private GameObject[] golems;
    [SerializeField] private GameObject[] demon;
    [SerializeField] private GameObject[] specialMonsters;
    [SerializeField] private int worriorMaxCount;
    [SerializeField] private int archerMaxCount;
    [SerializeField] private int magicianMaxCount;

    #endregion
    
    private void Awake() 
    {
        fsm = new StateMachine<States, StateDriverUnity>(this);

        fsm.ChangeState(States.Ready);
    }

    private void Update() 
    {
        fsm.Driver.Update.Invoke();
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
        EarlySpawnMonster(skel_0_queue, skeletons[0], worriorMaxCount);
        EarlySpawnMonster(skel_1_queue, skeletons[1], worriorMaxCount);
        EarlySpawnMonster(skel_2_queue, skeletons[2], worriorMaxCount);
        EarlySpawnMonster(skel_3_queue, skeletons[3], worriorMaxCount);

        roundInfoText.text = "Stage 1-1";

        GameManager.instance.SetTime(round1ClearTime);
    }

    IEnumerator Stage1_Enter()
    {
        for (int i = 0; i < count1; i++)
        {
            SpawnMonster(skel_0_queue, monsters1);

            yield return new WaitForSeconds(waitingTime);
        }

        for (int i = 0; i < count2; i++)
        {
            SpawnMonster(skel_1_queue, monsters1);

            yield return new WaitForSeconds(waitingTime);
        }

        yield return new WaitForSeconds(10f);

        roundInfoText.text = "Stage 1-2";

        for (int i = 0; i < count3; i++)
        {
            SpawnMonster(skel_0_queue, monsters2);

            yield return new WaitForSeconds(waitingTime);
        }

        for (int i = 0; i < count4; i++)
        {
            SpawnMonster(skel_1_queue, monsters2);

            yield return new WaitForSeconds(waitingTime);
        }

        for (int i = 0; i < count5; i++)
        {
            SpawnMonster(skel_2_queue, monsters2);

            yield return new WaitForSeconds(waitingTime);
        }

        for (int i = 0; i < count6; i++)
        {
            SpawnMonster(skel_3_queue, monsters2);

            yield return new WaitForSeconds(waitingTime);
        }

        yield return new WaitForSeconds(10f);

        roundInfoText.text = "Stage 1-3";

        for (int i = 0; i < count7; i++)
        {
            SpawnMonster(skel_0_queue, monsters3);

            yield return new WaitForSeconds(waitingTime);
        }

        for (int i = 0; i < count7; i++)
        {
            SpawnMonster(skel_1_queue, monsters3);

            yield return new WaitForSeconds(waitingTime);
        }

        for (int i = 0; i < count8; i++)
        {
            SpawnMonster(skel_2_queue, monsters3);

            yield return new WaitForSeconds(waitingTime);
        }

        for (int i = 0; i < count9; i++)
        {
            SpawnMonster(skel_3_queue, monsters3);

            yield return new WaitForSeconds(waitingTime);
        }

        yield return new WaitForSeconds(10f);
    }

    void Stage1_Update()
    {
        if (GameManager.instance.time == 0)
        {
            fsm.ChangeState(States.Stage2);
        }
    }

    void Stage1_Exit()
    {
        ClearChildObject(monsters1);
        ClearChildObject(monsters2);
        ClearChildObject(monsters3);

        EarlySpawnMonster(goblin_0_queue, goblins[0], worriorMaxCount);
        EarlySpawnMonster(goblin_1_queue, goblins[1], archerMaxCount);
        EarlySpawnMonster(goblin_2_queue, goblins[2], worriorMaxCount);
        EarlySpawnMonster(goblin_3_queue, goblins[3], magicianMaxCount);
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

        for (int i = 0; i < count10; i++)
        {
            SpawnMonster(goblin_0_queue, monsters4);

            yield return new WaitForSeconds(waitingTime);
        }

        for (int i = 0; i < count11; i++)
        {
            SpawnMonster(goblin_1_queue, monsters4);

            yield return new WaitForSeconds(waitingTime);
        }

        yield return new WaitForSeconds(10f);

        roundInfoText.text = "Stage 2-2";

        for (int i = 0; i < count12; i++)
        {
            SpawnMonster(goblin_0_queue, monsters5);

            yield return new WaitForSeconds(waitingTime);
        }

        for (int i = 0; i < count13; i++)
        {
            SpawnMonster(goblin_1_queue, monsters5);

            yield return new WaitForSeconds(waitingTime);
        }

        for (int i = 0; i < count14; i++)
        {
            SpawnMonster(goblin_2_queue, monsters5);

            yield return new WaitForSeconds(waitingTime);
        }

        yield return new WaitForSeconds(10f);

        roundInfoText.text = "Stage 2-3";

        for (int i = 0; i < count15; i++)
        {
            SpawnMonster(goblin_0_queue, monsters6);

            yield return new WaitForSeconds(waitingTime);
        }

        for (int i = 0; i < count16; i++)
        {
            SpawnMonster(goblin_1_queue, monsters6);

            yield return new WaitForSeconds(waitingTime);
        }

        for (int i = 0; i < count17; i++)
        {
            SpawnMonster(goblin_2_queue, monsters6);

            yield return new WaitForSeconds(waitingTime);
        }

        for (int i = 0; i < count18; i++)
        {
            SpawnMonster(goblin_3_queue, monsters6);

            yield return new WaitForSeconds(waitingTime);
        }

        yield return new WaitForSeconds(10f);

        roundInfoText.text = "Stage 2-4";

        for (int i = 0; i < count19; i++)
        {
            SpawnMonster(goblin_0_queue, monsters7);

            yield return new WaitForSeconds(waitingTime);
        }

        for (int i = 0; i < count20; i++)
        {
            SpawnMonster(goblin_1_queue, monsters7);

            yield return new WaitForSeconds(waitingTime);
        }

        for (int i = 0; i < count21; i++)
        {
            SpawnMonster(goblin_2_queue, monsters7);

            yield return new WaitForSeconds(waitingTime);
        }

        for (int i = 0; i < count22; i++)
        {
            SpawnMonster(goblin_3_queue, monsters7);

            yield return new WaitForSeconds(waitingTime);
        }

        yield return new WaitForSeconds(10f);

        roundInfoText.text = "Stage 2 Boss";

        GameObject monster = Instantiate(orc[0], bossSpawner.transform.position, Quaternion.identity);
        monster.transform.parent = monsters7.transform;
    }

    void Stage2_Update()
    {
        if (bossCleared)
        {
            fsm.ChangeState(States.Stage2);
        }
        else if (GameManager.instance.time == 0 && !bossCleared)
        {
            fsm.ChangeState(States.Finish);
        }
    }

    void Stage2_Exit()
    {
        ClearChildObject(monsters4);
        ClearChildObject(monsters5);
        ClearChildObject(monsters6);
        ClearChildObject(monsters7);

        EarlySpawnMonster(golem_0_queue, golems[0], worriorMaxCount);
        EarlySpawnMonster(golem_1_queue, golems[1], worriorMaxCount);
        EarlySpawnMonster(golem_2_queue, golems[2], magicianMaxCount);
        EarlySpawnMonster(golem_3_queue, golems[3], magicianMaxCount);
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

        for (int i = 0; i < count23; i++)
        {
            SpawnMonster(golem_0_queue, monsters8);

            yield return new WaitForSeconds(waitingTime);
        }

        for (int i = 0; i < count24; i++)
        {
            SpawnMonster(golem_1_queue, monsters8);

            yield return new WaitForSeconds(waitingTime);
        }

        yield return new WaitForSeconds(10f);

        roundInfoText.text = "Stage 3-2";

        for (int i = 0; i < count25; i++)
        {
            SpawnMonster(golem_0_queue, monsters9);

            yield return new WaitForSeconds(waitingTime);
        }

        for (int i = 0; i < count26; i++)
        {
            SpawnMonster(golem_1_queue, monsters9);

            yield return new WaitForSeconds(waitingTime);
        }

        for (int i = 0; i < count27; i++)
        {
            SpawnMonster(golem_2_queue, monsters9);

            yield return new WaitForSeconds(waitingTime);
        }

        yield return new WaitForSeconds(10f);

        roundInfoText.text = "Stage 3-3";

        for (int i = 0; i < count28; i++)
        {
            SpawnMonster(golem_1_queue, monsters10);

            yield return new WaitForSeconds(waitingTime);
        }

        for (int i = 0; i < count29; i++)
        {
            SpawnMonster(golem_2_queue, monsters10);

            yield return new WaitForSeconds(waitingTime);
        }

        for (int i = 0; i < count30; i++)
        {
            SpawnMonster(golem_3_queue, monsters10);

            yield return new WaitForSeconds(waitingTime);
        }

        yield return new WaitForSeconds(10f);

        roundInfoText.text = "Stage 3-4";
        
        for (int i = 0; i < count31; i++)
        {
            SpawnMonster(golem_0_queue, monsters11);

            yield return new WaitForSeconds(waitingTime);
        }

        for (int i = 0; i < count32; i++)
        {
            SpawnMonster(golem_1_queue, monsters11);

            yield return new WaitForSeconds(waitingTime);
        }

        for (int i = 0; i < count33; i++)
        {
            SpawnMonster(golem_2_queue, monsters11);

            yield return new WaitForSeconds(waitingTime);
        }

        for (int i = 0; i < count34; i++)
        {
            SpawnMonster(golem_3_queue, monsters11);

            yield return new WaitForSeconds(waitingTime);
        }

        yield return new WaitForSeconds(10f);

        roundInfoText.text = "Stage 3 Boss";

        GameObject monster = Instantiate(golems[4], bossSpawner.transform.position, Quaternion.identity);
        monster.transform.parent = monsters11.transform;
    }

    void Stage3_Update()
    {
        if (bossCleared)
        {
            fsm.ChangeState(States.BossStage);
        }
        else if (GameManager.instance.time == 0 && !bossCleared)
        {
            fsm.ChangeState(States.Finish);
        }
    }

    void Stage3_Exit()
    {
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

        GameObject monster = Instantiate(demon[0], bossSpawner.transform.position, Quaternion.identity);
        monster.transform.parent = monsters12.transform;
    }

    void BossStage_Update()
    {
        if (bossCleared)
        {
            fsm.ChangeState(States.Finish);
        }
    }

    void BossStage_Exit()
    {
        ClearChildObject(monsters12);
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

    public void EarlySpawnMonster(Queue<GameObject> queue, GameObject monster, int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject t_monster = Instantiate(monster, monsterPool.transform.position, Quaternion.identity);

            InsertMonsterInQueue(queue, t_monster);
        }
    }

    /*
    public void InsertMonsterInDictionary(Byte code, GameObject monster)
    {
        m_pool.Add(code, monster);
        monster.SetActive(false);
    }

    public GameObject GetMonsterInDictionary(Byte code)
    {
        m_pool.Remove(code);
        GameObject monster;
        if (m_pool.TryGetValue(code, out monster))
        {
            monster.SetActive(true);

            return monster;
        }
        else
        {
            return null;
        }
    }
    */

    public void InsertMonsterInQueue(Queue<GameObject> queue, GameObject monster)
    {
        queue.Enqueue(monster);
        monster.transform.parent = monsterPool.transform;
        monster.SetActive(false);
    }

    public GameObject GetMonsterInQueue(Queue<GameObject> queue)
    {
        GameObject monster = queue.Dequeue();

        return monster;
    }

    private void SpawnMonster(Queue<GameObject> queue, GameObject parent)
    {
        if (GetMonsterInQueue(queue) == null)
        { 
            
        }

        GameObject monster = GetMonsterInQueue(queue);
        monster.transform.position = enemySpawner[spawnerCount++ % 3].transform.position;
        monster.SetActive(true);
        monster.transform.parent = parent.transform;
    }

    private void ClearChildObject(GameObject parent)
    {
        Transform[] childList = parent.GetComponentsInChildren<Transform>();

        if (childList != null)
        {
            for (int i = 1; i < childList.Length; i++)
            {
                if (childList[i] != transform)
                    Destroy(childList[i].gameObject); // queue로 보내야함
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
