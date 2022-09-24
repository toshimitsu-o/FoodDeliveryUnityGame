using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public float length = 30;
    public float width = 30;

    public GameObject enemyTank;

    public int playerCount = 1;
    public int maxEnemiesPerPlayer = 10;
    public int currEnemies = 0;
    private int maxEnemies = 10;



    private float spawnWaitedTime = 0;
    public float spawnTimer = 3;

    private float checkWaitedTime = 0;
    private float checkTimer = 2;

    private float minX;
    private float maxX;
    private float minZ;
    private float maxZ;

    private void Start()
    {
        minX = transform.position.x - (length / 2);
        maxX = transform.position.x + (length / 2);
        minZ = transform.position.z - (length / 2);
        maxZ = transform.position.z + (length / 2);
    }

    private void Update()
    {
        spawnWaitedTime += Time.deltaTime;
        if (spawnWaitedTime > spawnTimer && currEnemies < maxEnemies)
        {
            Vector3 randPoint = new Vector3(Random.Range(minX, maxX), 0, Random.Range(minZ, maxZ));
            GameObject newEnemy = Instantiate(enemyTank, randPoint, Quaternion.identity);
            newEnemy.GetComponent<SimpleFSM>().enabled = true;
            spawnWaitedTime = 0;
        }

        CheckForCurPlayers();
    }

    private void CheckForCurPlayers()
    {
        checkWaitedTime += Time.deltaTime;
        if(checkWaitedTime > checkTimer)
        {
            playerCount = GameObject.FindGameObjectsWithTag("Player").Length;
            maxEnemies = maxEnemiesPerPlayer * playerCount;
            checkWaitedTime = 0;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(new Vector3(transform.position.x, transform.position.y + 1, transform.position.z)
            , new Vector3(length, 2, width));
    }
}
