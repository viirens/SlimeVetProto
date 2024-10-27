using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO.Enumeration;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class HighlightController : MonoBehaviour
{
    [SerializeField] GameObject highlighter;
    [SerializeField] GameObject progressBar; // Add a GameObject for the progress bar
    GameObject currentTarget;
    float currentProgress = 0f;
    bool isHighlighting = false;
    Transform fillBar;

    void Start()
    {
        fillBar = progressBar.transform.GetChild(0);
        progressBar.SetActive(false);
    }

    // void FixedUpdate()
    // {
    //     if (isHighlighting)
    //     {
    //         currentProgress += Time.deltaTime * 0.1f; // Example progress increment
    //         UpdateProgressBar(currentProgress);
    //         if (currentProgress >= 1f)
    //         {
    //             isHighlighting = false;
    //             currentProgress = 0f;
    //         }
    //     }
    // }

    public void Highlight(GameObject target)
    {
        // Get the Interactable component from the target
        Interactable interactable = target.GetComponent<Interactable>();
        if (interactable == null || !interactable.isInteractable)
        {
            Hide();
            return;
        }

        if (currentTarget == target)
        {
            return;
        }
        currentTarget = target;
        Vector3 position = target.transform.position;
        Highlight(position);
        isHighlighting = true; // Start highlighting
    }

    public void Highlight(Vector3 position)
    {
        highlighter.SetActive(true);
        highlighter.transform.position = position;
    }

    public void ShowProgressBar(Vector3 position)
    {
        progressBar.SetActive(true);
        progressBar.transform.position = position;
        UpdateProgressBar(0f);
    }

    public void Hide()
    {
        currentTarget = null;
        highlighter.SetActive(false);
        UpdateProgressBar(0); // Reset progress bar
        isHighlighting = false; // Stop highlighting
        currentProgress = 0f;
    }

    public void UpdateProgressBar(float progress)
    {
        progress = Mathf.Clamp01(progress);
        Vector3 scale = fillBar.localScale;
        scale.x = progress;
        fillBar.localScale = scale;

        Vector3 localPosition = fillBar.localPosition;
        localPosition.x = (progress - 1f) * 0.5f;
        fillBar.localPosition = localPosition;
    }

    public void HideProgressBar()
    {
        Debug.Log("Hiding progress bar");
        progressBar.SetActive(false);
    }

    // function that tests animation of progress bar
    public void TestAnimation()
    {
        StartCoroutine(AnimateProgressBar());
    }

    private IEnumerator AnimateProgressBar()
    {
        float progress = 0f;
        while (progress < 1f)
        {
            UpdateProgressBar(progress);
            progress += 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
    }
}
