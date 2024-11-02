using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkStationInteract : Interactable
{
    [SerializeField] private WorkStationType workStationType;

    void Start()
    {
        isInteractable = true;
    }

    public override void Interact(Character character)
    {
        switch(workStationType)
        {
            case WorkStationType.Fireplace:
                InteractWithFireplace(character);
                break;
            case WorkStationType.Stovetop:
                InteractWithStovetop(character);
                break;
            default:
                Debug.Log("Interacted with unknown workstation");
                break;
        }
    }

    private void InteractWithFireplace(Character character)
    {
        Debug.Log("Using the fireplace");
    }

    private void InteractWithStovetop(Character character)
    {
        Debug.Log("Using the stovetop");
    }
}

public enum WorkStationType
{
    Fireplace,
    Stovetop
}

