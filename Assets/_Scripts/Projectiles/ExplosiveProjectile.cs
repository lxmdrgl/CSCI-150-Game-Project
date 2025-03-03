using UnityEngine;
using System.Collections.Generic;
using Game.Combat.Damage;
using Game.Combat.KnockBack;
using Game.Utilities;

namespace Game.Projectiles
{
    public class ExplosiveProjectile : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private float explosionRadius;
        [SerializeField] private float explosionDamage;
        [SerializeField] private float knockbackStrength;
        [SerializeField] private Vector2 knockbackAngle;
        [SerializeField] private LayerMask whatIsPlayer;
        [SerializeField] private GameObject explosionEffect; // Visual explosion effect

        private Rigidbody2D rb;
        private bool hasExploded = false;

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            rb.linearVelocity = transform.right * speed;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!hasExploded && (collision.gameObject.layer == LayerMask.NameToLayer("Ground") || collision.CompareTag("Player")))
            {
                Explode();
            }
        }

        private void Explode()
        {
            hasExploded = true;
            Instantiate(explosionEffect, transform.position, Quaternion.identity); // Spawn explosion VFX

            // Find all players within explosion radius
            Collider2D[] hitObjects = Physics2D.OverlapCircleAll(transform.position, explosionRadius, whatIsPlayer);

            if (hitObjects.Length > 0)
            {
                Debug.Log($"Explosion hit {hitObjects.Length} targets!");

                // Apply damage
                if (CombatDamageUtilities.TryDamage(hitObjects, new DamageData(explosionDamage, gameObject), out _))
                {
                    Debug.Log($"Explosive dealt {explosionDamage} damage.");
                }

                // Apply knockback individually
                foreach (Collider2D hit in hitObjects)
                {
                    int facingDirection = transform.position.x < hit.transform.position.x ? 1 : -1;

                    CombatKnockBackUtilities.TryKnockBack(
                        new Collider2D[] { hit },
                        new KnockBackData(knockbackAngle, knockbackStrength, facingDirection, gameObject),
                        out _
                    );
                }
            }

            Destroy(gameObject, 0.1f); // Small delay to allow explosion effect to be seen
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, explosionRadius);
        }
    }
}
