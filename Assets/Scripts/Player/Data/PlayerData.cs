using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="newPlayerData", menuName ="Data/Player Data/Base Data")]
public class PlayerData : ScriptableObject
{
    [Header("Move State")]
    public float movementVelocity = 10f;

    [Header("Jump State")]
    public float jumpVelocity = 15f;
    public int amountOfJumps = 2;

    [Header("In Air State")]
    public float coyoteTime = 0.2f;

    [Header("Wall Jump State")]
    public float wallJumpVelocityX = 15f;
    public float wallJumpVelocityXMax = 15f;
    public float wallJumpVelocityY = 10f;
    public float wallJumpTime = 0.4f;
}
