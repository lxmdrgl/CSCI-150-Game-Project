using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newExplosiveAttackStateData", menuName = "Data/State Data/Explosive Attack State")]
public class D_ExplosiveAttackState : ScriptableObject
{
    public GameObject projectile;
    public float projectileDamage = 10f;
    public float projectileSpeed = 10f;
    public float projectileTravelDistance = 15f;
    public float explosionRadius = 3f;
    public Vector2 knockbackAngle = new Vector2(1, 1);
    public float knockbackStrength = 10f;
    public LayerMask whatIsPlayer;
}
