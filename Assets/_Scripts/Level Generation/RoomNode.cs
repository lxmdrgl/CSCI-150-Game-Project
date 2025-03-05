using UnityEngine;
using System.Collections.Generic;
using System.Linq; // Required for .ToList()

[SerializeField]
public class RoomNode : MonoBehaviour
{
    public string roomType;
    public GameObject roomObject;
    public RoomNode parent; // Parent node (null for root)
    
    [SerializeField]
    public List<RoomNode> children; // Child nodes

    void Start()
    {
        // load resources into rooms
        // if (roomType == "") 
        // {
        //     Debug.LogError(gameObject.name + ": roomType left empty");
        // } 
        // else 
        // {
        //     rooms = Resources.LoadAll<RoomManager>("Rooms/" + roomType).ToList();
        //     if (rooms.Count == 0) {
        //         Debug.LogError("Failed to load files from Resources/Rooms/" + roomType);
        //     }
        // }

        // create tree from hierarchy
        if (transform.parent != null)
        {
            parent = transform.parent.GetComponent<RoomNode>();
            if (parent != null)
            {
                parent.AddChild(this);
            }
        }

        // Ensure children are assigned correctly
        foreach (Transform childTransform in transform)
        {
            RoomNode childNode = childTransform.GetComponent<RoomNode>();
            if (childNode != null && !children.Contains(childNode))
            {
                AddChild(childNode);
            }
        }

        PrintTree();

        // Debug.Log(gameObject.name + ": " + children.Count);
    }

    // Add a child to this node
    public void AddChild(RoomNode child)
    {
        if (!children.Contains(child))
        {
            children.Add(child);
            child.parent = this; // Set this as the parent
        }
        
    }

    // Print tree structure recursively
    public void PrintTree(int depth = 0)
    {
        Debug.Log(new string('-', depth * 2) + gameObject.name);
        foreach (RoomNode child in children)
        {
            child.PrintTree(depth + 1);
        }
    }

}
