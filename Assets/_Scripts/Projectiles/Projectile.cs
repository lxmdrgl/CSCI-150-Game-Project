using UnityEngine;
using Game.CoreSystem;

namespace Game.Projectiles
{
    public class Projectile : MonoBehaviour
    {
        private float speed;
        private float travelDistance;
        private float xStartPos;

        [SerializeField]
        private float gravity;
        [SerializeField]
        private float damageRadius;

        [SerializeField]
        private Damage damageScript;
        private Rigidbody2D rb;

        private bool isGravityOn;
        private bool hasHitGround;

        [SerializeField]
        private LayerMask whatIsGround;
        [SerializeField]
        private LayerMask whatIsPlayer;
        [SerializeField]
        private Transform damagePosition;
        private Vector2 target;
        bool hasGravity;
        string projectileType;
        public float startingRotation;
        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            rb.gravityScale = 0.0f;
            isGravityOn = false;
            xStartPos = transform.position.x;

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
            if (!hasHitGround)
            {
                float angle = Mathf.Atan2(rb.linearVelocity.y, rb.linearVelocity.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
        }

        private void FixedUpdate()
        {
            if (!hasHitGround)
            {
                Collider2D damageHit = Physics2D.OverlapCircle(damagePosition.position, damageRadius, whatIsPlayer);
                Collider2D groundHit = Physics2D.OverlapCircle(damagePosition.position, damageRadius, whatIsGround);

                // damageHit.gameObject.GetComponentInChildren<DamageReceiver>().CanTakeDamage
                // Debug.Log("damageHit: " + damageHit.gameObject.name + " " + damageHit.gameObject.GetComponentInChildren<DamageReceiver>().CanTakeDamage);
                if (damageHit && damageHit.gameObject.GetComponentInChildren<DamageReceiver>().CanTakeDamage)
                {
                    damageScript.HandleCollision(damageHit);
                    Destroy(gameObject);
                }

                if (groundHit)
                {
                    hasHitGround = true;
                    rb.gravityScale = 0f;
                    rb.linearVelocity = Vector2.zero;
                    Destroy(gameObject, 1.0f);
                }

                if(hasGravity)
                {
                    if (Mathf.Abs(xStartPos - transform.position.x) >= travelDistance && !isGravityOn)
                    {
                        isGravityOn = true;
                        rb.gravityScale = gravity;
                    }
                }
            }        
        }

        public void FireProjectile(float speed, float travelDistance, Vector2 target, string projectileType, float startingRotation)
        {
            this.projectileType = projectileType;
            this.startingRotation = startingRotation;

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
        }
    }

}