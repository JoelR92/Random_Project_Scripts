using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//Highly Unoptimized implementation of an Octree for learning purposes

public class OctreeManager : MonoBehaviour
{

    public Transform player;
    // The maximum distance at which objects should be activated
    public float activationDistance = 10.0f;

    // The list of objects to activate/deactivate
    public List<GameObject> objectsToActivate;

    // The octree that will be used to partition the game world
    private Octree<GameObject> octree;

    private void Start()
    {
        // Create the octree with a root node that encompasses the entire game world
        Bounds worldBounds = new Bounds(Vector3.zero, Vector3.one * float.MaxValue);
        octree = new Octree<GameObject>(worldBounds, 4);

        // Add each object in the list to the octree
        foreach (GameObject obj in objectsToActivate)
        {
            octree.Add(obj, obj.transform.position);
        }
    }

    private void Update()
    {
        // Activate/deactivate objects based on their distance from the player
        Vector3 playerPosition = player.position;
        foreach (GameObject obj in objectsToActivate)
        {
            float distance = Vector3.Distance(playerPosition, obj.transform.position);
            bool shouldActivate = (distance <= activationDistance);
            obj.SetActive(shouldActivate);
        }
    }

    private void LateUpdate()
    {
        // Update the octree as objects move around
        foreach (GameObject obj in objectsToActivate)
        {
            octree.Update(obj, obj.transform.position);
        }
    }
}

public class Octree<T>
{
    private OctreeNode<T> rootNode;

    public Octree(Bounds bounds, int maxDepth)
    {
        rootNode = new OctreeNode<T>(bounds, maxDepth);
    }

    public void Add(T item, Vector3 position)
    {
        rootNode.Add(item, position);
    }

    public void Remove(T item)
    {
        rootNode.Remove(item);
    }

    public List<T> GetItemsInBounds(Bounds bounds)
    {
        return rootNode.GetItemsInBounds(bounds);
    }

    public void Update(T item, Vector3 position)
    {
        rootNode.Update(item, position);
    }
}

public class OctreeNode<T>
{
    private const int MAX_ITEMS = 8;

    private OctreeNode<T>[] children;
    private List<T> items;
    private Bounds bounds;
    private int depth;
    private int maxDepth;

    public OctreeNode(Bounds bounds, int maxDepth, int depth = 0)
    {
        children = new OctreeNode<T>[8];
        items = new List<T>();
        this.bounds = bounds;
        this.depth = depth;
        this.maxDepth = maxDepth;
    }

    public void Add(T item, Vector3 position)
    {
        // If this node is at max depth or capacity, add the item to the list of items in this node
        if (depth >= maxDepth || items.Count >= MAX_ITEMS)
        {
            items.Add(item);
            return;
        }

        // Otherwise, split the node into eight child nodes if they don't exist already
        if (children[0] == null)
        {
            Split();
        }

        // Add the item to the appropriate child node(s)
        foreach (OctreeNode<T> child in children)
        {
            if (child.bounds.Contains(position))
            {
                child.Add(item, position);
            }
        }
    }

    public void Remove(T item)
    {
        items.Remove(item);

        // If this node has no items left and has children, merge the children
        if (items.Count == 0 && children[0] != null)
        {
            Merge();
        }
    }

    public List<T> GetItemsInBounds(Bounds bounds)
    {
        List<T> result = new List<T>();

        // If the bounds don't intersect this node's bounds, return an empty list
        if (!bounds.Intersects(this.bounds))
        {
            return result;
        }

        // If this node is a leaf node, add all the items in this node to the result list
        if (children[0] == null)
        {
            result.AddRange(items);
        }

        // Otherwise, recursively call GetItemsInBounds on each child node that intersects the bounds
        else
        {
            foreach (OctreeNode<T> child in children)
            {
                if (bounds.Intersects(child.bounds))
                {
                    result.AddRange(child.GetItemsInBounds(bounds));
                }
            }
        }

        return result;
    }

    public void Update(T item, Vector3 position)
    {
        // If the item is in this node, remove it and re-add it to the octree
        if (items.Contains(item))
        {
            items.Remove(item);
            Add(item, position);
        }

        // Otherwise, try to update the item in one of the child nodes
        else
        {
            foreach (OctreeNode<T> child in children)
            {
                if (child.bounds.Contains(position))
                {
                    child.Update(item, position);
                    break;
                }
            }
        }
    }

    private void Split()
    {
        float childSize = bounds.size.x / 2.0f;
        float x = bounds.min.x;
        float y = bounds.min.y;
        float z = bounds.min.z;

        children[0] = new OctreeNode<T>(new Bounds(new Vector3(x, y, z), new Vector3(childSize, childSize, childSize)), maxDepth, depth + 1);
        children[1] = new OctreeNode<T>(new Bounds(new Vector3(x + childSize, y, z), new Vector3(childSize, childSize, childSize)), maxDepth, depth + 1);
        children[2] = new OctreeNode<T>(new Bounds(new Vector3(x, y + childSize, z), new Vector3(childSize, childSize, childSize)), maxDepth, depth + 1);
        children[3] = new OctreeNode<T>(new Bounds(new Vector3(x + childSize, y + childSize, z), new Vector3(childSize, childSize, childSize)), maxDepth, depth + 1);
        children[4] = new OctreeNode<T>(new Bounds(new Vector3(x, y, z + childSize), new Vector3(childSize, childSize, childSize)), maxDepth, depth + 1);
        children[5] = new OctreeNode<T>(new Bounds(new Vector3(x + childSize, y, z + childSize), new Vector3(childSize, childSize, childSize)), maxDepth, depth + 1);
        children[6] = new OctreeNode<T>(new Bounds(new Vector3(x, y + childSize, z + childSize), new Vector3(childSize, childSize, childSize)), maxDepth, depth + 1);
        children[7] = new OctreeNode<T>(new Bounds(new Vector3(x + childSize, y + childSize, z + childSize), new Vector3(childSize, childSize, childSize)), maxDepth, depth + 1);
    }

    private void Merge()
    {
        // Merge all the children's items into this node's item list
        foreach (OctreeNode<T> child in children)
        {
            items.AddRange(child.items);
        }

        // Clear the children array
        children = new OctreeNode<T>[8];
    }



}


