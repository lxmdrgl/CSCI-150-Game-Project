using System;
using UnityEngine;

[ExecuteAlways]
public class RoomDoor : MonoBehaviour
{
    public enum Direction { Right, Up, Left, Down }
    public Direction direction;
    public bool isDone = false;

    void OnValidate()
    {
        // Rotate from Direction enum selection
        transform.rotation = Quaternion.Euler(0, 0, (int)direction * 90);
    }
}
