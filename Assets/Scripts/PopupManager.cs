using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PopupManager : MonoBehaviour
{
    public GameObject popupPrefab; // Assign a popup prefab in the editor
    public GameObject progressBarPrefab;
    private GameObject popupInstance;
    private Slime currentSlime;
    private GameObject fillSpriteInstance;
    private float interactionDuration = 5f; // Duration in seconds for the interaction
    private float interactionTimer = 0f;
    private bool isInteracting = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isInteracting)
        {
            interactionTimer += Time.deltaTime;
            if (fillSpriteInstance != null)
            {
                RectTransform fillRect = fillSpriteInstance.GetComponent<RectTransform>();
                if (fillRect != null)
                {
                    float fillAmount = interactionTimer / interactionDuration;
                    fillRect.localScale = new Vector3(fillAmount, 1, 1);
                }
            }

            if (interactionTimer >= interactionDuration)
            {
                CompleteInteraction();
            }
        }
    }

    public void ShowPopup(Slime slime)
    {
        if (popupInstance == null && popupPrefab != null)
        {
            currentSlime = slime;
            
            // Convert the slime's world position to a screen position
            Vector3 slimeScreenPosition = Camera.main.WorldToScreenPoint(slime.transform.position + Vector3.up * 2);
            
            popupInstance = Instantiate(popupPrefab, slimeScreenPosition, Quaternion.identity);
            popupInstance.transform.SetParent(GameObject.Find("Canvas").transform, false); // Ensure it's part of the UI

            // Set the popup's position in the UI
            RectTransform popupRectTransform = popupInstance.GetComponent<RectTransform>();
            if (popupRectTransform != null)
            {
                popupRectTransform.anchoredPosition = slimeScreenPosition;
            }

            // Find the Button component in the popup and assign the OnClick event
            Button popupButton = popupInstance.GetComponentInChildren<Button>();
            if (popupButton != null)
            {
                popupButton.onClick.AddListener(OnPopupButtonClicked);
            }

            // Instantiate the progress bar and find the fill sprite
            GameObject progressBarInstance = Instantiate(progressBarPrefab, popupInstance.transform);
            fillSpriteInstance = progressBarInstance.transform.Find("Fill").gameObject;
            RectTransform fillRect = fillSpriteInstance.GetComponent<RectTransform>();
            if (fillRect != null)
            {
                fillRect.localScale = new Vector3(0, 1, 1);
            }

            isInteracting = true;
            interactionTimer = 0f;
        }
    }

    private void CompleteInteraction()
    {
        isInteracting = false;
        if (popupInstance != null && currentSlime != null)
        {
            Destroy(popupInstance);
            currentSlime.SetState(Slime.SlimeState.Healthy);
            currentSlime.GetComponent<SlimeMovement>().MoveToExit();
        }
    }

    public void OnPopupButtonClicked()
    {
        Debug.Log("Popup button clicked");
    }
}
