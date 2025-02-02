using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

[CreateAssetMenu(fileName ="newPlayerData", menuName ="Data/Player Data/Base Data")]
public class PlayerData : ScriptableObject
{
    [Header("Move State")]
    public float movementVelocity = 15f;

    [Header("Jump State")]
    public float jumpVelocity = 15f;
    public int amountOfJumps = 2;

    [Header("Dash State")]
    public float dashVelocity = 15f;
    public float dashTime = 0.25f;
    public float dashCooldown = 0.1f;

    [Header("In Air State")]
    public float coyoteTime = 0.2f;

    [Header("Wall Jump State")]
    public float wallJumpVelocityX = 15f;
    public float wallJumpVelocityXMax = 15f;
    public float wallJumpVelocityY = 20f;
    public float wallJumpTime = 0.2f;

    [Header("Platform")]
    public float platformVelocity = 30f;
    public float platformMovementVelocity = 5f;

    [Header("Player Type")]
    public PlayerType playerType = PlayerType.knight;

}

public enum PlayerType {
    knight,
    beast
}
