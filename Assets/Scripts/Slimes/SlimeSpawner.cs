using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeSpawner : MonoBehaviour
{
    [Header("Slime Spawn Settings")]
    [SerializeField] private GameObject slimePrefab;

    [System.Serializable]
    public class Wave
    {
        public int slimeCount;
        public float spawnInterval;
        public int itemTier;
    }

    [SerializeField] private List<Wave> waves;
    private int currentWaveIndex = 0;

    void Start()
    {
        StartCoroutine(SpawnWaves());
    }

    IEnumerator SpawnWaves()
    {
        while (currentWaveIndex < waves.Count)
        {
            Wave currentWave = waves[currentWaveIndex];
            for (int i = 0; i < currentWave.slimeCount; i++)
            {
                SpawnSlime(currentWave.itemTier);
                yield return new WaitForSeconds(currentWave.spawnInterval);
            }
            currentWaveIndex++;
            yield return new WaitForSeconds(5f);
        }
    }

    void SpawnSlime(int itemTier)
    {
        // Debug.Log("Spawning slime with tier: " + itemTier);
        if (GameManager.instance.entryWayPoints.Count == 0 || slimePrefab == null)
            return;

        int spawnIndex = Random.Range(0, GameManager.instance.entryWayPoints.Count);
        Transform spawnPoint = GameManager.instance.entryWayPoints[spawnIndex];

        GameObject slimeObject = Instantiate(slimePrefab, spawnPoint.position, Quaternion.identity);
        Slime slime = slimeObject.GetComponent<Slime>();
        if (slime != null)
        {
            slime.SetItemTier(itemTier);
        }
    }
}
