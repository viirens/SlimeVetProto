using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawnManager : MonoBehaviour
{
    public static ItemSpawnManager instance;
    [SerializeField] GameObject pickUpItemPrefab;
    private void Awake()
    {
        instance = this;
    }

    
    public void SpawnItem(Vector3 position, Item item, int count)
    {
        GameObject clone = Instantiate(pickUpItemPrefab, position, Quaternion.identity);
        clone.GetComponent<PickUpItem>().Set(item, count);
    }
}
