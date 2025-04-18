using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private ItemContainer inventory; // Reference to the player's inventory
    [SerializeField] private ToolbarController toolbarController;

    public bool HasItem(Item item)
    {
        return inventory.HasItem(item);
    }

    public void RemoveItem(Item item, int count)
    {
        inventory.RemoveItem(item, count);
    }


    public Item GetSelectedItem()
    {
        return toolbarController.GetItem;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
