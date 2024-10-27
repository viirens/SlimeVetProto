using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootContainerInteract : Interactable
{
    [SerializeField] GameObject closedChest;
    [SerializeField] GameObject openedChest;
    [SerializeField] bool opened;

    void Start()
    {
        isInteractable = true; // Set interactable to true by default
    }

    public override void Interact(Character character)
    {
        if (!opened)
        {
            opened = true;
            closedChest.SetActive(false);
            openedChest.SetActive(true);
        }
    }
}
