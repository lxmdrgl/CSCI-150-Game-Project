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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Rotate from Direction enum selection
        transform.rotation = Quaternion.Euler(0, 0, (int)exitDirection * 90);
    }
}
