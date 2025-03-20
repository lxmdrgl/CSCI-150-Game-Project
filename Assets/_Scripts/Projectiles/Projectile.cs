using UnityEngine;
using Game.CoreSystem;
using Game.Weapons.Components;
using Mono.Cecil.Cil;
using System.Collections.Generic;

using static Game.Utilities.CombatDamageUtilities;
using static Game.Utilities.StunDamageUtilities;
using Game.Combat.Damage;
using System.Linq;
using System.Collections;

namespace Game.Projectiles
{
    public class Projectile : MonoBehaviour
    {
        // Fire Data
        private float damage;
        private float stun;
        private float velocity;
        private Vector2 direction;
        private bool rotate;
        private bool pierce;
        private bool explosive;
        private float explosiveRadius;
        private bool target;
        private float targetRadius;
        bool hasGravity;
        private float gravityScale;
        private LayerMask whatIsGround;
        private LayerMask whatIsDamageable;

        // Other Data
        private Rigidbody2D rb;
        private PolygonCollider2D hitbox;
        private ContactFilter2D filterGround;
        private ContactFilter2D filterDamageable;
        private List<Collider2D> detectedGround = new List<Collider2D>();
        private List<Collider2D> detectedDamageable = new List<Collider2D>();
        private List<Collider2D> hasDamaged = new List<Collider2D>();
        private bool hasHitGround;
        private float attack;
        private int facingDirection;
        float totalDamage;
        float totalStun;
        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            hitbox = GetComponent<PolygonCollider2D>();

            filterGround.useLayerMask = true;
            filterDamageable.useLayerMask = true;
            filterGround.SetLayerMask(whatIsGround);
            filterDamageable.SetLayerMask(whatIsDamageable);

            if (hasGravity)
            {
                rb.gravityScale = gravityScale;
            }
            else
            {
                rb.gravityScale = 0f;
            }

            totalDamage = damage * (attack / 100f); 
            totalStun = stun * (attack / 100f); 

            if (target)
            {
                if (!HandleTargetDirection())
                {
                    rb.linearVelocity = new Vector2(direction.x * velocity * facingDirection, direction.y * velocity);
                    // Debug.Log("Target not found: " + rb.linearVelocity);
                }
            }
            else 
            {
                rb.linearVelocity = new Vector2(direction.x * velocity * facingDirection, direction.y * velocity);
            }

        }

        private void Update()
        {
            if (rotate && !hasHitGround)
            {
                float angle = Mathf.Atan2(rb.linearVelocity.y, rb.linearVelocity.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
        }

        private void FixedUpdate()
        {
            if (!hasHitGround)
            {
                Physics2D.OverlapCollider(hitbox, filterGround, detectedGround);
                Physics2D.OverlapCollider(hitbox, filterDamageable, detectedDamageable);


                if (detectedDamageable.Count > 0)
                {
                    if (explosive)
                    {
                        // Explosive, ignore pierce
                        HandleExplosiveProjectile();
                    }
                    else
                    {
                        // Pierce or not pierce 
                        HandleProjectile();
                    }
                }

                if (detectedGround.Count > 0)
                {
                    hasHitGround = true;
                    rb.linearVelocity = Vector2.zero; 
                    rb.gravityScale = 0f; 
                    Debug.Log("Hit Ground: " + hasHitGround + ", " + rb.linearVelocity + ", " + rb.gravityScale);
                    if (explosive)
                    {
                        HandleExplosiveProjectile();
                    }
                    Destroy(gameObject, 1.0f);
                }
            }        
        }

        private void HandleExplosiveProjectile()
        {
            Vector2 position = new Vector2(transform.position.x, transform.position.y);
            Collider2D[] detectedExplosive = Physics2D.OverlapCircleAll(position, explosiveRadius, whatIsDamageable);

            if (detectedExplosive.Length > 0)
            {
                bool tryDamage = TryDamage(detectedExplosive, new DamageData(totalDamage, gameObject), out _); 
                bool tryStun = TryStunDamage(detectedExplosive, new Combat.StunDamage.StunDamageData(totalStun, gameObject), out _); 
                Debug.Log($"hit (damage, stun): {tryDamage}, {tryStun}");
            }

            Destroy(gameObject);
        }

        private void HandleProjectile()
        {
            Collider2D[] notDamaged = detectedDamageable.Except(hasDamaged).ToArray();
            hasDamaged.AddRange(detectedDamageable);

            if (notDamaged.Length > 0)
            {
                bool tryDamage = TryDamage(notDamaged, new DamageData(totalDamage, gameObject), out _); 
                bool tryStun = TryStunDamage(notDamaged, new Combat.StunDamage.StunDamageData(totalStun, gameObject), out _); 
                Debug.Log($"hit (damage, stun): {tryDamage}, {tryStun}");
            }

            if (!pierce)
            {
                Destroy(gameObject);
            }
        }

        private bool HandleTargetDirection()
        {
            Vector2 position = new Vector2(transform.position.x, transform.position.y);
            Collider2D[] detectedTarget = Physics2D.OverlapCircleAll(position, targetRadius, whatIsDamageable);

            if (detectedTarget.Length == 0)
            {
                Debug.Log("No target in radius found");
                return false;
            }

            List<Collider2D> validTargets = new List<Collider2D>();

            foreach (Collider2D detected in detectedTarget)
            {
                // Exclude self and other projectiles
                if (detected.gameObject != gameObject && !detected.CompareTag("Projectile"))
                {
                    validTargets.Add(detected);
                }
            }

            if (validTargets.Count == 0)
            {
                Debug.Log("No valid target in radius found");
                return false;
            }

            Collider2D nearestTarget = null;
            float minDistance = float.MaxValue;

            foreach (Collider2D detected in detectedTarget)
            {
                Vector2 targetPosition = detected.transform.position;
                float direction = targetPosition.x - transform.position.x; 

                if (/* Mathf.Sign(direction) == facingDirection */ true)
                {
                    float distance = (targetPosition - (Vector2)transform.position).sqrMagnitude;

                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        nearestTarget = detected;
                    }
                }
            }

            if (nearestTarget == null)
            {
                Debug.Log("No nearest target in front found: " + detectedTarget.Length);
                return false;
            }

            float gEffective = Mathf.Abs(Physics2D.gravity.y * rb.gravityScale);

            float dx = nearestTarget.transform.position.x - transform.position.x;
            float dy = nearestTarget.transform.position.y - transform.position.y;

            float vy = velocity;
            float discriminant = vy * vy - 2 * gEffective * dy;

            if (discriminant < 0)
            {
                Debug.Log("No target direction valid: " + nearestTarget);
                return false;
            }

            float t1 = (-vy + Mathf.Sqrt(discriminant)) / -gEffective;
            float t2 = (-vy - Mathf.Sqrt(discriminant)) / -gEffective;

            float tTotal = Mathf.Max(t1, t2);

            // Adjust vx calculation to consider facing direction
            // float vx = Mathf.Abs(dx / tTotal) * facingDirection;
            float vx = dx / tTotal;

            rb.linearVelocity = new Vector2(vx, vy);
            Debug.Log("Target found: " + rb.linearVelocity);
            return true;
        }

        public void FireProjectile(AttackProjectileFire fireData, float attack, int facingDirection)
        {
            this.damage = fireData.damage;
            this.stun = fireData.stun;

            this.velocity = fireData.velocity;
            this.direction = fireData.direction;
            this.rotate = fireData.rotate;
            this.pierce = fireData.pierce;
            this.explosive = fireData.explosive;
            this.explosiveRadius = fireData.explosiveRadius;
            this.target = fireData.target;
            this.targetRadius = fireData.targetRadius;
            this.hasGravity = fireData.hasGravity;
            this.gravityScale = fireData.gravityScale;
            this.whatIsGround = fireData.whatIsGround;
            this.whatIsDamageable = fireData.whatIsDamageable;

            this.attack = attack;
            this.facingDirection = facingDirection;
        }
    }

}