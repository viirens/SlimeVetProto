using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    Transform player;
    [SerializeField] float speed = 4f;
    [SerializeField] float pickUpDistance = 3f;
    [SerializeField] float timeToLive = 30f;
    // [SerializeField] float despawnDistance = 20f;
    public Item item;
    public int count = 1;
    private void Awake()
    {
        player = GameManager.instance.player.transform;
    }

    public void Set(Item item, int count)
    {
        this.item = item;
        this.count = count;

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = item.Icon;

        
    }

    private void Update()
    {
        timeToLive -= Time.deltaTime;

        if (timeToLive < 0)
        {
            Destroy(gameObject);
        }

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > pickUpDistance)
        {
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

        if (distance < 0.1f)
        {
            //TODO Should be moved into specified controller rather than being checked here
            if (GameManager.instance.inventoryContainer != null)
            {
                GameManager.instance.inventoryContainer.Add(item, count);
            }
            else
            {
                Debug.LogWarning("No inventory container found");
            }
            Destroy(gameObject);
        }
    }
}