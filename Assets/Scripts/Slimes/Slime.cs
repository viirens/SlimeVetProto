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

        RandomizeColor();

        highlightController = FindObjectOfType<HighlightController>();

        ChooseRandomItem();
    }

    void Update()
    {
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
        }
    }

    public override void InteractHold(Character character)
    {
        if (isInteractable && currentState == SlimeState.Injured && character.HasItem(requiredItem))
        {
            if (!isBeingHealed)
            {
                isBeingHealed = true;
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
    }

    public void CompleteHealing()
    {
        isBeingHealed = false;
        SetState(SlimeState.Healthy);

        SlimeMovement slimeMovement = GetComponent<SlimeMovement>();
        if (slimeMovement != null)
        {
            slimeMovement.MoveToExit();
        }

        itemSprite.SetActive(false);
        highlightController.RemoveProgressBar(gameObject);

        GameManager.instance.IncrementSlimesHealed();
    }

    private void ChooseRandomItem()
    {
        if (itemRequirement != null && itemRequirement.possibleItems.Count > 0)
        {
            List<Item> tieredItems = itemRequirement.GetItemsByTier(itemTier);
            Debug.Log($"[Slime] itemTier: {itemTier}, tieredItems count: {tieredItems.Count}");
            foreach (Item item in tieredItems)
            {
                Debug.Log($"[Slime] Tiered item: {item.Name}, tiers: {string.Join(",", item.tiers)}");
            }

            if (tieredItems.Count == 0)
            {
                Debug.LogWarning($"[Slime] No items found for tier {itemTier}, using all possible items.");
                tieredItems = itemRequirement.possibleItems;
            }

            int randomIndex = Random.Range(0, tieredItems.Count);
            requiredItem = tieredItems[randomIndex];
            Debug.Log($"[Slime] Selected required item: {requiredItem.Name}, tiers: {string.Join(",", requiredItem.tiers)}");

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
