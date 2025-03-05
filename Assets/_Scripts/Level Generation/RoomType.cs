using UnityEngine;
using System.Collections.Generic;
using System.Linq; // Required for .ToList()

public class RoomType : MonoBehaviour
{
    public string prefabName;
    public List<RoomManager> list = new List<RoomManager>();

    void Start()
    {
        list = Resources.LoadAll<RoomManager>("Rooms/" + prefabName).ToList();
        if (list.Count == 0) {
            Debug.LogError("Failed to load files from Resources/Rooms/" + prefabName);
        }
    }
}