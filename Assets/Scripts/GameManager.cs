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
    public TextMeshProUGUI gameOverText;
    private int slimesHealed = 0;
    private bool isGameOver = false;

    [Header("Game Settings")]
    [SerializeField] public float timeLimit = 180f; // Time limit in seconds
    [SerializeField] private int slimesToHeal = 5;    // Number of slimes to heal to win
    


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

        Debug.Log("Entry waypoints: " + entryWayPoints.Count);
        Debug.Log("Exit waypoints: " + exitWayPoints.Count);

        UpdateSlimesHealedUI();
        gameOverText.gameObject.SetActive(false);
    }

    public GameObject player;
    public ItemContainer inventoryContainer;
    public ItemDragAndDropController itemDragAndDropController;
    public ItemSpawnManager itemSpawnManager;

    public void IncrementSlimesHealed()
    {
        slimesHealed++;
        UpdateSlimesHealedUI();
        CheckWinCondition();
    }

    void UpdateSlimesHealedUI()
    {
        slimesHealedText.text = "Slimes Healed: " + slimesHealed + " / " + slimesToHeal;
    }

    public void TimeUp()
    {
        if (!isGameOver)
        {
            isGameOver = true;
            GameOver(false);
        }
    }

    void CheckWinCondition()
    {
        if (slimesHealed >= slimesToHeal && !isGameOver)
        {
            isGameOver = true;
            GameOver(true);
        }
    }

    void GameOver(bool hasWon)
    {
        // if (hasWon)
        // {
        //     gameOverText.text = "You Win!";
        // }
        // else
        // {
        //     gameOverText.text = "Game Over";
        // }
        // gameOverText.gameObject.SetActive(true);

        // Optionally, stop spawning slimes
        // SlimeSpawner spawner = FindObjectOfType<SlimeSpawner>();
        // if (spawner != null)
        // {
        //     spawner.StopSpawning();
        // }

        // Disable player controls if necessary
        // player.GetComponent<PlayerController>().enabled = false;

        if (hasWon)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
