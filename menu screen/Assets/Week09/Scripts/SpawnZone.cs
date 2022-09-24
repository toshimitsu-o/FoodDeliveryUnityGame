using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnZone : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject enemyZone;
    public GameObject spawnerCheck;

    private float minX;
    private float maxX;
    private float minZ;
    private float maxZ;

    public float length = 25;
    public float width = 25;

    private void Start()
    {
        //Gets the boundaries from the center of the spawn point
        minX = transform.position.x - (length / 2);
        maxX = transform.position.x + (length / 2);
        minZ = transform.position.z - (length / 2);
        maxZ = transform.position.z + (length / 2);

        //finds a random point within the spawnbox
        Vector3 randPoint = new Vector3(Random.Range(minX, maxX), 0, Random.Range(minZ, maxZ));
        
        //Spawn the player here
        Instantiate(playerPrefab, randPoint, Quaternion.identity);

        //small wait time to allow loading of any existing enemyspawners
        Invoke("EnableEnemySpawner", 1.0f);
    }

    private void EnableEnemySpawner()
    {
        //Using this to make sure any joining players dont start spawning extra enemies
        if (!GameObject.FindGameObjectWithTag("EnemySpawnZone"))
        {
            //This gameobject is tagged with EnemySpawnZone preventing this from running more than once per game
            Instantiate(spawnerCheck, transform.position, Quaternion.identity);
            enemyZone.GetComponent<EnemySpawner>().enabled = true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        //offsetting the y by 1 to get the gizmo out of the ground
        Gizmos.DrawWireCube(new Vector3(transform.position.x, transform.position.y + 1, transform.position.z)
            , new Vector3(length, 2, width));
    }
}
