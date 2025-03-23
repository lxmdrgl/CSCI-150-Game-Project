using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newExplosiveAttackStateData", menuName = "Data/State Data/Explosive Attack State")]
public class D_ExplosiveAttackState : ScriptableObject
{
    public GameObject projectile;
    public float projectileDamage = 10f;
    public float projectileSpeed = 12f;
    public float projectileTravelDistance = 10f;
    public float explosionRadius = 3f; // Explosion area
    public Vector2 knockbackAngle = Vector2.one;
    public float knockbackStrength = 10f;
    public float gravityScale = 3f;
    public LayerMask whatIsPlayer;
}