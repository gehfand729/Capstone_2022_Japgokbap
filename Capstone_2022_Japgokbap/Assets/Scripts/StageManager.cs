using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Text;

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

    public Dictionary<Byte, GameObject> m_pool = new Dictionary<byte, GameObject>();
    public Queue<GameObject> m_queue = new Queue<GameObject>();

    [Header ("Settings")]
    public int waitingTime;
    public int spawnerCount;
    public bool roundEnded;
    public int stageCount;
    public int roundCount;

    [Header ("Objects")]
    public GameObject expClones;
    public GameObject expObject;

    [Header ("1-1")]
    public GameObject monsters1;
    public int count1;
    public int count2;

    [Header ("1-2")]
    public GameObject monsters2;
    public int count3;
    public int count4;

    [Header ("1-3")]
    public GameObject monsters3;
    public int count5;
    public int count6;

    [Header ("2-1")]
    public GameObject monsters4;
    public int count7;
    public int count8;

    [Header ("2-2")]
    public GameObject monsters5;
    public int count9;
    public int count10;

    [Header ("2-3")]
    public GameObject monsters6;
    public int count11;
    public int count12;

    [Header ("2-4")]
    public GameObject monsters7;
    public int count13;
    public int count14;

    [Header ("3-1")]
    public GameObject monsters8;
    public int count15;
    public int count16;

    [Header ("3-2")]
    public GameObject monsters9;
    public int count17;
    public int count18;

    [Header ("3-3")]
    public GameObject monsters10;
    public int count19;
    public int count20;

    [Header ("3-4")]
    public GameObject monsters11;
    public int count21;
    public int count22;

    [Header ("3-5")]
    public GameObject monsters12;
    public int count23;
    public int count24;

    #endregion

    #region "Private"

    private static StageManager m_instance;

    [Header ("Spawners")]
    [SerializeField] private GameObject monsterPool;
    [SerializeField] private GameObject[] enemySpawner;

    [Header ("Monsters")]
    [SerializeField] private GameObject[] skeletons;
    [SerializeField] private GameObject[] goblins;
    [SerializeField] private GameObject[] orc;
    [SerializeField] private GameObject[] golems;
    [SerializeField] private GameObject[] demon;
    [SerializeField] private GameObject[] specialMonsters;

    #endregion

    private void Awake()
    {
        // 파일에 기록된 수만큼 몬스터 미리 생성해야함
        EarlySpawnMonster(skeletons[0], count1);
        EarlySpawnMonster(skeletons[1], count2);
        EarlySpawnMonster(skeletons[2], count3);
        EarlySpawnMonster(skeletons[3], count4);

        EarlySpawnMonster(goblins[0], count5);
        EarlySpawnMonster(goblins[1], count6);
        EarlySpawnMonster(goblins[2], count7);
        EarlySpawnMonster(goblins[3], count8);
        EarlySpawnMonster(orc[0], count9);

        EarlySpawnMonster(golems[0], count11);
        EarlySpawnMonster(golems[1], count12);
        EarlySpawnMonster(golems[2], count13);
        EarlySpawnMonster(golems[3], count14);
        EarlySpawnMonster(golems[4], count15);

        EarlySpawnMonster(demon[0], count16);
    }

    private void Start() 
    {
        StartCoroutine(SpawnMonsters());
    }

    #region "Public Methods"

    public void EarlySpawnMonster(GameObject monster, int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject t_monster = Instantiate(monster, monsterPool.transform.position, Quaternion.identity);
            InsertMonsterInQueue(t_monster);
        }
    }

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

    public void InsertMonsterInQueue(GameObject monster)
    {
        m_queue.Enqueue(monster);
        monster.transform.parent = monsterPool.transform;
        monster.SetActive(false);
    }

    public GameObject GetMonsterInQueue()
    {
        GameObject monster = m_queue.Dequeue();

        return monster;
    }

    #endregion

    #region "Private Methods"
    
    private IEnumerator SpawnMonsters()
    {
        while (!roundEnded)
        {
            SpawnMonster();
            // switch (stageCount)
            // {
            //     case 1 : 
            //         Spawn1Stage();
            //         break;
            //     case 2 :
            //         Spawn2Stage();
            //         break;
            //     case 3 :
            //         Spawn3Stage();
            //         break;
            // }
            yield return new WaitForSeconds(waitingTime);
        }
    }

    private IEnumerator Spawn1Stage()
    {
        if (roundCount == 1)
        {
            SpawnMonster();

            yield return new WaitForSeconds(waitingTime);
        }
        else if (roundCount == 2)
        {
            SpawnMonster();

            yield return new WaitForSeconds(waitingTime);
        }
        else if (roundCount == 3)
        {
            SpawnMonster();

            yield return new WaitForSeconds(waitingTime);
        }

        stageCount++;
        roundCount = 0;
    }

    private IEnumerator Spawn2Stage()
    {
        if (roundCount == 1)
        {
            SpawnMonster();

            yield return new WaitForSeconds(waitingTime);
        }
        else if (roundCount == 2)
        {
            SpawnMonster();

            yield return new WaitForSeconds(waitingTime);
        }
        else if (roundCount == 3)
        {
            SpawnMonster();

            yield return new WaitForSeconds(waitingTime);
        }
        else if (roundCount == 4)
        {
            SpawnMonster();

            yield return new WaitForSeconds(waitingTime);
        }

        stageCount++;
        roundCount = 0;
    }

    private IEnumerator Spawn3Stage()
    {
        if (roundCount == 1)
        {
            SpawnMonster();

            yield return new WaitForSeconds(waitingTime);
        }
        else if (roundCount == 2)
        {
            SpawnMonster();

            yield return new WaitForSeconds(waitingTime);
        }
        else if (roundCount == 3)
        {
            SpawnMonster();

            yield return new WaitForSeconds(waitingTime);
        }
        else if (roundCount == 4)
        {
            SpawnMonster();

            yield return new WaitForSeconds(waitingTime);
        }
        else if (roundCount == 5)
        {
            SpawnMonster();

            yield return new WaitForSeconds(waitingTime);
        }

        roundEnded = true;
    }

    private void SpawnMonster()
    {
        // if (spawnerCount > 10)
        // {
        //     roundCount++;

        //     return;
        // }

        GameObject monster = GetMonsterInQueue();
        monster.transform.position = enemySpawner[spawnerCount++ % 3].transform.position;
        monster.SetActive(true);
        monster.transform.parent = monsters1.transform;
    }

    #endregion

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
