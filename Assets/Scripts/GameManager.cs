using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public List<Transform> entryWayPoints;
    public List<Transform> exitWayPoints;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        entryWayPoints = new List<Transform>(GameObject.Find("SpawnLocations").GetComponentsInChildren<Transform>());
        exitWayPoints = new List<Transform>(GameObject.Find("ExitLocations").GetComponentsInChildren<Transform>());
        Debug.Log("Entry waypoints: " + entryWayPoints.Count);
        Debug.Log("Exit waypoints: " + exitWayPoints.Count);
    }

    public GameObject player;
    public ItemContainer inventoryContainer;
    public ItemDragAndDropController itemDragAndDropController;
    public ItemSpawnManager itemSpawnManager;
}
