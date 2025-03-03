using UnityEngine;
using Game.CoreSystem;

namespace Game.Projectiles
{
    public class Projectile : MonoBehaviour
    {
        private float speed;
        private float travelDistance;
        private float xStartPos;

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
            } else if (projectileType == "radialLobbing")
            {
                RadialLobbing();
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

        private void RadialLobbing()
        {
            isGravityOn = true;
            rb.gravityScale = gravity;

            float gEffective = Mathf.Abs(Physics2D.gravity.y * rb.gravityScale);

            float dx = target.x - transform.position.x;
            float dy = target.y - transform.position.y;

            float vy = speed;
            float discriminant = vy * vy - 2 * gEffective * dy;

            if (discriminant < 0)
            {
                Debug.LogWarning("No real solution exists for given parameters. Increase initialVy.");
                return;
            }

            float t1 = (-vy + Mathf.Sqrt(discriminant)) / -gEffective;
            float t2 = (-vy - Mathf.Sqrt(discriminant)) / -gEffective;
            
            float tTotal = Mathf.Max(t1, t2);

            float vx = dx / tTotal;

            // Debug.Log($"vx: {vx}, vy: {vy}, t1: {t1}, t2: {t2}, tTotal: {tTotal} gravityScale: {rb.gravityScale} gravity: {gravity}");

            rb.linearVelocity = new Vector2(vx, vy);
        }
    }

}