using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemy;
    public GameObject[] enemySpawner;
    public Transform[] spawnPos;
    List<Transform> availableSpawns;
    public int enemyCount;

    // Start is called before the first frame update
    void Start()
    {
        enemySpawner = GameObject.FindGameObjectsWithTag("Spawn");

        spawnPos = new Transform[enemySpawner.Length];

        for (int i = 0; i < enemySpawner.Length; i++)
        {
            spawnPos[i] = enemySpawner[i].transform;
        }

        availableSpawns = new List<Transform>(spawnPos);

        if (spawnPos == null)
            return;

        StartCoroutine(SpawnRandom());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnRandom()
    {
        while (enemyCount <= spawnPos.Length && availableSpawns.Count > 0)
        {
            int randomSpawnIndex = Random.Range(0, availableSpawns.Count);
            Transform spawnLocation = availableSpawns[randomSpawnIndex];
            availableSpawns.RemoveAt(randomSpawnIndex);
            Instantiate(enemy, spawnLocation.position, Quaternion.identity);
            yield return new WaitForSeconds(0);
            enemyCount += 1;
            Debug.Log("Spawned");
        }
    }
}
