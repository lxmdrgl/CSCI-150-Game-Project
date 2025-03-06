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

    void OnEnable()
    {
        // assign parent
        if (transform.parent != null)
        {
            parent = transform.parent.GetComponent<RoomNode>();
            if (parent != null)
            {
                parent.AddChild(this);
                // Debug.Log(gameObject.name + ": " + parent.gameObject.name + "->" + gameObject.name);
            }
        }

        // Ensure children are assigned correctly
        foreach (Transform childTransform in transform)
        {
            RoomNode childNode = childTransform.GetComponent<RoomNode>();
            if (childNode != null && !children.Contains(childNode))
            {
                AddChild(childNode);
                // Debug.Log(gameObject.name + ": " + gameObject.name + "->" + childNode.gameObject.name);
            }
        }   
    }

    void Start()
    {
        if (parent == null) {
            // PrintTree();
        }
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
