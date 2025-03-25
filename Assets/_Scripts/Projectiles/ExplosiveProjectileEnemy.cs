using UnityEngine;
using System.Collections.Generic;
using Game.Combat.Damage;
using Game.Combat.KnockBack;
using Game.Utilities;

namespace Game.Projectiles
{
    public class ExplosiveProjectileEnemy : MonoBehaviour
    {
        [SerializeField] private float explosionRadius;
        [SerializeField] private float explosionDamage;
        [SerializeField] private float knockbackStrength;
        [SerializeField] private Vector2 knockbackAngle;
        [SerializeField] private LayerMask whatIsPlayer;
        [SerializeField] private GameObject explosionEffect;

        private Rigidbody2D rb;
        private bool hasExploded = false;
        private float speed;
        private float travelDistance;
        private Vector2 startPosition;
        private Vector2 target;
        bool hasGravity;
        string projectileType;
        public float startingRotation;
         private float gravity;

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            rb.linearVelocity = transform.right * speed;
            startPosition = transform.position;

            if(projectileType == "radialWithGravity")
            {
                Vector2 direction = (target - (Vector2)transform.position).normalized;
                rb.linearVelocity = direction * speed;
            }
            else if (projectileType == "radialNoGravity")
            {
                Vector2 direction = (target - (Vector2)transform.position).normalized;
                rb.linearVelocity = direction * speed;
            }
            else if(projectileType == "linearWithGravity")
            {
                rb.linearVelocity = transform.right * speed;
            }
        }

        private void Update()
        {
            if (Vector2.Distance(startPosition, transform.position) >= travelDistance)
            {
                Explode();
            }
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
            Instantiate(explosionEffect, transform.position, Quaternion.identity);

            Collider2D[] hitObjects = Physics2D.OverlapCircleAll(transform.position, explosionRadius, whatIsPlayer);

            if (hitObjects.Length > 0)
            {
                Debug.Log($"💥 Explosion hit {hitObjects.Length} targets!");

                if (CombatDamageUtilities.TryDamage(hitObjects, new DamageData(explosionDamage, gameObject), out _))
                {
                    Debug.Log($"✅ Explosive dealt {explosionDamage} damage.");
                }

                CombatKnockBackUtilities.TryKnockBack(
                    hitObjects,
                    new KnockBackData(knockbackAngle, knockbackStrength, transform.position.x < hitObjects[0].transform.position.x ? 1 : -1, gameObject),
                    out _
                );
            }

            Destroy(gameObject);
        }

        // `FireProjectile()` so `TriggerAttack()` can call it!
        public void FireProjectile(float speed, float travelDistance, Vector2 target, string projectileType, float startingRotation, float gravity)
        {
            this.projectileType = projectileType;
            this.startingRotation = startingRotation;
            this.gravity = gravity;
            // Debug.Log("Gravity: " + this.gravity + " , " + gravity);

            if(projectileType == "radialWithGravity")
            {
                this.speed = speed;
                this.target = target;
                this.travelDistance = travelDistance;
                hasGravity = true;
            }
            else if (projectileType == "radialNoGravity")
            {
                this.speed = speed;
                this.target = target;
                hasGravity = false;
            }
            else if(projectileType =="linearWithGravity")
            {
                this.speed = speed;
                this.travelDistance = travelDistance;
                hasGravity = true;
            }
            else if (projectileType == "radialLobbing")
            {
                this.speed = speed;
                this.target = target;
                hasGravity = true;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, explosionRadius);
        }
    }
    
}

