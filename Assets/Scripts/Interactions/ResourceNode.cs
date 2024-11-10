using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(BoxCollider2D))]
public class ResourceNode : ToolHit
{
    [SerializeField] GameObject pickUpDrop;
    [SerializeField] int dropCount = 5;
    [SerializeField] int itemCountInOneDrop = 1;
    [SerializeField] float spread = 0.7f;
    [SerializeField] Item item;
    [SerializeField] ResourceNodeType nodeType;

    public override void Hit()
    {
        Debug.Log("Hitting resource node");
        while (dropCount > 0)
        {
            dropCount--;

            Vector3 position = transform.position;
            position.x += spread * UnityEngine.Random.value - spread / 2; 
            position.y += spread * UnityEngine.Random.value - spread / 2;

            GameManager.instance.itemSpawnManager.SpawnItem(position, item, itemCountInOneDrop);
        }

        Destroy(gameObject);
    }

    // public override void HitHold()
    // {
    //     StartCoroutine(HitCoroutine());
    //     isBeingHit = true;
    //     highlightController.ShowProgressBar(gameObject, transform.position);
    // }

    // private IEnumerator HitCoroutine()
    // {
    //     hitProgress += Time.deltaTime;
    //     Debug.Log(gameObject);
    //     highlightController.UpdateProgressBar(gameObject, hitProgress / hitTime);
    //     Debug.Log("Hit progress: " + hitProgress);
    //     Debug.Log("Hit time: " + hitTime);
    //     if (hitProgress >= hitTime)
    //     {
    //         isBeingHit = false;
    //         hitProgress = 0f;
    //         highlightController.RemoveProgressBar(gameObject);
    //         Hit();
    //     }
    //     yield return null;
    // }

    // public override void InteractReleased(Character character)
    // {
    //     if (isBeingHit)
    //     {
    //         isBeingHit = false;
    //         hitProgress = 0f;
    //     }
    // }

    public override bool CanBeHit(List<ResourceNodeType> canBeHitByTool)
    {
        return canBeHitByTool.Contains(nodeType);
    }
}