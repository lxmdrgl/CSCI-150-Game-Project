using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newEnemyDashStateData", menuName = "Data/State Data/Enemy Dash State")]
public class D_EnemyDashState : ScriptableObject
{
    public float attackRadius = 0.5f;
    public float attackDamage = 10f;
    public Vector2 knockbackAngle = Vector2.one;
    public float knockbackStrength = 10f;

    public float dashTime = 0.2f;
    public float dashSpeed = 10f;

    public LayerMask whatIsPlayer;
}
