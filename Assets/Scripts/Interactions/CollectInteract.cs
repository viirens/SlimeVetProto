using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectInteract : Interactable
{
    [SerializeField] private Item item; // Add this line

    void Start()
    {
        isInteractable = true; // Set interactable to true by default
    }

    override public void Interact(Character character)
    {
        Debug.Log("Collected item");

        // Check if the GameManager's inventoryContainer is not null
        if (GameManager.instance.inventoryContainer != null)
        {
            // Add the item to the inventory
            GameManager.instance.inventoryContainer.Add(item, 1);
        }
        else
        {
            Debug.LogWarning("No inventory container found");
        }
    }
}
