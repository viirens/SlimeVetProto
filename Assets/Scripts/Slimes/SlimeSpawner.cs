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

    public void StartWave(int waveIndex)
    {
        if (waveIndex < waves.Count)
        {
            StartCoroutine(SpawnWave(waves[waveIndex]));
        }
    }

    IEnumerator SpawnWave(Wave wave)
    {
        for (int i = 0; i < wave.slimeCount; i++)
        {
            SpawnSlime(wave.itemTier);
            yield return new WaitForSeconds(wave.spawnInterval);
        }
    }

    void SpawnSlime(int itemTier)
    {
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

    public int GetTotalWaves()
    {
        return waves.Count;
    }
}
