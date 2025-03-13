using UnityEngine;

public class ExplosiveProjectile : MonoBehaviour
{
    private Rigidbody2D rb;
    private float speed;
    private float travelDistance;
    private float explosionDelay = 0.5f; // Short delay for grenade-like effect
    private Vector2 startPosition;
    private bool hasLanded;

    public void FireProjectile(float projectileSpeed, float projectileTravelDistance)
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 1f;
        speed = projectileSpeed;
        travelDistance = projectileTravelDistance;
        startPosition = transform.position;

        // Launch at a 45-degree angle toward the direction it’s facing
        Vector2 direction = transform.right; // Based on rotation set in attack state
        rb.linearVelocity = direction * speed;
    }

    private void Update()
    {
        if (!hasLanded && Vector2.Distance(startPosition, transform.position) >= travelDistance)
        {
            StopAndExplode();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground") || collision.CompareTag("Player"))
        {
            StopAndExplode();
        }
    }

    private void StopAndExplode()
    {
        if (!hasLanded)
        {
            hasLanded = true;
            rb.linearVelocity = Vector2.zero;
            Invoke("Explode", explosionDelay); // Explode after a short delay
        }
    }

    private void Explode()
    {
        // Add your explosion logic here (e.g., deal AoE damage, spawn effect)
        Debug.Log("Boom!");
        Destroy(gameObject);
    }
}