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
using UnityEngine.Splines;
using Unity.VisualScripting;

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

                List<Collider2D> detectedGroundFiltered = null;
                foreach(Collider2D detected in detectedGround)
                {
                    if (!detected.CompareTag("Platform"))
                    {
                        detectedGroundFiltered.Add(detected);
                    }
                }

                if (detectedGroundFiltered.Count > 0)
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

            foreach (Collider2D detected in validTargets)
            {
                Vector2 targetPosition = detected.transform.position;
                float checkDirection = targetPosition.x - transform.position.x; 

                if (Mathf.Sign(checkDirection) == facingDirection /* true */)
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

            // Prevent division by zero and unreachable targets
            if (Mathf.Approximately(dx, 0) || Mathf.Approximately(velocity, 0))
            {
                Debug.LogError("Cannot calculate direction: zero distance or zero velocity.");
                return false;
            }

            // X direction is always fixed to 1
            float directionX = dx > 0 ? 1f : -1f;

            // Calculate time to reach the target horizontally
            float time = Mathf.Abs(dx) / velocity;

            // Calculate the required vertical velocity using the projectile motion equation
            // vy = (dy + 0.5 * g * t^2) / t
            float vy = (dy + 0.5f * gEffective * time * time) / time;

            // Normalize Y velocity between 0 and 1
            float directionY = vy / velocity;

            // Check if the calculated Y direction is outside the valid range [0, 1]
            if (Mathf.Abs(directionY) > 1.732f) // 60 degrees
            {
                Debug.LogWarning("Target is unreachable with given velocity, gravity, and angle limit.");
                return false;
            }

            // Create the direction vector with X fixed to 1 and Y calculated
            Vector2 direction = new Vector2(directionX, directionY);

            // Apply the calculated velocity
            rb.linearVelocity = direction * velocity;
            Debug.Log($"Target found: velocity {rb.linearVelocity}, direction: {direction}, dxdy: {dx}, {dy}");
            // Debug.Log($"near: {nearestTarget}, {nearestTarget.transform.position.x}, {nearestTarget.transform.position.y}");
            // Debug.Log($"transform: {transform.position.x}, {transform.position.y}");
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