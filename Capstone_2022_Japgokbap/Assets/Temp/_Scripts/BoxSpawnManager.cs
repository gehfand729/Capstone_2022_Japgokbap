using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxSpawnManager : MonoBehaviour
{
    #region "Private"
    private int needCount;
    [SerializeField] private float randomRange;
    [SerializeField] private GameObject box;
    private Vector3 playerPos;
    private Transform playerTsf;
    
    #endregion

    #region "Static"
    static public int killedEnemyCount;
    
    #endregion

    private void Awake(){
        playerTsf = GameObject.FindWithTag("Player").GetComponent<Transform>();

        needCount = 10;

        randomRange = 25.0f;
    }

    private void Update()
    {
        BoxSpawn();
    }

    private void BoxSpawn()
    {
        if(killedEnemyCount >= needCount)
        {
            killedEnemyCount = 0;
            playerPos = playerTsf.position;
            float randomPosX = Random.Range(playerPos.x - randomRange, playerPos.x + randomRange);
            float randomPosZ = Random.Range(playerPos.z - randomRange, playerPos.z + randomRange);

            Vector3 spawnPos = new Vector3(randomPosX, playerPos.y + 1.5f, randomPosZ);

            Instantiate(box, spawnPos, Quaternion.identity);
        }
    }
}
