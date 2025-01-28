using System;
using UnityEngine;

[ExecuteAlways]
public class RoomExit : MonoBehaviour
{
    public enum Direction {
        Right,
        Up,
        Left,
        Down
    }

    public Direction exitDirection;

    void Start()
    {
        
    }
    
    void Update()
    {
        // Rotate from Direction enum selection
        transform.rotation = Quaternion.Euler(0, 0, (int)exitDirection * 90);
    }
}
