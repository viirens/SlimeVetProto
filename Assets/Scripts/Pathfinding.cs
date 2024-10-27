using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public bool walkable;
    public Vector2 worldPosition;
    public int gridX;
    public int gridY;
    public int gCost;
    public int hCost;
    public Node parent;

    public int fCost { get { return gCost + hCost; } }

    public Node(bool walkable, Vector2 worldPosition, int gridX, int gridY)
    {
        this.walkable = walkable;
        this.worldPosition = worldPosition;
        this.gridX = gridX;
        this.gridY = gridY;
    }
}

public class Pathfinding : MonoBehaviour
{
    private Grid grid; // Reference to the Grid component

    void Awake()
    {
        // Find the Grid component in the scene
        grid = FindObjectOfType<Grid>();
        if (grid == null)
        {
            Debug.LogError("Grid component not found in the scene!");
        }
    }

    public List<Node> FindPath(Vector2 startPos, Vector2 targetPos)
    {
        if (grid == null)
        {
            Debug.LogError("Grid is not initialized!");
            return null;
        }

        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);

        // Check if the target node is walkable
        if (!targetNode.walkable)
        {
            targetNode = FindClosestWalkableNode(targetNode);
        }

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == targetNode)
            {
                return RetracePath(startNode, targetNode);
            }

            foreach (Node neighbour in grid.GetNeighbours(currentNode))
            {
                if (!neighbour.walkable || closedSet.Contains(neighbour))
                {
                    continue;
                }

                // Check for diagonal movement
                if (IsDiagonal(currentNode, neighbour) && !CanMoveDiagonally(currentNode, neighbour))
                {
                    continue;
                }

                int newCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                if (newCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = currentNode;

                    if (!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                }
            }
        }

        return null;
    }

    Node FindClosestWalkableNode(Node targetNode)
    {
        Node closestNode = null;
        int closestDistance = int.MaxValue;

        foreach (Node node in grid.grid)
        {
            if (node.walkable)
            {
                int distance = GetDistance(targetNode, node);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestNode = node;
                }
            }
        }

        return closestNode;
    }

    List<Node> RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();
        return path;
    }

    int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }

    bool IsDiagonal(Node nodeA, Node nodeB)
    {
        return nodeA.gridX != nodeB.gridX && nodeA.gridY != nodeB.gridY;
    }

    bool CanMoveDiagonally(Node currentNode, Node neighbour)
    {
        Node nodeX = grid.grid[currentNode.gridX, neighbour.gridY];
        Node nodeY = grid.grid[neighbour.gridX, currentNode.gridY];
        return nodeX.walkable && nodeY.walkable;
    }
}
