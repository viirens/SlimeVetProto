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

    private bool isBeingHealed = false;
    private float healingProgress = 0f;
    [SerializeField] private float healingDuration = 5f; 

    private HighlightController highlightController;

    void Start()
    {
        requiresOverlap = customRequiresOverlap;
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateSprite();
        itemSprite.SetActive(false); 

        ChooseRandomItem();

        timer = timeToLive;
        isTimerActive = true;

        RandomizeColor();
        // Debug.Log("Slime initialized with state: " + currentState);

        highlightController = FindObjectOfType<HighlightController>();
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
        if (isInteractable && currentState == SlimeState.Injured && character.HasItem(requiredItem))
        {
            character.RemoveItem(requiredItem, 1);
            StartHealing();
            // Debug.Log("Slime healing started by character: " + character.name);
        }
        else
        {
            Debug.Log("Interaction failed: Character does not have the required item or slime is not interactable");
        }
    }

    public override void InteractHold(Character character)
    {
        if (isInteractable && currentState == SlimeState.Injured && character.HasItem(requiredItem))
        {
            Debug.Log("InteractHold called");
            if (!isBeingHealed)
            {
                // Debug.Log("InteractHold called and isBeingHealed is false");
                
                isBeingHealed = true;
                isTimerActive = false;
                highlightController.ShowProgressBar(gameObject, transform.position);
            }

            healingProgress += Time.deltaTime;
            highlightController.UpdateProgressBar(gameObject, healingProgress / healingDuration);

            if (healingProgress >= healingDuration)
            {
                character.RemoveItem(requiredItem, 1);
                CompleteHealing();
            }
        }
        else
        {
            Debug.Log("InteractHold failed: Slime not interactable or character lacks required item.");
        }
    }

    public override void InteractReleased(Character character)
    {
        if (isBeingHealed)
        {
            isBeingHealed = false;
            healingProgress = 0f;
            isTimerActive = true;
            highlightController.HideProgressBar(gameObject);
        }
    }

    private void StartHealing()
    {
        isBeingHealed = true;
        healingProgress = 0f;
        isTimerActive = false;
        Debug.Log("Healing process started");
    }

    private void CompleteHealing()
    {
        isBeingHealed = false;
        SetState(SlimeState.Healthy);
        // Debug.Log("Healing complete: Slime state set to Healthy.");

        SlimeMovement slimeMovement = GetComponent<SlimeMovement>();
        if (slimeMovement != null)
        {
            slimeMovement.MoveToExit();
            // Debug.Log("Slime is moving to exit.");
        }

        itemSprite.SetActive(false);
        highlightController.RemoveProgressBar(gameObject);
    }

    private void ChooseRandomItem()
    {
        if (itemRequirement != null && itemRequirement.possibleItems.Count > 0)
        {
            int randomIndex = Random.Range(0, itemRequirement.possibleItems.Count);
            requiredItem = itemRequirement.possibleItems[randomIndex];
            // Debug.Log("Slime requires item: " + requiredItem.Name);

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
        float hue = Random.Range(0f, 1f);
        float saturation = Random.Range(0.5f, 1f);
        float value = Random.Range(0.7f, 1f);

        Color randomColor = Color.HSVToRGB(hue, saturation, value);
        spriteRenderer.color = randomColor;
        // Debug.Log("Slime color randomized to: " + randomColor);
    }

    public void SetInteractable(bool value)
    {
        isInteractable = value;
        // Debug.Log("Slime interactable set to: " + value);
    }

    public void SetState(SlimeState newState)
    {
        currentState = newState;
        UpdateSprite();
        // Debug.Log("Slime state updated to: " + currentState);
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
        // Debug.Log("Slime sprite updated to match state: " + currentState);
    }

    public SlimeState GetCurrentState()
    {
        return currentState;
    }

    public void ShowItemSprite() 
    {
        itemSprite.SetActive(true);
        // Debug.Log("Item sprite shown");
    }

    private void SlimeDies()
    {
        Debug.Log("Slime died due to timeout.");
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        highlightController.RemoveProgressBar(gameObject);
    }
}
