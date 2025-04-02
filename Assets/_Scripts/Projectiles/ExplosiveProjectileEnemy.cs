using UnityEngine;
using System.Collections.Generic;
using Game.Combat.Damage;
using Game.Combat.KnockBack;
using Game.Utilities;

namespace Game.Projectiles
{
    public class ExplosiveProjectileEnemy : MonoBehaviour
    {
        [SerializeField] private float explosionTimer;
        [SerializeField] private float explosionRadius;
        [SerializeField] private float explosionDamage;
        [SerializeField] private float knockbackStrength;
        [SerializeField] private Vector2 knockbackAngle;
        [SerializeField] private LayerMask whatIsPlayer;
        [SerializeField] private LayerMask whatIsGround;
        [SerializeField] private GameObject explosionEffect;

        private Rigidbody2D rb;
        private PolygonCollider2D hitbox;
        private bool hasExploded = false;
        private bool startTimer = false;
        private float startTime = 0f;
        
        private float speed;
        private float travelDistance;
        private Vector2 startPosition;
        private List<Collider2D> detectedGround = new List<Collider2D>();
        private List<Collider2D> detectedDamageable = new List<Collider2D>();
        private ContactFilter2D filterGround;
        private ContactFilter2D filterDamageable;

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            hitbox = GetComponent<PolygonCollider2D>();

            filterGround.useLayerMask = true;
            filterDamageable.useLayerMask = true;
            filterGround.SetLayerMask(whatIsGround);
            filterDamageable.SetLayerMask(whatIsPlayer);

            rb.linearVelocity = transform.right * speed;
            startPosition = transform.position;
        }

        private void Update()
        {
            if (Vector2.Distance(startPosition, transform.position) >= travelDistance)
            {
                Physics2D.OverlapCollider(hitbox, filterGround, detectedGround);
                Physics2D.OverlapCollider(hitbox, filterDamageable, detectedDamageable);

                if (detectedDamageable.Count > 0)
                {

                }

                if (detectedGround.Count > 0)
                {
                    startTimer = true;
                    startTime = Time.time;
                }

                if (!hasExploded && startTimer && Time.time - startTime >= explosionTimer)
                {
                    Explode();
                    hasExploded = true;
                }
            }
        }

        /* private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!hasExploded && (collision.gameObject.layer == LayerMask.NameToLayer("Ground") || collision.CompareTag("Player")))
            {
                Explode();
            }
        } */

        private void Explode()
        {
            Collider2D[] hitObjects = Physics2D.OverlapCircleAll(transform.position, explosionRadius, whatIsPlayer);

            if (hitObjects.Length > 0)
            {
                HandleDamage(hitObjects);
            }

            Destroy(gameObject);
        }

        private void HandleDamage(Collider2D[] hitObjects)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);

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

        // `FireProjectile()` so `TriggerAttack()` can call it!
        public void FireProjectile(float speed, float travelDistance)
        {
            this.speed = speed;
            this.travelDistance = travelDistance;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, explosionRadius);
        }
    }
}
