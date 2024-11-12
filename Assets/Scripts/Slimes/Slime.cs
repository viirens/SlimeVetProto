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

    private Character interactingCharacter;

    private int itemTier = 1;

    void Start()
    {
        requiresOverlap = customRequiresOverlap;
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateSprite();
        itemSprite.SetActive(false); 

        timer = timeToLive;
        isTimerActive = true;

        RandomizeColor();

        highlightController = FindObjectOfType<HighlightController>();

        ChooseRandomItem();
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

        if (isBeingHealed && !IsCharacterInRange())
        {
            CancelHealing();
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
            if (!isBeingHealed)
            {
                isBeingHealed = true;
                isTimerActive = false;
                interactingCharacter = character;
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
            CancelHealing();
        }
    }

    public override void InteractReleased(Character character)
    {
        CancelHealing();
    }

    private void StartHealing()
    {
        isBeingHealed = true;
        healingProgress = 0f;
        isTimerActive = false;
        Debug.Log("Healing process started");
    }

    public void CompleteHealing()
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

        // Increment the slimes healed count
        GameManager.instance.IncrementSlimesHealed();
    }

    private void ChooseRandomItem()
    {
        if (itemRequirement != null && itemRequirement.possibleItems.Count > 0)
        {
            Debug.Log("Choosing random item for slime with tier: " + itemTier);
            List<Item> tieredItems = itemRequirement.GetItemsByTier(itemTier);
            Debug.Log("Tiered items count: " + tieredItems.Count);
            if (tieredItems.Count == 0)
                tieredItems = itemRequirement.possibleItems;

            int randomIndex = Random.Range(0, tieredItems.Count);
            requiredItem = tieredItems[randomIndex];

            SpriteRenderer itemSpriteRenderer = itemSprite.GetComponent<SpriteRenderer>();
            if (itemSpriteRenderer != null)
            {
                itemSpriteRenderer.sprite = requiredItem.Icon;
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

    private void CancelHealing()
    {
        if (isBeingHealed)
        {
            isBeingHealed = false;
            healingProgress = 0f;
            isTimerActive = true;
            interactingCharacter = null;
            highlightController.RemoveProgressBar(gameObject);
        }
    }

    private bool IsCharacterInRange()
    {
        if (interactingCharacter == null)
            return false;

        CharacterInteractController interactController = interactingCharacter.GetComponent<CharacterInteractController>();
        if (interactController == null)
            return false;

        return interactController.CurrentInteractable == this;
    }

    public void SetItemTier(int tier)
    {
        itemTier = tier;
    }
}
