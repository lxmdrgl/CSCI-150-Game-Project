using System;
using Game.CoreSystem;
using Unity.Cinemachine;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public GameObject player1;
    public GameObject player2;
    public CinemachineCamera singleplayerCinemachineCamera;
    public CinemachineCamera multiplayerCinemachineCamera;
    private Death death1;
    private Death death2;

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

    public void SetDependencies()
    {
        if (player1 != null)
        {
            death1 = player1.GetComponentInChildren<Core>().GetComponentInChildren<Death>();
            death1.OnDeath += () => HandleOnPlayerDeath(0);
        }
        if (player2 != null)
        {
            death2 = player2.GetComponentInChildren<Core>().GetComponentInChildren<Death>();
            death2.OnDeath += () => HandleOnPlayerDeath(1);
        }
    }

    public void OnDisable()
    {
        if (death1 != null)
        {
            death1.OnDeath -= () => HandleOnPlayerDeath(0);
        }
        if (death2 != null)
        {
            death2.OnDeath -= () => HandleOnPlayerDeath(1);
        }
    }

    public void HandleOnPlayerDeath(int index)
    {
        // singleplayerCinemachineCamera.enabled = true;
        // multiplayerCinemachineCamera.enabled = false;
        if (index == 0 && player2 != null)
        {
            singleplayerCinemachineCamera.Target.TrackingTarget = player2.transform;
            UnityEngine.Debug.Log("Singleplayer camera set to player 2");
        } else if (index == 1 && player1 != null)
        {
            singleplayerCinemachineCamera.Target.TrackingTarget = player1.transform;
            UnityEngine.Debug.Log("Singleplayer camera set to player 1");
        }
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
