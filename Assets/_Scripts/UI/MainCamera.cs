using System;
using Unity.Cinemachine;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public GameObject player1;
    public GameObject player2;

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (player1 == null || player2 == null)
        {
            return;
        }
        // CheckPlayerBounds(player1.transform, player2.transform);
        CheckPlayerBounds(player2.transform, player1.transform);
    }

    private void CheckPlayerBounds(Transform player, Transform otherPlayer)
    {
        Vector3 viewportPos = mainCamera.WorldToViewportPoint(player.position);

        // Check if the player is outside the camera's field of view
        if (viewportPos.x < -0.1 || viewportPos.x > 1.1 || viewportPos.y < -0.1 || viewportPos.y > 1.1)
        {
            Debug.Log("Teleporting " + player.name + " to " + otherPlayer.name);
            TeleportPlayer(player, otherPlayer.position);
        }
    }

    private void TeleportPlayer(Transform player, Vector3 targetPosition)
    {
        player.position = targetPosition;
    }
}
