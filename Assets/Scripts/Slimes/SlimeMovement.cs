using UnityEngine;
using System.Collections.Generic;

public class SlimeMovement : MonoBehaviour
{
    public float speed = 5f;
    public float arrivalThreshold = 0.1f;
    public float obstacleAvoidanceDistance = 1f;
    private Rigidbody2D rb;
    public bool hasArrived = false;
    private bool isExiting = false; // Add this flag to track exiting state
    private Pathfinding pathfinding;
    private Grid grid;
    private List<Node> path;
    private int targetIndex;
    private LineRenderer lineRenderer;
    public bool isInteractable = false; // Add this flag to track interactability
    private Transform waypoint; // Declare the waypoint variable

    // Add this variable
    [SerializeField] private float waypointRadius = 10f; // Radius around the waypoint for slimes to pick positions

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        pathfinding = GetComponent<Pathfinding>();
        grid = FindObjectOfType<Grid>(); // Find the Grid component in the scene
        lineRenderer = GetComponent<LineRenderer>();

        grid = FindObjectOfType<Grid>();
        if (grid == null)
        {
            Debug.LogError("Grid component not found in the scene!");
            return;
        }

        // Find the Waypoint by tag
        GameObject waypointObject = GameObject.Find("Waypoint");
        if (waypointObject != null)
        {
            waypoint = waypointObject.transform;
            Debug.Log("Waypoint found: " + waypointObject.name);
        }
        else
        {
            Debug.LogError("Waypoint GameObject with tag 'Waypoint' not found!");
            return;
        }

        // Get a random position around the waypoint
        Vector2 targetPosition = GetRandomPositionAroundWaypoint();

        // Find path to the target position
        path = pathfinding.FindPath(transform.position, targetPosition);
        targetIndex = 0;
        hasArrived = false; // Ensure hasArrived is initialized to false
        SetInteractableState(false); // Initialize interactability to false
        DrawPath();
    }

    // Add this method to calculate random target position
    private Vector2 GetRandomPositionAroundWaypoint()
    {
        Vector2 randomOffset = Random.insideUnitCircle * waypointRadius;
        Vector2 randomPos = (Vector2)waypoint.position + randomOffset;

        // Ensure the random position is on a walkable node
        Node targetNode = grid.NodeFromWorldPoint(randomPos);
        if (targetNode != null && targetNode.walkable)
        {
            return targetNode.worldPosition;
        }
        else
        {
            // If not walkable, fallback to waypoint position
            return waypoint.position;
        }
    }

    void FixedUpdate()
    {
        if (path != null && targetIndex < path.Count)
        {
            MoveAlongPath();
        }
        else if (hasArrived && isExiting)
        {
            Destroy(gameObject); // Destroy the slime only when exiting
        }
    }

    void MoveAlongPath()
    {
        if (targetIndex >= path.Count)
        {
            hasArrived = true;
            SetInteractableState(!isExiting); // Update interactability based on exiting state
            return;
        }

        Node targetNode = path[targetIndex];
        Vector2 targetPosition = targetNode.worldPosition;
        Vector2 direction = (targetPosition - rb.position).normalized;
        Vector2 newPosition = rb.position + direction * speed * Time.fixedDeltaTime;
        rb.MovePosition(newPosition);

        if (Vector2.Distance(rb.position, targetPosition) <= arrivalThreshold)
        {
            targetIndex++;
            if (targetIndex >= path.Count)
            {
                hasArrived = true;
                SetInteractableState(!isExiting); // Update interactability based on exiting state
                if (isExiting)
                {
                    Debug.Log("Arrived at exit");
                }
                else
                {
                    Slime slime = GetComponent<Slime>();
                    if (slime != null)
                    {
                        slime.ShowItemSprite(); // Show the item sprite when arriving at the waypoint
                    }
                    Debug.Log("Arrived at waypoint");
                }
            }
        }
    }

    private void SetInteractableState(bool state)
    {
        Slime slime = GetComponent<Slime>();
        if (slime != null)
        {
            slime.SetInteractable(state);
        }
    }

    public void MoveToExit()
    {
        if (grid == null)
        {
            Debug.LogError("Grid is not initialized!");
            return;
        }

        // Set the exiting flag to true
        isExiting = true;

        // Set interactability to false as it starts moving to the exit
        SetInteractableState(false);

        // Implement logic to move the slime to the nearest map edge
        // For simplicity, let's assume the exit is to the right
        Vector2 exitPosition = new Vector2(grid.gridWorldSize.x / 2, transform.position.y);
        path = pathfinding.FindPath(transform.position, exitPosition);
        targetIndex = 0;
        hasArrived = false;
        DrawPath();
    }

    void DrawPath()
    {
        if (lineRenderer == null || path == null || path.Count == 0)
        {
            if (lineRenderer != null)
            {
                lineRenderer.positionCount = 0;
            }
            return;
        }

        lineRenderer.positionCount = path.Count;
        for (int i = 0; i < path.Count; i++)
        {
            lineRenderer.SetPosition(i, path[i].worldPosition);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collided with " + collision.gameObject.name);
    }
}
