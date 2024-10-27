using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PopupManager : MonoBehaviour
{
    public GameObject popupPrefab; // Assign a popup prefab in the editor
    private GameObject popupInstance;
    private Slime currentSlime;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
        }
    }

    public void OnPopupButtonClicked()
    {
        Debug.Log("Popup button clicked");
        if (popupInstance != null && currentSlime != null)
        {
            Destroy(popupInstance);
            currentSlime.SetState(Slime.SlimeState.Healthy);
            Debug.Log(currentSlime.GetComponent<SlimeMovement>());
            // Debug.Log(currentSlime.GetComponent<SlimeMovement>().MoveToExit);
            currentSlime.GetComponent<SlimeMovement>().MoveToExit();
        }
    }

}
