using UnityEngine;
using Game.CoreSystem;

public class Damage : MonoBehaviour
{
    [SerializeField] private float damageAmount;

    // Method to dynamically set the damage
    public void SetDamage(float amount)
    {
        damageAmount = amount;
    }

    public void HandleCollision(Collider2D collision)
    {
        // Check if the collision is with the Player
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            if (player != null)
            {
                // Apply damage to the player's health
                Stats stats = player.Core.GetCoreComponent<Stats>();
                if (stats != null)
                {
                    stats.Health.Decrease(damageAmount);
                }
            }
        }
    }
}
