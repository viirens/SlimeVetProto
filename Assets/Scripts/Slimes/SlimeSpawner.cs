using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeSpawner : MonoBehaviour
{
    [Header("Slime Spawn Settings")]
    [SerializeField] private GameObject slimePrefab;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private float minSpawnInterval = 5f;
    [SerializeField] private float maxSpawnInterval = 10f;

    private bool spawning = true;

    void Start()
    {
        Debug.Log("SlimeSpawner started.");
        StartCoroutine(SpawnSlimes());
    }

    IEnumerator SpawnSlimes()
    {
        while (spawning)
        {
            float spawnDelay = Random.Range(minSpawnInterval, maxSpawnInterval);
            Debug.Log($"Next slime will spawn in {spawnDelay} seconds.");
            yield return new WaitForSeconds(spawnDelay);

            SpawnSlime();
        }
    }

    void SpawnSlime()
    {
        if (spawnPoints.Length == 0 || slimePrefab == null)
        {
            Debug.LogError("Spawn points or slime prefab not set up correctly.");
            return;
        }

        int spawnIndex = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[spawnIndex];

        Instantiate(slimePrefab, spawnPoint.position, Quaternion.identity);
        Debug.Log($"Spawned a slime at {spawnPoint.position} (Spawn Point Index: {spawnIndex}).");
    }
}
