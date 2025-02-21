using UnityEngine;
using System.Collections.Generic;
using System.Linq; // Required for .ToList()

public class RoomType : MonoBehaviour
{
    public string name;
    public List<RoomManager> list = new List<RoomManager>();

    void Start() // Use Start instead of Update
    {
        list = Resources.LoadAll<RoomManager>("Rooms").ToList();
    }
}