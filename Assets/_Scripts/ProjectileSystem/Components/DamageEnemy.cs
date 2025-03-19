using UnityEngine;
using Game.CoreSystem;
using Game.Combat.Damage;
using Game.Combat.KnockBack;
using Game.Combat.StunDamage;
using Game.Utilities;
using System.Collections.Generic;
using Game.Projectiles;

public class DamageEnemy : MonoBehaviour
{
    /* [SerializeField]  */private float damageAmount;
    private Vector2 knockbackAngle;
    private float knockbackStrength;
    private ProjectileEnemy projectile;

    private void Awake()
    {
        projectile = GetComponentInParent<ProjectileEnemy>();
    }

    // Method to dynamically set the damage
    public void SetDamage(float amount, Vector2 knockbackAngle, float knockbackStrength)
    {
        damageAmount = amount;
        this.knockbackAngle = knockbackAngle;
        this.knockbackStrength = knockbackStrength;
    }

    public void HandleCollision(Collider2D collision)
{
    if (collision.CompareTag("Player"))
    {
        List<Collider2D> detected = new List<Collider2D> { collision };

        Debug.Log("Detected: " + detected.ToArray() + " count: " + detected.Count);

        // Apply Damage
        if (CombatDamageUtilities.TryDamage(detected.ToArray(), new DamageData(damageAmount, gameObject), out var damageables))
        {
            foreach (var damageable in damageables)
            {
                Debug.Log("Projectile Dealing " + damageAmount + " Damage To Player");
            }
        }
        else
        {
            Debug.Log("No damageable objects detected.");
        }

        // Check Knockback
        int facingDirection;
        if (projectile.startingRotation <= 0.05 && projectile.startingRotation >= -0.05) {
            facingDirection = 1;
        } else {
            facingDirection = -1;
        }
        Debug.Log($"Attempting Knockback - Angle: {knockbackAngle}, Strength: {knockbackStrength}, FacingDirection: {facingDirection}");

        bool didKnock = CombatKnockBackUtilities.TryKnockBack(
            detected.ToArray(),
            new KnockBackData(knockbackAngle, knockbackStrength, facingDirection, gameObject),
            out _
        );

        if (didKnock)
        {
            Debug.Log("✅ Projectile applied knockback to Player.");
        }
        else
        {
            Debug.Log("❌ No knockback applied. Check Player's Knockback Component.");
        }
    }
}

}
