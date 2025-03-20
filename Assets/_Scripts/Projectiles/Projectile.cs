using UnityEngine;
using Game.CoreSystem;
using Game.Weapons.Components;
using Mono.Cecil.Cil;
using System.Collections.Generic;

using static Game.Utilities.CombatDamageUtilities;
using static Game.Utilities.StunDamageUtilities;
using Game.Combat.Damage;
using System.Linq;

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

            rb.linearVelocity = direction * velocity * facingDirection;
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
                    // Find damageable that have not been damaged 
                    Collider2D[] notDamaged = detectedDamageable.Except(hasDamaged).ToArray();
                    // Add damageable to has been damaged
                    hasDamaged.AddRange(detectedDamageable);

                    float totalDamage = damage * (attack / 100f); 
                    float totalStun = stun * (attack / 100f); 

                    if (!notDamaged.Any()) // check if not empty
                    {
                        TryDamage(notDamaged, new DamageData(totalDamage, gameObject), out _); 
                        TryStunDamage(notDamaged, new Combat.StunDamage.StunDamageData(totalStun, gameObject), out _); 
                    }

                    if (!pierce)
                    {
                        Destroy(gameObject);
                    }
                }

                if (detectedGround.Count > 0)
                {
                    hasHitGround = true;
                    rb.linearVelocity = Vector2.zero;
                    rb.gravityScale = 0f;
                    Destroy(gameObject, 1.0f);
                }
            }        
        }

        public void FireProjectile(AttackProjectileFire fireData, float attack, int facingDirection)
        {
            this.damage = fireData.damage;
            this.stun = fireData.stun;

            this.velocity = fireData.velocity;
            this.direction = fireData.direction;
            this.rotate = fireData.rotate;
            this.pierce = fireData.pierce;
            this.hasGravity = fireData.hasGravity;
            this.gravityScale = fireData.gravityScale;
            this.whatIsGround = fireData.whatIsGround;
            this.whatIsDamageable = fireData.whatIsDamageable;

            this.attack = attack;
            this.facingDirection = facingDirection;
        }
    }

}