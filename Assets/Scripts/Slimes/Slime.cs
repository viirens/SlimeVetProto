using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Interactable
{
    [SerializeField] private bool customRequiresOverlap = false;
    public enum SlimeState
    {
        Healthy,
        Injured
    }

    [SerializeField]
    private SlimeState currentState = SlimeState.Injured; 
    public Sprite healthySprite;
    public Sprite injuredSprite;
    private SpriteRenderer spriteRenderer;

    [SerializeField] private GameObject itemSprite; 
    [SerializeField] private SlimeItemRequirement itemRequirement;
    private Item requiredItem;

    [Header("Slime Timer Settings")]
    [SerializeField] private float timeToLive = 30f; // Time in seconds before the slime dies
    private float timer;
    private bool isTimerActive = false;

    void Start()
    {
        requiresOverlap = customRequiresOverlap;
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateSprite();
        itemSprite.SetActive(false); 

        ChooseRandomItem();

        timer = timeToLive;
        isTimerActive = true;

        // Randomize the color of the slime
        RandomizeColor();
    }

    void Update()
    {
        if (isTimerActive)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                SlimeDies();
            }
        }
    }

    public override void Interact(Character character)
    {
        if (isInteractable && currentState == SlimeState.Injured)
        {
            if (character.HasItem(requiredItem))
            {
                character.RemoveItem(requiredItem, 1); // Remove the item from the inventory
                SetState(SlimeState.Healthy);
                Debug.Log("Slime healed");

                SlimeMovement slimeMovement = GetComponent<SlimeMovement>();
                if (slimeMovement != null)
                {
                    slimeMovement.MoveToExit();
                }

                itemSprite.SetActive(false);
                isTimerActive = false; // Stop the timer when healed
            }
            else
            {
                Debug.Log("Character does not have the required item");
            }
        }
    }

    private void ChooseRandomItem()
    {
        if (itemRequirement != null && itemRequirement.possibleItems.Count > 0)
        {
            int randomIndex = Random.Range(0, itemRequirement.possibleItems.Count);
            requiredItem = itemRequirement.possibleItems[randomIndex];
            Debug.Log("Slime requires: " + requiredItem.Name);

            // Set the sprite of the itemSprite GameObject to the icon of the required item
            SpriteRenderer itemSpriteRenderer = itemSprite.GetComponent<SpriteRenderer>();
            if (itemSpriteRenderer != null)
            {
                itemSpriteRenderer.sprite = requiredItem.Icon;
            }
            else
            {
                Debug.LogError("Item sprite GameObject does not have a SpriteRenderer component.");
            }
        }
    }

    private void RandomizeColor()
    {
        // Generate a random hue value
        float hue = Random.Range(0f, 1f);
        // Set saturation and value to ensure bright and vibrant colors
        float saturation = Random.Range(0.5f, 1f);
        float value = Random.Range(0.7f, 1f);

        // Convert HSV to RGB and apply to the sprite renderer
        Color randomColor = Color.HSVToRGB(hue, saturation, value);
        spriteRenderer.color = randomColor;
    }

    public void SetInteractable(bool value)
    {
        isInteractable = value;
    }

    public void SetState(SlimeState newState)
    {
        currentState = newState;
        UpdateSprite();
    }

    private void UpdateSprite()
    {
        switch (currentState)
        {
            case SlimeState.Healthy:
                spriteRenderer.sprite = healthySprite;
                break;
            case SlimeState.Injured:
                spriteRenderer.sprite = injuredSprite;
                break;
        }
    }

    public SlimeState GetCurrentState()
    {
        return currentState;
    }

    public void ShowItemSprite() 
    {
        itemSprite.SetActive(true);
    }

    private void SlimeDies()
    {
        Debug.Log("Slime died due to timeout.");

        // Add death animation or effects here if desired

        Destroy(gameObject); // Remove the slime from the game
    }
}
