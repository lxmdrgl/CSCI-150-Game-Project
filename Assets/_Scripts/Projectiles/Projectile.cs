using UnityEngine;

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

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();

            rb.gravityScale = 0.0f;
            rb.linearVelocity = transform.right * speed;

            isGravityOn = false;

            xStartPos = transform.position.x;
        }

        private void Update()
        {
            if (!hasHitGround)
            {
                //attackDetails.position = transform.position;

                if (isGravityOn)
                {
                    float angle = Mathf.Atan2(rb.linearVelocity.y, rb.linearVelocity.x) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                }
            }
        }

        private void FixedUpdate()
        {
            if (!hasHitGround)
            {
                Collider2D damageHit = Physics2D.OverlapCircle(damagePosition.position, damageRadius, whatIsPlayer);
                Collider2D groundHit = Physics2D.OverlapCircle(damagePosition.position, damageRadius, whatIsGround);


                if (damageHit)
                {
                    damageScript.HandleCollision(damageHit);
                    Debug.Log("damageHit");
                    Destroy(gameObject);
                }

                if (groundHit)
                {
                    hasHitGround = true;
                    rb.gravityScale = 0f;
                    rb.linearVelocity = Vector2.zero;
                    Destroy(gameObject, 1.0f);
                }


                if (Mathf.Abs(xStartPos - transform.position.x) >= travelDistance && !isGravityOn)
                {
                    isGravityOn = true;
                    rb.gravityScale = gravity;
                }
            }        
        }

        public void FireProjectile(float speed, float travelDistance)
        {
            this.speed = speed;
            this.travelDistance = travelDistance;
            //attackDetails.damageAmount = damage;
        }

        /*
        private float speed;
        private float travelDistance;
        private Vector2 startPosition;

        private void Start()
        {
            // Store the starting position
            startPosition = transform.position;
        }

        private void Update()
        {
            // Move the projectile in a straight line
            transform.Translate(Vector2.right * speed * Time.deltaTime);

            // Destroy the projectile if it exceeds its travel distance
            if (Vector2.Distance(startPosition, transform.position) >= travelDistance)
            {
                Destroy(gameObject);
            }
        }

        // Initialize the projectile's movement and travel properties
        public void FireProjectile(float speed, float travelDistance)
        {
            this.speed = speed;
            this.travelDistance = travelDistance;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            // Delegate collision and damage handling to the damage.cs component
            Damage damageComponent = GetComponent<Damage>();
            if (damageComponent != null)
            {
                damageComponent.HandleCollision(collision);
            }

            // Destroy the projectile after dealing damage
            Destroy(gameObject);
        }
        */
    }

}