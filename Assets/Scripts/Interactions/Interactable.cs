using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public bool isInteractable { get; set; } = true;

    [Tooltip("Does interaction require collider overlap?")]
    public bool requiresOverlap { get; set; } = true;

    [Tooltip("Does the player need to face the object to interact?")]
    public bool requiresFacing { get; set; } = true;

    public virtual void Interact(Character character)
    {
        Debug.Log("Interact with " + gameObject.name);
    }
}
