using System;
using UnityEngine;
using UnityEngine.Tilemaps;

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

    void Start()
    {
        // Only run this when the game is playing
        if (Application.isPlaying)
        {
            TilemapRenderer tilemapRenderer = GetComponentInChildren<TilemapRenderer>();
            if (tilemapRenderer != null)
            {
                tilemapRenderer.enabled = false;
            }
        }
    }
}
