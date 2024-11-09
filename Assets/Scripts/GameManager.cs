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
        entryWayPoints.Remove(GameObject.Find("SpawnLocations").transform);

        exitWayPoints = new List<Transform>(GameObject.Find("ExitLocations").GetComponentsInChildren<Transform>());
        exitWayPoints.Remove(GameObject.Find("ExitLocations").transform);

        Debug.Log("Entry waypoints: " + entryWayPoints.Count);
        Debug.Log("Exit waypoints: " + exitWayPoints.Count);
    }

    public GameObject player;
    public ItemContainer inventoryContainer;
    public ItemDragAndDropController itemDragAndDropController;
    public ItemSpawnManager itemSpawnManager;
}
