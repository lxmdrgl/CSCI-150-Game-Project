using UnityEngine;

namespace Game.Projectiles
{
    public class Projectile : MonoBehaviour
    {
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
    }
}
