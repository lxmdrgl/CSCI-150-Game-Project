using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq; // Required for .ToList()

[SerializeField]
public class RoomNode : MonoBehaviour
{
    public static List<string> RoomTypes = new List<string>(); // Dynamic "enum"
    [SerializeField] public string roomType;
    public GameObject roomObject;
    public RoomNode parent; // Parent node (null for root)
    
    [SerializeField]
    public List<RoomNode> children; // Child nodes

    public List<RoomManager> listTriedRooms = new List<RoomManager>();

    void OnEnable()
    {
        UpdateRoomTypes(); // Refresh room types list

        // Ensure roomType is valid
        if (!RoomTypes.Contains(roomType))
        {
            roomType = RoomTypes.Count > 0 ? RoomTypes[0] : "None"; // Assign default valid type
        }

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

    private void OnValidate()
    {
        UpdateRoomTypes();
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

    public static void UpdateRoomTypes()
    {
        string path = Path.Combine(Application.dataPath, "Resources/Rooms");
        if (!Directory.Exists(path))
        {
            RoomTypes = new List<string> { "None" }; // Default value
            return;
        }

        RoomTypes = new List<string>(Directory.GetDirectories(path));
        for (int i = 0; i < RoomTypes.Count; i++)
        {
            RoomTypes[i] = Path.GetFileName(RoomTypes[i]); // Extract folder names only
        }
    }

    public string RoomType
    {
        get => roomType;
        set
        {
            if (RoomTypes.Contains(value))
                roomType = value;
        }
    }
}
