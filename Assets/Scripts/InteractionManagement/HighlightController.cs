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
    [SerializeField] private GameObject progressBarPrefab;
    private Dictionary<GameObject, GameObject> progressBarInstances = new Dictionary<GameObject, GameObject>();
    private Dictionary<GameObject, Transform> fillBars = new Dictionary<GameObject, Transform>();
    GameObject currentTarget;
    float currentProgress = 0f;
    bool isHighlighting = false;
    Transform fillBar;

    void Start()
    {
        fillBar = progressBarPrefab.transform.GetChild(0);
        progressBarPrefab.SetActive(false);
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

    public void ShowProgressBar(GameObject target, Vector3 position)
    {
        Debug.Log($"Attempting to show progress bar for target: {target.name} at position: {position}");

        if (!progressBarInstances.ContainsKey(target))
        {
            Debug.Log("Progress bar instance not found for target. Creating new instance.");
            GameObject progressBarInstance = Instantiate(progressBarPrefab, position, Quaternion.identity);
            progressBarInstance.SetActive(true);
            Transform fillBar = progressBarInstance.transform.GetChild(0);
            progressBarInstances[target] = progressBarInstance;
            fillBars[target] = fillBar;
        }
        else
        {
            Debug.Log("Progress bar instance already exists for target. Reusing existing instance.");
            progressBarInstances[target].SetActive(true);
            progressBarInstances[target].transform.position = position;
        }

        UpdateProgressBar(target, 0f);
        Debug.Log("Progress bar shown and initialized to 0.");
    }

    public void Hide()
    {
        currentTarget = null;
        highlighter.SetActive(false);
        // Debug.Log(currentTarget);
        // UpdateProgressBar(currentTarget, 0f);
        isHighlighting = false;
        currentProgress = 0f;
    }

    public void UpdateProgressBar(GameObject target, float progress)
    {
        if (fillBars.ContainsKey(target))
        {
            progress = Mathf.Clamp01(progress);
            Transform fillBar = fillBars[target];
            Vector3 scale = fillBar.localScale;
            scale.x = progress;
            fillBar.localScale = scale;

            Vector3 localPosition = fillBar.localPosition;
            localPosition.x = (progress - 1f) * 0.5f;
            fillBar.localPosition = localPosition;
        }
    }

    public void HideProgressBar(GameObject target)
    {
        if (progressBarInstances.ContainsKey(target))
        {
            progressBarInstances[target].SetActive(false);
        }
    }

    public void RemoveProgressBar(GameObject target)
    {
        if (progressBarInstances.ContainsKey(target))
        {
            Destroy(progressBarInstances[target]);
            progressBarInstances.Remove(target);
            fillBars.Remove(target);
        }
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
            UpdateProgressBar(currentTarget, progress);
            progress += 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
    }
}
