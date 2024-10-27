using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkInteract : Interactable
{
        void Start()
    {
        isInteractable = true; // Set interactable to true by default
    }
    override public void Interact(Character character)
    {
        Debug.Log("Talked to NPC");
    }
}
