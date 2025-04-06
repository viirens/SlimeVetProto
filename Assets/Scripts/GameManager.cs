using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public List<Transform> entryWayPoints;
    public List<Transform> exitWayPoints;
    public TextMeshProUGUI slimesHealedText;
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI waveTimerText;
    public TextMeshProUGUI gameOverText;
    private int totalSlimesHealed = 0;
    private int slimesHealedInWave = 0;
    private bool isGameOver = false;

    [Header("Game Settings")]
    public float waveTimeLimit = 60f;
    public int slimesToHealPerWave = 3;
    private int currentWaveIndex = 0;

    public GameObject player;
    public ItemContainer inventoryContainer;
    public ItemDragAndDropController itemDragAndDropController;
    public ItemSpawnManager itemSpawnManager;

    private Coroutine waveTimerCoroutine;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        entryWayPoints = new List<Transform>(GameObject.Find("SpawnLocations").GetComponentsInChildren<Transform>());
        entryWayPoints.Remove(GameObject.Find("SpawnLocations").transform);

        exitWayPoints = new List<Transform>(GameObject.Find("ExitLocations").GetComponentsInChildren<Transform>());
        exitWayPoints.Remove(GameObject.Find("ExitLocations").transform);

        UpdateWaveText();
        UpdateSlimesHealedUI();
        gameOverText.gameObject.SetActive(false);
        StartWave();
    }

    public void IncrementSlimesHealed()
    {
        totalSlimesHealed++;
        slimesHealedInWave++;
        UpdateSlimesHealedUI();
        CheckWaveCompletion();
    }

    void UpdateSlimesHealedUI()
    {
        slimesHealedText.text = "Slimes Healed: " + slimesHealedInWave + " / " + slimesToHealPerWave;
    }

    void UpdateWaveText()
    {
        waveText.text = "Wave: " + (currentWaveIndex + 1);
    }

    void StartWave()
    {
        slimesHealedInWave = 0;
        UpdateWaveText();
        UpdateSlimesHealedUI();
        SlimeSpawner spawner = FindObjectOfType<SlimeSpawner>();
        if (spawner != null)
        {
            spawner.StartWave(currentWaveIndex);
        }
        waveTimerCoroutine = StartCoroutine(WaveTimer());
    }

    IEnumerator WaveTimer()
    {
        float timer = waveTimeLimit;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            UpdateWaveTimerUI(timer);
            yield return null;
        }
        if (!isGameOver)
        {
            DestroyRemainingSlimes();
            GameOver(false);
        }
    }

    void UpdateWaveTimerUI(float timer)
    {
        int minutes = Mathf.FloorToInt(timer / 60f);
        int seconds = Mathf.FloorToInt(timer % 60f);
        waveTimerText.text = string.Format("Time Left In Wave: {0:00}:{1:00}", minutes, seconds);
    }

    void CheckWaveCompletion()
    {
        if (slimesHealedInWave >= slimesToHealPerWave && !isGameOver)
        {
            if (waveTimerCoroutine != null)
            {
                StopCoroutine(waveTimerCoroutine);
                waveTimerCoroutine = null;
            }

            DestroyRemainingSlimes();

            currentWaveIndex++;
            if (currentWaveIndex >= GetTotalWaves())
            {
                GameOver(true);
            }
            else
            {
                StartWave();
            }
        }
    }

    int GetTotalWaves()
    {
        SlimeSpawner spawner = FindObjectOfType<SlimeSpawner>();
        if (spawner != null)
        {
            return spawner.GetTotalWaves();
        }
        return 0;
    }

    void GameOver(bool hasWon)
    {
        isGameOver = true;
        if (hasWon)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    void DestroyRemainingSlimes()
    {
        Slime[] remainingSlimes = FindObjectsOfType<Slime>();
        foreach (Slime slime in remainingSlimes)
        {
            Destroy(slime.gameObject);
        }
    }
}
